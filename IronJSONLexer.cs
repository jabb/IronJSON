
using System;

namespace IronJSON
{

	internal class IronJSONLexer
	{
		private string m_string;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public IronJSONLexer(string str)
		{
			m_string = str;
		}
		
		/// <summary>
		/// Returns a token stream from the string.
		/// </summary>
		/// <returns>
		/// A <see cref="IronJSONTokenStream"/>
		/// </returns>
		public IronJSONTokenStream GenerateTokenStream()
		{
			IronJSONTokenStream tokenStream = new IronJSONTokenStream();
			int line = 0;
			
			for (int i = 0; i < m_string.Length; ++i)
			{
				if (m_string[i] == '{')
				{
					tokenStream.Add(new IronJSONToken(TokenType.LeftCurlyBracket));
				}
				else if (m_string[i] == '}')
				{
					tokenStream.Add(new IronJSONToken(TokenType.RightCurlyBracket));
				}
				else if (m_string[i] == '[')
				{
					tokenStream.Add(new IronJSONToken(TokenType.LeftSquareBracket));
				}
				else if (m_string[i] == ']')
				{
					tokenStream.Add(new IronJSONToken(TokenType.RightSquareBracket));
				}
				else if (m_string[i] == ',')
				{
					tokenStream.Add(new IronJSONToken(TokenType.Comma));
				}
				else if (m_string[i] == ':')
				{
					tokenStream.Add(new IronJSONToken(TokenType.Colon));
				}
				// Numbers.
				else if (Char.IsDigit(m_string[i]) || m_string[i] == '-')
				{
					// Get the full length of the number.
					int j = i;
					while (Char.IsDigit(m_string[j]) || m_string[j] == 'e' ||
					       m_string[j] == 'E' || m_string[j] == '.' ||
					       m_string[j] == '+' || m_string[j] == '-')
					{
						j++;
						
						if (j >= m_string.Length)
							break;
					}
					
					// Attempt conversion.
					try
					{
						long l = System.Convert.ToInt64(m_string.Substring(i, j - i));
						tokenStream.Add(new IronJSONToken(l));
						i = j - 1;
					}
					catch
					{
						try
						{
							double d = System.Convert.ToDouble(m_string.Substring(i, j - i));
							tokenStream.Add(new IronJSONToken(d));
							i = j - 1;
						}
						catch
						{
							throw new IronJSONException("bad number on line " + line.ToString() + ": " + m_string[i].ToString());
						}
					}
				}
				// Strings
				else if (m_string[i] == '"')
				{
					string s = "";
					while (true)
					{
						i++;
						if (i >= m_string.Length)
						{
							throw new IronJSONException("unterminated string on line " + line.ToString());
						}
						else if (m_string[i] == '\\')
						{
							i++;
							if (i >= m_string.Length)
							{
								throw new IronJSONException("unterminated string on line " + line.ToString());
							}
							
							s += "\\" + m_string[i];
							// TODO: Convert unicode.
						}
						else if (m_string[i] == '"')
						{
							break;
						}
						else
						{
							s += m_string[i];
						}
					}
					
					// Unescape characters.
					string temp = String.Copy(s);
					temp = temp.Replace("\\\"", "\"");
					temp = temp.Replace("\\\\", "\\");
					temp = temp.Replace("\\/", "/");
					temp = temp.Replace("\\b", "\b");
					temp = temp.Replace("\\f", "\f");
					temp = temp.Replace("\\n", "\n");
					temp = temp.Replace("\\r", "\r");
					temp = temp.Replace("\\t", "\t");
					
					tokenStream.Add(new IronJSONToken(temp));
				}
				// Whitespace, mostly.
				else if (m_string[i] == '\n')
				{
					line++;
				}
				else if (m_string[i] == '\r')
				{
					if (i + 1 < m_string.Length && m_string[i + 1] == '\n')
					{
						i++;
						line++;
					}
					else
					{
						throw new IronJSONException("bad newline on line " + line.ToString());
					}
				}
				else if (Char.IsWhiteSpace(m_string[i]))
				{
				}
				// Used for checking for true, false and null.
				else
				{
					string tmpString = m_string.Substring(i);
					
					if (tmpString.IndexOf("true") == 0)
					{
						tokenStream.Add(new IronJSONToken(TokenType.True));
						i += 3; // Length of "true" minus one. 1 will be added later.
					}
					else if (tmpString.IndexOf("false") == 0)
					{
						tokenStream.Add(new IronJSONToken(TokenType.False));
						i += 4; // Length of "false" minus one. 1 will be added later.
					}
					else if (tmpString.IndexOf("null") == 0)
					{
						tokenStream.Add(new IronJSONToken(TokenType.Null));
						i += 3; // Length of "null" minus one. 1 will be added later.
					}
					else
					{
						throw new IronJSONException("bad character on line " + line.ToString() + ": " + m_string[i].ToString());
					}
				}
			}
			
			return tokenStream;
		}
	}
}
