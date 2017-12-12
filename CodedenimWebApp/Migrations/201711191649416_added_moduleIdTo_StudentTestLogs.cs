namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_moduleIdTo_StudentTestLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentTestLogs", "ModuleId", c => c.Int(nullable: false));
            CreateIndex("dbo.StudentTestLogs", "ModuleId");
            AddForeignKey("dbo.StudentTestLogs", "ModuleId", "dbo.Modules", "ModuleId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentTestLogs", "ModuleId", "dbo.Modules");
            DropIndex("dbo.StudentTestLogs", new[] { "ModuleId" });
            DropColumn("dbo.StudentTestLogs", "ModuleId");
        }
    }
}
