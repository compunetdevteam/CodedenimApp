namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class corperEnrollments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CorperEnrolledCourses",
                c => new
                    {
                        CorperEnrolledCoursesId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        CorperCallUpNumber = c.String(),
                        StudentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CorperEnrolledCoursesId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId);
            
            AddColumn("dbo.Courses", "CorperEnrolledCourses_CorperEnrolledCoursesId", c => c.Int());
            CreateIndex("dbo.Courses", "CorperEnrolledCourses_CorperEnrolledCoursesId");
            AddForeignKey("dbo.Courses", "CorperEnrolledCourses_CorperEnrolledCoursesId", "dbo.CorperEnrolledCourses", "CorperEnrolledCoursesId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CorperEnrolledCourses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Courses", "CorperEnrolledCourses_CorperEnrolledCoursesId", "dbo.CorperEnrolledCourses");
            DropIndex("dbo.Courses", new[] { "CorperEnrolledCourses_CorperEnrolledCoursesId" });
            DropIndex("dbo.CorperEnrolledCourses", new[] { "StudentId" });
            DropColumn("dbo.Courses", "CorperEnrolledCourses_CorperEnrolledCoursesId");
            DropTable("dbo.CorperEnrolledCourses");
        }
    }
}
