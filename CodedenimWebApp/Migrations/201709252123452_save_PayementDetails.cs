namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class save_PayementDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfessionalPayments",
                c => new
                    {
                        ProfessionalPaymentId = c.Int(nullable: false, identity: true),
                        ProfessionalWorkerId = c.String(),
                        PaymentDateTime = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPayed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProfessionalPaymentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProfessionalPayments");
        }
    }
}
