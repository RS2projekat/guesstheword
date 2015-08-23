using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;
using GTW_Server.Services;
using GTW_Server.Models;

namespace GTW_Server.Controllers
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