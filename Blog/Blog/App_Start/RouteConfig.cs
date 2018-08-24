using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog
{
    public class RouteConfig
    {
        /*ASP.NET MVC 调用不同的控制器类（和其中不同的操作方法），具体取决于传入的 URL。使用 ASP.NET MVC 的默认 URL 路由逻辑使用如下格式来确定哪些代码以调用：
         *[Controller]/[ActionName]/[Parameters]
         * */
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}