namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIntArrayIn_assignCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssignCourseCategories", "Courses_CourseId", "dbo.Courses");
            DropIndex("dbo.AssignCourseCategories", new[] { "Courses_CourseId" });
            RenameColumn(table: "dbo.AssignCourseCategories", name: "Courses_CourseId", newName: "CourseId");
            AlterColumn("dbo.AssignCourseCategories", "CourseId", c => c.Int(nullable: false));
            CreateIndex("dbo.AssignCourseCategories", "CourseId");
            AddForeignKey("dbo.AssignCourseCategories", "CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignCourseCategories", "CourseId", "dbo.Courses");
            DropIndex("dbo.AssignCourseCategories", new[] { "CourseId" });
            AlterColumn("dbo.AssignCourseCategories", "CourseId", c => c.Int());
            RenameColumn(table: "dbo.AssignCourseCategories", name: "CourseId", newName: "Courses_CourseId");
            CreateIndex("dbo.AssignCourseCategories", "Courses_CourseId");
            AddForeignKey("dbo.AssignCourseCategories", "Courses_CourseId", "dbo.Courses", "CourseId");
        }
    }
}
