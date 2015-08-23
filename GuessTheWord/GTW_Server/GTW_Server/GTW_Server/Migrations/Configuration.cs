namespace GTW_Server.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using GTW_Server.DAL.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<GTW_Server.DAL.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GTW_Server.DAL.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            
            context.Database.ExecuteSqlCommand("DELETE FROM Users");
            User u1 = new User() {Username = "admin", Password = "admin"};

            context.Users.Add(u1);

            context.SaveChanges();
        }
    }
}
