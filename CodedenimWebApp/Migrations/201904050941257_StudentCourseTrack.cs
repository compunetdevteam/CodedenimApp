namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentCourseTrack : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentCourseTracks",
                c => new
                    {
                        StudentCourseTrackId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(maxLength: 128),
                        CourseId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StudentCourseTrackId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.CourseId);
            
            AddColumn("dbo.Courses", "NoOfDays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentCourseTracks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentCourseTracks", "CourseId", "dbo.Courses");
            DropIndex("dbo.StudentCourseTracks", new[] { "CourseId" });
            DropIndex("dbo.StudentCourseTracks", new[] { "StudentId" });
            DropColumn("dbo.Courses", "NoOfDays");
            DropTable("dbo.StudentCourseTracks");
        }
    }
}
