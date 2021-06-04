using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace  GsEPWv8_5_MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            if (System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString()== "3plpro.gensoftcorp.com")
            {
                routes.MapRoute(
                              name: "Default",
                              url: "{controller}/{action}/{id}",
                              defaults: new { controller = "Login", action = "UserLogin", id = UrlParameter.Optional }
                          );
            }
            if (System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString() == "3plecom.gensoftcorp.com")
            {
                routes.MapRoute(
                              name: "Default",
                              url: "{controller}/{action}/{id}",
                              defaults: new { controller = "LoginEcom", action = "LoginView", id = UrlParameter.Optional }
                          );
            }
            if (System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString() == "3plb2b.gensoftcorp.com")
            {
                routes.MapRoute(
                              name: "Default",
                              url: "{controller}/{action}/{id}",
                              defaults: new { controller = "LoginB2B", action = "UserLogin", id = UrlParameter.Optional }
                          );
            }
        }
    }
}
