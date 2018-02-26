namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vidoLocationOnCourses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "VideoLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "VideoLocation");
        }
    }
}
