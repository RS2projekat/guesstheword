using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using GTW_Server.DAL.Models;

namespace GTW_Server.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("DatabaseContext")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }

    }
}