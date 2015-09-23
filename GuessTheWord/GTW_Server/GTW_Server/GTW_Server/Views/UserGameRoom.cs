using GTW_Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTW_Server.Views
{
    public class UserGameRoom
    {
        public User user { get; set; }

        public GameRoom gameRoom { get; set; }
    }
}