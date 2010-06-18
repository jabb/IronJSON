using System;
using System.IO;

namespace IronJSON
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			JSONManager json = new JSONManager("parse.json");
			
			Console.WriteLine(json.GetIntegerFrom("this"));
			
			json.Cd(JSONManager.Path.Absolute, "ha", "arr");
			json.SetToBoolean(0, true);
			json.SetToBoolean(1, false);
			json.SetToArray(2, 10);
			
			Console.WriteLine(json.IsArray(2));
			
			Console.WriteLine(json.CurrentPath());
			
			json.CdBack();
			json.Cd(JSONManager.Path.Relative, "doesntexist");
			
			
			json.Save("parse2.json");
		}
	}
}
