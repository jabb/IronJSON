
using System;
using System.Collections;
using System.IO;

namespace IronJSON
{
	public interface IJSONSerializable
	{
		void JSONSerialize(JSONSerializer ser);
		void JSONDeserialize(JSONSerializer ser);
	}
	
	public class JSONSerializer
	{
		public JSONManager Manager { private get; set; }
		
		public JSONSerializer()
		{
			Close();
		}
		
		public JSONSerializer(string filename)
		{
			Open(filename);
		}
		
		public void Open(string filename)
		{
			Manager = new JSONManager(filename);
		}
		
		public void Close()
		{
			Manager = new JSONManager();
		}
				
		public void Save(string filename)
		{
			StreamWriter writer = new StreamWriter(filename);
			
			writer.WriteLine(Manager.ToString());
			
			writer.Close();
		}
		
		public void Serialize(string key, IJSONSerializable json)
		{
			Manager.SetToObject(key);
			Manager.Cd(JSONManager.Path.Relative, key);
			if (json != null)
				json.JSONSerialize(this);
			else
				SerializeObject(key, null);
			Manager.CdBack();
		}
		
		public void SerializeArray(string key, object[] array)
		{
			Manager.SetToArray(key, array.Length);
			Manager.Cd(JSONManager.Path.Relative, key);
			if (array != null)
			{
				for (int i = 0; i < array.Length; ++i)
				{
					SerializeObject(i, array[i]);
				}
			}
			else
				SerializeObject(key, null);
			Manager.CdBack();
		}

		public void SerializeObject(object key, object o)
		{
			if (o is string)
				Manager.SetToString(key, (string)o);
			else if (o is int)
				Manager.SetToInteger(key, (long)(int)o);
			else if (o is long)
				Manager.SetToInteger(key, (long)o);
			else if (o is double)
				Manager.SetToFloat(key, (double)o);
			else if (o is float)
				Manager.SetToFloat(key, (double)(float)o);
			else if (o is bool)
				Manager.SetToBoolean(key, (bool)o);
			else if (o == null)
				Manager.SetToNull(key);
		}
		
		public void Deserialize(string key, IJSONSerializable json)
		{
			Manager.Cd(JSONManager.Path.Relative, key);
			json.JSONDeserialize(this);
			Manager.CdBack();
		}
		
		public object[] DeserializeArray(string key)
		{
			Manager.Cd(JSONManager.Path.Relative, key);
			
			object[] array = new object[Manager.CurrentArraySize];

			for (int i = 0; i < array.Length; ++i)
			{
				array[i] = DeserializeObject(i);
			}
			
			Manager.CdBack();
			
			return array;
		}

		public object DeserializeObject(object key)
		{
			if (Manager.IsString(key))
				return Manager.GetStringFrom(key);
			else if (Manager.IsInteger(key))
				return Manager.GetIntegerFrom(key);
			else if (Manager.IsFloat(key))
				return Manager.GetFloatFrom(key);
			else if (Manager.IsBoolean(key))
				return Manager.GetBooleanFrom(key);
			else if (Manager.IsNull(key))
				return null;
			return null;
		}
	}
}
