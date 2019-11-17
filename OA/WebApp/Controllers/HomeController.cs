using Model;
using Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController :BaseController // Controller
    {
        IBLL.IUserInfoService UserInfoService { get; set; }
        // GET: Home
        public ActionResult Index()
        {
            ViewData["name"] = LoginUser.UName;
            return View();
        }

        public ActionResult HomePage()
        {
            return View();
        }

        #region 过滤登录用户的菜单权限
        /// <summary>
        /// 1.可以按照用户 角色 权限这条线找出登录用户的权限 放在一个集合中
        /// 2 可以按照用户 权限这条线找出用户的权限 放在另一个集合中
        /// 3 将这两个集合合并成一个集合
        /// 4 把禁止的权限从集合中清除
        /// 5 把过滤好的集合中的重复权限清除
        /// 6 把过滤好的菜单权限生成JSOn返回
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenu()
        {
            //按照第一条线
            //获取登录用户的信息
            var userInfo = UserInfoService.LoadEntities(u=>u.ID==LoginUser.ID).FirstOrDefault();
            //获取登录用户的角色
            var userRoleInfo = userInfo.RoleInfo;
            short actionTypeEnum = (short)ActionTypeEnum.MenuActionType;
            var loginUserMenuActions = (from r in userRoleInfo
                                        from a in r.ActionInfo
                                        where a.ActionTypeEnum == actionTypeEnum
                                        select a).ToList();
            //按照第二条线
            var userActions = from a in userInfo.R_UserInfo_ActionInfo
                              select a.ActionInfo;
            var userMenuActions = (from a in userActions
                                   where a.ActionTypeEnum == actionTypeEnum
                                   select a).ToList();
            loginUserMenuActions.AddRange(userMenuActions);
            //4 把禁止的权限从集合中清除
            var fobidActions = (from a in userInfo.R_UserInfo_ActionInfo
                                where a.IsPass == false
                                select a.ActionInfoID).ToList();
            var loginUserAllowActions=loginUserMenuActions.Where(a=>(!fobidActions.Contains(a.ID)));
            //5 将集合中重复的权限删除
            var lastLoginUserActions=loginUserAllowActions.Distinct<ActionInfo>(new EqualityComparer());//去除重复的元素
            //6 生成JSON返回
            var temp = from a in lastLoginUserActions
                       select new { icon =a.MenuIcon, title = a.ActionInfoName, url = a.Url };
            return Json(temp,JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}