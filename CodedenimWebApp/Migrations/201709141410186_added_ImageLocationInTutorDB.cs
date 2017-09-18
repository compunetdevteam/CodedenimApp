namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_ImageLocationInTutorDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tutors", "imageLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tutors", "imageLocation");
        }
    }
}
