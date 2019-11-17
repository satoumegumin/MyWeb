using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace NewAjax
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// web应用程序第一次启用时调用该方法 并且该方法只被调用一次
        /// 网站的入口 相当于winform应用程序的main函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            //这个方法中一般完成整个应用程序的初始化
            //定时任务框架
        }
        /// <summary>
        /// 开始回话的时候执行
        /// 网站部署在服务器上 当客户端和服务器连接时候 触发这个方法
        /// 但是当该用户通过浏览器再次访问该网站下的其他页面时 该方法不会被执行
        /// 通过一个sessionId判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            //这个方法一般可以用来统计是第几个访客
            //application 也是一种服务端的状态保持机制 放在该对象中的数据是共享的
            Application.Lock();
            int count = Convert.ToInt32(Application["count"]);
            count++;
            Application["count"] = count;
            Application.UnLock();
        }
        /// <summary>
        /// 请求管道的第一个事件  请求管道的第一个时间触发后被调用
        /// 想注册第几个事件都可以在这里写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 这个方法非常重要
        /// 全局的异常处点 把错误信息记录在日志文件里可以在这里
        /// 抛异常的页面都会执行这个方法 可以在这里捕获异常信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //获取上一次错误的信息
            Exception ex= HttpContext.Current.Server.GetLastError();
            //提供一个队列 先了解一下
        }
        /// <summary>
        /// 回话结束的时候执行 用户关闭浏览器不会立刻执行
        /// 服务器不知道用户走了  session消失后执行此方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            int count = Convert.ToInt32(Application["count"]);
            count--;
            Application["count"] = count;
            Application.UnLock();
        }
        /// <summary>
        /// 基本上不怎么用的方法 
        /// 应用程序结束时候发生
        /// 一般没有时间执行这个方法 基本上用不到的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}