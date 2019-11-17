using DALFactory;
using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class BaseService<T> where T:class,new()
    {
        public IDBSession CurrentDBSession
        {
            //get { return new DbSession(); }//暂时先NEW
            get { return DBSessionFactory.CreateDBSession(); }
        }

        public IDAL.IBaseDal<T> CurrentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseService()
        {
            SetCurrentDal();//子类一定要实现抽象方法
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public T AddEntities(T entities)
        {
            CurrentDal.AddEntities(entities);
            CurrentDBSession.SaveChanges();
            return entities;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool DeleteEntites(T entities)
        {
            CurrentDal.DeleteEntites(entities);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool EditEntities(T entities)
        {
            CurrentDal.EditEntities(entities);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 查询过滤
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
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
            return CurrentDal.LoadPageEntities(pageIndex, pageSize,out totalCount, whereLambda, orderbyLambda, isAsc);
        }
    }
}
