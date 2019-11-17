using Model;
using Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public partial interface IUserInfoService:IBaseService<UserInfo>
    {
        bool DeleteUserInfos(string[] idArray);

        IQueryable<UserInfo> LoadSearchEntities(UserInfoSearch userInfoSearch,short delFlag);

        bool SetUserRoleInfo(int userId,List<int> roleIdList);
        bool SetUserActionInfo(int actionId,int userId,bool isPass);
    }
}
