namespace SocialMediasAssistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablePerConcrete : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InstagramPosts",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Contents", t => t.ID)
                .Index(t => t.ID);
            
            DropColumn("dbo.Contents", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contents", "Discriminator", c => c.String(maxLength: 128));
            DropForeignKey("dbo.InstagramPosts", "ID", "dbo.Contents");
            DropIndex("dbo.InstagramPosts", new[] { "ID" });
            DropTable("dbo.InstagramPosts");
        }
    }
}
