using Commom;
using IBLL;
using Model;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        public UserInfo LoginUser = null;
        

        //执行控制器的方法之前先执行该方法
        //这是另外一种使用方法过滤器的方法
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);
            bool isSus = false;
            if (Request.Cookies["sesId"] != null)
            {
                //filterContext.HttpContext.Response.Redirect("/Login/Index"); //必须要拿到一个actionresult 如果用这个方法 还会往下走
                //没有返回result 会继续走 
                //filterContext.Result = Redirect("/Login/Index");
                string sesId = Request.Cookies["sesId"].Value;
                object obj = MemcacheHelper.Get(sesId);
                if (obj!=null)
                {
                    UserInfo userInfo = SerializeHelper.DeserializeToObject<UserInfo>(obj.ToString());
                    LoginUser = userInfo;
                    isSus = true;
                    MemcacheHelper.Set(sesId,obj,DateTime.Now.AddMinutes(20));//模拟滑动过期时间
                    //先留个后门方便测试 这个用户登录的话 后面的都不走了 项目做完了 这个要删除掉
                    if (LoginUser.UName=="326209")
                    {
                        return;
                    }


                    //完成权限校验
                    //获取用户请求的URL地址
                    string url = Request.Url.AbsolutePath;
                    //获取请求方式
                    string httpMethod = Request.HttpMethod;
                    //根据获取的url地址与请求方式查看用户是否有访问权限
                    IApplicationContext ctx = ContextRegistry.GetContext();
                    IUserInfoService userInfoService =(IUserInfoService)ctx.GetObject("UserInfoService");
                    IActionInfoService actionInfoService = (IActionInfoService)ctx.GetObject("ActionInfoService");

                    var actionInfo= actionInfoService.LoadEntities(a=>a.HttpMethod==httpMethod&&a.Url==url.ToLower()).FirstOrDefault();
                    var loginUserInfo = userInfoService.LoadEntities(u=>u.ID==LoginUser.ID).FirstOrDefault();
                    //先按照用户权限这条线进行过滤
                    var isExe = (from a in loginUserInfo.R_UserInfo_ActionInfo
                                 where a.ActionInfoID == actionInfo.ID
                                 select a).FirstOrDefault();
                    if (isExe!=null)
                    {
                        if (isExe.IsPass)
                        {
                            return;
                        }
                        else
                        {
                            filterContext.Result = Redirect("/error.html");
                            return;
                        }
                    }
                    else
                    {
                        //按照第二条线过滤
                        var loginRole = loginUserInfo.RoleInfo;
                        var count = (from r in loginRole
                                    from a in r.ActionInfo
                                    where a.ID == actionInfo.ID
                                    select a).Count();
                        if (count<1)
                        {
                            filterContext.Result = Redirect("/error.html");
                            return;
                        }
                    }
                }
            }
            if (isSus==false)
            {
                filterContext.Result = Redirect("/Login/Index");
            }
        }
    }
}