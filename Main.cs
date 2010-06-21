using System;
using System.IO;
using IronJSON;

namespace Example
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			try
			{
				JSONManager json = new JSONManager("sample.json");
				
				json.Save("formatted.json");
			}
			catch (JSONException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}
	}
}