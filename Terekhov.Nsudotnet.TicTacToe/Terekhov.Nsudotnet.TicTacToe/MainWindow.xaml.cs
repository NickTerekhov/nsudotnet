using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MTicTacToeWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private readonly Field[,] _field = new Field[3, 3];
		private readonly User[,] _winners = new User[3, 3];
		private User _userTurn = User.Tic;
		private int? _nextTurnAllowedX;
		private int? _nextTurnAllowedY;
		private User _winner = User.Nobody;

		public MainWindow()
		{
			InitWinners();
			InitField();
			InitializeComponent();
		}

		private void InitField()
		{
			for (var i = 0; i < 3; i++)
			{
				for (var j = 0; j < 3; j++)
				{
					_field[i, j] = new Field();
				}
			}
		}

		private void InitWinners()
		{
			for (var i = 0; i < 3; i++)
			{
				for (var j = 0; j < 3; j++)
				{
					_winners[i, j] = User.Nobody;
				}
			}
		}

		private void GridButton_OnClick(object sender, RoutedEventArgs e)
		{
			try
			{
				var parent = ((FrameworkElement)sender).Parent as Grid;
				var button = sender as Button;
				if (button == null)
				{
					return;
				}

				if (parent != null)
				{
					var gridColumn = Grid.GetColumn(parent);
					var gridRow = Grid.GetRow(parent);

					var fieldColumn = Grid.GetColumn(button);
					var fieldRow = Grid.GetRow(button);

					if (_nextTurnAllowedX != null && _nextTurnAllowedY != null)
					{
						if (_nextTurnAllowedX.Value != gridRow || _nextTurnAllowedY != gridColumn)
						{
							throw new Exception("You are not allowed to make a turn here!");
						}
					}

					var winner = _field[gridColumn, gridRow].SetField(_userTurn, fieldColumn, fieldRow);
					button.Content = _userTurn == User.Tic ? "X" : "O";
					if (_winners[gridRow, gridColumn] == User.Nobody)
					{
						button.Background = _userTurn == User.Tic ? Brushes.HotPink : Brushes.Aqua;
					}

					var freeCellsInDestCell = 0;
					for (var i = 0; i < 3; i++)
					{
						for (var j = 0; j < 3; j++)
						{
							freeCellsInDestCell += _field[fieldRow, fieldColumn].FieldsSet[i, j] == User.Nobody ? 1 : 0;
						}
					}
					if (freeCellsInDestCell < 1)
					{
						_nextTurnAllowedX = null;
						_nextTurnAllowedY = null;
					}
					else
					{
						_nextTurnAllowedX = fieldRow;
						_nextTurnAllowedY = fieldColumn;
					}
					
					if (winner != User.Nobody && winner == _userTurn)
					{
						_winners[gridRow, gridColumn] = _userTurn;
						foreach (var b in parent.Children.OfType<Button>())
						{
							b.Background = _userTurn == User.Tic ? Brushes.HotPink : Brushes.Aqua;
						}

						var horizontal = 0;
						var vertical = 0;

						for (var i = 0; i < 3; i++)
						{
							if (_winners[i, gridColumn] == _userTurn)
							{
								horizontal++;
							}

							if (_winners[gridRow, i] == _userTurn)
							{
								vertical++;
							}
						}

						if (horizontal == 3 || vertical == 3)
						{
							_winner = _userTurn;
						}
						else if ((gridRow + gridColumn) % 2 == 0)
						{
							if (_winners[1, 1] == _userTurn)
							{
								if ((_winners[0, 0] == _userTurn && _winners[2, 2] == _userTurn) || (_winners[0, 2] == _userTurn && _winners[2, 0] == _userTurn))
								{
									_winner = _userTurn;
								}
							}
						}

						if (_winner != User.Nobody)
						{
							var result = MessageBox.Show(String.Format("User {0} won! Restart?", _winner), "Confirmation",
								MessageBoxButton.YesNo, MessageBoxImage.Question);
							if (result == MessageBoxResult.No)
							{
								Application.Current.Shutdown();
							}
							else if (result == MessageBoxResult.Yes)
							{
								InitField();
								InitWinners();
								foreach (var b in GridsGrid.Children.OfType<Grid>().SelectMany(g => g.Children.OfType<Button>()))
								{
									b.Content = "";
									b.Background = Brushes.LightGray;
								}
							}
						}
					}
					_userTurn = _userTurn == User.Tic ? User.Tac : User.Tic;
				}
			}
			catch (Exception exception)
			{
				var result = MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				if (result == MessageBoxResult.OK)
				{
					//nothing?
				}
			}
		}
	}
}
