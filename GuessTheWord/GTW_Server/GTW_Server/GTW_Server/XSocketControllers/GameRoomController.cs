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
using GTW_Server.Models;

namespace GTW_Server.XSocketControllers
{
    public class GameRoomController : XSocketController
    {
        User user { get; set; }
        GameRoom gameRoom { get; set; }

        public bool register(User u)
        {
            try 
            {
                user = new User() { Id = u.Id, Username = u.Username, Role = u.Role };
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public IEnumerable<GameRoom> listInactiveRooms() 
        {
            return ServerContext.Instance.roomServices.getRooms();
            //return ServerContext.Instance.gameRooms.FindAll(g => g.Users.Count != 4);
        }

        public bool makeNewRoom(GameRoom newgr) 
        {
            return ServerContext.Instance.addNewRoom(newgr.Name, user);
        }

        public bool getIntoRoom(GameRoom room)
        {
            return ServerContext.Instance.addNewPlayer(user, room);
        }

        public void chatMessage(IMessage message)
        {
            this.InvokeTo(c => c.gameRoom.Id == gameRoom.Id, new UserMessage() { message = message.Data, user = user}, "chatmessage");
        }

        public void startGame(IMessage message) 
        {
            // ovo moze biti problem, ne znam sta je Data
            gameRoom.Word = message.Data;
            this.InvokeTo(c => c.gameRoom.Id == gameRoom.Id, user, "start");
        }

        public void endGame()
        {
            ServerContext.Instance.roomServices.addRoom(gameRoom);
            this.InvokeTo(c => c.gameRoom.Id == gameRoom.Id, user, "end");
        }

        public void sendCanvas(IMessage message)
        {
            this.InvokeTo(c => c.gameRoom.Id == gameRoom.Id, new UserMessage() { user = user, message = message.Data}, "recievecanvas");
        }

        public void guessTheWord(IMessage message)
        {
            if (message.ToString().Equals(gameRoom.Word, StringComparison.CurrentCultureIgnoreCase) == true)
            {
                gameRoom.WinnerId = user.Id;

                ServerContext.Instance.roomServices.addRoom(gameRoom);


                this.InvokeTo(c => c.gameRoom.Id == gameRoom.Id, user, "win");

                gameRoom = null;
            }
            else
            {
                this.InvokeTo(c => c.gameRoom.Id == gameRoom.Id, new UserMessage() { message = message.Data, user = user}, "wrong");
            }
        }


    }
}