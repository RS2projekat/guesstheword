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
    public class GameRoomInactiveController : XSocketController
    {
        User user { get; set; }

        public IEnumerable<GameRoom> listInactiveRooms() 
        {
            return ServerContext.Instance.inactiveRooms;
        }

        public bool makeNewRoom(GameRoom newgr) 
        {
            return ServerContext.Instance.addNewRoom(newgr.Name, user);
        }

        public bool getIntoRoom(GameRoom room)
        {
            return ServerContext.Instance.addNewPlayer(user, room);
        }
        
    }
}