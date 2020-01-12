using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domain.SSO.Server
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = this.txtUserName.Text.Trim();
            string password = this.txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                this.lblMessage.Text = "用户名不能为空";
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                this.lblMessage.Text = "密码不能为空";
                return;
            }

            if (Domain.Security.SmartAuthenticate.AuthenticateUser(userName, password, true))
            {
                this.lblMessage.Text = "登录成功";
                string returnUrl = Request["returnUrl"];
                if (string.IsNullOrEmpty(returnUrl))
                    Response.Redirect("default.aspx");
                else
                    Response.Redirect(returnUrl);
            }
            else
            {
                this.lblMessage.Text = "登录失败，请重试";
                return;
            }
        }
    }
}