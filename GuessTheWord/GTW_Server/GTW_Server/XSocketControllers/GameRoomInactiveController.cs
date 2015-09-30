using GTW_Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XSockets.Core.XSocket;

namespace GTW_Server.XSocketControllers
{
    public class GameRoomInactiveController : XSocketController
    {
        User user { get; set; }

        public bool register(User u)
        {
            try
            {
                user = new User() { Username = u.Username, Role = u.Role };
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public IEnumerable<GameRoom> listInactiveRooms()
        {
            return ServerContext.Instance.gameRooms.FindAll(g => g.Users.Count != 4);
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