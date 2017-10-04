namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedFileLocationToPersonTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "FileLocation", c => c.String());
            AddColumn("dbo.Tutors", "FileLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tutors", "FileLocation");
            DropColumn("dbo.Students", "FileLocation");
        }
    }
}
