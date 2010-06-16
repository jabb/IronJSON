
using System;
using System.Collections;
using System.Text;

namespace IronJSON
{
	public class IronJSONObject
	{
		Hashtable m_table;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public IronJSONObject()
		{
			m_table = new Hashtable();
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
			// TODO: Fix indentation.
			StringBuilder builder = new StringBuilder();
			
			builder.Append("{\n");
			
			foreach (DictionaryEntry de in m_table)
			{
				// TODO: Convert newlines and such to escape sequences.
				builder.Append("\"");
				builder.Append(de.Key);
				builder.Append("\"");
				builder.Append(": ");
				builder.Append(de.Value.ToString());
			}
			
			if (m_table.Count > 0)
				builder.Remove(builder.Length - 2, 2); // Eat the trailing comma.
			builder.Append("\n}");
			
			return builder.ToString();
		}
	}
}
