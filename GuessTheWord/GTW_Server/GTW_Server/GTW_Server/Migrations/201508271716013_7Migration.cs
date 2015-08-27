namespace GTW_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "GameRoom_Id", c => c.Int());
            CreateIndex("dbo.Users", "GameRoom_Id");
            AddForeignKey("dbo.Users", "GameRoom_Id", "dbo.GameRooms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "GameRoom_Id", "dbo.GameRooms");
            DropIndex("dbo.Users", new[] { "GameRoom_Id" });
            DropColumn("dbo.Users", "GameRoom_Id");
        }
    }
}
