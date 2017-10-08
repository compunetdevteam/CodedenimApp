namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_fields_to_ProfesionalPaymentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfessionalPayments", "Email", c => c.String());
            AddColumn("dbo.ProfessionalPayments", "CoursePayedFor", c => c.String());
            AddColumn("dbo.ProfessionalPayments", "PaymentDate", c => c.String());
            AddColumn("dbo.ProfessionalPayments", "PayStackCustomerId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfessionalPayments", "PayStackCustomerId");
            DropColumn("dbo.ProfessionalPayments", "PaymentDate");
            DropColumn("dbo.ProfessionalPayments", "CoursePayedFor");
            DropColumn("dbo.ProfessionalPayments", "Email");
        }
    }
}
