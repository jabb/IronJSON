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
		Null,
		
		// The None token. Doesn't match anything.
		Nothing,
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
		/// <summary>
		/// Static method for converting token types to a readable format.
		/// </summary>
		/// <param name="type">
		/// A <see cref="TokenType"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public static string TokenTypeToString(TokenType type)
		{
			switch (type)
			{
			case TokenType.LeftCurlyBracket:
				return "{";
			case TokenType.RightCurlyBracket:
				return "}";
			case TokenType.LeftSquareBracket:
				return "[";
			case TokenType.RightSquareBracket:
				return "]";
			case TokenType.Comma:
				return ",";
			case TokenType.Colon:
				return ":";
			case TokenType.String:
				return "string";
			case TokenType.Float:
				return "number";
			case TokenType.Integer:
				return "number";
			case TokenType.True:
				return "bool";
			case TokenType.False:
				return "bool";
			case TokenType.Null:
				return "null";
			case TokenType.Nothing:
				return "nothing";
			default:
				return "unknown";
			}
		}
		
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
					throw new JSONException("invalid string token retrieval");
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
					throw new JSONException("invalid float token retrieval");
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
					throw new JSONException("invalid integer token retrieval");
				return m_data.intgr;
			}
		}
		
		override public string ToString()
		{
			switch (m_type)
			{
			case TokenType.Colon:
				return "':'";
			case TokenType.Comma:
				return "','";
			case TokenType.False:
				goto case TokenType.True;
			case TokenType.True:
				return "bool";
			case TokenType.Float:
				return m_data.flt.ToString();
			case TokenType.Integer:
				return m_data.intgr.ToString();
			case TokenType.LeftCurlyBracket:
				return "'{'";
			case TokenType.LeftSquareBracket:
				return "'['";
			case TokenType.Nothing:
				return "nothing";
			case TokenType.Null:
				return "null";
			case TokenType.RightCurlyBracket:
				return "'}'";
			case TokenType.RightSquareBracket:
				return "']'";
			case TokenType.String:
				return "\"" + m_data.strng + "\"";
			default:
				return "unknown";
			}
		}
	}
}
