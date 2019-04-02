namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedCoursesTotheEnrollCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EnrollForCourses", "CourseId", c => c.Int(nullable: false));
            CreateIndex("dbo.EnrollForCourses", "CourseId");
            AddForeignKey("dbo.EnrollForCourses", "CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EnrollForCourses", "CourseId", "dbo.Courses");
            DropIndex("dbo.EnrollForCourses", new[] { "CourseId" });
            DropColumn("dbo.EnrollForCourses", "CourseId");
        }
    }
}
