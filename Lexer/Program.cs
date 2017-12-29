using Compiler.LexicalAnalyzer;
using System;
using System.IO;

namespace Compiler
{
	class Program
	{
		private static readonly int MIN_ARGS_COUNT = 1;

		static int Main(string[] args)
		{
			try
			{
				if (args.Length < MIN_ARGS_COUNT)
				{
					throw new ArgumentException("Invalid arguments count");
				}

				StreamReader reader = new StreamReader(args[0]);
				Lexer lexer = new Lexer(reader.ReadToEnd());
				Token token = lexer.Read();

				while (token.Type != TokenType.Eof)
				{
					Console.WriteLine(token);
					token = lexer.Read();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

				return 1;
			}

			return 0;
		}
	}
}
