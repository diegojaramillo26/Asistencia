using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Asistencia
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                 name: "Asistencia",
                 url: "diligenciar-asistencia/{id}",
                 defaults: new { controller = "Asistentes", action = "Create" }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Eventos", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
