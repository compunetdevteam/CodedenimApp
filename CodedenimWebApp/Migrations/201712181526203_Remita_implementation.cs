namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remita_implementation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RemitaPaymentLogs",
                c => new
                    {
                        RemitaPaymentLogId = c.Int(nullable: false, identity: true),
                        OrderId = c.String(),
                        StatusCode = c.String(),
                        TransactionMessage = c.String(),
                        Rrr = c.String(),
                        PaymentName = c.String(),
                        PaymentDate = c.DateTime(nullable: false),
                        Amount = c.String(),
                        PayerName = c.String(),
                    })
                .PrimaryKey(t => t.RemitaPaymentLogId);
            
            AddColumn("dbo.StudentPayments", "OrderId", c => c.String());
            AddColumn("dbo.StudentPayments", "PaymentStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentPayments", "PaymentStatus");
            DropColumn("dbo.StudentPayments", "OrderId");
            DropTable("dbo.RemitaPaymentLogs");
        }
    }
}
