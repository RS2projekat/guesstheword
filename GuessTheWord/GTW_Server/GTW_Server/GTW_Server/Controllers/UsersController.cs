using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GTW_Server.DAL.Models;
using GTW_Server.Services;
using System.Web.Http.Description;

namespace GTW_Server.Controllers
{
    public class UsersController : ApiController
    {
        [HttpGet]
        [Route("GTW/Users/")]
        public IEnumerable<User> GetUsers()
        {
            try
            {
                using (UserServices us = new UserServices()) 
                {
                    return us.getUsers();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("GTW/Users/Login")]
        public IHttpActionResult LoginUser(User user)
        {
            using(UserServices us = new UserServices())
            {
                var u = us.getUser(user);

                if (u == null)
                    return NotFound();

                return Ok(u);

            }
        }

        [HttpGet]
        [Route("GTW/Users/{id}/Role")]
        public IHttpActionResult GetUserRole(int id)
        {
            using(UserServices us = new UserServices())
            {
                var userRole = us.getUserRole(id);
                if (userRole.Equals("None"))
                    return BadRequest();
                else 
                    return Ok(userRole);
            }
        }

        [HttpGet]
        [Route("GTW/Users/{id}/GameRooms")]
        public IEnumerable<GameRoom> GetUserGameRooms(int id)
        {
            using (UserServices us = new UserServices())
            {
                return us.getRoomsForUser(id);
            }
        }

        [HttpPost]
        [Route("GTW/Users/Register")]
        public IHttpActionResult PostUser(User user)
        {
            using(UserServices us = new UserServices())
            {
                 ;
                if (us.addUser(user) == false)
                    return BadRequest();
                else
                    return Ok();
            }
        }

        [HttpPost]
        [Route("GTW/Users/{id}/Role/")]
        public IHttpActionResult UpdateRole(int id,User user)
        {
            using(UserServices us = new UserServices())
            {
                if (us.updateUserRole(id, user.Role) == true)
                    return Ok();
                else
                    return BadRequest();
            }
        }

        [HttpDelete]
        [Route("GTW/Users/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            using (UserServices us = new UserServices())
            {
                
                if (us.deleteUser(id) == true)
                    return Ok();
                else
                    return BadRequest();
            }
        }

        
    }
}
