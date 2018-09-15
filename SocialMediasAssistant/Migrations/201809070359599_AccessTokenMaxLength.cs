namespace SocialMediasAssistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessTokenMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccessTokens", "DecryptedAccessToken", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccessTokens", "DecryptedAccessToken", c => c.String(maxLength: 300));
        }
    }
}
