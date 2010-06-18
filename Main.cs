using System;
using System.IO;

namespace IronJSON
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			JSONManager json = new JSONManager();
			
			json.SetToObject("mobs");
			json.Cd(JSONManager.Path.Relative, "mobs");
			
			ConstructMob(json, "goblin", 100, 0, new string[]{"sword", null, null});
			
			ConstructMob(json, "troll", 300, 0, new string[]{"fist", "crush", null});
			
			ConstructMob(json, "wizard", 120, 60, new string[]{"fireball", null, null});
			
			json.Save("mobs.json");
		}
		
		public static void ConstructMob(JSONManager json, string name, int hp, int mp, string[] attacks)
		{
			json.SetToObject(name);
			json.Cd(JSONManager.Path.Relative, name);
			
			json.SetToString("name", name);
			json.SetToInteger("health", hp);
			json.SetToInteger("mana", mp);
			
			json.SetToArray("attacks", attacks.Length);
			json.Cd(JSONManager.Path.Relative, "attacks");
			for (int i = 0; i < attacks.Length; ++i)
			{
				if (attacks[i] == null)
					json.SetToNull(i);
				else
					json.SetToString(i, attacks[i]);
			}
			
			json.CdBack();
			json.CdBack();
		}
	}
}
