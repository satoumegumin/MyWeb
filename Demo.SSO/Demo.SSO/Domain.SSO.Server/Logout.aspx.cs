using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domain.SSO.Server
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Domain.Security.SmartAuthenticate.SignOut();
            //删除相关的SSOToken
            Domain.SSO.Entity.SSOToken.SSOTokenList.RemoveAll(m => m.LoginID == Session.SessionID);

            string returnUrl = Request["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
                Response.Redirect(returnUrl);
            else
                Response.Redirect("default.aspx");
        }
    }
}