using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// 负责创建EF数据操作上下文的实例
    /// 必须保证线程内的唯一
    /// </summary>
    public class DBContextFactory
    {
        public static DbContext CreateDBContext()
        {
            //httpcontext通过callcontext来保证线程内唯一的 所以这里用httpcontext和callcontext一样
            DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
            if (dbContext==null)
            {
                dbContext = new OAEntities1();
                CallContext.SetData("dbContext",dbContext);
            }
            return dbContext;
        }
    }
}
