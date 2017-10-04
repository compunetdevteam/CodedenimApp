namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDateToForumQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForumQuestions", "PostDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForumQuestions", "PostDate");
        }
    }
}
