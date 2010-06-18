using System;
using System.IO;

namespace IronJSON
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			try
			{
				JSONManager json = new JSONManager("sandbox.json");
				json.Save("formatted.json");
			}
			catch (IronJSONException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}
	}
}
