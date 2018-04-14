namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paypalImplementation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentPaypalPayments",
                c => new
                    {
                        StudentPaypalPaymentId = c.Int(nullable: false, identity: true),
                        PaymentStatus = c.String(),
                        PayerFirstName = c.String(),
                        PayerLastName = c.String(),
                        Amount = c.String(),
                        TxToken = c.String(),
                        ReceiverEmail = c.String(),
                        ItemName = c.String(),
                        Currency = c.String(),
                        PayerEmail = c.String(),
                        PaymentDate = c.String(),
                        CourseCategoryId = c.Int(nullable: false),
                        StudentId = c.String(maxLength: 128),
                        PayerId = c.String(),
                    })
                .PrimaryKey(t => t.StudentPaypalPaymentId)
                .ForeignKey("dbo.CourseCategories", t => t.CourseCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.CourseCategoryId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentPaypalPayments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentPaypalPayments", "CourseCategoryId", "dbo.CourseCategories");
            DropIndex("dbo.StudentPaypalPayments", new[] { "StudentId" });
            DropIndex("dbo.StudentPaypalPayments", new[] { "CourseCategoryId" });
            DropTable("dbo.StudentPaypalPayments");
        }
    }
}
