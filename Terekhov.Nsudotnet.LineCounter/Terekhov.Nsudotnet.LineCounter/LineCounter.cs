using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace LineCounter
{
	class LineCounter
	{
		private static List<string> FileExtentions { get; set; }
		private static int Count { get; set; }
		private static Comments _comments;
		private const string CommentsFile = "comments.xml";

		static void Main(string[] args)
		{
			ParseParameters(args);
			if (FileExtentions.Count == 0)
			{
				Console.WriteLine("No file extentions to check.");
				return;
			}
			Count = 0;
			UpDateComments();
			CalculateLinesInDirectory(Directory.GetCurrentDirectory());
			Console.WriteLine("Line count in project is {0}.", Count);
		}

		private static void UpDateComments()
		{
			var serializer = new XmlSerializer(typeof(Comments));
			_comments = new Comments();
			if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), CommentsFile)))
			{
				var commentFileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), CommentsFile), FileMode.Open);
				try
				{
					_comments = (Comments) serializer.Deserialize(commentFileStream);
				}
				catch (InvalidOperationException e)
				{
					Console.WriteLine(e.Message);
					Console.WriteLine("Using c# comment style.");
				}
			}
			else
			{
				_comments = new Comments();
				var myWriter = new StreamWriter("comments.xml");
				serializer.Serialize(myWriter, _comments);
				myWriter.Close();
			}
		}

		private static void ParseParameters(IEnumerable<string> args)
		{
			FileExtentions = new List<string>();
			foreach (var extention in args)
			{
				if (!extention.StartsWith("*."))
				{
					Console.WriteLine("Invalid parameter: {0}.", extention);
				}
				else
				{
					FileExtentions.Add(extention);
				}
			}
		}

		private static void CalculateLinesInDirectory(string directoryName)
		{
			foreach (var subdirectoryName in Directory.EnumerateDirectories(directoryName))
			{
				CalculateLinesInDirectory(Path.Combine(directoryName, subdirectoryName));
			}
			foreach (var fileName in FileExtentions.SelectMany(extention => Directory.EnumerateFiles(Directory.GetCurrentDirectory(), extention)))
			{
				CalculateLinesInFile(fileName);
			}
		}

		private static void CalculateLinesInFile(string fileName)
		{
			var isMultyLineComment = false;
			foreach (var cleanedLine in File.ReadAllLines(fileName).Select(line => line.Replace("\t", " ")))
			{
				var hasStringDefinition = false;
				var lineWithoutComments = DeleteCommentsFromLine(cleanedLine, ref isMultyLineComment, ref hasStringDefinition);
				if (!String.IsNullOrWhiteSpace(lineWithoutComments) || hasStringDefinition)
				{
					Count++;
				}
			}
		}

		private static string DeleteCommentsFromLine(string line, ref bool isMultyLineComment, ref bool hasStringDefinition)
		{
			int stopMultyLineComment;

			if (isMultyLineComment)
			{
				if ((stopMultyLineComment = line.IndexOf(_comments.MultyLineCommentStop, StringComparison.Ordinal)) == -1)
				{
					line = string.Empty;
				}
				else
				{
					try
					{
						line = line.Substring(stopMultyLineComment + _comments.MultyLineCommentStop.Length);
					}
					catch (ArgumentOutOfRangeException)
					{
						line = string.Empty;
					}
					isMultyLineComment = false;
				}
			}
			int doubleQuote;
			while ((doubleQuote = line.IndexOf(_comments.StringDefinition, StringComparison.Ordinal)) != -1)
			{
				var closeDoubleQuote = line.Remove(0, doubleQuote + 1)
				                           .IndexOf(_comments.StringDefinition, StringComparison.Ordinal);
				line = closeDoubleQuote != -1 ? line.Remove(doubleQuote, closeDoubleQuote + _comments.StringDefinition.Length) : line.Remove(doubleQuote);
				hasStringDefinition = true;
			}
			var startMultyLineComment = line.IndexOf(_comments.MultyLineCommentStart, StringComparison.Ordinal);
			var singleLineComment = line.IndexOf(_comments.SingleLineComment, StringComparison.Ordinal);
			while (startMultyLineComment != -1 || singleLineComment != -1)
			{
				if (startMultyLineComment == -1 ||
				    (singleLineComment != -1 && startMultyLineComment != -1 && singleLineComment < startMultyLineComment))
				{
					line = line.Remove(singleLineComment);
				}
				if (singleLineComment == -1 ||
				    (singleLineComment != -1 && startMultyLineComment != -1 && startMultyLineComment < singleLineComment))
				{
					if ((stopMultyLineComment = line.IndexOf(_comments.MultyLineCommentStop, StringComparison.Ordinal)) != -1 &&
					    startMultyLineComment < stopMultyLineComment)
					{
						line = line.Remove(startMultyLineComment,
						                   stopMultyLineComment - startMultyLineComment + _comments.MultyLineCommentStop.Length);
					}
					else
					{
						line = line.Remove(startMultyLineComment);
						isMultyLineComment = true;
					}
				}
				startMultyLineComment = line.IndexOf(_comments.MultyLineCommentStart, StringComparison.Ordinal);
				singleLineComment = line.IndexOf(_comments.SingleLineComment, StringComparison.Ordinal);
			}
			return line;
		}
	}
}