
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace IronJSON
{
	/// <summary>
	/// Different value types.
	/// </summary>
	internal enum ValueType
	{
		String,
		Integer,
		Float,
		Object,
		Array,
		True,
		False,
		Null
	}
	
	/// <summary>
	/// Union of value data.
	/// </summary>
	[System.Runtime.InteropServices.StructLayout(LayoutKind.Explicit)]
	internal struct ValueData
	{
		[System.Runtime.InteropServices.FieldOffset(0)]
		public string strng;
		[System.Runtime.InteropServices.FieldOffset(0)]
		public long intgr;
		[System.Runtime.InteropServices.FieldOffset(0)]
		public double flt;
		[System.Runtime.InteropServices.FieldOffset(0)]
		public IronJSONObject objct;
		[System.Runtime.InteropServices.FieldOffset(0)]
		public ArrayList arry;
	}
	
	internal class IronJSONValue
	{
		private ValueType m_type;
		private ValueData m_data;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">
		/// A <see cref="ValueType"/>
		/// </param>
		public IronJSONValue(ValueType type)
		{
			m_type = type;
			
			if (m_type == ValueType.Array)
			{
				m_data.arry = new ArrayList();
			}
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="str">
		/// A <see cref="System.String"/>
		/// </param>
		public IronJSONValue(string str) : this(ValueType.String)
		{
			m_data.strng = str;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="i">
		/// A <see cref="System.Int64"/>
		/// </param>
		public IronJSONValue(long i) : this(ValueType.Integer)
		{
			m_data.intgr = i;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="f">
		/// A <see cref="System.Double"/>
		/// </param>
		public IronJSONValue(double f) : this(ValueType.Float)
		{
			m_data.flt = f;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="o">
		/// A <see cref="IronJSONObject"/>
		/// </param>
		public IronJSONValue(IronJSONObject o) : this(ValueType.Object)
		{
			m_data.objct = o;
		}
		
		/// <value>
		/// Returns the type of value this is.
		/// </value>
		public ValueType Type
		{
			get
			{
				return m_type;
			}
		}
		
		/// <value>
		/// 
		/// </value>
		public string String
		{
			get
			{
				if (m_type != ValueType.String)
					throw new IronJSONException("invalid string value retrieval");
				return m_data.strng;
			}
		}
		
		/// <value>
		/// 
		/// </value>
		public long Integer
		{
			get
			{
				if (m_type != ValueType.Integer)
					throw new IronJSONException("invalid integer value retrieval");
				return m_data.intgr;
			}
		}
		
		/// <value>
		/// 
		/// </value>
		public double Float
		{
			get
			{
				if (m_type != ValueType.Float)
					throw new IronJSONException("invalid float value retrieval");
				return m_data.flt;
			}
		}
		
		/// <value>
		/// 
		/// </value>
		public IronJSONObject Obj
		{
			get
			{
				if (m_type != ValueType.Object)
					throw new IronJSONException("invalid object value retrieval");
				return m_data.objct;
			}
		}
		
		/// <value>
		/// 
		/// </value>
		public ArrayList Array
		{
			get
			{
				if (m_type != ValueType.Array)
					throw new IronJSONException("invalid array value retrieval");
				return m_data.arry;
			}
		}
		
		/// <summary>
		/// Converts this value to a string to be written out to a
		/// JSON file.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		override public string ToString()
		{
			return ToString(0);
		}
		
		/// <summary>
		/// Converts this value to a string to be written out to a
		/// JSON file.
		/// </summary>
		/// <param name="indentLevel">
		/// A <see cref="System.Int32"/> which indicates the indentation
		/// level to use.
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string ToString(int indentLevel)
		{
			StringBuilder builder = new StringBuilder();
			
			if (m_type == ValueType.String)
			{
				builder.Append("\"");
				// Escape certain characters.
				string temp = String.Copy(m_data.strng);
				temp = temp.Replace("\"", "\\\"");
				temp = temp.Replace("\\", "\\\\");
				temp = temp.Replace("/", "\\/");
				temp = temp.Replace("\b", "\\b");
				temp = temp.Replace("\f", "\\f");
				temp = temp.Replace("\n", "\\n");
				temp = temp.Replace("\r", "\\r");
				temp = temp.Replace("\t", "\\t");
				builder.Append(temp);
				
				builder.Append("\"");
			}
			else if (m_type == ValueType.Integer)
			{
				builder.Append(System.Convert.ToString(m_data.intgr));
			}
			else if (m_type == ValueType.Float)
			{
				builder.Append(System.Convert.ToString(m_data.flt));
			}
			else if (m_type == ValueType.Object)
			{
				builder.Append(m_data.objct.ToString(indentLevel));
			}
			else if (m_type == ValueType.Array)
			{
				builder.Append("[\n");
				foreach (IronJSONValue val in m_data.arry)
				{
					for (int i = 0; i < indentLevel + 1; i++ )
					{
						builder.Append("\t");	
					}	
					builder.Append(val.ToString());
				}
				if (m_data.arry.Count > 0)
					builder.Remove(builder.Length - 2, 2); // Eat the trailing comma.
				builder.Append("\n");
				for (int i = 0; i < indentLevel; i++ )
				{
					builder.Append("\t");
				}	
				builder.Append("]");
			}
			else if (m_type == ValueType.True)
			{
				builder.Append("true");
			}
			else if (m_type == ValueType.False)
			{
				builder.Append("false");
			}
			else if (m_type == ValueType.Null)
			{
				builder.Append("null");
			}
			
			builder.Append(",\n");
			
			return builder.ToString();
		}
	}
}
