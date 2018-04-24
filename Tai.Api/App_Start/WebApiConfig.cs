using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Tai.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //跨域问题
            config.EnableCors(new EnableCorsAttribute("*","*","*"));
            // Web API 配置和服务

            // Web API 特性路由
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new Filters.ApiSecurityBaseFilter());
            //config.Filters.Add(new Filters.ApiSecurityFilter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
