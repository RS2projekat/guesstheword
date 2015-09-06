using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTW_Server.DAL.Models
{
    public class WeeklyScore
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Score { get; set; }
        public string Date { get; set; }
    }
}