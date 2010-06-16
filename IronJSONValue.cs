
using System;
using System.Runtime.InteropServices;

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
		public IronJSONValue[] arry;
	}
	
	public class IronJSONValue
	{
		private ValueType m_type;
		private ValueData m_data;
		
		public IronJSONValue()
		{
		}
	}
}
