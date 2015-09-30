using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.DAL;
using GTW_Server.DAL.Models;
using System.Data.Entity;

namespace GTW_Server.Services
{
    public class RoomServices : IDisposable
    {
        public IEnumerable<GameRoom> getRooms()
        {
            return ServerContext.Instance.databaseContext.GameRooms.Include("Users").ToList().OrderByDescending(x => x.Date);
        }
        public GameRoom getRoom(int idRoom)
        {
            try
            {
                var room = (from r in ServerContext.Instance.databaseContext.GameRooms
                            where r.Id == idRoom
                            select r).Include("Users").First();
                return room;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool addRoom(GameRoom room)
        {
            try
            {
                if (getRoom(room.Id) != null)
                    return false;
                else
                {
                    ServerContext.Instance.databaseContext.GameRooms.Add(room);
                    ServerContext.Instance.databaseContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool deleteRoom(int idRoom)
        {
            try
            {
                var room = ServerContext.Instance.databaseContext.GameRooms.Find(idRoom);

                if (room == null)
                    return false;

                ServerContext.Instance.databaseContext.GameRooms.Remove(room);

                ServerContext.Instance.databaseContext.SaveChanges();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool makeWinner(int idUser, int idGameRoom)
        {
            try
            {

                var gr = (from g in ServerContext.Instance.databaseContext.GameRooms
                          where g.Id == idGameRoom
                          select g).First();
                gr.WinnerId = idUser;

                ServerContext.Instance.databaseContext.SaveChanges();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool addUsersToRoom(ICollection<User> usersId, int idRoom)
        {
            try
            {
                var room = (from r in ServerContext.Instance.databaseContext.GameRooms
                            where r.Id == idRoom
                            select r).First();
                ICollection<User> users = new List<User>();

                foreach (var us in usersId)
                {
                    var nesto = (from u in ServerContext.Instance.databaseContext.Users
                                 where u.Id == us.Id
                                 select u).First();


                    users.Add(nesto);
                }

                room.Users = users;

                ServerContext.Instance.databaseContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public ICollection<User> getUsersInRoom(int idRoom)
        {
            try
            {
                var users = (from r in ServerContext.Instance.databaseContext.GameRooms
                             where r.Id == idRoom
                             select r).Include("Users").First().Users.ToList();
                return users;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Dispose()
        {
        }
    }
}