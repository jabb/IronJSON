
using System;
using System.Collections;
using System.Text;

namespace IronJSON
{
	internal class IronJSONObject
	{
		Hashtable m_table;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public IronJSONObject()
		{
			m_table = new Hashtable();
		}
		
		/// <summary>
		/// Returns true if the key exists.
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool ContainsKey(string key)
		{
			return m_table.ContainsKey(key);
		}
		
		/// <value>
		/// Gets/sets dictionary entries. Returns null if the
		/// entry doesn't exist.
		/// </value>
		public IronJSONValue this[string key]
		{
			get
			{
				if (m_table.Contains(key))
					return (IronJSONValue)m_table[key];
				else
					return null;
			}
			set
			{
				m_table[key] = value;
			}
		}
		
		/// <summary>
		/// Converts this object to a string to be written out to a
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
		/// Converts this object to a string to be written out to a
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
			// TODO: Fix indentation.
			StringBuilder builder = new StringBuilder();
			IronJSONValue currentValue;
			
			builder.Append("{\n");
			
			foreach (DictionaryEntry de in m_table)
			{
				// TODO: Convert newlines and such to escape sequences.
				for (int i = 0; i < indentLevel + 1; i++ )
				{
					builder.Append("\t");	
				}				
				builder.Append("\"");
				// Escape certain characters.
				string temp = String.Copy((string)de.Key);
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
				builder.Append(": ");			
				currentValue = de.Value as IronJSONValue;
				builder.Append(currentValue.ToString(indentLevel+1));
			}
			
			if (m_table.Count > 0)
				builder.Remove(builder.Length - 2, 2); // Eat the trailing comma.
			builder.Append("\n");
			for (int i = 0; i < indentLevel; i++ )
			{
				builder.Append("\t");
			}			
			builder.Append("}");
			
			return builder.ToString();
		}
	}
}
