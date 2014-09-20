namespace Munchkin_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordHash", c => c.String());
            DropColumn("dbo.Users", "Password");
            DropColumn("dbo.Users", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "State", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Password", c => c.String());
            DropColumn("dbo.Users", "PasswordHash");
        }
    }
}
