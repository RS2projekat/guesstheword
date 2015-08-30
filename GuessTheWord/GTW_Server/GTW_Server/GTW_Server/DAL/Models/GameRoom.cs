using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTW_Server.DAL.Models
{
    public class GameRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Word { get; set; }
        public int WinnerId { get; set; }
        public int PainterId { get; set; }
        public ICollection<User> Users { get; set; }
            
    }
}