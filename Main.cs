using System;
using System.IO;

namespace IronJSON
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			JSONManager json = new JSONManager("sample.json");
			
			
			json.Save("formatted.json");
		}
	}
}
