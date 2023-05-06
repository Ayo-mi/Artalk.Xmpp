using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace Artalk.Xmpp.Core {
	/// <summary>
	/// Represents the base class for XML stanzas as are used by XMPP from which
	/// all implementations must derive.
	/// </summary>
	public abstract class Stanza {
		/// <summary>
		/// The XmlElement containing the actual data.
		/// </summary>
		protected XmlElement element;
        protected XmlDocument data;

        public Jid To
        {
            get
            {
                string v = element.GetAttribute("to");
                return String.IsNullOrEmpty(v) ? null : new Jid(v);
            }
            set
            {
                if (value == null)
                    element.RemoveAttribute("to");
                else
                    element.SetAttribute("to", value.ToString());
            }
        }   

		/// <summary>
		/// Specifies the JID of the sender. If this is null, the stanza was generated
		/// by the client's server.
		/// </summary>
		public Jid From {
			get {
				string v = element.GetAttribute("from");
				return String.IsNullOrEmpty(v) ? null : new Jid(v);
			}
			set {
				if (value == null)
					element.RemoveAttribute("from");
				else
					element.SetAttribute("from", value.ToString());
			}
		}

		public string Username { get; protected set; }

        public string Password { get; protected set; }

        public string Host { get; protected set; }

        /// <summary>
        /// The ID of the stanza, which may be used for internal tracking of stanzas.
        /// </summary>
        public string Id {
			get {
				var v = element.GetAttribute("id");
				return String.IsNullOrEmpty(v) ? null : v;
			}
			set {
				if (value == null)
					element.RemoveAttribute("id");
				else
					element.SetAttribute("id", value);
			}
		}

		/// <summary>
		/// The language of the XML character data if the stanza contains data that is
		/// intended to be presented to a human user.
		/// </summary>
		public CultureInfo Language {
			get {
				string v = element.GetAttribute("xml:lang");
				return String.IsNullOrEmpty(v) ? null : new CultureInfo(v);
			}
			set {
				if (value == null)
					element.RemoveAttribute("xml:lang");
				else
					element.SetAttribute("xml:lang", value.Name);
			}
		}

		/// <summary>
		/// The data of the stanza.
		/// </summary>
		public XmlElement Data {
			get {
				return element;
			}
		}

		/// <summary>
		/// Determines whether the stanza is empty, i.e. has no child nodes.
		/// </summary>
		public bool IsEmpty {
			get {
				return Data.IsEmpty;
			}
		}

		/// <summary>
		/// Initializes a new instance of the Stanza class.
		/// </summary>
		/// <param name="namespace">The xml namespace of the stanza, if any.</param>
		/// <param name="to">The JID of the intended recipient for the stanza.</param>
		/// <param name="from">The JID of the sender.</param>
		/// <param name="id">The ID of the stanza.</param>
		/// <param name="language">The language of the XML character data of
		/// the stanza.</param>
		/// <param name="data">The content of the stanza.</param>
		public Stanza(string @namespace = null, Jid to = null,
			Jid from = null, string id = null, CultureInfo language = null,
			params XmlElement[] data) {
				string name = GetType().Name.ToLowerInvariant();
				element = Xml.Element(name, @namespace);
				To = to;
				From = from;
				Id = id;
				Language = language;
				foreach (XmlElement e in data) {
					if (e != null)
						element.Child(e);
				}
		}
       
        /// <summary>
        /// Initializes a new instance of the Stanza class.
        /// </summary>
        /// <param name="namespace">The xml namespace of the stanza, if any.</param>
        /// <param name="username">New user unique name</param>
        /// <param name="password">New user authentication key</param>
        /// <param name="data">The content of the stanza.</param>
        public Stanza(string @namespace = null, string username = null,
			string password = null, string host = null, params XmlDocument[] data)
        {             
            Username = username;
            Password = password;
			Host = host;

			this.data = new XmlDocument();
			
			data[0] = this.data;
            var iqElement = data[0].CreateElement("iq");
            var queryElement = data[0].CreateElement("query");
					

            XmlAttribute eleAttr = data[0].CreateAttribute("type");
            eleAttr.Value = "set";
            iqElement.Attributes.Append(eleAttr);

            XmlAttribute eleAttr2 = data[0].CreateAttribute("id");
            eleAttr2.Value = "reg2";
            iqElement.Attributes.Append(eleAttr2);

            XmlAttribute eleAttr3 = data[0].CreateAttribute("to");
            eleAttr3.Value = Host;
            iqElement.Attributes.Append(eleAttr3);

            XmlAttribute namespaceAttr = data[0]	.CreateAttribute("xmlns");
            namespaceAttr.Value = "jabber:iq:register";
            queryElement.Attributes.Append(namespaceAttr);

            XmlElement usernameEle = data[0].CreateElement("username");
            usernameEle.InnerText = Username;
            queryElement.AppendChild(usernameEle);

            XmlElement pswEle = data[0].CreateElement("password");
            pswEle.InnerText = Password;
            queryElement.AppendChild(pswEle);
            iqElement.AppendChild(queryElement);
            data[0].AppendChild(iqElement);
            Console.WriteLine(data[0].OuterXml);
        }

        /// <summary>
        /// Initializes a new instance of the Stanza class using the specified
        /// XmlElement.
        /// </summary>
        /// <param name="element">The XmlElement to create the stanza from.</param>
        /// <exception cref="ArgumentNullException">The element parameter is
        /// null.</exception>
        protected Stanza(XmlElement element) {
			element.ThrowIfNull("element");
			this.element = element;
		}

        protected Stanza(XmlDocument element)
        {
            element.ThrowIfNull("element");
            this.data = element;
        }

        /// <summary>
        /// Returns a textual representation of this instance of the Stanza class.
        /// </summary>
        /// <returns>A textual representation of this Stanza instance.</returns>
        public override string ToString() {
			return element.ToXmlString();
		}

        public string XmlDocToString()
        {
            return data.OuterXml;
        }
    }
}
