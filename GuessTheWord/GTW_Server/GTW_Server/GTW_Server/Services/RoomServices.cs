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
            using(DatabaseContext db = new DatabaseContext())
            {
                return db.GameRooms.Include("Users").ToList();
            }
        }
        public GameRoom getRoom(int idRoom)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var room = (from r in db.GameRooms
                                where r.Id == idRoom
                                select r).Include("Users").First();
                    return room;

                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }        

        public bool addRoom(GameRoom room)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    if (getRoom(room.Id) != null)
                        return false;
                    else
                    {
                        db.GameRooms.Add(room);
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool deleteRoom(int idRoom)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var room = db.GameRooms.Find(idRoom);

                    if (room == null)
                        return false;

                    db.GameRooms.Remove(room);

                    db.SaveChanges();

                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool makeWinner(int idUser, int idGameRoom)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {

                    var gr = (from g in db.GameRooms
                              where g.Id == idGameRoom
                              select g).First();
                    gr.WinnerId = idUser;

                    db.SaveChanges();

                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }

        public bool addUsersToRoom(ICollection<User> usersId, int idRoom) 
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var room = (from r in db.GameRooms
                               where r.Id == idRoom
                               select r).First();
                    ICollection<User> users = new List<User>();

                    foreach(var us in usersId)
                    {
                        var nesto = (from u in db.Users
                                    where u.Id == us.Id
                                    select u).First();


                        users.Add(nesto);
                    }

                    room.Users = users;

                    db.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }

        public ICollection<User> getUsersInRoom(int idRoom) 
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var users = (from r in db.GameRooms
                                where r.Id == idRoom
                                select r).Include("Users").First().Users.ToList();
                    return users;
                }
                catch (Exception e)
                {
                    return null ;
                }

            }
        }

        public void Dispose()
        {
        }
    }
}