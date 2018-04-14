namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodedenimTestAnswer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestAnswers",
                c => new
                    {
                        TestAnswerId = c.Int(nullable: false, identity: true),
                        TestAnswerContent = c.String(),
                        hasAnswered = c.Boolean(nullable: false),
                        DateSubmited = c.DateTimeOffset(nullable: false, precision: 7),
                        TestQuestionId = c.Int(nullable: false),
                        StudentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TestAnswerId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.TestQuestions", t => t.TestQuestionId, cascadeDelete: true)
                .Index(t => t.TestQuestionId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestAnswers", "TestQuestionId", "dbo.TestQuestions");
            DropForeignKey("dbo.TestAnswers", "StudentId", "dbo.Students");
            DropIndex("dbo.TestAnswers", new[] { "StudentId" });
            DropIndex("dbo.TestAnswers", new[] { "TestQuestionId" });
            DropTable("dbo.TestAnswers");
        }
    }
}
