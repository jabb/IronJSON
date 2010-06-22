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
			JSONSerializer serializer = new JSONSerializer(filename);
            Car car = new Car("wrong", "Red", 4, "Me!");
			car.AnotherCar = new Car("wrong", "White", 4, "Wife");
			car.AnotherCar.AnotherCar = new Car("wrong", "Red", 0, "No one");
            
            serializer.Deserialize("car", car);
            
            //serializer.Save(filename);
			
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
			if (AnotherCar != null)
				json.Serialize("car", AnotherCar);
			double[] array = new double[3]{3.1, 4.1, 5.9};
			object[] narray = new object[array.Length];
			Array.Copy(array, narray, array.Length);
			json.SerializeArray("arr", narray);
	        json.SerializeObject("name", name);
	        json.SerializeObject("color", color);
	        json.SerializeObject("wheels", wheels);
	        json.SerializeObject("owner", owner);
	    }
	    
	    public void JSONDeserialize(JSONSerializer json)
	    {
			if (AnotherCar != null)
				json.Deserialize("car", AnotherCar);
			object[] narray = json.DeserializeArray("arr");
			double[] array = new double[narray.Length];
			
			Array.Copy(narray, array, narray.Length);
			
	        name = (string)json.DeserializeObject("name");
	        color = (string)json.DeserializeObject("color");
	        wheels = Convert.ToInt32(json.DeserializeObject("wheels"));
	        owner = (string)json.DeserializeObject("owner");
	    }
	}
}
