using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.DAL.Models;

namespace GTW_Server.Views
{
    public class UserMessage
    {
        public User user { get; set; }

        public String message { get; set; }
    }
}