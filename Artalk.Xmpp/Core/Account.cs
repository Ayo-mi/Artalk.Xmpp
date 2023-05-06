using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Artalk.Xmpp.Core
{
    public class Account : Stanza
    {
        /// <summary>
		/// Initializes a new instance of the Message class.
		/// </summary>
		/// <param name="username">The JID of the intended recipient for the stanza.</param>
		/// <param name="password">The JID of the sender.</param>
		/// <param name="data">The content of the stanza.</param>		
		/// the stanza.</param>
		public Account(string username, string password, string host = null, XmlDocument data = null)
            : base(null, username, password, host, data )
        {
        }

        /// <summary>
		/// Initializes a new instance of the Message class from the specified
		/// Xml element.
		/// </summary>
		/// <param name="element">An Xml element representing an Message stanza.</param>
		/// <exception cref="ArgumentNullException">The element parameter is null.</exception>
		public Account(XmlDocument element)
            : base(element)
        {
        }
    }
}
