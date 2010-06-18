using System;
using System.IO;

namespace IronJSON
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Test("test1.json");
			Test("test2.json");
			Test("test3.json");
			Test("test4.json");
			Test("test5.json");
		}
		
		static void Test(string filename)
		{
			Console.WriteLine("\n\nTesting: " + filename + "...");
			try
			{
				JSONManager json = new JSONManager(filename);
				json.Save(filename + "f.json");
			}
			catch (JSONException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				Console.WriteLine("Check error message for correctness.");
				return;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				Console.WriteLine("Test failed!");
				return;
			}
			Console.WriteLine("Test passed!");
		}
	}
}
