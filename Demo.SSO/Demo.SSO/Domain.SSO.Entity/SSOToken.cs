using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.SSO.Entity
{
    public class SSOToken
    {
        //Token ID，作为cookie存储在浏览器端
        public string ID { get; set; }
        //认证的域，例如：qeefee.com
        public string Domain { get; set; }
        //认证时间
        public DateTime AuthTime { get; set; }
        //Token超时时间，单位为秒(s)
        public int TimeOut { get; set; }
        //认证的用户
        public SSOUser User { get; set; }
        //标记当前登录的ID
        public string LoginID { get; set; }

        public SSOToken()
        {
            this.ID = Guid.NewGuid().ToString("N");
            this.AuthTime = DateTime.Now;
            this.TimeOut = 60 * 10; //10分钟
        }

        //是否超时
        public bool IsTimeOut()
        {
            return AuthTime.AddSeconds(TimeOut) < DateTime.Now;
        }

        private static List<SSOToken> ssoTokenList = new List<SSOToken>();
        public static List<SSOToken> SSOTokenList
        {
            get { return ssoTokenList; }
        }
    }
}
