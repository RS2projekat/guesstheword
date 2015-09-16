namespace GTW_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        Word = c.String(),
                        WinnerId = c.Int(nullable: false),
                        PainterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Role = c.String(),
                        GameRoom_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameRooms", t => t.GameRoom_Id)
                .Index(t => t.GameRoom_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "GameRoom_Id", "dbo.GameRooms");
            DropIndex("dbo.Users", new[] { "GameRoom_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.GameRooms");
        }
    }
}
