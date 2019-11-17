using Model;
using Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class RoleInfoController : BaseController//Controller
    {
        IBLL.IRoleInfoService RoleInfoService { get; set; }
        IBLL.IActionInfoService ActionInfoService { get; set; }
        // GET: RoleInfo
        public ActionResult Index()
        {
            return View();
        }

        #region 读取角色列表

        public ActionResult GetRoleInfoList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;
            short delFlag = (short)DeleteEnumType.Normal;
            var roleInfoList = RoleInfoService.LoadPageEntities<int>(pageIndex, pageSize, out totalCount, r => r.DelFlag == delFlag, r => r.ID, true);
            var temp = from r in roleInfoList
                       select new
                       {
                           Remark = r.Remark,
                           ID = r.ID,
                           Sort = r.Sort,
                           SubTime = r.SubTime,
                           RoleName = r.RoleName
                       };
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /// <summary>
        /// 返回添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowAddRoleInfo()
        {
            return View();
        }
        /// <summary>
        /// 完成角色的添加
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public ActionResult AddRoleInfo(RoleInfo roleInfo)
        {
            roleInfo.DelFlag = (short)DeleteEnumType.Normal;
            roleInfo.ModifiedOn = DateTime.Now;
            roleInfo.SubTime = DateTime.Now;
            RoleInfoService.AddEntities(roleInfo);
            return Content("ok");
        }

        /// <summary>
        /// 显示用户权限信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowRoleAction()
        {
            int id = int.Parse(Request["id"]);
            var roleInfo = RoleInfoService.LoadEntities(r => r.ID == id).FirstOrDefault();//获取要分配的权限的角色信息。
            ViewBag.RoleInfo = roleInfo;
            //获取所有的权限。
            short delFlag = (short)DeleteEnumType.Normal;
            var actionInfoList = ActionInfoService.LoadEntities(a => a.DelFlag == delFlag).ToList();
            //要分配权限的角色以前有哪些权限。
            var actionIdList = (from a in roleInfo.ActionInfo
                                select a.ID).ToList();
            ViewBag.ActionInfoList = actionInfoList;
            ViewBag.ActionIdList = actionIdList;
            return View();
        }
        /// <summary>
        /// 完成角色的权限分配
        /// </summary>
        /// <returns></returns>
        public ActionResult SetRoleActionInfo()
        {
            int roleId = int.Parse(Request["roleId"]);//获取角色编号
            string[] allkeys = Request.Form.AllKeys;//获取所有表单的name属性的值
            List<int> list = new List<int>();
            foreach (string key in allkeys)
            {
                if (key.StartsWith("cba_"))
                {
                    string k = key.Replace("cba_","");
                    list.Add(Convert.ToInt32(k));
                }
            }
            if (RoleInfoService.SetRoleActionInfo(roleId,list))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
    }
}