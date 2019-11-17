using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BaseDal<T>where T:class,new()
    {

        DbContext dB = DBContextFactory.CreateDBContext();
        //不需要实现IBaseDal接口  
        //OAEntities1 dB = new OAEntities1();//先new 出来 后面再创建线程内唯一对象
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public T AddEntities(T entities)
        {
            //没有添加ef的引用 不能点出来 可以创建ef空模型 在删除掉
            dB.Set<T>().Add(entities);
            //dB.SaveChanges();
            return entities;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool DeleteEntites(T entities)
        {
            dB.Entry<T>(entities).State = System.Data.Entity.EntityState.Deleted;
            return true;
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool EditEntities(T entities)
        {
            dB.Entry<T>(entities).State = System.Data.Entity.EntityState.Modified;
            return true;
        }
        /// <summary>
        /// 查询过滤
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return dB.Set<T>().Where<T>(whereLambda);//不能点T  用set<T>() 这个方法拿到类型
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderbyLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            var temp = dB.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            if (isAsc)//升序
            {
                temp = temp.OrderBy<T, s>(orderbyLambda).Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            }
            return temp;
        }
    }
}
