using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.DAL;
using GTW_Server.DAL.Models;
using System.Data.Entity;

namespace GTW_Server.Services
{
    public class DatabaseServices
    {
        public User getUser(User user)
        {

            using (DatabaseContext db = new DatabaseContext())
            {
                try 
                {
                    var us = from u in db.Users
                             where u.Username == user.Username && u.Password == user.Password
                             select u;
                    if (us.Count() != 1)
                        return null;
                    else
                        return us.First();
                }
                catch (Exception e) 
                {
                    return null;
                }
            }
        }

        public bool addUser(User user)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    if (getUser(user) != null)
                        return false;
                    else 
                    {
                        db.Users.Add(user);

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
        public bool updateUser(User user) 
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    db.Entry(user).State = EntityState.Modified;

                    db.SaveChanges();

                    return true;
                }
                catch(Exception e)
                {
                    return false;
                }

            }
        }

        public bool deleteUser(int idUser)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var user = db.Users.Find(idUser);

                    if (user == null)
                        return false;

                    db.Users.Remove(user);

                    db.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public IEnumerable<GameRoom> getRoomsForUser(int idUser) 
        {
            using (DatabaseContext db = new DatabaseContext()) 
            {
                try
                {
                    var rooms = (from u in db.Users
                                where u.Id == idUser
                                select u).Include("Rooms").First();

                    return rooms.Rooms;

                }
                catch(Exception e)
                {
                    return null;
                }
            }
        }

        public bool makeWinner(int idUser, int idGameRoom)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var gr = (from g in db.GameRooms
                             where g.Id == idGameRoom
                             select g).First();
                    var user = (from u in db.Users
                                where u.Id == idUser
                                select u).First();
                    gr.Winner = user;

                    db.SaveChanges();

                    return true;

                }
                catch(Exception e)
                {
                    return false;
                }

            }
        }

        public GameRoom getRoom(int idRoom)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var room = (from r in db.GameRooms
                                where r.Id == idRoom
                                select r).First();
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
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    if (getRoom(room.Id) != null)
                        return false;
                    else
                    {
                        db.GameRooms.Add(room);
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
            using(DatabaseContext db = new DatabaseContext())
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
                catch(Exception e)
                {
                    return false;
                }
            }
        }
    }
}
