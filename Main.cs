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
			
			json.SetToInteger("anint", 10);
			
			// Create a JSON object and "CD" to it so we can add stuff.
			json.SetToObject("anobject");
			json.Cd(JSONManager.Path.Relative, "anobject");
			
			// Add stuff to it.
			json.SetToString("astring", "Hello, world!");
			
			json.Save("example.json");
		}
	}
}