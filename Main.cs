using System;
using System.IO;

namespace IronJSON
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Test2();
		}
		
		public static void Test1()
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
				if (stream.CurrentToken.Type == TokenType.String)
					Console.WriteLine("{0}: \"{1}\"", stream.CurrentToken.Type, stream.CurrentToken.String);
				else if (stream.CurrentToken.Type == TokenType.Float)
					Console.WriteLine("{0}: {1}", stream.CurrentToken.Type, stream.CurrentToken.Float);
				else if (stream.CurrentToken.Type == TokenType.Integer)
					Console.WriteLine("{0}: {1}", stream.CurrentToken.Type, stream.CurrentToken.Integer);
				else
					Console.WriteLine("{0}", stream.CurrentToken.Type);
				stream.ToNextToken();
			} while (!stream.AtEnd());
		}
		
		public static void Test2()
		{
			IronJSONObject obj = new IronJSONObject();
			IronJSONObject nobj = new IronJSONObject();
			
			nobj["is"] = new IronJSONValue("json");
			nobj["!"] = new IronJSONValue(ValueType.Null);
			
			obj["this"] = new IronJSONValue(5);
			obj["ha"] = new IronJSONValue(nobj);
			IronJSONValue arr = new IronJSONValue(ValueType.Array);
			arr.Array.Add(new IronJSONValue(5));
			arr.Array.Add(new IronJSONValue(10));
			arr.Array.Add(new IronJSONValue(4.3E-10));
			arr.Array.Add(new IronJSONValue(15));
			obj["arr"] = arr;
			
			Console.WriteLine(obj.ToString());
		}
	}
}
