using Commom;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        IBLL.IUserInfoService UserInfoService { get; set; }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        #region 验证码
        public ActionResult ShowValidateCode()
        {
            ValidateCode validateCode = new ValidateCode();
            string code=validateCode.CreateValidateCode(6);
            Session["validateCode"] = code;
            byte[]buffer=validateCode.CreateValidateGraphic(code);
            return File(buffer,"image/jpeg");
        }
        #endregion


        #region 用户登录
        public ActionResult UserLogin()
        {
            string validateCode = Session["validateCode"] != null ? Session["validateCode"].ToString() : string.Empty;
            string userValidateCode = Request["vCode"];
            if (string.IsNullOrEmpty(validateCode))
            {
                return Content("no:验证码错误");
            }
            Session["validateCode"] = null;
            if (!validateCode.Equals(userValidateCode ,StringComparison.InvariantCultureIgnoreCase))
            {
                return Content("no:验证码错误");
            }
            string userName = Request["LoginCode"];
            string userPwd = Request["LoginPwd"];
            var userInfo= UserInfoService.LoadEntities(u=>u.UName==userName).FirstOrDefault();//根据用户名找用户
            if (userInfo!=null)
            {
                if (userInfo.UPwd==userPwd)
                {
                    Session["userInfo"] = userInfo;
                    //有个问题 存到session的问题 如果部署到多台服务器上
                    //产生一个GUID值作为Memache的键.
                    string sesId = Guid.NewGuid().ToString();
                    MemcacheHelper.Set(sesId, SerializeHelper.SerializeToString(userInfo), DateTime.Now.AddMinutes(20));//将登陆信息存储到memcache中
                    Response.Cookies["sesId"].Value = sesId;//将key以cookie的形式返回给浏览器

                    

                    return Content("ok:登录成功");
                }
                else
                {
                    return Content("no:密码错误,请重新输入");
                }

            }
            else
            {
                return Content("no:用户名不存在");
            }

        } 
        #endregion
    }
}