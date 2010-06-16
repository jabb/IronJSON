
using System;
using System.Collections.Generic;

namespace IronJSON
{
	public class IronJSONTokenStream
	{
		private List<IronJSONToken> m_tokens;
		private int m_index;
		
		/// <summary>
		/// 
		/// </summary>
		public IronJSONTokenStream()
		{
			m_tokens = new List<IronJSONToken>();
			m_index = 0;
		}
		
		/// <summary>
		/// Returns true if the position is at the end of the stream. The
		/// only token available is PreviousToken.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool AtEnd()
		{
			return m_index == m_tokens.Count;
		}
		
		/// <summary>
		/// Resets the position to point back at the beginning.
		/// </summary>
		public void Reset()
		{
			m_index = 0;
		}
		
		/// <summary>
		/// Adds a token to the stream.
		/// </summary>
		/// <param name="token">
		/// A <see cref="IronJSONToken"/>
		/// </param>
		public void Add(IronJSONToken token)
		{
			m_tokens.Add(token);
		}
		
		/// <summary>
		/// Returns the current token.
		/// </summary>
		public IronJSONToken CurrentToken
		{
			get
			{
				return m_tokens[m_index];
			}
		}
		
		
		/// <summary>
		/// Increments the stream position. Returns false when at 
		/// the end of the stream.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool ToNextToken()
		{
			m_index++;
			
			return !AtEnd();
		}
		
		/// <summary>
		/// Returns the previous token. Keeps the position the same.
		/// </summary>
		public IronJSONToken PreviousToken
		{
			get
			{
				return m_tokens[m_index - 1];
			}
		}
	}
}
