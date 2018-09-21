namespace SocialMediasAssistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueLinks : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contents", "Link", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contents", "Link", c => c.String());
        }
    }
}
