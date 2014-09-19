namespace Munchkin_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PassHash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PassHash", c => c.String());
            DropColumn("dbo.Users", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Password", c => c.String());
            DropColumn("dbo.Users", "PassHash");
        }
    }
}
