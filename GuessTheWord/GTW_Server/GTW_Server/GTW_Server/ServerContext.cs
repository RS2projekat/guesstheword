using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.DAL.Models;

namespace GTW_Server
{
    public class ServerContext : IDisposable
    {
        private static ServerContext instance;

        public List<GameRoom> activeRooms { get; set; }
        public List<GameRoom> inactiveRooms { get; set; }
        public Random idGenerator { get; set; }
        public int proba { get; set; }

        public List<GameRoom> listInactiveRooms()
        {
            return inactiveRooms;
        }
        public bool addNewRoom(string name, User user) 
        {
            try
            {
                GameRoom gr = new GameRoom() { Id = idGenerator.Next(), Name = name, Date = DateTime.Now, PainterId = user.Id };
                gr.Users = new List<User>();
                gr.Users.Add(user);

                inactiveRooms.Add(gr);
                
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool addNewPlayer(User user, GameRoom gameroom)
        {
            try
            {
                foreach (var gr in inactiveRooms)
                    if (gr.Id == gameroom.Id)
                    {
                        gr.Users.Add(user);
                        break;
                    }
                return true;
            }
            catch (Exception e) 
            {
                return false;
            }
        }

        public bool activateRoom(int idRoom)
        {
            try
            {
                var room = inactiveRooms.Find(x => x.Id == idRoom);

                if (room == null)
                    return false;

                activeRooms.Add(room);

                inactiveRooms.Remove(room);

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        private ServerContext() 
        {
            activeRooms = new List<GameRoom>();
            inactiveRooms = new List<GameRoom>();
            idGenerator = new Random();
        }
        public static ServerContext Instance 
        {
            get
            {
                if(instance == null)
                {
                    instance = new ServerContext();
                }
                return instance;
            }
        }


        public void Dispose()
        {
        }
    }
}