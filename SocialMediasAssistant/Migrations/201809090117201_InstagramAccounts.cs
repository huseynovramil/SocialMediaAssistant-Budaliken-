namespace SocialMediasAssistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstagramAccounts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "Discriminator", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "Discriminator");
        }
    }
}
