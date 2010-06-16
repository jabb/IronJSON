
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace IronJSON
{
	/// <summary>
	/// Different value types.
	/// </summary>
	public enum ValueType
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
	public struct ValueData
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
	
	public class IronJSONValue
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
				builder.Append(m_data.strng);
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
					builder.Append(val.ToString());
					builder.Append(",\n");
				}
				if (m_data.arry.Count > 0)
					builder.Remove(builder.Length - 2, 2); // Eat the trailing comma.
				builder.Append("\n]");
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
