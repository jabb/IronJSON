
using System;

namespace IronJSON
{
	public class IronJSONParser
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
			
			if (m_tokenStream.CurrentToken.Type != TokenType.LeftCurlyBracket)
				throw ParseError("expected '{'");
			
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
				if (!m_tokenStream.ToNextToken())
					throw ParseError("expected ':'");
				
				// Parse the value.
				obj[id] = ParseValue();
				
				// Skip the ',' or '}'
				if (!m_tokenStream.ToNextToken())
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
				throw ParseError("expected value, got: " + m_tokenStream.CurrentToken.Type);
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
			return null;
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
		private IronJSONException ParseError(string message)
		{
			return new IronJSONException(message);
		}
	}
	
	internal interface IAbstractSyntaxTree
	{
		void AcceptVisitor(IASTVisitor visitor);
	}
	
	internal interface IASTVisitor
	{
		// List of all the AST classes the visitor can visit.
		void Visit(ASTObject ast);
	}
	
	internal class ASTObject : IAbstractSyntaxTree
	{
		public ASTObject()
		{
			
		}
		
		public void AcceptVisitor(IASTVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}