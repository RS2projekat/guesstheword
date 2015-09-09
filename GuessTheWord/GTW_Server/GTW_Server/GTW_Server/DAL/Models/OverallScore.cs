using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTW_Server.DAL.Models
{
    public class OverallScore
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}