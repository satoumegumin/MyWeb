using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IBaseService<T>where T:class,new()
    {
        IDBSession CurrentDBSession { get; }
        IDAL.IBaseDal<T> CurrentDal { get; set; }
        T AddEntities(T entities);
        bool DeleteEntites(T entities);
        bool EditEntities(T entities);
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc);

    }
}
