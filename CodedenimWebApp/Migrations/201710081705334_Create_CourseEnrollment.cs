namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_CourseEnrollment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseEnrollments",
                c => new
                    {
                        CourseEnrollmentId = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        Student_StudentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CourseEnrollmentId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .Index(t => t.CourseId)
                .Index(t => t.Student_StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseEnrollments", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.CourseEnrollments", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseEnrollments", new[] { "Student_StudentId" });
            DropIndex("dbo.CourseEnrollments", new[] { "CourseId" });
            DropTable("dbo.CourseEnrollments");
        }
    }
}
