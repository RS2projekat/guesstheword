namespace GTW_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6Migration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameRooms", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "GameRoom_Id", "dbo.GameRooms");
            DropForeignKey("dbo.GameRooms", "Winner_Id", "dbo.Users");
            DropIndex("dbo.GameRooms", new[] { "User_Id" });
            DropIndex("dbo.GameRooms", new[] { "Winner_Id" });
            DropIndex("dbo.Users", new[] { "GameRoom_Id" });
            AddColumn("dbo.GameRooms", "WinnerId", c => c.Int(nullable: false));
            DropColumn("dbo.GameRooms", "User_Id");
            DropColumn("dbo.GameRooms", "Winner_Id");
            DropColumn("dbo.Users", "GameRoom_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "GameRoom_Id", c => c.Int());
            AddColumn("dbo.GameRooms", "Winner_Id", c => c.Int());
            AddColumn("dbo.GameRooms", "User_Id", c => c.Int());
            DropColumn("dbo.GameRooms", "WinnerId");
            CreateIndex("dbo.Users", "GameRoom_Id");
            CreateIndex("dbo.GameRooms", "Winner_Id");
            CreateIndex("dbo.GameRooms", "User_Id");
            AddForeignKey("dbo.GameRooms", "Winner_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Users", "GameRoom_Id", "dbo.GameRooms", "Id");
            AddForeignKey("dbo.GameRooms", "User_Id", "dbo.Users", "Id");
        }
    }
}
