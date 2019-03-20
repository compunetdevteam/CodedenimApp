namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSummerNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TopicMaterialUploads", "TextContent", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TopicMaterialUploads", "TextContent");
        }
    }
}
