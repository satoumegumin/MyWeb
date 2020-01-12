using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.SSO.Server
{
    public interface IAuthTokenService
    {
        Domain.SSO.Entity.SSOToken ValidateToken(string tokenID);
        bool KeepToken(string tokenID);
    }
}