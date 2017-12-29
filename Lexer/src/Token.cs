namespace Compiler.LexicalAnalyzer
{
	public sealed class Token
	{
		public Token(TokenType type, bool isValid, string text = "")
		{
			Type = type;
			Text = text;
			IsValid = isValid;
		}

		public TokenType Type { get; private set; }
		public string Text { get; private set; }
		public bool IsValid { get; private set; }

		public override string ToString()
		{
			return string.Format(PATTERN, Text, Type, IsValid);
		}

		private readonly string PATTERN = "<Token Text=\"{0}\" Type=\"{1}\" IsValid=\"{2}\"></Token>";
	}
}
