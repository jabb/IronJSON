
using System;
using System.Collections;
using System.IO;

namespace IronJSON
{
	public interface IJSONSerializable
	{
		void JSONSerialize(JSONSerializer ser);
		void JSONDeserialize(JSONDeserializer deser);
	}
	
	public class JSONSerializer
	{
		private JSONManager Manager { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public JSONSerializer()
		{
			New();
		}
		
		/// <summary>
		/// 
		/// </summary>
		public void New()
		{
			Manager = new JSONManager();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		public void Save(string filename)
		{
			StreamWriter writer = new StreamWriter(filename);
			
			writer.WriteLine(Manager.ToString());
			
			writer.Close();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="json">
		/// A <see cref="IJSONSerializable"/>
		/// </param>
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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		public void SerializeArrayBegin(string key)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToArray(key, 0);
				Manager.Cd(JSONManager.Path.Relative, key);
			}
			// TODO: Handle error.
		}
		
		/// <summary>
		/// 
		/// </summary>
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
		
		/// <summary>
		/// 
		/// </summary>
		public void SerializeArrayEnd()
		{
			if (Manager.IsCurrentArray())
			{
				Manager.CdBack();
			}
			// TODO: Handle error.
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="s">
		/// A <see cref="System.String"/>
		/// </param>
		public void SerializeString(string key, string s)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToString(key, s);
			}
			// TODO: Handle error.
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s">
		/// A <see cref="System.String"/>
		/// </param>
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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="i">
		/// A <see cref="System.Int64"/>
		/// </param>
		public void SerializeInteger(object key, long i)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToInteger(key, i);
			}
			// TODO: Handle error.
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i">
		/// A <see cref="System.Int64"/>
		/// </param>
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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="f">
		/// A <see cref="System.Double"/>
		/// </param>
		public void SerializeFloat(string key, double f)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToFloat(key, f);
			}
			// TODO: Handle error.
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="f">
		/// A <see cref="System.Double"/>
		/// </param>
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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="b">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public void SerializeBoolean(string key, bool b)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToBoolean(key, b);
			}
			// TODO: Handle error.
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="b">
		/// A <see cref="System.Boolean"/>
		/// </param>
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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		public void SerializeNull(string key)
		{
			if (!Manager.IsCurrentArray())
			{
				Manager.SetToNull(key);
			}
			// TODO: Handle error.
		}
		
		/// <summary>
		/// 
		/// </summary>
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
	}
	
	public class JSONDeserializer
	{
		private JSONManager Manager { get; set; }
		// This is used for serializing arrays. It keeps
		// track of the position in the array so we
		// can incremently go through each element.
		private Stack ArrayPositionStack { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		public JSONDeserializer(string filename)
		{
			Open(filename);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		public void Open(string filename)
		{
			Manager = new JSONManager(filename);
			ArrayPositionStack = new Stack();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="json">
		/// A <see cref="IJSONSerializable"/>
		/// </param>
		public void Deserialize(string key, IJSONSerializable json)
		{
			Manager.Cd(JSONManager.Path.Relative, key);
			json.JSONDeserialize(this);
			Manager.CdBack();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		public void DeserializeArrayBegin(string key)
		{
			Manager.Cd(JSONManager.Path.Relative, key);
			ArrayPositionStack.Push(0);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
		public int DeserializeArraySize()
		{
			if (Manager.IsCurrentArray())
			{
				return Manager.CurrentArraySize;
			}
			
			return -1;
			// TODO: Handle error.
		}
		
		/// <summary>
		/// 
		/// </summary>
		public void DeserializeArrayBegin()
		{
			if (Manager.IsCurrentArray())
			{
				int pos = (int)ArrayPositionStack.Pop();
				Manager.Cd(JSONManager.Path.Relative, pos);
				ArrayPositionStack.Push(pos + 1);
				ArrayPositionStack.Push(0);
			}
			// TODO: Handle error.
		}
		
		/// <summary>
		/// 
		/// </summary>
		public void DeserializeArrayEnd()
		{
			Manager.CdBack();
			ArrayPositionStack.Pop();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string DeserializeString(string key)
		{
			if (!Manager.IsCurrentArray())
			{
				return Manager.GetStringFrom(key);
			}
			
			throw new JSONException("failed to deserialize string from: " + key);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string DeserializeString()
		{
			if (Manager.IsCurrentArray())
			{
				int pos = (int)ArrayPositionStack.Pop();
				ArrayPositionStack.Push(pos + 1);
				
				return Manager.GetStringFrom(pos);
			}
			
			throw new JSONException("failed to deserialize string from: non-array");
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Int64"/>
		/// </returns>
		public long DeserializeInteger(string key)
		{
			if (!Manager.IsCurrentArray())
			{
				return Manager.GetIntegerFrom(key);
			}
			
			throw new JSONException("failed to deserialize integer from: " + key);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int64"/>
		/// </returns>
		public long DeserializeInteger()
		{
			if (Manager.IsCurrentArray())
			{
				int pos = (int)ArrayPositionStack.Pop();
				ArrayPositionStack.Push(pos + 1);
				
				return Manager.GetIntegerFrom(pos);
			}
			
			throw new JSONException("failed to deserialize integer from: non-array");
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/>
		/// </returns>
		public double DeserializeFloat(string key)
		{
			if (!Manager.IsCurrentArray())
			{
				return Manager.GetFloatFrom(key);
			}
			
			throw new JSONException("failed to deserialize float from: " + key);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Double"/>
		/// </returns>
		public double DeserializeFloat()
		{
			if (Manager.IsCurrentArray())
			{
				int pos = (int)ArrayPositionStack.Pop();
				ArrayPositionStack.Push(pos + 1);
				
				return Manager.GetFloatFrom(pos);
			}
			
			throw new JSONException("failed to deserialize float from: non-array");
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool DeserializeBoolean(string key)
		{
			if (!Manager.IsCurrentArray())
			{
				return Manager.GetBooleanFrom(key);
			}
			
			throw new JSONException("failed to deserialize boolean from: " + key);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool DeserializeBoolean()
		{
			if (Manager.IsCurrentArray())
			{
				int pos = (int)ArrayPositionStack.Pop();
				ArrayPositionStack.Push(pos + 1);
				
				return Manager.GetBooleanFrom(pos);
			}
			
			throw new JSONException("failed to deserialize boolean from: non-array");
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool DeserializeIsNull(string key)
		{
			if (!Manager.IsCurrentArray())
			{
				return Manager.IsNull(key);
			}
			
			throw new JSONException("failed to deserialize null from: " + key);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool DeserializeIsNull()
		{
			if (Manager.IsCurrentArray())
			{
				int pos = (int)ArrayPositionStack.Peek();
				
				return Manager.IsNull(pos);
			}
			
			throw new JSONException("failed to deserialize null from: non-array");
		}
	}
}
