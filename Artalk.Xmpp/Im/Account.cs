using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Artalk.Xmpp.Im
{
    public class Account : Core.Account
    {       
        public Account(string username, string password, string host) : base(username, password, host)
        {
            Username = username;
            Password = password;
            Host = host;
        }

    }
}
