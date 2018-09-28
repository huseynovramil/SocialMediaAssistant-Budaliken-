namespace SocialMediasAssistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PointsDefaultValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Points", c => c.Int(defaultValue:10, defaultValueSql:"10"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Points", c => c.Int(nullable: false));
        }
    }
}
