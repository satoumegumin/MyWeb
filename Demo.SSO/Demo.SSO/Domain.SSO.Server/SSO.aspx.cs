using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domain.SSO.Server
{
    public partial class SSO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string returnUrl = Request["returnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
            {
                return;
            }
            else
            {
                //判断returnUrl是否为信任的Domain
            }

            //判断当前是否登录
            if (Domain.Security.SmartAuthenticate.LoginUser != null)
            {
                //生成Token，并持久化Token
                Domain.SSO.Entity.SSOToken token = new Entity.SSOToken();
                
                token.User = new Entity.SSOUser();
                token.User.UserName = Domain.Security.SmartAuthenticate.LoginUser.UserName;
                token.LoginID = Session.SessionID;
                Domain.SSO.Entity.SSOToken.SSOTokenList.Add(token);

                //拼接返回的url，参数中带Token
                string spliter = returnUrl.Contains('?') ? "&" : "?";
                returnUrl = returnUrl + spliter + "token=" + token.ID;
                Response.Redirect(returnUrl);
            }
            else
            {
                //重定向到登录页面
                string loginUrl = "login.aspx?returnUrl=";
                string loginReturnUrl = string.Format("sso.aspx?returnUrl=" + returnUrl);
                loginUrl += Server.UrlEncode(loginReturnUrl);
                Response.Redirect(loginUrl);
            }
        }
    }
}