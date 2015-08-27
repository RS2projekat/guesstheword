namespace GTW_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameRooms", "Word", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameRooms", "Word");
        }
    }
}
