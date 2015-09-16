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
    public class GameRoomsController : ApiController
    {
        [HttpGet]
        [Route("GTW/GameRooms/")]
        public IEnumerable<GameRoom> GetRooms()
        {
            try
            {
                return ServerContext.Instance.roomServices.getRooms();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GTW/GameRooms/{id}")]

        public GameRoom GetRoom(int id)
        {
            try
            {
                return ServerContext.Instance.roomServices.getRoom(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("GTW/GameRooms/")]
        public IHttpActionResult AddRoom(GameRoom room)
        {
            try
            {
               if (ServerContext.Instance.roomServices.addRoom(room) == false)
                  return BadRequest();
               return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        //[HttpPost]
        //[Route("GTW/GameRooms/")]
        //public IHttpActionResult AddRoom([FromBody] string room)
        //{
        //    try
        //    {
        //        using (RoomServices rs = new RoomServices())
        //        {
        //            if (rs.addRoom(room) == false)
        //                return BadRequest() ;
        //            return Ok();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpPost]
        [Route("GTW/GameRooms/{idr}/Winner/")]

        public IHttpActionResult addWinner(int idr, User user)
        {
            try
            {
                if(ServerContext.Instance.roomServices.makeWinner(user.Id, idr) == false)
                   return BadRequest();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("GTW/GameRooms/{idr}/Users/")]
        public IHttpActionResult addUsers(int idr, ICollection<User> users)
        {
            try
            {
                if (ServerContext.Instance.roomServices.addUsersToRoom(users, idr) == false)
                   return BadRequest();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GTW/GameRooms/{idr}/Users/")]

        public ICollection<User> getUsers(int idr)
        {
            try
            {
                return ServerContext.Instance.roomServices.getUsersInRoom(idr);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
