using System;
using System.IO;
using System.Collections;

namespace IronJSON
{
	/// <summary>
	/// Loads/reads/modifies/saves JSON objects.
	/// </summary>
	public class JSONManager
	{
		
		#region Exception Classes
		
		/// <summary>
		/// Base class for all the exceptions.
		/// </summary>
		public class KeyException : JSONException
		{
			public KeyException(string message) :
				base(message)
			{
			}
		}
		
		/// <summary>
		/// Thrown when a key isn't found.
		/// </summary>
		public class KeyNotFoundException : KeyException
		{
			public KeyNotFoundException(string message) :
				base("key not found: " + message)
			{
			}
		}
		
		/// <summary>
		/// Thrown when trying to retrieve a different value
		/// from the one that exists.
		/// </summary>
		public class ValueAccessException : KeyException
		{
			public ValueAccessException(string tried, string actual) : 
				base("tried to retrieve " + tried + " from " + actual)
			{
			}
		}
		
		/// <summary>
		/// Thrown when passed an invalid key. Keys are
		/// integers or strings. Or when trying to access
		/// an object entry with an integer.
		/// </summary>
		public class InvalidKeyException : KeyException
		{
			public InvalidKeyException(string message) :
				base("invalid key: " + message)
			{
			}
		}
		
		/// <summary>
		/// Thrown when trying to "CD" to a non-array or non-object
		/// value. Or performing array based operations (resizing)
		/// on a non-array.
		/// </summary>
		public class InvalidLocationException : KeyException
		{
			public InvalidLocationException(string message) :
				base("invalid location: " + message)
			{
			}
		}
		
		#endregion
		
		#region Constants
		
		/// <summary>
		/// Enumeration of possible pathing.
		/// </summary>
		public enum Path
		{
			Absolute,
			Relative
		}
		
		#endregion
		
		#region Private Member
		
		// The JSON object.
		private IronJSONObject m_obj;
		
		// The "current directory", if you will.
		private IronJSONValue m_cd;
		
		private ArrayList m_curPath;
		
		#endregion
		
		#region Constructors
		
		/// <summary>
		/// Opens a JSON file.
		/// </summary>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		public JSONManager(string filename)
		{
			this.Open(filename);
			m_curPath = new ArrayList();
		}
		
		/// <summary>
		/// Constructs an empty JSON object.
		/// </summary>
		public JSONManager()
		{
			m_obj = new IronJSONObject();
			m_cd = new IronJSONValue(m_obj);
			m_curPath = new ArrayList();
		}
		
		#endregion
		
		#region Open/Save Functions
		
		/// <summary>
		/// Opens a new file to be read. Overwrite the current
		/// JSON object.
		/// </summary>
		public void Open(string filename)
		{
			StreamReader reader = new StreamReader(filename);
			IronJSONLexer lexer;
			IronJSONTokenStream stream;
			IronJSONParser parser;
			
			lexer = new IronJSONLexer(reader.ReadToEnd());
			
			stream = lexer.GenerateTokenStream();
			
			parser = new IronJSONParser(stream);
			
			parser.Parse();
			m_obj = parser.Obj;
			m_cd = new IronJSONValue(m_obj);
			m_curPath = new ArrayList();
			
			reader.Close();
		}
		
		/// <summary>
		/// Saves the current JSON object to file.
		/// </summary>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		public void Save(string filename)
		{
			StreamWriter writer = new StreamWriter(filename);
			
			writer.WriteLine(m_obj.ToString());
			
			writer.Close();
		}
		
		#endregion
		
