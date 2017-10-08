namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeFieldsOnPaymentType_Mistake : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PaymentTypes", "Email");
            DropColumn("dbo.PaymentTypes", "CoursePayedFor");
            DropColumn("dbo.PaymentTypes", "PaymentDate");
            DropColumn("dbo.PaymentTypes", "PayStackCustomerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentTypes", "PayStackCustomerId", c => c.String());
            AddColumn("dbo.PaymentTypes", "PaymentDate", c => c.String());
            AddColumn("dbo.PaymentTypes", "CoursePayedFor", c => c.String());
            AddColumn("dbo.PaymentTypes", "Email", c => c.String());
        }
    }
}
