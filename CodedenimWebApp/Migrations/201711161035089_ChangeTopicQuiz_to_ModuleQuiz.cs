namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTopicQuiz_to_ModuleQuiz : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuizLogs", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.StudentTopicQuizs", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.QuizRules", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.TopicQuizs", "TopicId", "dbo.Topics");
            DropIndex("dbo.QuizRules", new[] { "TopicId" });
            DropIndex("dbo.TopicQuizs", new[] { "TopicId" });
            DropIndex("dbo.QuizLogs", new[] { "TopicId" });
            DropIndex("dbo.StudentTopicQuizs", new[] { "TopicId" });
            RenameColumn(table: "dbo.QuizRules", name: "TopicId", newName: "Topic_TopicId");
            RenameColumn(table: "dbo.TopicQuizs", name: "TopicId", newName: "Topic_TopicId");
            AddColumn("dbo.QuizRules", "ModuleId", c => c.Int(nullable: false));
            AddColumn("dbo.TopicQuizs", "ModuleId", c => c.Int(nullable: false));
            AddColumn("dbo.QuizLogs", "ModuleId", c => c.Int(nullable: false));
            AddColumn("dbo.StudentTopicQuizs", "ModuleId", c => c.Int(nullable: false));
            AlterColumn("dbo.QuizRules", "Topic_TopicId", c => c.Int());
            AlterColumn("dbo.TopicQuizs", "Topic_TopicId", c => c.Int());
            CreateIndex("dbo.QuizRules", "ModuleId");
            CreateIndex("dbo.QuizRules", "Topic_TopicId");
            CreateIndex("dbo.TopicQuizs", "ModuleId");
            CreateIndex("dbo.TopicQuizs", "Topic_TopicId");
            CreateIndex("dbo.QuizLogs", "ModuleId");
            CreateIndex("dbo.StudentTopicQuizs", "ModuleId");
            AddForeignKey("dbo.QuizRules", "ModuleId", "dbo.Modules", "ModuleId", cascadeDelete: true);
            AddForeignKey("dbo.TopicQuizs", "ModuleId", "dbo.Modules", "ModuleId", cascadeDelete: true);
            AddForeignKey("dbo.QuizLogs", "ModuleId", "dbo.Modules", "ModuleId", cascadeDelete: true);
            AddForeignKey("dbo.StudentTopicQuizs", "ModuleId", "dbo.Modules", "ModuleId", cascadeDelete: true);
            AddForeignKey("dbo.QuizRules", "Topic_TopicId", "dbo.Topics", "TopicId");
            AddForeignKey("dbo.TopicQuizs", "Topic_TopicId", "dbo.Topics", "TopicId");
            DropColumn("dbo.QuizLogs", "TopicId");
            DropColumn("dbo.StudentTopicQuizs", "TopicId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentTopicQuizs", "TopicId", c => c.Int(nullable: false));
            AddColumn("dbo.QuizLogs", "TopicId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TopicQuizs", "Topic_TopicId", "dbo.Topics");
            DropForeignKey("dbo.QuizRules", "Topic_TopicId", "dbo.Topics");
            DropForeignKey("dbo.StudentTopicQuizs", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.QuizLogs", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.TopicQuizs", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.QuizRules", "ModuleId", "dbo.Modules");
            DropIndex("dbo.StudentTopicQuizs", new[] { "ModuleId" });
            DropIndex("dbo.QuizLogs", new[] { "ModuleId" });
            DropIndex("dbo.TopicQuizs", new[] { "Topic_TopicId" });
            DropIndex("dbo.TopicQuizs", new[] { "ModuleId" });
            DropIndex("dbo.QuizRules", new[] { "Topic_TopicId" });
            DropIndex("dbo.QuizRules", new[] { "ModuleId" });
            AlterColumn("dbo.TopicQuizs", "Topic_TopicId", c => c.Int(nullable: false));
            AlterColumn("dbo.QuizRules", "Topic_TopicId", c => c.Int(nullable: false));
            DropColumn("dbo.StudentTopicQuizs", "ModuleId");
            DropColumn("dbo.QuizLogs", "ModuleId");
            DropColumn("dbo.TopicQuizs", "ModuleId");
            DropColumn("dbo.QuizRules", "ModuleId");
            RenameColumn(table: "dbo.TopicQuizs", name: "Topic_TopicId", newName: "TopicId");
            RenameColumn(table: "dbo.QuizRules", name: "Topic_TopicId", newName: "TopicId");
            CreateIndex("dbo.StudentTopicQuizs", "TopicId");
            CreateIndex("dbo.QuizLogs", "TopicId");
            CreateIndex("dbo.TopicQuizs", "TopicId");
            CreateIndex("dbo.QuizRules", "TopicId");
            AddForeignKey("dbo.TopicQuizs", "TopicId", "dbo.Topics", "TopicId", cascadeDelete: true);
            AddForeignKey("dbo.QuizRules", "TopicId", "dbo.Topics", "TopicId", cascadeDelete: true);
            AddForeignKey("dbo.StudentTopicQuizs", "TopicId", "dbo.Topics", "TopicId", cascadeDelete: true);
            AddForeignKey("dbo.QuizLogs", "TopicId", "dbo.Topics", "TopicId", cascadeDelete: true);
        }
    }
}
