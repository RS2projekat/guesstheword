using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;

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
        /// <param name="message"></param>
        public void ChatMessage(IMessage message)
        {
            this.InvokeToAll(message);
        }

        public override void OnOpened()
        {
            Console.WriteLine("otvoren");
        }

        public override void OnClosed()
        {
            Console.WriteLine("zatvoren");
        }

        public override void OnReopened()
        {
            Console.WriteLine("ponovo otvoren");    
        }
    }
}