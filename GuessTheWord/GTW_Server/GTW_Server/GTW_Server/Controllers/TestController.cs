using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GTW_Server;
using GTW_Server.DAL.Models;

namespace GTW_Server.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("GTW/Test/")]
        public void Test()
        {
            ServerContext.Instance.addNewRoom("neka soba", new User() {Id = 19, Username = "pera" });
            var x = ServerContext.Instance.inactiveRooms.First();
            ServerContext.Instance.addNewPlayer(new User() { Id = 23, Username = "pera2" }, x);
            var y = ServerContext.Instance.inactiveRooms;
            ServerContext.Instance.activateRoom(x.Id);
            Console.WriteLine(x);
        }

    }
}
