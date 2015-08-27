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
using Microsoft.Owin.Host.SystemWeb;

namespace GTW_Server.XSocketControllers
{
    [XSocketMetadata("chat")]
    public class ChatController : XSocketController
    {
        /// <summary>
        /// This will broadcast any message to all clients
        /// connected to this controller.
        /// To use Pub/Sub replace InvokeToAll with PublishToAll
        /// </summary>
        public void ChatMessage(IMessage message)
        {
            ServerContext.Instance.proba = message.ToString().Count();
            this.InvokeToAll(message);
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