using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DALFactory
{
    public static  class DBSessionFactory
    {
        public static IDBSession CreateDBSession()
        {
            IDBSession dBSession = (IDBSession)CallContext.GetData("dBSession");
            if (dBSession==null)
            {
                dBSession = new DbSession();
                CallContext.SetData("dBSession",dBSession);
            }
            return dBSession;
        }
    }
}
