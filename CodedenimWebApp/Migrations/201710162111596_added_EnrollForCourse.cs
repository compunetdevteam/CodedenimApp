namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_EnrollForCourse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EnrollForCourses",
                c => new
                    {
                        EnrollForCourseId = c.Int(nullable: false, identity: true),
                        CourseCategoryId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Student_StudentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EnrollForCourseId)
                .ForeignKey("dbo.CourseCategories", t => t.CourseCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .Index(t => t.CourseCategoryId)
                .Index(t => t.Student_StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EnrollForCourses", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.EnrollForCourses", "CourseCategoryId", "dbo.CourseCategories");
            DropIndex("dbo.EnrollForCourses", new[] { "Student_StudentId" });
            DropIndex("dbo.EnrollForCourses", new[] { "CourseCategoryId" });
            DropTable("dbo.EnrollForCourses");
        }
    }
}
