using IBLL;
using Model;
using Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public partial class UserInfoService : BaseService<UserInfo>,IUserInfoService
    {
      
        /// <summary>
        /// 批量删除多条用户数据
        /// </summary>
        /// <param name="idArray"></param>
        /// <returns></returns>
        public bool DeleteUserInfos(string[] idArray)
        {
            List<int> list = new List<int>();
            foreach (string Id in idArray)
            {
                list.Add(Convert.ToInt32(Id));
            }
            var userInfoList = this.CurrentDBSession.UserInfoDal.LoadEntities(u=>list.Contains(u.ID));
            foreach (var userInfo in userInfoList)
            {
                this.CurrentDBSession.UserInfoDal.DeleteEntites(userInfo);
            }
            return this.CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 完成用户信息的搜索
        /// </summary>
        /// <param name="userInfoSearch"></param>
        /// <param name="delFlag"></param>
        /// <returns></returns>
        public IQueryable<UserInfo> LoadSearchEntities(UserInfoSearch userInfoSearch, short delFlag)
        {
            var temp = this.CurrentDBSession.UserInfoDal.LoadEntities(c=>c.DelFlag==delFlag);
            //根据用户名来搜索
            if (!string.IsNullOrEmpty(userInfoSearch.UserName))
            {
                temp = temp.Where<UserInfo>(u => u.UName.Contains(userInfoSearch.UserName));
            }
            //根据标记信息
            if (!string.IsNullOrEmpty(userInfoSearch.UserRemark))
            {
                temp = temp.Where<UserInfo>(u => u.Remark.Contains(userInfoSearch.UserRemark));
            }
            userInfoSearch.TotalCount = temp.Count();
            return temp.OrderBy<UserInfo, int>(u => u.ID).Skip<UserInfo>((userInfoSearch.PageIndex - 1) * userInfoSearch.PageSize).Take<UserInfo>(userInfoSearch.PageSize);
        }
        /// <summary>
        /// 为角色配置权限
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="userId"></param>
        /// <param name="isPass"></param>
        /// <returns></returns>
        public bool SetUserActionInfo(int actionId, int userId, bool isPass)
        {
            //这里并不是单纯的往中间表差一条记录就行了
            //判断以前是否有了该权限 有了修改就可以 没有就插入
            var r_userInfo_actionInfo = this.CurrentDBSession.R_UserInfo_ActionInfoDal.LoadEntities(u=>u.UserInfoID==userId&&u.ActionInfoID==actionId).FirstOrDefault();
            if (r_userInfo_actionInfo!=null)
            {
                r_userInfo_actionInfo.IsPass = isPass;
                this.CurrentDBSession.R_UserInfo_ActionInfoDal.EditEntities(r_userInfo_actionInfo);
            }
            else
            {
                R_UserInfo_ActionInfo r_UserInfo_ActionInfo = new R_UserInfo_ActionInfo();
                r_UserInfo_ActionInfo.IsPass = isPass;
                r_UserInfo_ActionInfo.UserInfoID = userId;
                r_UserInfo_ActionInfo.ActionInfoID = actionId;
                this.CurrentDBSession.R_UserInfo_ActionInfoDal.AddEntities(r_UserInfo_ActionInfo);
            }
            return this.CurrentDBSession.SaveChanges();
        }

        /// <summary>
        /// 为用户配置角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIdList"></param>
        /// <returns></returns>
        public bool SetUserRoleInfo(int userId, List<int> roleIdList)
        {
            var userInfo = this.CurrentDBSession.UserInfoDal.LoadEntities(u=>u.ID==userId).FirstOrDefault();//根据用户编号查找用户信息
            if (userInfo!=null)
            {
                userInfo.RoleInfo.Clear();
                foreach (int roleId in roleIdList)
                {
                    var roleInfo = this.CurrentDBSession.RoleInfoDal.LoadEntities(r=>r.ID==roleId).FirstOrDefault();
                    userInfo.RoleInfo.Add(roleInfo);
                }
                return this.CurrentDBSession.SaveChanges();
            }
            return false;
        }

        //public override void SetCurrentDal()
        //{
        //    CurrentDal = this.CurrentDBSession.UserInfoDal;
        //}
    }
}
