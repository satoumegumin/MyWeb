using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Search
{
    public class UserInfoSearch:BaseSearch
    {
        //构建搜索信息的类
        public string UserName { get; set; }
        public string UserRemark { get; set; }
    }
}
