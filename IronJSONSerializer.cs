
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
				SerializeNull(key);
			Manager.CdBack();
		}
		
		public void SerializeArrayBegin(string key)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToArray(key, 0);
				Manager.Cd(JSONManager.Path.Relative, key);
			}
			// TODO: Handle error.
		}
		
		public void SerializeArrayBegin()
		{
			if (Manager.IsCurrentArray())
			{
				int length = Manager.CurrentArraySize;
				Manager.CurrentArraySize = length + 1;
				Manager.SetToArray(length, 0);
				Manager.Cd(JSONManager.Path.Relative, length);
			}
			// TODO: Handle error.
		}
		
		public void SerializeArrayEnd()
		{
			if (Manager.IsCurrentArray())
			{
				Manager.CdBack();
			}
			// TODO: Handle error.
		}

		public void SerializeString(string key, string s)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToString(key, s);
			}
			// TODO: Handle error.
		}
		
		public void SerializeString(string s)
		{
			if (Manager.IsCurrentArray())
			{
				int length = Manager.CurrentArraySize;
				Manager.CurrentArraySize = length + 1;
				Manager.SetToString(length, s);
			}
			// TODO: Handle error.
		}
		
		public void SerializeInteger(object key, long i)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToInteger(key, i);
			}
			// TODO: Handle error.
		}
		
		public void SerializeInteger(long i)
		{
			if (Manager.IsCurrentArray())
			{
				int length = Manager.CurrentArraySize;
				Manager.CurrentArraySize = length + 1;
				Manager.SetToInteger(length, i);
			}
			// TODO: Handle error.
		}
		
		public void SerializeFloat(string key, double f)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToFloat(key, f);
			}
			// TODO: Handle error.
		}
		
		public void SerializeFloat(double f)
		{
			if (Manager.IsCurrentArray())
			{
				int length = Manager.CurrentArraySize;
				Manager.CurrentArraySize = length + 1;
				Manager.SetToFloat(length, f);
			}
			// TODO: Handle error.
		}
		
		public void SerializeBoolean(string key, bool b)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToBoolean(key, b);
			}
			// TODO: Handle error.
		}
		
		public void SerializeBoolean(bool b)
		{
			if (Manager.IsCurrentArray())
			{
				int length = Manager.CurrentArraySize;
				Manager.CurrentArraySize = length + 1;
				Manager.SetToBoolean(length, b);
			}
			// TODO: Handle error.
		}
		
		public void SerializeNull(string key)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToNull(key);
			}
			// TODO: Handle error.
		}
		
		public void SerializeNull()
		{
			if (Manager.IsCurrentArray())
			{
				int length = Manager.CurrentArraySize;
				Manager.CurrentArraySize = length + 1;
				Manager.SetToNull(length);
			}
			// TODO: Handle error.
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
