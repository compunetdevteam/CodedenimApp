namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeToDateTimeOffset : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForumQuestions", "PostDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForumQuestions", "PostDate", c => c.DateTime(nullable: false));
        }
    }
}
