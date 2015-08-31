using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;
using GTW_Server.Services;
using GTW_Server.DAL.Models;

namespace GTW_Server.XSocketControllers
{
    public class GameRoomActiveController : XSocketController
    {
        User user { get; set; }
        GameRoom gameroom { get; set; }

        public void ChatMessage(IMessage message)
        {
            this.InvokeTo(c => c.gameroom.Id == gameroom.Id, message, "chatmessage");
        }

        public void GuessTheWord(IMessage message)
        {
            if(message.ToString().Equals(gameroom.Word, StringComparison.CurrentCultureIgnoreCase) == true)
            {
                gameroom.WinnerId = user.Id;

                this.InvokeTo(c => c.gameroom.Id == gameroom.Id, gameroom, "win");
            }
            else
            {
                this.InvokeTo(c => c.gameroom.Id == gameroom.Id, message, "wrong");
            }
        }


        public override void OnOpened()
        {
        }

        public override void OnClosed()
        {
        }
    }
}