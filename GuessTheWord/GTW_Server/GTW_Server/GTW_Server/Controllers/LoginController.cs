using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;
using GTW_Server.Models;
using GTW_Server.Services;

namespace GTW_Server.Controllers
{
    [XSocketMetadata("login")]
    public class LoginController : XSocketController
    {
        public void SignIn(LoginUserModel user)
        {
            if (UserService.checkIfUserExists(user) == true)
            { 
                this.Invoke(user.Username, "loggedin");
            }
            else
            {
                this.Invoke("notloggedin");
            }
        }

        public void SignUp(LoginUserModel user)
        {
            if (UserService.addUser(user) == true)
                this.Invoke("signedup");
            else
                this.Invoke("nosignedup");
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