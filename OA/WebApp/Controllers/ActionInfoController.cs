using Commom;
using Model;
using Model.EnumType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ActionInfoController : BaseController//Controller
    {
        IBLL.IActionInfoService ActionInfoService { get; set; }
        // GET: ActionInfo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 读取权限列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetActionInfoList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;
            short delFlag = (short)DeleteEnumType.Normal;
            var actionInfoList = ActionInfoService.LoadPageEntities<int>(pageIndex, pageSize, out totalCount, a => a.DelFlag == delFlag, r => r.ID, true);
            var temp = from a in actionInfoList
                       select new
                       {
                           ID = a.ID,
                           ActionInfoName = a.ActionInfoName,
                           Sort = a.Sort,
                           SubTime = a.SubTime,
                           Remark = a.Remark,
                           Url = a.Url,
                           ActionTypeEnum = a.ActionTypeEnum,
                           HttpMethod = a.HttpMethod
                       };
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFileUp()
        {
            HttpPostedFileBase file = Request.Files["fileUp"];
            string fileName = Path.GetFileName(file.FileName);
            string fileExe = Path.GetExtension(fileName);
            if (fileExe==".jpg"||fileExe==".png")
            {
                string dir = "/ImageIcon/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dir)));
                string newFileName = Guid.NewGuid().ToString();
                string fullDir = dir + newFileName + fileExe;
                file.SaveAs(Request.MapPath(fullDir));
                string smallDir = dir + "_small" + newFileName + fileExe;
                ImageClass.MakeThumbnail(Request.MapPath(fullDir), Request.MapPath(smallDir),30,30,"H");
                return Content("ok:"+smallDir);
            }
            else
            {
                return Content("no:文件类型错误");
            }
        }

        #region 完成权限的添加
        public ActionResult AddActionInfo(ActionInfo actionInfo)
        {
            actionInfo.DelFlag = 0;
            actionInfo.ModifiedOn = DateTime.Now.ToString();
            actionInfo.SubTime = DateTime.Now;
            actionInfo.Url = actionInfo.Url.ToLower();
            ActionInfoService.AddEntities(actionInfo);
            return Content("ok");
        } 
        #endregion
    }
}