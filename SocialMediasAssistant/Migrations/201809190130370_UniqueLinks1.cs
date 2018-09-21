namespace SocialMediasAssistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueLinks1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Contents", "Link", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Contents", new[] { "Link" });
        }
    }
}
