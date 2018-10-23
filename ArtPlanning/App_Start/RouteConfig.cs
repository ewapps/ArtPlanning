using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ArtPlanning
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "WarehouseManagement",
                url: "WarehouseManagement/{action}/{id}",
                defaults: new { controller = "AS_Tools_WarehouseManagement", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Users",
                url: "Users/{action}/{id}",
                defaults: new { controller = "AS_Users", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Agenda",
                url: "Agenda/{action}/{id}",
                defaults: new { controller = "AS_Agenda", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default Authentication",
                url: "Authentication/{action}/{id}",
                defaults: new { controller = "AS_Authentication", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Home",
                url: "Home/{action}/{id}",
                defaults: new { controller = "AS_Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "AS_Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
