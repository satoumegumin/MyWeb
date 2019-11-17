using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    /// <summary>
    /// 业务层调用的是数据会话层的接口
    /// </summary>
    public partial interface IDBSession
    {
        DbContext DB { get; }
        //IUserInfoDal UserInfoDal { get; set; }
        bool SaveChanges();
    }
}
