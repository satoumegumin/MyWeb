using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Domain.Security
{
    public class SmartPrincipal : IPrincipal
    {
        #region IPrincipal 成员

        private IIdentity _identity;
        public IIdentity Identity
        {
            get { return _identity; }
        }


        public bool IsInRole(string roleName)
        {
            if (Identity == null || !Identity.IsAuthenticated || string.IsNullOrEmpty(Identity.Name))
                return false;

            return false;
        }

        #endregion

        public SmartPrincipal(IIdentity identity)
        {
            _identity = identity;
        }
    }
}