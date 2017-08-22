namespace CodedenimWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initalMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseCategories",
                c => new
                    {
                        CourseCategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CourseCategoryId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseCategoryId = c.Int(nullable: false),
                        CourseCode = c.String(),
                        CourseName = c.String(),
                        CourseDescription = c.String(),
                        ExpectedTime = c.Int(nullable: false),
                        DateAdded = c.DateTime(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.CourseCategories", t => t.CourseCategoryId, cascadeDelete: true)
                .Index(t => t.CourseCategoryId);
            
            CreateTable(
                "dbo.AssesmentQuestionAnswers",
                c => new
                    {
                        AssesmentQuestionAnswerId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        Question = c.String(nullable: false),
                        Option1 = c.String(),
                        Option2 = c.String(),
                        Option3 = c.String(),
                        Option4 = c.String(),
                        Answer = c.String(nullable: false),
                        QuestionHint = c.String(),
                        QuestionType = c.String(),
                        IsFillInTheGag = c.Boolean(nullable: false),
                        IsMultiChoiceAnswer = c.Boolean(nullable: false),
                        IsSingleChoiceAnswer = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AssesmentQuestionAnswerId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        ModuleId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        ModuleName = c.String(),
                        ModuleDescription = c.String(),
                        ExpectedTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ModuleId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        TopicId = c.Int(nullable: false, identity: true),
                        ModuleId = c.Int(nullable: false),
                        TopicName = c.String(),
                        ExpectedTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TopicId)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.TopicMaterialUploads",
                c => new
                    {
                        TopicMaterialUploadId = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        Tutor = c.Int(nullable: false),
                        FileType = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        FileLocation = c.String(),
                        MaterialByTutor_TutorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TopicMaterialUploadId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .ForeignKey("dbo.Tutors", t => t.MaterialByTutor_TutorId)
                .Index(t => t.TopicId)
                .Index(t => t.MaterialByTutor_TutorId);
            
            CreateTable(
                "dbo.Tutors",
                c => new
                    {
                        TutorId = c.String(nullable: false, maxLength: 128),
                        Designation = c.String(),
                        MaritalStatus = c.String(),
                        IsActiveTutor = c.Boolean(nullable: false),
                        ActiveStatus = c.String(),
                        StaffRole = c.String(),
                        FirstName = c.String(maxLength: 50),
                        MiddleName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Gender = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        TownOfBirth = c.String(),
                        StateOfOrigin = c.String(),
                        Nationality = c.String(),
                        CountryOfBirth = c.String(),
                        IsAcctive = c.Boolean(nullable: false),
                        DateOfBirth = c.DateTime(),
                        Age = c.Int(nullable: false),
                        Passport = c.Binary(),
                    })
                .PrimaryKey(t => t.TutorId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Body = c.String(),
                        TopicId = c.Int(nullable: false),
                        TutorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .ForeignKey("dbo.Tutors", t => t.TutorId)
                .Index(t => t.TopicId)
                .Index(t => t.TutorId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.QuizRules",
                c => new
                    {
                        QuizRuleId = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        ScorePerQuestion = c.Double(nullable: false),
                        TotalQuestion = c.Int(nullable: false),
                        MaximumTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuizRuleId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.StudentQuestions",
                c => new
                    {
                        StudentQuestionId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(maxLength: 128),
                        TopicId = c.Int(nullable: false),
                        Question = c.String(),
                        Option1 = c.String(),
                        Option2 = c.String(),
                        Option3 = c.String(),
                        Option4 = c.String(),
                        Check1 = c.Boolean(nullable: false),
                        Check2 = c.Boolean(nullable: false),
                        Check3 = c.Boolean(nullable: false),
                        Check4 = c.Boolean(nullable: false),
                        FilledAnswer = c.String(),
                        Answer = c.String(),
                        QuestionHint = c.String(),
                        QuestionNumber = c.Int(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                        IsFillInTheGag = c.Boolean(nullable: false),
                        IsMultiChoiceAnswer = c.Boolean(nullable: false),
                        SelectedAnswer = c.String(),
                        TotalQuestion = c.Int(nullable: false),
                        ExamTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StudentQuestionId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128),
                        EnrollmentDate = c.DateTime(),
                        ProgrammeId = c.Int(),
                        Active = c.Boolean(nullable: false),
                        IsGraduated = c.Boolean(nullable: false),
                        FirstName = c.String(maxLength: 50),
                        MiddleName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Gender = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        TownOfBirth = c.String(),
                        StateOfOrigin = c.String(),
                        Nationality = c.String(),
                        CountryOfBirth = c.String(),
                        IsAcctive = c.Boolean(nullable: false),
                        DateOfBirth = c.DateTime(),
                        Age = c.Int(nullable: false),
                        Passport = c.Binary(),
                    })
                .PrimaryKey(t => t.StudentId);
            
            CreateTable(
                "dbo.StudentAssesments",
                c => new
                    {
                        StudentAssesmentId = c.Int(nullable: false, identity: true),
                        CousreId = c.Int(nullable: false),
                        TotalScore = c.Double(nullable: false),
                        TotalQuestion = c.Int(nullable: false),
                        StudentId = c.String(maxLength: 128),
                        Course_CourseId = c.Int(),
                    })
                .PrimaryKey(t => t.StudentAssesmentId)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.Course_CourseId);
            
            CreateTable(
                "dbo.StudentTestLogs",
                c => new
                    {
                        StudentTestLogId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(maxLength: 128),
                        TopicId = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                        TotalScore = c.Double(nullable: false),
                        ExamTaken = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StudentTestLogId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.SubmitAssignments",
                c => new
                    {
                        SubmitAssignmentId = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        StudentId = c.String(maxLength: 128),
                        AssignmentBody = c.String(),
                        AttachmentLocation = c.String(),
                    })
                .PrimaryKey(t => t.SubmitAssignmentId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.AssignmentReviews",
                c => new
                    {
                        AssignmentReviewId = c.Int(nullable: false, identity: true),
                        SubmitAssignmentId = c.Int(nullable: false),
                        ReviewComment = c.String(),
                        Rating = c.String(),
                    })
                .PrimaryKey(t => t.AssignmentReviewId)
                .ForeignKey("dbo.SubmitAssignments", t => t.SubmitAssignmentId, cascadeDelete: true)
                .Index(t => t.SubmitAssignmentId);
            
            CreateTable(
                "dbo.TopicAssignments",
                c => new
                    {
                        TopicAssignmentId = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        AssignmentTitle = c.String(),
                        AssignmentDescription = c.String(),
                        AssignmentDueDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TopicAssignmentId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.TopicQuizs",
                c => new
                    {
                        TopicQuizId = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        Question = c.String(nullable: false),
                        Option1 = c.String(),
                        Option2 = c.String(),
                        Option3 = c.String(),
                        Option4 = c.String(),
                        Answer = c.String(nullable: false),
                        QuestionHint = c.String(),
                        QuestionType = c.String(),
                        IsFillInTheGag = c.Boolean(nullable: false),
                        IsMultiChoiceAnswer = c.Boolean(nullable: false),
                        IsSingleChoiceAnswer = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TopicQuizId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TagPosts",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Post_PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Post_PostId })
                .ForeignKey("dbo.Tags", t => t.Tag_TagId, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Post_PostId, cascadeDelete: true)
                .Index(t => t.Tag_TagId)
                .Index(t => t.Post_PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TopicQuizs", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.TopicAssignments", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.StudentQuestions", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.SubmitAssignments", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.SubmitAssignments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.AssignmentReviews", "SubmitAssignmentId", "dbo.SubmitAssignments");
            DropForeignKey("dbo.StudentTestLogs", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.StudentTestLogs", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentQuestions", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentAssesments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentAssesments", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.QuizRules", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Topics", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.TopicMaterialUploads", "MaterialByTutor_TutorId", "dbo.Tutors");
            DropForeignKey("dbo.Posts", "TutorId", "dbo.Tutors");
            DropForeignKey("dbo.Posts", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.TagPosts", "Post_PostId", "dbo.Posts");
            DropForeignKey("dbo.TagPosts", "Tag_TagId", "dbo.Tags");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.TopicMaterialUploads", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Modules", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "CourseCategoryId", "dbo.CourseCategories");
            DropForeignKey("dbo.AssesmentQuestionAnswers", "CourseId", "dbo.Courses");


            DropIndex("dbo.TagPosts", new[] { "Post_PostId" });
            DropIndex("dbo.TagPosts", new[] { "Tag_TagId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TopicQuizs", new[] { "TopicId" });
            DropIndex("dbo.TopicAssignments", new[] { "TopicId" });
            DropIndex("dbo.AssignmentReviews", new[] { "SubmitAssignmentId" });
            DropIndex("dbo.SubmitAssignments", new[] { "StudentId" });
            DropIndex("dbo.SubmitAssignments", new[] { "TopicId" });
            DropIndex("dbo.StudentTestLogs", new[] { "TopicId" });
            DropIndex("dbo.StudentTestLogs", new[] { "StudentId" });
            DropIndex("dbo.StudentAssesments", new[] { "Course_CourseId" });
            DropIndex("dbo.StudentAssesments", new[] { "StudentId" });
            DropIndex("dbo.StudentQuestions", new[] { "TopicId" });
            DropIndex("dbo.StudentQuestions", new[] { "StudentId" });
            DropIndex("dbo.QuizRules", new[] { "TopicId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropIndex("dbo.Posts", new[] { "TutorId" });
            DropIndex("dbo.Posts", new[] { "TopicId" });
            DropIndex("dbo.TopicMaterialUploads", new[] { "MaterialByTutor_TutorId" });
            DropIndex("dbo.TopicMaterialUploads", new[] { "TopicId" });
            DropIndex("dbo.Topics", new[] { "ModuleId" });
            DropIndex("dbo.Modules", new[] { "CourseId" });
            DropIndex("dbo.AssesmentQuestionAnswers", new[] { "CourseId" });
            DropIndex("dbo.Courses", new[] { "CourseCategoryId" });



            DropTable("dbo.TagPosts");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TopicQuizs");
            DropTable("dbo.TopicAssignments");
            DropTable("dbo.AssignmentReviews");
            DropTable("dbo.SubmitAssignments");
            DropTable("dbo.StudentTestLogs");
            DropTable("dbo.StudentAssesments");
            DropTable("dbo.Students");
            DropTable("dbo.StudentQuestions");
            DropTable("dbo.QuizRules");
            DropTable("dbo.Tags");
            DropTable("dbo.Comments");
            DropTable("dbo.Posts");
            DropTable("dbo.Tutors");
            DropTable("dbo.TopicMaterialUploads");
            DropTable("dbo.Topics");
            DropTable("dbo.Modules");
            DropTable("dbo.AssesmentQuestionAnswers");
            DropTable("dbo.Courses");
            DropTable("dbo.CourseCategories");
        }
    }
}
