using log4net;
using Spring.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApp.Models;

namespace WebApp
{
    public class MvcApplication :SpringMvcApplication //System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();//读取了配置文件中关于Log4Net的配置信息

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //开启一个线程 扫描异常信息队列
            string filePath = Server.MapPath("/Log/");
            //线程池 开启线程 帮助我们扫描队列
            ThreadPool.QueueUserWorkItem((a)=> {
                while (true)//线程不能结束 因此死循环
                {
                    //判断一下队列中是否有数据
                    if (MyExceptionAttribute.Exec.Count()>0)
                    {
                        Exception ex= MyExceptionAttribute.Exec.Dequeue();
                        if (ex!=null)
                        {
                            //将异常信息写到日志文件中
                            //string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                            //File.AppendAllText(filePath+fileName+".txt",ex.ToString(),System.Text.Encoding.UTF8);
                            ILog logger = LogManager.GetLogger("errorMsg");
                            logger.Error(ex.ToString());
                        }
                    }
                    else
                    {
                        //如果队列中没有数据 休息一会
                        Thread.Sleep(10000);
                    }
                }
            },filePath);

        }
    }
}
