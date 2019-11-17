using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EnumType
{
    public enum DeleteEnumType
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal=0,
        /// <summary>
        /// 逻辑删除
        /// </summary>
        LogicalDeleted=1
    }
}
