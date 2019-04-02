namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPropertiesToEnrolledForCoursed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EnrollForCourses", "DateStarted", c => c.DateTime(nullable: false));
            AddColumn("dbo.EnrollForCourses", "hasCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EnrollForCourses", "hasCompleted");
            DropColumn("dbo.EnrollForCourses", "DateStarted");
        }
    }
}
