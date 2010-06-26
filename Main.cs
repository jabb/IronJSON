using System;
using System.IO;
using IronJSON;

namespace Example
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			string filename = "data.json";
			JSONSerializer serializer = new JSONSerializer();
			Car car = new Car("wrong", "Red", 4, "Me!");
			car.AnotherCar = new Car("wrong", "White", 4, "Wife");
			car.AnotherCar.AnotherCar = new Car("wrong", "Red", 0, "No one");
			
			serializer.Serialize("car", car);
			
			serializer.Save(filename);
			
			Console.WriteLine("{0} {1} {2}", car.name, car.AnotherCar.name, car.AnotherCar.AnotherCar.name);
		}
	}
	
	public class Car : IJSONSerializable
	{
		public string name;
		public string color;
		public int wheels;
		private string owner;
		public Car AnotherCar { get; set; }
	    
		public Car(string name, string color, int wheels, string owner)
		{
			this.name = name;
			this.color = color;
			this.wheels = wheels;
			this.owner = owner;
			AnotherCar = null;
		}
	    
		public void JSONSerialize(JSONSerializer json)
		{
			json.Serialize("child car", AnotherCar);
			json.SerializeString("name", name);
			json.SerializeString("color", color);
			json.SerializeInteger("wheels", wheels);
			json.SerializeString("owner", owner);
			
			json.SerializeArrayBegin("array");
			for (int i = 0; i < 100; ++i)
				json.SerializeArrayInteger(i);
			json.SerializeArrayEnd();
		}
	    
		public void JSONDeserialize(JSONSerializer json)
		{
			
		}
	}
}
