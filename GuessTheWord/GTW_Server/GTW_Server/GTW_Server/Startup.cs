using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using GTW_Server.DAL;
using GTW_Server;
using System.Web.Http;
using Newtonsoft.Json;

[assembly: OwinStartup(typeof(Startup))]

namespace GTW_Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "GTW/{controller}/{id}/",
                defaults: new { id = RouteParameter.Optional }
            );


            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None; 

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.Add(json);

            // Initialize Web API
            appBuilder.UseWebApi(config); 

            // Initialize sockets
            appBuilder.UseXSockets();
            
        
        }

    }
}