using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework;
using XSockets.Plugin.Framework.Attributes;
using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;
using Microsoft.AspNet.Identity.Owin;
using GTW_Server;

namespace GTW_Server.XSocketControllers
{

    [XSocketMetadata("GlobalController", PluginRange.Internal)]

    public class GlobalController : XSocketController
    {
        private Timer timer;

        public GlobalController()
        {
            timer = new Timer(10000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }
        
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            //Sending a message to all clients on the Chat controller
            this.InvokeToAll<ChatController>("Udje u ovaj drndavi kontroler", "tick");
            test();

        }

        void test()
        {
            var x = ServerContext.Instance.userServices.getUsers();
          
        }
    }
 }