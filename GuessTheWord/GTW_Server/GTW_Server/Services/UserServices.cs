using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.DAL;
using GTW_Server.DAL.Models;
using System.Data.Entity;

namespace GTW_Server.Services
{
    public class UserServices : IDisposable
    {
        public IEnumerable<User> getUsers()
        {
            try
            {
                return ServerContext.Instance.databaseContext.Users.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public User getUser(User user)
        {

            try
            {
                var us = (from u in ServerContext.Instance.databaseContext.Users
                          where u.Username == user.Username && u.Password == user.Password
                          select u);
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

        public string getUserRole(int idUser)
        {
            try
            {
                var userRole = from u in ServerContext.Instance.databaseContext.Users
                               where u.Id == idUser
                               select u.Role;
                if (userRole.Count() != 1)
                    return "None";
                return userRole.First();
            }
            catch
            {
                return "Error";
            }
        }

        public bool addUser(User user)
        {
            try
            {
                if (getUser(user) != null)
                    return false;
                else
                {
                    User u = new User() { Username = user.Username, Password = user.Password, Role = "User" };
                    ServerContext.Instance.databaseContext.Users.Add(u);

                    ServerContext.Instance.databaseContext.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool updateUserRole(int idUser, string Role)
        {
            try
            {
                User user = (from us in ServerContext.Instance.databaseContext.Users
                             where us.Id == idUser
                             select us).First();


                user.Role = Role;

                ServerContext.Instance.databaseContext.Entry(user).State = EntityState.Modified;

                ServerContext.Instance.databaseContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool deleteUser(int idUser)
        {
            try
            {
                var user = ServerContext.Instance.databaseContext.Users.Find(idUser);

                if (user == null)
                    return false;

                ServerContext.Instance.databaseContext.Users.Remove(user);

                ServerContext.Instance.databaseContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<GameRoom> getRoomsForUser(int idUser)
        {
            try
            {
                List<GameRoom> gamerooms = new List<GameRoom>();

                var rooms = (from r in ServerContext.Instance.databaseContext.GameRooms
                             select r).Include("Users").ToList();

                foreach (var room in rooms)
                {

                    foreach (var user in room.Users.ToList())
                    {
                        if (user.Id == idUser)
                        {
                            gamerooms.Add(room);
                            break;
                        }
                    }
                }
                // TODO: Return rooms for one user
                return gamerooms;

            }
            catch (Exception e)
            {
                return null;
            }
        }



        public bool changePassword(int idUser, string newpass)
        {
            try
            {
                var user = (from u in ServerContext.Instance.databaseContext.Users
                            where u.Id == idUser
                            select u).First();

                user.Password = newpass;

                ServerContext.Instance.databaseContext.Entry(user).State = EntityState.Modified;

                ServerContext.Instance.databaseContext.SaveChanges();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public String test()
        {
            try
            {
                String a = ServerContext.Instance.databaseContext.Users.First().Username.ToString();
                return a;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public void Dispose()
        {
        }
    }
}