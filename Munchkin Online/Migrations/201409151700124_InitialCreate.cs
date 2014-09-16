namespace Munchkin_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(),
                        Nickname = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        VkId = c.String(),
                        VkHash = c.String(),
                        State = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                        LastActivity = c.DateTime(nullable: false),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.GameLogEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameLogEntries", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropIndex("dbo.GameLogEntries", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "User_Id" });
            DropTable("dbo.GameLogEntries");
            DropTable("dbo.Users");
        }
    }
}
