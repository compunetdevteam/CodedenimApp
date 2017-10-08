namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Commented_theEmail_IsuniqueAttr : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Students", new[] { "Email" });
            DropIndex("dbo.Tutors", new[] { "Email" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Tutors", "Email", unique: true);
            CreateIndex("dbo.Students", "Email", unique: true);
        }
    }
}
