namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cosPriceRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "CoursePrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "CoursePrice", c => c.String());
        }
    }
}
