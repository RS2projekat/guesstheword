﻿using System;
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
        [Route("GTW/Test/{id}")]
        public int Test(int id)
        {
			return id;
        }

    }
}
