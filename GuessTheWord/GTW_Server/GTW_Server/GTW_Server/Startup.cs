using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server;
using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;
using System.Data.Entity;
using GTW_Server.DAL;

[assembly: OwinStartup(typeof(Startup))]

namespace GTW_Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Initialize sockets
            appBuilder.UseXSockets();

        }

    }
}