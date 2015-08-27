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

[assembly: OwinStartup(typeof(Startup))]

namespace GTW_Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Initialize sockets
            appBuilder.UseXSockets();
            
          //  appBuilder.CreatePerOwinContext<ServerContext>(InvokeServerContext);
         //   appBuilder.CreatePerOwinContext<ServiceContainer>(InvokeServiceContainer);

        }

        //public ServerContext InvokeServerContext()
        //{
        //    return ServerContext.Instance;
        //}

        //public ServiceContainer InvokeServiceContainer()
        //{
        //    return ServiceContainer.Instance;
        //}
    }
}