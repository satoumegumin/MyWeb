using DAL;
using IDAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALFactory
{
    /// <summary>
    /// 数据会话层 就是有个工厂类 负责完成所有数据操作实例的创建 
    /// 然后业务层通过会话层来获取要操作的实例
    /// 所以数据会话层将业务层 与数据层解耦
    /// 在数据回话层中提供一个方法:完成所有数据的保存
    /// </summary>
    public partial class DbSession:IDBSession
    {
        public DbContext DB
        {
            get
            {
                return DBContextFactory.CreateDBContext();
            }
        }
        //private IUserInfoDal _userInfoDal;
        //public IUserInfoDal UserInfoDal
        //{
        //    get
        //    {
        //        if (_userInfoDal==null)
        //        {
        //            //_userInfoDal = new UserInfoDal();
        //            _userInfoDal = AbstractFactory.CreateUserInfoDal();//通过抽象工厂封装了类 的实例的创建
        //        }
        //        return _userInfoDal;
        //    }
        //    set
        //    {
        //        _userInfoDal=value;
        //    }
        //}


        /// <summary>
        /// 虽然简单 确实架构的亮点
        /// 一个业务中经常涉及到对多张表的操作
        /// 我们希望连接一次数据库 完成对多张表的操作
        /// 提供性能  数据层里面的savechage就不要了
        /// 工作单元模式 多条数据连接一次数据库 进行保存
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return DB.SaveChanges() > 0;
        }
    }
}
