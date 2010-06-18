using System;
using System.Runtime.InteropServices;

namespace IronJSON
{
	/// <summary>
	/// Different IronJSONToken types.
	/// </summary>
	internal enum TokenType
	{
		// Character tokens.
		LeftCurlyBracket,
		RightCurlyBracket,
		LeftSquareBracket,
		RightSquareBracket,
		Comma,
		Colon,
		
		// Command tokens.
		String,
		Float,
		Integer,
		True,
		False,
		Null
	}
	
	/// <summary>
	/// Union of possible token values.
	/// </summary>
	[System.Runtime.InteropServices.StructLayout(LayoutKind.Explicit)]
	internal struct TokenData
	{
		[System.Runtime.InteropServices.FieldOffset(0)]
		public long intgr;
		[System.Runtime.InteropServices.FieldOffset(0)]
		public double flt;
		// Visual Studio complains and throws an exception when object
		// overlap basic datatypes. So these are set to the offset of
		// the largest basic types (double).
		[System.Runtime.InteropServices.FieldOffset(sizeof(double))]
		public string strng;
	}
	
	internal class IronJSONToken
	{
		private TokenType 	m_type;
		private TokenData	m_data;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">
		/// A <see cref="Type"/>
		/// </param>
		public IronJSONToken(TokenType type)
		{
			m_type = type;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="str">
		/// A <see cref="String"/>
		/// </param>
		public IronJSONToken(String str) : this(TokenType.String)
		{
			m_data.strng = str;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="d">
		/// A <see cref="System.Double"/>
		/// </param>
		public IronJSONToken(double f) : this(TokenType.Float)
		{
			m_data.flt = f;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="l">
		/// A <see cref="System.Int64"/>
		/// </param>
		public IronJSONToken(long i) : this(TokenType.Integer)
		{
			m_data.intgr = i;
		}
		
		/// <summary>
		/// Returns the type of token.
		/// </summary>
		public TokenType Type
		{
			get
			{
				return m_type;
			}
		}
		
		/// <summary>
		/// Returns the string for this token only if this token
		/// is of the type Token.String, otherwise it throws an
		/// IronJSONException.
		/// </summary>
		public string String
		{
			get
			{
				if (m_type != TokenType.String)
					throw new IronJSONException("invalid string token retrieval");
				return m_data.strng;
			}
		}
		
		/// <summary>
		/// Returns the float for this token only if this token
		/// is of the type Token.Float, otherwise it throws an
		/// IronJSONException.
		/// </summary>
		public double Float
		{
			get
			{
				if (m_type != TokenType.Float)
					throw new IronJSONException("invalid float token retrieval");
				return m_data.flt;
			}
		}
		
		/// <summary>
		/// Returns the integer for this token only if this token
		/// is of the type Token.Integer, otherwise it throws an
		/// IronJSONException.
		/// </summary>
		public long Integer
		{
			get
			{
				if (m_type != TokenType.Integer)
					throw new IronJSONException("invalid integer token retrieval");
				return m_data.intgr;
			}
		}
	}
}
