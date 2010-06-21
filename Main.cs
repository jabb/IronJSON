using System;
using System.IO;
using IronJSON;

namespace Example
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			for (int i = 1; i <= 9; ++i)
				Test("test" + i.ToString() + ".json");
		}
		
		public static void Test(string filename)
		{
			try
			{
				JSONManager json = new JSONManager(filename);
			}
			catch (JSONException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				return;
			}
			
			Console.WriteLine("No errors");
		}
	}
}