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
                using (RoomServices rs = new RoomServices())
                {
                    return rs.getRooms();
                }
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
                using (RoomServices rs = new RoomServices())
                {
                    return rs.getRoom(id);
                }
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
                using (RoomServices rs = new RoomServices())
                {
                    if (rs.addRoom(room) == false)
                        return BadRequest();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("GTW/GameRooms/{idr}/Winner/")]

        public IHttpActionResult addWinner(int idr, User user)
        {
            try
            {
                using (RoomServices rs = new RoomServices())
                {
                    if(rs.makeWinner(user.Id, idr) == false)
                        return BadRequest();
                    return Ok();
                }
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
                using (RoomServices rs = new RoomServices())
                {
                    if (rs.addUsersToRoom(users, idr) == false)
                        return BadRequest();
                    return Ok();
                }
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
                using (RoomServices rs = new RoomServices())
                {
                    return rs.getUsersInRoom(idr);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
