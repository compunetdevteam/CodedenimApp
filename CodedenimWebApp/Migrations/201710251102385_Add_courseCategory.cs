namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_courseCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CorperEnrolledCourses", "CourseCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.CorperEnrolledCourses", "CourseCategoryId");
            AddForeignKey("dbo.CorperEnrolledCourses", "CourseCategoryId", "dbo.CourseCategories", "CourseCategoryId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CorperEnrolledCourses", "CourseCategoryId", "dbo.CourseCategories");
            DropIndex("dbo.CorperEnrolledCourses", new[] { "CourseCategoryId" });
            DropColumn("dbo.CorperEnrolledCourses", "CourseCategoryId");
        }
    }
}
