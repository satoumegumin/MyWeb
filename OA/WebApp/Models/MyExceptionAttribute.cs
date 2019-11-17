﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class MyExceptionAttribute:HandleErrorAttribute
    {
        //.net中自带的队列
        public static Queue<Exception> Exec = new Queue<Exception>();

        /// <summary>
        /// 可以捕获异常数据
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Exception ex = filterContext.Exception;
            //写到队列
            Exec.Enqueue(ex);//将数据写到队列里面

            //跳转到错误页面
            filterContext.HttpContext.Response.Redirect("/error.html");
        }
    }
}