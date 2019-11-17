using Model;
using Model.EnumType;
using Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class UserInfoController : BaseController//Controller
    {
        IBLL.IUserInfoService UserInfoService { get; set; }
        IBLL.IRoleInfoService RoleInfoService { get; set; }

        IBLL.IActionInfoService ActionInfoService { get; set; }
        IBLL.IR_UserInfo_ActionInfoService R_UserInfo_ActionInfoService { get; set; }
        // GET: UserInfo

        #region 用户列表主页
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

        #region 获取用户列表
        public ActionResult GetUserInfoList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            //接受搜索条件
            string userName = Request["name"];
            string userRemark = Request["remark"];
            int totalcount=0;
            //构建搜索条件
            UserInfoSearch userInfoSearch = new UserInfoSearch(){
                UserName=userName,
                UserRemark=userRemark,
                PageIndex=pageIndex,
                PageSize=pageSize,
                TotalCount=totalcount
            };
            short delFlag = (short)DeleteEnumType.Normal;
            //根据构建好的搜索条件完成搜索
            var userInfoList = UserInfoService.LoadSearchEntities(userInfoSearch,delFlag);
            //获取分页数据
            //var userInfoList = userInfoService.LoadPageEntities<int>(pageIndex, pageSize, out totalcount, c => c.DelFlag == delFlag, c => c.ID, true);
            var temp = from u in userInfoList
                       select new
                       {
                           ID = u.ID,
                           UName = u.UName,
                           UPwd = u.UPwd,
                           Remark = u.Remark,
                           SubTime = u.SubTime
                       };
            return Json(new { rows = temp, total =userInfoSearch.TotalCount },JsonRequestBehavior.AllowGet);//这个json 属性名是固定的
        }
        #endregion


        #region 批量删除数据
        public ActionResult DeleteUserInfo(string StrId)
        {
            string[] idArrya = StrId.Split(new char[] { ',' });
            if (UserInfoService.DeleteUserInfos(idArrya))
            {
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }

        }
        #endregion


        #region 添加用户数据
        public ActionResult AddUserInfo(UserInfo userInfo)
        {
            userInfo.DelFlag = 0;
            userInfo.SubTime = DateTime.Now;
            userInfo.ModifiedOn = DateTime.Now;
            if (UserInfoService.AddEntities(userInfo)!=null)
            {
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
            

        }
        #endregion


        #region 展示要修改的数据
        public ActionResult ShowEditInfo()
        {
            int id =Convert.ToInt32(Request["id"]);
            UserInfo userInfo=UserInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            return Json(userInfo,JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 进行数据的修改
        public ActionResult EditUserInfo(UserInfo userInfo)
        {
            userInfo.ModifiedOn = DateTime.Now;
            if (UserInfoService.EditEntities(userInfo))
            {
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
        }
        #endregion



        #region 显示要为用户分配角色
        public ActionResult ShowUserRoleInfo()
        {
            int id =Convert.ToInt32(Request["id"]);
            var userInfo = UserInfoService.LoadEntities(u=>u.ID==id).FirstOrDefault();
            ViewBag.UserInfo = userInfo;
            //查询所有角色
            short delFlag = (short)DeleteEnumType.Normal;
            var allRoleInfo = RoleInfoService.LoadEntities(r=>r.DelFlag==delFlag).ToList();
            //查询一下要分配的用户以前有了那些角色
            var allUserRoleIdList = from r in userInfo.RoleInfo
                                    select r.ID;
            ViewBag.AllRoleList = allRoleInfo.ToList();
            ViewBag.AllUserRoleIdList = allUserRoleIdList.ToList();
            return View();
        }
        #endregion

        #region 为用户分配角色
        public ActionResult SetUserInfo()
        {
            int userId = Convert.ToInt32(Request["userId"]);
            string[] allKeys = Request.Form.AllKeys;//获取请求表单所有name属性的值
            List<int> roleIdList = new List<int>();
            foreach (string key in allKeys)
            {
                if (key.StartsWith("cba_"))
                {
                    string k = key.Replace("cba_", "");
                    roleIdList.Add(int.Parse(k));
                }
            }
            if (UserInfoService.SetUserRoleInfo(userId,roleIdList))//设置用户的角色
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 显示因为用户分配权限的界面
        public ActionResult ShowUserAction()
        {
            int userId = int.Parse(Request["userId"]);
            var userInfo = UserInfoService.LoadEntities(u => u.ID == userId).FirstOrDefault();
            ViewBag.UserInfo = userInfo;
            //获取所有的权限
            short delFlag = (short)DeleteEnumType.Normal;
            var allActionList= ActionInfoService.LoadEntities(a => a.DelFlag == delFlag).ToList();
            //获取要分配的用户已经有的权限
            var allUserActionList = (from a in userInfo.R_UserInfo_ActionInfo
                                   select a).ToList();
            ViewBag.AllActionList = allActionList;
            ViewBag.AllUserActionIdList = allUserActionList;
            return View();
        }
        #endregion

        #region 完成用户权限的分配
        public ActionResult SetUserAction()
        {
            int actionId =int.Parse(Request["actionId"]);
            int userId = int.Parse(Request["userId"]);
            bool isPass = Request["isPass"] == "true" ? true : false;
            if (UserInfoService.SetUserActionInfo(actionId,userId,isPass))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        } 
        #endregion
        /// <summary>
        /// 清除角色权限
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearAction()
        {
            int actionId = int.Parse(Request["actionId"]);
            int userId = int.Parse(Request["userId"]);
            var userAction = (R_UserInfo_ActionInfoService.LoadEntities(r => r.UserInfoID == userId && r.ActionInfoID == actionId)).FirstOrDefault();
            if (userAction!=null)
            {
                if (R_UserInfo_ActionInfoService.DeleteEntites(userAction))
                {
                    return Content("ok:删除成功");
                }
                else
                {
                    return Content("no:删除失败");
                }
            }
            else
            {
                return Content("no:该用户还没有这个权限");
            }
        }
    }
}