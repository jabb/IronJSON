using System;
using System.IO;
using IronJSON;

namespace Example
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			JSONSerializer serializer = new JSONSerializer();
			Car car = new Car("wrong", "Red", 4, "Me!");
			car.AnotherCar = new Car("wrong", "White", 4, "Wife");
			car.AnotherCar.AnotherCar = new Car("wrong", "Red", 0, "No one");
			
			serializer.Serialize("car", car);
			
			serializer.Save("data.json");
			
			JSONDeserializer deserializer = new JSONDeserializer("data.json");
			
			deserializer.Deserialize("car", car);
			
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
	    
		public void JSONSerialize(JSONSerializer ser)
		{
			if (AnotherCar != null)
				ser.Serialize("child car", AnotherCar);
			ser.SerializeString("name", name);
			ser.SerializeString("color", color);
			ser.SerializeInteger("wheels", wheels);
			ser.SerializeString("owner", owner);
			
			ser.SerializeArrayBegin("array");
			for (int i = 1; i <= 10; ++i)
			{
				ser.SerializeArrayBegin();
				for (int j = 1; j <= 10; ++j)
					ser.SerializeInteger(i * j);
				ser.SerializeArrayEnd();
			}
			ser.SerializeArrayEnd();
		}
	    
		public void JSONDeserialize(JSONDeserializer deser)
		{
			if (AnotherCar != null)
				deser.Deserialize("child car", AnotherCar);
			name = deser.DeserializeString("name");
			color = deser.DeserializeString("color");
			wheels = (int)deser.DeserializeInteger("wheels");
			owner = deser.DeserializeString("owner");
			
			deser.DeserializeArrayBegin("array");
			for (int i = 1; i <= deser.DeserializeArraySize(); ++i)
			{
				deser.DeserializeArrayBegin();
				for (int j = 1; j <= deser.DeserializeArraySize(); ++j)
					deser.DeserializeInteger();
				deser.DeserializeArrayEnd();
			}
			deser.DeserializeArrayEnd();
		}
	}
}
