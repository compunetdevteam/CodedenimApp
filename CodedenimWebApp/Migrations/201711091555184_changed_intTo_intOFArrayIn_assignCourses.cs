namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed_intTo_intOFArrayIn_assignCourses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssignCourseCategories", "CourseId", "dbo.Courses");
            DropIndex("dbo.AssignCourseCategories", new[] { "CourseId" });
            RenameColumn(table: "dbo.AssignCourseCategories", name: "CourseId", newName: "Courses_CourseId");
            AlterColumn("dbo.AssignCourseCategories", "Courses_CourseId", c => c.Int());
            CreateIndex("dbo.AssignCourseCategories", "Courses_CourseId");
            AddForeignKey("dbo.AssignCourseCategories", "Courses_CourseId", "dbo.Courses", "CourseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignCourseCategories", "Courses_CourseId", "dbo.Courses");
            DropIndex("dbo.AssignCourseCategories", new[] { "Courses_CourseId" });
            AlterColumn("dbo.AssignCourseCategories", "Courses_CourseId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.AssignCourseCategories", name: "Courses_CourseId", newName: "CourseId");
            CreateIndex("dbo.AssignCourseCategories", "CourseId");
            AddForeignKey("dbo.AssignCourseCategories", "CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
        }
    }
}
