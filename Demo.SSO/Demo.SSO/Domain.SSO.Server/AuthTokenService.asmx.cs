using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Domain.SSO.Server
{
    /// <summary>
    /// AuthTokenService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class AuthTokenService : System.Web.Services.WebService, IAuthTokenService
    {
        #region IAuthTokenService 成员

        [WebMethod]
        public Entity.SSOToken ValidateToken(string tokenID)
        {
            if (!KeepToken(tokenID))
                return null;

            var token = Domain.SSO.Entity.SSOToken.SSOTokenList.Find(m => m.ID == tokenID);
            return token;
        }

        [WebMethod]
        public bool KeepToken(string tokenID)
        {
            var token = Domain.SSO.Entity.SSOToken.SSOTokenList.Find(m => m.ID == tokenID);
            if (token == null)
                return false;
            if (token.IsTimeOut())
                return false;

            token.AuthTime = DateTime.Now;
            return true;
        }

        #endregion
    }
}
