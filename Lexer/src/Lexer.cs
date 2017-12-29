using System;
using System.Text.RegularExpressions;

namespace Compiler.LexicalAnalyzer
{
	public enum TokenType
	{
		Identifier,
		Keyword,
		Number,
		Operator,
		Undefined,
		Eof,
	}

	public class Lexer
	{
		public Lexer(string text)
		{
			Text = text;
		}

		public Token Read()
		{
			Token result;
			IsSeparatorPassed = SkipGarbage() ? true : IsSeparatorPassed;

			if (IsEof)
			{
				result = new Token(TokenType.Eof, true);
			}
			else if (FindTokenByPattern(IDENTIFIER, out Match match))
			{
				if (KEYWORD.Match(match.Value).Value == match.Value)
				{
					result = CreateToken(TokenType.Keyword, match);
				}
				else
				{
					result = CreateToken(TokenType.Identifier, match);
				}

				IsSeparatorPassed = false;
			}
			else if (FindTokenByPattern(NUMBER, out match))
			{
				result = CreateToken(TokenType.Number, match);
				IsSeparatorPassed = false;
			}
			else if (FindTokenByPattern(OPERATOR, out match))
			{
				IsSeparatorPassed = true;
				result = CreateToken(TokenType.Operator, match);
			}
			else
			{
				IsSeparatorPassed = false;
				++CaretPos;
				result = CreateToken(TokenType.Undefined, Text[CaretPos - 1].ToString());
			}

			return result;
		}

		public static readonly int MAX_IDENTIFIER_LENGTH = 120;
		public static readonly Regex KEYWORD = ToTokenRegex(
			"static|main|extends|return|new|this|public" +
			"|void|class|String|int|boolean|if|else" +
			"|while|true|false|System.out.print");
		public static readonly Regex IDENTIFIER = ToTokenRegex(
			"[_a-zA-Z][_a-zA-Z0-9]{1," + Convert.ToString(MAX_IDENTIFIER_LENGTH - 1) + "}");
		public static readonly Regex NUMBER = ToTokenRegex(
			"([1-9][0-9]*)|[0-9]");
		private static readonly Regex OPERATOR = ToTokenRegex(
			"&&|<|>|==|=|!|\\+|-|\\*|\\/" +
			"|\\{|\\}|\\(|\\)|\\[|\\]" +
			"|;|,|\\.");
		private static readonly Regex COMMENT = ToTokenRegex(
			"\\/\\/(.|\n^ \n)*");

		private int CaretPos { get; set; }
		private bool IsEof { get { return CaretPos >= Text.Length; } }
		private string Text { get; set; }
		private bool IsSeparatorPassed { get; set; } = true;

		private static Regex ToTokenRegex(string regex)
		{
			return new Regex("^(" + regex + ")", RegexOptions.Compiled);
		}

		private bool SkipGarbage()
		{
			int caretPositionBefore = CaretPos;

			while (!IsEof)
			{
				if (Char.IsWhiteSpace(Text[CaretPos]))
				{
					CaretPos++;

					continue;
				}
				else if (FindTokenByPattern(COMMENT, out Match match))
				{
					continue;
				}

				break;
			}

			return caretPositionBefore != CaretPos;
		}

		private bool FindTokenByPattern(Regex pattern, out Match result)
		{
			result = pattern.Match(Text, CaretPos, Text.Length - CaretPos);
			CaretPos += result.Success ? result.Length : 0;

			return result.Success;
		}

		private Token CreateToken(TokenType type, Match match)
		{
			return CreateToken(type, match.Value);
		}

		private Token CreateToken(TokenType type, string value)
		{
			return new Token(type, IsSeparatorPassed, value);
		}
	}
}
