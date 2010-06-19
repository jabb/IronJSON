using System;
using System.IO;
using IronJSON;

namespace Example
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			JSONManager json = new JSONManager();
			
			json.SetToArray("arr", 5);
			
			json.Cd(JSONManager.Path.Relative, "arr");
			json.SetToBoolean(0, true);
			json.CurrentArraySize = 1;
			
			json.Save("examplef.json");
		}
	}
}