namespace SocialMediasAssistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueLinks2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Contents", new[] { "Link" });
            AlterColumn("dbo.Contents", "Link", c => c.String(maxLength: 100));
            CreateIndex("dbo.Contents", "Link", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Contents", new[] { "Link" });
            AlterColumn("dbo.Contents", "Link", c => c.String(maxLength: 50));
            CreateIndex("dbo.Contents", "Link", unique: true);
        }
    }
}
