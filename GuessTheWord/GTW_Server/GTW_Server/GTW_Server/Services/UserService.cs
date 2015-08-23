using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.Models;
using GTW_Server.DAL;
using GTW_Server.DAL.Models;

namespace GTW_Server.Services
{
    public class UserService
    {
        public static bool checkIfUserExists(LoginUserModel user)
        {

            using (DatabaseContext db = new DatabaseContext())
            {
                try 
                {
                    var us = from u in db.Users
                             where u.Username == user.Username && u.Password == user.Password
                             select u;
                    return us.Count() == 1;

                }
                catch (Exception e) 
                {
                    return false;
                }
            }
        }

        public static bool addUser(LoginUserModel user)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    if (checkIfUserExists(user) == true)
                        return false;
                    else 
                    {
                        User newUser = new User() { Username = user.Username, Password = user.Password };

                        db.Users.Add(newUser);

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
    }
}