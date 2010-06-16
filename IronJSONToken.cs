using System;

namespace IronJSON
{
	/// <summary>
	/// Different IronJSONToken types.
	/// </summary>
	public enum Token
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
	
	public class IronJSONToken
	{
		private Token 	m_type;
		private string 	m_string;
		private double 	m_float;
		private long 	m_integer;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">
		/// A <see cref="Type"/>
		/// </param>
		public IronJSONToken(Token type)
		{
			m_type = type;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="str">
		/// A <see cref="String"/>
		/// </param>
		public IronJSONToken(String str) : this(Token.String)
		{
			m_string = str;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="d">
		/// A <see cref="System.Double"/>
		/// </param>
		public IronJSONToken(double f) : this(Token.Float)
		{
			m_float = f;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="l">
		/// A <see cref="System.Int64"/>
		/// </param>
		public IronJSONToken(long i) : this(Token.Integer)
		{
			m_integer = i;
		}
		
		/// <summary>
		/// Returns the type of token.
		/// </summary>
		public Token Type
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
				if (m_type != Token.String)
					throw new IronJSONException("invalid string token retrieval");
				return m_string;
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
				if (m_type != Token.Float)
					throw new IronJSONException("invalid float token retrieval");
				return m_float;
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
				if (m_type != Token.Integer)
					throw new IronJSONException("invalid integer token retrieval");
				return m_integer;
			}
		}
	}
}
