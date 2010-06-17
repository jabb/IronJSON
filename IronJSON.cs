using System;
using System.IO;

namespace IronJSON
{
	/// <summary>
	/// Loads/reads/modifies/saves JSON objects.
	/// </summary>
	public class IronJSON
	{
		// The JSON object.
		private IronJSONObject m_obj;
		
		/// <summary>
		/// Opens a JSON file.
		/// </summary>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		public IronJSON(string filename)
		{
			this.Open(filename);
		}
		
		/// <summary>
		/// Constructs an empty JSON object.
		/// </summary>
		public IronJSON()
		{
			m_obj = new IronJSONObject();
		}
		
		/// <summary>
		/// Opens a new file to be read. Overwrite the current
		/// JSON object.
		/// </summary>
		public void Open(string filename)
		{
			StreamReader reader = new StreamReader("sample.json");
			IronJSONLexer lexer;
			IronJSONTokenStream stream;
			IronJSONParser parser;
			
			lexer = new IronJSONLexer(reader.ReadToEnd());
			
			stream = lexer.GenerateTokenStream();
			
			parser = new IronJSONParser(stream);
			
			parser.Parse();
			m_obj = parser.Obj;
			
			reader.Close();
		}
		
		/// <summary>
		/// Saves the current JSON object to file.
		/// </summary>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		public void Save(string filename)
		{
			StreamWriter writer = new StreamWriter(filename);
			
			writer.WriteLine(m_obj.ToString());
			
			writer.Close();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="keys">
		/// A <see cref="System.Object"/>
		/// </param>
		public void SetInteger(int i, string key, params object[] keys)
		{
			
		}
	}
}
