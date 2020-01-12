using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Domain.Security
{
    public class SmartIdentity : IIdentity
    {
        #region IIdentity 成员

        public string AuthenticationType
        {
            get { return "Domain.Authentication"; }
        }

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        #endregion

        public SmartIdentity(string username, bool isAuthenticated)
        {
            _name = username;
            _isAuthenticated = isAuthenticated;
        }

        public SmartIdentity(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            if (!SmartAuthenticate.ValidateUser(username, password)) { return; }

            _isAuthenticated = true;
            _name = username;
        }
    }
}