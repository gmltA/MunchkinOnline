namespace Munchkin_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeVKUserData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "VkAccessToken", c => c.String());
            DropColumn("dbo.Users", "VkId");
            DropColumn("dbo.Users", "VkHash");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "VkHash", c => c.String());
            AddColumn("dbo.Users", "VkId", c => c.String());
            DropColumn("dbo.Users", "VkAccessToken");
        }
    }
}
