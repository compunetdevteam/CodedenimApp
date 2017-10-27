namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_courseDescriptionToCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseCategories", "CategoryDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourseCategories", "CategoryDescription");
        }
    }
}
