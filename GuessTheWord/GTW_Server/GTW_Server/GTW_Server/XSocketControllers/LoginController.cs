using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;
using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;
using Microsoft.AspNet.Identity.Owin;
using GTW_Server.Services;
using GTW_Server.DAL;
using GTW_Server.DAL.Models;

namespace GTW_Server.XSocketControllers
{
    [XSocketMetadata("login")]
    public class LoginController : XSocketController
    {
        public void SignIn(User user)
        {
          
        }

        public void SignUp(User user)
        {
           
        }


        public override void OnOpened()
        {
        }

        public override void OnClosed()
        {
        }

        public override void OnReopened()
        {
        }
    }
}