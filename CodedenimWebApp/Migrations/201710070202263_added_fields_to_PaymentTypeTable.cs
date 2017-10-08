namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_fields_to_PaymentTypeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentTypes", "Email", c => c.String());
            AddColumn("dbo.PaymentTypes", "CoursePayedFor", c => c.String());
            AddColumn("dbo.PaymentTypes", "PaymentDate", c => c.String());
            AddColumn("dbo.PaymentTypes", "PayStackCustomerId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentTypes", "PayStackCustomerId");
            DropColumn("dbo.PaymentTypes", "PaymentDate");
            DropColumn("dbo.PaymentTypes", "CoursePayedFor");
            DropColumn("dbo.PaymentTypes", "Email");
        }
    }
}
