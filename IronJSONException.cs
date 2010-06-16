
using System;

namespace IronJSON
{
	/// <summary>
	/// Custon Iron JSON exception. Used for everything in the library.
	/// </summary>
	public class IronJSONException : Exception
	{
		/// <summary>
		/// Pass a message to the exception.
		/// </summary>
		/// <param name="message">
		/// A <see cref="System.String"/>
		/// </param>
		public IronJSONException(string message) : base(message)
		{
		}
	}
}
