using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IBaseDal<T>where T:class,new()
    {
        /// <summary>
        /// 查询的方法
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 分页的方法
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderbyLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda,
           System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc);
        /// <summary>
        /// 删除的方法
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        bool DeleteEntites(T entities);
        /// <summary>
        /// 编辑的方法
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        bool EditEntities(T entities);

        /// <summary>
        /// 添加的方法
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        T AddEntities(T entities);
    }
}
