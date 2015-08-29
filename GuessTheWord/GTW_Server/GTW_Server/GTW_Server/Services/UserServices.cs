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
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    return db.Users.ToList();
                }
                catch(Exception e)
                {
                    return null;
                }
            }
        }
        public User getUser(User user)
        {

            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var us = (from u in db.Users
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
        }

        public string getUserRole(int idUser)
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    var userRole = from u in db.Users
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
                        User u = new User() { Username = user.Username, Password = user.Password, Role = "User" };
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
        public bool updateUserRole(int idUser, string Role)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    User user = (from us in db.Users
                                where us.Id == idUser
                                select us).First();


                    user.Role = Role;
                    
                    db.Entry(user).State = EntityState.Modified;

                    db.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }

        public bool deleteUser(int idUser)
        {
            using (DatabaseContext db = new DatabaseContext())
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
                    // TODO: Return rooms for one user
                    return null;

                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        

        public bool changePassword(int idUser, string newpass) 
        {
            using(DatabaseContext db = new DatabaseContext())
            {
                try 
                {
                    var user = (from u in db.Users
                               where u.Id == idUser
                               select u).First();

                    user.Password = newpass;

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

        public void Dispose()
        {
        }
    }
}