namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesToCosandCosCat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "CourseCategoryId", "dbo.CourseCategories");
            DropIndex("dbo.Courses", new[] { "CourseCategoryId" });
            AddColumn("dbo.CourseCategories", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CourseCategories", "StudentType", c => c.String());
            DropColumn("dbo.Courses", "CourseCategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "CourseCategoryId", c => c.Int(nullable: false));
            DropColumn("dbo.CourseCategories", "StudentType");
            DropColumn("dbo.CourseCategories", "Amount");
            CreateIndex("dbo.Courses", "CourseCategoryId");
            AddForeignKey("dbo.Courses", "CourseCategoryId", "dbo.CourseCategories", "CourseCategoryId", cascadeDelete: true);
        }
    }
}
