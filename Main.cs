using System;
using System.IO;

namespace IronJSON
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			JSONManager json = new JSONManager(args[0]);
			
			
			Console.WriteLine(json.ToString());
		}
	}
}
