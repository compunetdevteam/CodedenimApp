namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodedenimTestQuestion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestQuestions",
                c => new
                    {
                        TestQuestionId = c.Int(nullable: false, identity: true),
                        QuestionContent = c.String(),
                        QuestionInstruction = c.String(),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TestQuestionId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestQuestions", "CourseId", "dbo.Courses");
            DropIndex("dbo.TestQuestions", new[] { "CourseId" });
            DropTable("dbo.TestQuestions");
        }
    }
}
