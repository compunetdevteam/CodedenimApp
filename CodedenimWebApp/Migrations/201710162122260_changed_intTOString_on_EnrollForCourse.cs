namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed_intTOString_on_EnrollForCourse : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.EnrollForCourses", new[] { "Student_StudentId" });
            DropColumn("dbo.EnrollForCourses", "StudentId");
            RenameColumn(table: "dbo.EnrollForCourses", name: "Student_StudentId", newName: "StudentId");
            AlterColumn("dbo.EnrollForCourses", "StudentId", c => c.String(maxLength: 128));
            CreateIndex("dbo.EnrollForCourses", "StudentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.EnrollForCourses", new[] { "StudentId" });
            AlterColumn("dbo.EnrollForCourses", "StudentId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.EnrollForCourses", name: "StudentId", newName: "Student_StudentId");
            AddColumn("dbo.EnrollForCourses", "StudentId", c => c.Int(nullable: false));
            CreateIndex("dbo.EnrollForCourses", "Student_StudentId");
        }
    }
}
