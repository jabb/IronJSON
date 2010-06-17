using System;
using System.IO;

namespace IronJSON
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			IronJSON json = new IronJSON();
			
			json.Save("empty.json");
		}
		
		public static void Test1()
		{
			StreamReader reader = new StreamReader("sample.json");
			IronJSONLexer lexer;
			IronJSONTokenStream stream;
			IronJSONParser parser;
			
			try
			{
				lexer = new IronJSONLexer(reader.ReadToEnd());
			}
			finally
			{
				reader.Close();
			}
			stream = lexer.GenerateTokenStream();
			
			parser = new IronJSONParser(stream);
			
			parser.Parse();
			
			Console.WriteLine(parser.Obj.ToString());
		}
		
		/// <summary>
		/// Test for outputing IronJSONObjects
		/// </summary>
		public static void Test2()
		{
			IronJSONObject obj = new IronJSONObject();
			IronJSONObject nobj = new IronJSONObject();
			
			nobj["is"] = new IronJSONValue("json");
			nobj["!"] = new IronJSONValue(ValueType.Null);
			
			obj["this"] = new IronJSONValue(5);
			obj["ha"] = new IronJSONValue(nobj);
			
			Console.WriteLine(obj.ToString());
		}
	}
}
