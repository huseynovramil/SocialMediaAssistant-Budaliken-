namespace SocialMediasAssistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentPoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "Points", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "Points");
        }
    }
}
