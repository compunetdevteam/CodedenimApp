namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedSomeThings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCourseDetails",
                c => new
                    {
                        UserCourseDetailId = c.String(nullable: false, maxLength: 128),
                        UserCourseName = c.String(),
                        CourseCategory = c.String(),
                        CourseId = c.Int(nullable: false),
                        PaymentDate = c.String(),
                    })
                .PrimaryKey(t => t.UserCourseDetailId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserCourseDetails");
        }
    }
}
