namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedCategoryFromEnrollForCoursesTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EnrollForCourses", "CourseCategoryId", "dbo.CourseCategories");
            DropIndex("dbo.EnrollForCourses", new[] { "CourseCategoryId" });
            RenameColumn(table: "dbo.EnrollForCourses", name: "CourseCategoryId", newName: "CourseCategory_CourseCategoryId");
            AlterColumn("dbo.EnrollForCourses", "CourseCategory_CourseCategoryId", c => c.Int());
            CreateIndex("dbo.EnrollForCourses", "CourseCategory_CourseCategoryId");
            AddForeignKey("dbo.EnrollForCourses", "CourseCategory_CourseCategoryId", "dbo.CourseCategories", "CourseCategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EnrollForCourses", "CourseCategory_CourseCategoryId", "dbo.CourseCategories");
            DropIndex("dbo.EnrollForCourses", new[] { "CourseCategory_CourseCategoryId" });
            AlterColumn("dbo.EnrollForCourses", "CourseCategory_CourseCategoryId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.EnrollForCourses", name: "CourseCategory_CourseCategoryId", newName: "CourseCategoryId");
            CreateIndex("dbo.EnrollForCourses", "CourseCategoryId");
            AddForeignKey("dbo.EnrollForCourses", "CourseCategoryId", "dbo.CourseCategories", "CourseCategoryId", cascadeDelete: true);
        }
    }
}
