using System;

namespace MTicTacToeLogic
{
	public class Field
	{
		public User Winner = User.Nobody;

		public readonly User[,] FieldsSet = new User[3,3];

		public Field()
		{
			for (var i = 0; i < 3; i++)
			{
				for (var j = 0; j < 3; j++)
				{
					FieldsSet[i, j] = User.Nobody;
				}
			}
		}

		public User SetField(User user, int x, int y)
		{
			if (FieldsSet[x, y] != User.Nobody)
			{
				throw new Exception(String.Format("{0}:{1} already have a value!", x, y));
			}
			FieldsSet[x, y] = user;
			var horizontal = 0;
			var vertical = 0;

			for (var i = 0; i < 3; i++)
			{
				if (FieldsSet[i, y] == user)
				{
					horizontal++;
				}

				if (FieldsSet[x, i] == user)
				{
					vertical++;
				}
			}

			if (horizontal == 3 || vertical == 3)
			{
				Winner = user;
			}
			else if ((x + y) % 2 == 0)
			{
				if (FieldsSet[1, 1] == user)
				{
					if ((FieldsSet[0, 0] == user && FieldsSet[2, 2] == user) || (FieldsSet[0, 2] == user && FieldsSet[2, 0] == user))
					{
						Winner = user;
					}
				}
			}

			return Winner;
		}
	}

	public enum User
	{
		Tic,
		Tac,
		Nobody
	}
}
