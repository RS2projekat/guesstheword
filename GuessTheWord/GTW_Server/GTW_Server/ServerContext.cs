using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.DAL.Models;
using GTW_Server.DAL;
using GTW_Server.Services;

namespace GTW_Server
{
    public class ServerContext : IDisposable
    {
        private static ServerContext instance;
        public List<GameRoom> gameRooms { get; set; }
        public RoomServices roomServices { get; set; }
        public UserServices userServices { get; set; }
        public Random idGenerator { get; set; }
        public DatabaseContext databaseContext { get; set; }

        public bool addNewRoom(string name, User user) 
        {
            try
            {
                GameRoom gr = new GameRoom() { Id = idGenerator.Next(), Name = name, Date = DateTime.Now, PainterId = user.Id };
                gr.Users = new List<User>();
                gr.Users.Add(user);

                gameRooms.Add(gr);
                
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
                foreach (var gr in gameRooms)
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

        private ServerContext() 
        {
            gameRooms = new List<GameRoom>();
            idGenerator = new Random();
            roomServices = new RoomServices();
            userServices = new UserServices();
            databaseContext = new DatabaseContext();
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