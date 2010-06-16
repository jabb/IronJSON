using System;
using System.IO;

namespace IronJSON
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			StreamReader reader = new StreamReader("sample.json");
			IronJSONLexer lexer;
			try
			{
				lexer = new IronJSONLexer(reader.ReadToEnd());
			}
			finally
			{
				reader.Close();
			}
			IronJSONTokenStream stream = lexer.GenerateTokenStream();
			
			do 
			{
				if (stream.CurrentToken.Type == Token.String)
					Console.WriteLine("{0}: \"{1}\"", stream.CurrentToken.Type, stream.CurrentToken.String);
				else if (stream.CurrentToken.Type == Token.Float)
					Console.WriteLine("{0}: {1}", stream.CurrentToken.Type, stream.CurrentToken.Float);
				else if (stream.CurrentToken.Type == Token.Integer)
					Console.WriteLine("{0}: {1}", stream.CurrentToken.Type, stream.CurrentToken.Integer);
				else
					Console.WriteLine("{0}", stream.CurrentToken.Type);
				stream.ToNextToken();
			} while (!stream.AtEnd());
		}
	}
}
