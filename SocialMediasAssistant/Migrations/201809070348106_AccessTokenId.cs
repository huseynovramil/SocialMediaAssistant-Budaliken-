namespace SocialMediasAssistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessTokenId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AccessTokens");
            AddColumn("dbo.AccessTokens", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AccessTokens", "DecryptedAccessToken", c => c.String(maxLength: 300));
            AddPrimaryKey("dbo.AccessTokens", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AccessTokens");
            AlterColumn("dbo.AccessTokens", "DecryptedAccessToken", c => c.String(nullable: false, maxLength: 300));
            DropColumn("dbo.AccessTokens", "Id");
            AddPrimaryKey("dbo.AccessTokens", "DecryptedAccessToken");
        }
    }
}
