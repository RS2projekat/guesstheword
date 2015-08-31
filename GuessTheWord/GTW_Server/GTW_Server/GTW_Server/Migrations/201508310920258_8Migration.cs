namespace GTW_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameRooms", "PainterId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameRooms", "PainterId");
        }
    }
}