		#region Pathing Functions
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path">
		/// A <see cref="Path"/>
		/// </param>
		/// <param name="keys">
		/// A <see cref="System.Object[]"/>
		/// </param>
		public void Cd(Path path, params object[] keys)
		{
			if (path == Path.Absolute)
			{
				m_cd = new IronJSONValue(m_obj);
				m_curPath.Clear();
			}
			
			foreach (object o in keys)
			{
				if (o is int)
				{
					if (m_cd.Type != ValueType.Array)
						throw new InvalidKeyException(CurrentPath() + o.ToString());
					try
					{
						m_cd = (IronJSONValue)m_cd.Array[(int)o];
					}
					// Out of range of the array.
					catch (System.ArgumentOutOfRangeException)
					{
						throw new KeyNotFoundException(CurrentPath() + o.ToString());
					}
				}
				else if (o is string)
				{
					if (m_cd.Type != ValueType.Object)
						throw new InvalidKeyException(CurrentPath() + o.ToString());
					else if (!m_cd.Obj.ContainsKey((string)o))
						throw new KeyNotFoundException(CurrentPath() + o.ToString());
					m_cd = (IronJSONValue)m_cd.Obj[(string)o];
				}
				else
				{
					throw new InvalidKeyException(o.ToString());
				}
				
				// Validate, make sure we CDed to an object or array.
				if (m_cd.Type != ValueType.Array && m_cd.Type != ValueType.Object)
					throw new InvalidLocationException(CurrentPath() + o.ToString());
			
				m_curPath.Add(o);
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public void CdBack()
		{
			ArrayList tmpPath = (ArrayList)m_curPath.Clone();
			tmpPath.RemoveAt(m_curPath.Count - 1);
			Cd(Path.Absolute);
			foreach (object o in tmpPath)
				Cd(Path.Relative, o);
		}
		
		public string CurrentPath()
		{
			string s = "/";
			
			foreach (object o in m_curPath)
				s += o.ToString() + '/';
			
			return s;
		}
		
		#endregion
		
		#region Checking Functions
		
		public bool Exists(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
			{
				return (int)key >= 0 && (int)key < m_cd.Array.Count;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				return m_cd.Obj.ContainsKey((string)key);
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool IsFloat(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				return val.Type == ValueType.Float;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = m_cd.Obj[(string)key];
				return val.Type == ValueType.Float;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool IsInteger(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				return val.Type == ValueType.Integer;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = m_cd.Obj[(string)key];
				return val.Type == ValueType.Integer;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool IsString(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				return val.Type == ValueType.String;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = m_cd.Obj[(string)key];
				return val.Type == ValueType.String;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool IsBoolean(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				return val.Type == ValueType.True ||
					val.Type == ValueType.False;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = m_cd.Obj[(string)key];
				return val.Type == ValueType.True ||
					val.Type == ValueType.False;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool IsNull(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				return val.Type == ValueType.Null;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = m_cd.Obj[(string)key];
				return val.Type == ValueType.Null;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool IsArray(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				return val.Type == ValueType.Array;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = m_cd.Obj[(string)key];
				return val.Type == ValueType.Array;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool IsObject(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				return val.Type == ValueType.Object;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = m_cd.Obj[(string)key];
				return val.Type == ValueType.Object;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool CanCdTo(object key)
		{
			return IsObject(key) || IsArray(key);
		}
		
		#endregion
		
		#region SetTo Functions
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		public void SetToObject(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
				m_cd.Array[(int)key] = new IronJSONValue(ValueType.Object);
			else if (key is string && m_cd.Type == ValueType.Object)
				m_cd.Obj[(string)key] = new IronJSONValue(ValueType.Object);
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="size">
		/// A <see cref="System.Int32"/>
		/// </param>
		public void SetToArray(object key, int size)
		{
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue arr = new IronJSONValue(ValueType.Array);
				
				// Set every element to null.
				for (int i = 0; i < size; ++i)
					arr.Array.Add(new IronJSONValue(ValueType.Null));
				
				m_cd.Array[(int)key] = arr;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue arr = new IronJSONValue(ValueType.Array);
				
				// Set every element to null.
				for (int i = 0; i < size; ++i)
					arr.Array.Add(new IronJSONValue(ValueType.Null));
				
				m_cd.Obj[(string)key] = arr;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="i">
		/// A <see cref="System.Int32"/>
		/// </param>
		public void SetToInteger(object key, long i)
		{
			if (key is int && m_cd.Type == ValueType.Array)
				m_cd.Array[(int)key] = new IronJSONValue(i);
			else if (key is string && m_cd.Type == ValueType.Object)
				m_cd.Obj[(string)key] = new IronJSONValue(i);
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="f">
		/// A <see cref="System.Double"/>
		/// </param>
		public void SetToFloat(object key, double f)
		{
			if (key is int && m_cd.Type == ValueType.Array)
				m_cd.Array[(int)key] = new IronJSONValue(f);
			else if (key is string && m_cd.Type == ValueType.Object)
				m_cd.Obj[(string)key] = new IronJSONValue(f);
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="str">
		/// A <see cref="System.String"/>
		/// </param>
		public void SetToString(object key, string str)
		{
			if (key is int && m_cd.Type == ValueType.Array)
				m_cd.Array[(int)key] = new IronJSONValue(str);
			else if (key is string && m_cd.Type == ValueType.Object)
				m_cd.Obj[(string)key] = new IronJSONValue(str);
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="b">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public void SetToBoolean(object key, bool b)
		{
			if (key is int && m_cd.Type == ValueType.Array)
				m_cd.Array[(int)key] = new IronJSONValue(b);
			else if (key is string && m_cd.Type == ValueType.Object)
				m_cd.Obj[(string)key] = new IronJSONValue(b);
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		public void SetToNull(object key)
		{
			if (key is int && m_cd.Type == ValueType.Array)
				m_cd.Array[(int)key] = new IronJSONValue(ValueType.Null);
			else if (key is string && m_cd.Type == ValueType.Object)
				m_cd.Obj[(string)key] = new IronJSONValue(ValueType.Null);
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		#endregion
		
		#region GetFrom Functions
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/>
		/// </returns>
		public double GetFloatFrom(object key)
		{
			if (!Exists(key))
				throw new KeyNotFoundException(key.ToString());
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				if (val.Type != ValueType.Float)
					throw new ValueAccessException("Float", val.Type.ToString());
				return val.Float;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Obj[(string)key];
				if (val.Type != ValueType.Float)
					throw new ValueAccessException("Float", val.Type.ToString());
				return val.Float;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Int64"/>
		/// </returns>
		public long GetIntegerFrom(object key)
		{
			if (!Exists(key))
				throw new KeyNotFoundException(key.ToString());
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				if (val.Type != ValueType.Integer)
					throw new ValueAccessException("Integer", val.Type.ToString());
				return val.Integer;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Obj[(string)key];
				if (val.Type != ValueType.Integer)
					throw new ValueAccessException("Integer", val.Type.ToString());
				return val.Integer;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string GetStringFrom(object key)
		{
			if (!Exists(key))
				throw new KeyNotFoundException(key.ToString());
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				if (val.Type != ValueType.String)
					throw new ValueAccessException("String", val.Type.ToString());
				return String.Copy(val.String);
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Obj[(string)key];
				if (val.Type != ValueType.String)
					throw new ValueAccessException("String", val.Type.ToString());
				return String.Copy(val.String);
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool GetBooleanFrom(object key)
		{
			if (!Exists(key))
				throw new KeyNotFoundException(key.ToString());
			if (key is int && m_cd.Type == ValueType.Array)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Array[(int)key];
				if (val.Type != ValueType.True || val.Type != ValueType.False)
					throw new ValueAccessException("Bool", val.Type.ToString());
				
				if (val.Type == ValueType.True)
					return true;
				else
					return false;
			}
			else if (key is string && m_cd.Type == ValueType.Object)
			{
				IronJSONValue val = (IronJSONValue)m_cd.Obj[(string)key];
				if (val.Type != ValueType.True || val.Type != ValueType.False)
					throw new ValueAccessException("Bool", val.Type.ToString());
				
				if (val.Type == ValueType.True)
					return true;
				else
					return false;
			}
			else
				throw new InvalidKeyException(key.ToString());
		}
		
		#endregion
		
		#region Object-defined Functions
		
		override public string ToString()
		{
			return m_obj.ToString();
		}
		
		#endregion
	}
}
