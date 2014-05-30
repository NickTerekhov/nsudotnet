using System;

namespace Terekhov.Nsudotnet.LineCounter
{
	[Serializable]
	public class Comments
	{
		public string SingleLineComment { get; set; }
		public string MultyLineCommentStart { get; set; }
		public string MultyLineCommentStop { get; set; }
		public string StringDefinition { get; set; }

		public Comments()
		{
			SingleLineComment = "//";
			MultyLineCommentStart = "/*";
			MultyLineCommentStop = "*/";
			StringDefinition = "\"";
		}
	}
}
