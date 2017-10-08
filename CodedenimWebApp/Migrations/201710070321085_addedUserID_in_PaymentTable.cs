namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUserID_in_PaymentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfessionalPayments", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfessionalPayments", "UserId");
        }
    }
}
