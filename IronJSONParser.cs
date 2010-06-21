
using System;
using System.Text;

namespace IronJSON
{
	internal class IronJSONParser
	{
		private IronJSONTokenStream m_tokenStream;
		private IronJSONObject m_obj; // The final product after parsing.
		
		public IronJSONParser(IronJSONTokenStream tokenStream)
		{
			m_tokenStream = tokenStream;
			m_obj = null;
		}
		
		/// <summary>
		/// Parse the token stream.
		/// </summary>
		public void Parse()
		{
			m_obj = ParseObject();
			
			// Reset for future calculations.
			m_tokenStream.Reset();
		}
		
		/// <summary>
		/// Returns the result of parsing. Or null if nothing was ever
		/// parsed.
		/// </summary>
		public IronJSONObject Obj
		{
			get
			{
				return m_obj;
			}
		}
		
		/// <summary>
		/// Parse an object.
		/// </summary>
		private IronJSONObject ParseObject()
		{
			IronJSONObject obj = new IronJSONObject();
			
			VerifyToken(m_tokenStream.CurrentToken,
			            new TokenType[]{TokenType.LeftCurlyBracket});
			
			// Skip the '{'
			if (!m_tokenStream.ToNextToken())
				throw ParseError("expected '}'");
			
			while (!m_tokenStream.AtEnd() &&
			       m_tokenStream.CurrentToken.Type == TokenType.String)
			{
				// Save the string.
				string id = m_tokenStream.CurrentToken.String;
				m_tokenStream.ToNextToken(); // Skip the id.
				
				// Skip the ':'
				VerifyToken(m_tokenStream.CurrentToken, new TokenType[]{TokenType.Colon});
				if (!m_tokenStream.ToNextToken())
					throw ParseError("expected ':'");
				
				// Parse the value.
				obj[id] = ParseValue();
				
				// Skip the ',' or '}'. Error if it's not either.
				VerifyToken(m_tokenStream.CurrentToken, 
				            new TokenType[]{TokenType.Comma, TokenType.RightCurlyBracket});
				if (!m_tokenStream.ToNextToken())
					break;
				
				// If what we skipped what a '}', break.
				if (m_tokenStream.PreviousToken.Type == TokenType.RightCurlyBracket)
					break;
			}
			
			return obj;
		}
		
		/// <summary>
		/// Parse a value.
		/// </summary>
		/// <returns>
		/// A <see cref="IronJSONValue"/>
		/// </returns>
		private IronJSONValue ParseValue()
		{
			switch (m_tokenStream.CurrentToken.Type)
			{
			case TokenType.String:
				m_tokenStream.ToNextToken();
				return new IronJSONValue(m_tokenStream.PreviousToken.String);
				
			case TokenType.Float:
				m_tokenStream.ToNextToken();
				return new IronJSONValue(m_tokenStream.PreviousToken.Float);
				
			case TokenType.Integer:
				m_tokenStream.ToNextToken();
				return new IronJSONValue(m_tokenStream.PreviousToken.Integer);
				
			case TokenType.True:
				m_tokenStream.ToNextToken();
				return new IronJSONValue(ValueType.True);
				
			case TokenType.False:
				m_tokenStream.ToNextToken();
				return new IronJSONValue(ValueType.False);
				
			case TokenType.Null:
				m_tokenStream.ToNextToken();
				return new IronJSONValue(ValueType.Null);
				
			case TokenType.LeftCurlyBracket:
				return new IronJSONValue(ParseObject());
				
			case TokenType.LeftSquareBracket:
				return ParseList();
				
			default:
				throw ParseError("expected value got: " +
				                 IronJSONToken.TokenTypeToString(m_tokenStream.CurrentToken.Type));
			}
		}
		
		/// <summary>
		/// Parses a list.
		/// </summary>
		/// <returns>
		/// A <see cref="IronJSONValue"/>
		/// </returns>
		private IronJSONValue ParseList()
		{
			IronJSONValue val = new IronJSONValue(ValueType.Array);
			
			VerifyToken(m_tokenStream.CurrentToken,
			            new TokenType[]{TokenType.LeftSquareBracket});
			
			// Skip the '['
			if (!m_tokenStream.ToNextToken())
				throw ParseError("expected ']'");
			
			while (!m_tokenStream.AtEnd())
			{
				// If what we hit a ']', break.
				if (m_tokenStream.CurrentToken.Type == TokenType.RightSquareBracket)
				{
					m_tokenStream.ToNextToken();
					break;
				}
				
				// Parse the value.
				val.Array.Add(ParseValue());
				
				// Skip the ',' or ']'. Error if it's not either.
				VerifyToken(m_tokenStream.CurrentToken, 
				            new TokenType[]{TokenType.Comma, TokenType.RightSquareBracket});
				if (!m_tokenStream.ToNextToken())
					break;
				
				// If what we hit a ']', break.
				if (m_tokenStream.PreviousToken.Type == TokenType.RightSquareBracket)
					break;
			}
			
			return val;
		}
		
		private void VerifyToken(IronJSONToken tok, TokenType[] verify)
		{
			StringBuilder errormsg = new StringBuilder("expected ");
			bool verified = false;
			
			foreach (TokenType t in verify)
			{
				errormsg.Append("'" + IronJSONToken.TokenTypeToString(t) + "' or ");
				if (tok.Type == t)
					verified = true;
			}
			
			if (verify.Length > 0)
				errormsg.Remove(errormsg.Length - 4, 4);
			
			errormsg.Append(" got: " + tok.ToString());
			
			if (!verified)
				throw ParseError(errormsg.ToString());
		}
		
		/// <summary>
		/// Returns a parser error.
		/// </summary>
		/// <param name="message">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="IronJSONException"/>
		/// </returns>
		private JSONException ParseError(string message)
		{
			return new JSONException("line " + m_tokenStream.GetLineNumber() + ": " + message);
		}
	}
}
