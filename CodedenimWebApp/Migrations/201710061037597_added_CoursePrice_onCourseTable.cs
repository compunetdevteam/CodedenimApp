namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_CoursePrice_onCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CoursePrice", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "CoursePrice");
        }
    }
}
