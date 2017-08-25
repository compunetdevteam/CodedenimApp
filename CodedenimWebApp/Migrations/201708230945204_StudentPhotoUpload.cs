namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentPhotoUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Student_StudentId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Files", "Student_StudentId");
            AddForeignKey("dbo.Files", "Student_StudentId", "dbo.Students", "StudentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "Student_StudentId", "dbo.Students");
            DropIndex("dbo.Files", new[] { "Student_StudentId" });
            DropColumn("dbo.Files", "Student_StudentId");
        }
    }
}
