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
                return ServerContext.Instance.userServices.getUsers();
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
            var u = ServerContext.Instance.userServices.getUser(user);

            if (u == null)
                return NotFound();

            return Ok(new User() {  Username = u.Username, Role = u.Role});

        }

        [HttpGet]
        [Route("GTW/Users/{id}/Role")]
        public IHttpActionResult GetUserRole(int id)
        {
           var userRole = ServerContext.Instance.userServices.getUserRole(id);
           if (userRole.Equals("None"))
                return BadRequest();
           else 
                return Ok(userRole);
        }

        [HttpGet]
        [Route("GTW/Users/{id}/GameRooms")]
        public IEnumerable<GameRoom> GetUserGameRooms(int id)
        {
            return ServerContext.Instance.userServices.getRoomsForUser(id);
        }

        [HttpPost]
        [Route("GTW/Users/Register")]
        public IHttpActionResult PostUser(User user)
        {
           if (ServerContext.Instance.userServices.addUser(user) == false)
              return BadRequest();
           else
               return Ok(new User() { Username = user.Username, Role = user.Role});  
        }

        [HttpPost]
        [Route("GTW/Users/{id}/Role/")]
        public IHttpActionResult UpdateRole(int id,User user)
        {
           if (ServerContext.Instance.userServices.updateUserRole(id, user.Role) == true)
               return Ok();
           else
               return BadRequest();
        }

        [HttpDelete]
        [Route("GTW/Users/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
          if (ServerContext.Instance.userServices.deleteUser(id) == true)
               return Ok();
          else
               return BadRequest();
        }

    }
    
}
