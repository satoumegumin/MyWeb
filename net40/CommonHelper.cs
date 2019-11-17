using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CRUD.TempLates
{
    public static class CommonHelper
    {
        public static string RenderCompalte(string names, object data)
        {
            VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH,
                System.Web.Hosting.HostingEnvironment.MapPath("~/"));//模板文件所在的文件夹
            vltEngine.Init();//初始化
            VelocityContext velContext = new VelocityContext();
            velContext.Put("Data",data);//设置参数 在模板中可以通过$data来引用
            

            Template vltTemplate = vltEngine.GetTemplate(names);
            StringWriter vltWriter = new StringWriter();
            vltTemplate.Merge(velContext, vltWriter);

            string html = vltWriter.GetStringBuilder().ToString();
            return html;          

        }
    }
}