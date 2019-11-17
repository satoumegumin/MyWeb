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
            log4net.Config.XmlConfigurator.Configure();//��ȡ�������ļ��й���Log4Net��������Ϣ

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //����һ���߳� ɨ���쳣��Ϣ����
            string filePath = Server.MapPath("/Log/");
            //�̳߳� �����߳� ��������ɨ�����
            ThreadPool.QueueUserWorkItem((a)=> {
                while (true)//�̲߳��ܽ��� �����ѭ��
                {
                    //�ж�һ�¶������Ƿ�������
                    if (MyExceptionAttribute.Exec.Count()>0)
                    {
                        Exception ex= MyExceptionAttribute.Exec.Dequeue();
                        if (ex!=null)
                        {
                            //���쳣��Ϣд����־�ļ���
                            //string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                            //File.AppendAllText(filePath+fileName+".txt",ex.ToString(),System.Text.Encoding.UTF8);
                            ILog logger = LogManager.GetLogger("errorMsg");
                            logger.Error(ex.ToString());
                        }
                    }
                    else
                    {
                        //���������û������ ��Ϣһ��
                        Thread.Sleep(10000);
                    }
                }
            },filePath);

        }
    }
}
