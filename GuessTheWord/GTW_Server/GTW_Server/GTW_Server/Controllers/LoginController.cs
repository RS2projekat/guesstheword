using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;
using GTW_Server.Models;

namespace GTW_Server.Controllers
{
    [XSocketMetadata("login")]
    public class LoginController : XSocketController
    {
        /// <param name="signin"></param>
        public void SignIn(LoginUserModel user)
        {
            if (user.Username.Equals("admin") && user.Password.Equals("admin"))
            {
                this.Invoke(user.Username, "loggedin");
            }
            else
            {
                this.Invoke("notloggedin");
            }
        }

        /// <param name="signup"></param>
        public void SignUp(LoginUserModel user)
        {
            if (user.Password.Equals(user.ConfirmPassword) == false)
                this.Invoke("noequalpass");
            else
                this.Invoke("signedup");
        }
    }
}