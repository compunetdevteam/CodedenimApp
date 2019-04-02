namespace CodedenimWebApp.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignCourseCategories",
                c => new
                {
                    AssignCourseCategoryId = c.Int(nullable: false, identity: true),
                    CourseId = c.Int(nullable: false),
                    CourseCategoryId = c.Int(nullable: false),
                    StudentAssignedCourse_StudentAssignedCourseId = c.Int(),
                })
                .PrimaryKey(t => t.AssignCourseCategoryId)
                .ForeignKey("dbo.CourseCategories", t => t.CourseCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.StudentAssignedCourses", t => t.StudentAssignedCourse_StudentAssignedCourseId)
                .Index(t => t.CourseId)
                .Index(t => t.CourseCategoryId)
                .Index(t => t.StudentAssignedCourse_StudentAssignedCourseId);

            CreateTable(
                "dbo.CourseCategories",
                c => new
                {
                    CourseCategoryId = c.Int(nullable: false, identity: true),
                    CategoryName = c.String(),
                    Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    StudentType = c.String(),
                    CategoryDescription = c.String(),
                    ImageLocation = c.String(),
                })
                .PrimaryKey(t => t.CourseCategoryId);

            CreateTable(
                "dbo.EnrollForCourses",
                c => new
                {
                    EnrollForCourseId = c.Int(nullable: false, identity: true),
                    CourseCategoryId = c.Int(nullable: false),
                    StudentId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.EnrollForCourseId)
                .ForeignKey("dbo.CourseCategories", t => t.CourseCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.CourseCategoryId)
                .Index(t => t.StudentId);

            CreateTable(
                "dbo.Students",
                c => new
                {
                    StudentId = c.String(nullable: false, maxLength: 128),
                    MatricNo = c.String(),
                    CallUpNo = c.String(),
                    EnrollmentDate = c.DateTime(),
                    Active = c.Boolean(nullable: false),
                    Title = c.String(),
                    AccountType = c.String(),
                    IsGraduated = c.Boolean(nullable: false),
                    Institution = c.String(),
                    StateOfService = c.String(),
                    Batch = c.String(),
                    Discpline = c.String(),
                    FirstName = c.String(maxLength: 50),
                    MiddleName = c.String(maxLength: 50),
                    LastName = c.String(maxLength: 50),
                    Gender = c.String(),
                    Email = c.String(maxLength: 100),
                    PhoneNumber = c.String(),
                    TownOfBirth = c.String(),
                    StateOfOrigin = c.String(),
                    Nationality = c.String(),
                    CountryOfBirth = c.String(),
                    IsAcctive = c.Boolean(nullable: false),
                    DateOfBirth = c.DateTime(),
                    Age = c.Int(),
                    Passport = c.Binary(),
                    FileLocation = c.String(),
                })
                .PrimaryKey(t => t.StudentId);

            CreateTable(
                "dbo.StudentAssignedCourses",
                c => new
                {
                    StudentAssignedCourseId = c.Int(nullable: false, identity: true),
                    StudentId = c.String(maxLength: 128),
                    CourseId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.StudentAssignedCourseId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.CourseId);

            CreateTable(
                "dbo.Courses",
                c => new
                {
                    CourseId = c.Int(nullable: false, identity: true),
                    CourseCode = c.String(),
                    CourseName = c.String(),
                    CourseDescription = c.String(),
                    ExpectedTime = c.Int(nullable: false),
                    DateAdded = c.DateTime(),
                    Points = c.Int(nullable: false),
                    CourseImage = c.Binary(),
                    FileLocation = c.String(),
                    VideoLocation = c.String(),
                    CorperEnrolledCourses_CorperEnrolledCoursesId = c.Int(),
                })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.CorperEnrolledCourses", t => t.CorperEnrolledCourses_CorperEnrolledCoursesId)
                .Index(t => t.CorperEnrolledCourses_CorperEnrolledCoursesId);

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
                "dbo.CourseEnrollments",
                c => new
                {
                    CourseEnrollmentId = c.Int(nullable: false, identity: true),
                    StudentId = c.Int(nullable: false),
                    CourseId = c.Int(nullable: false),
                    HasStartCourse = c.Boolean(nullable: false),
                    HasEndCourse = c.Boolean(nullable: false),
                    Student_StudentId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.CourseEnrollmentId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .Index(t => t.CourseId)
                .Index(t => t.Student_StudentId);

            CreateTable(
                "dbo.Fora",
                c => new
                {
                    CourseId = c.Int(nullable: false),
                    ForumName = c.String(),
                    Description = c.String(),
                    LastPosted = c.DateTime(nullable: false),
                    CreationDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId);

            CreateTable(
                "dbo.ForumQuestions",
                c => new
                {
                    ForumQuestionId = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    QuestionName = c.String(),
                    PostDate = c.DateTimeOffset(nullable: false, precision: 7),
                    CourseId = c.Int(nullable: false),
                    StudentId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.ForumQuestionId)
                .ForeignKey("dbo.Fora", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.CourseId)
                .Index(t => t.StudentId);

            CreateTable(
                "dbo.ForumAnswers",
                c => new
                {
                    ForumAnswerId = c.Int(nullable: false, identity: true),
                    Answer = c.String(),
                    ReplyDate = c.DateTime(nullable: false),
                    ForumQuestionId = c.Int(nullable: false),
                    UserId = c.String(),
                })
                .PrimaryKey(t => t.ForumAnswerId)
                .ForeignKey("dbo.ForumQuestions", t => t.ForumQuestionId, cascadeDelete: true)
                .Index(t => t.ForumQuestionId);

            CreateTable(
                "dbo.VoteForumAnswers",
                c => new
                {
                    VoteForumAnswerId = c.Int(nullable: false, identity: true),
                    ForumAnswerId = c.Int(nullable: false),
                    Vote = c.Int(nullable: false),
                    UserId = c.String(),
                })
                .PrimaryKey(t => t.VoteForumAnswerId);

            CreateTable(
                "dbo.ForumQuestionViews",
                c => new
                {
                    ForumQuestionId = c.Int(nullable: false),
                    ViewCounter = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ForumQuestionId)
                .ForeignKey("dbo.ForumQuestions", t => t.ForumQuestionId)
                .Index(t => t.ForumQuestionId);

            CreateTable(
                "dbo.ForumViews",
                c => new
                {
                    ContentViewId = c.Int(nullable: false),
                    ViewCounter = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ContentViewId)
                .ForeignKey("dbo.Fora", t => t.ContentViewId)
                .Index(t => t.ContentViewId);

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
                "dbo.StudentTestLogs",
                c => new
                {
                    StudentTestLogId = c.Int(nullable: false, identity: true),
                    StudentId = c.String(maxLength: 128),
                    ModuleId = c.Int(nullable: false),
                    Score = c.Double(nullable: false),
                    TotalScore = c.Double(nullable: false),
                    ExamTaken = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.StudentTestLogId)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.ModuleId);

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
                    Tutor = c.String(),
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
                    ImageLocation = c.String(),
                    FirstName = c.String(maxLength: 50),
                    MiddleName = c.String(maxLength: 50),
                    LastName = c.String(maxLength: 50),
                    Gender = c.String(),
                    Email = c.String(maxLength: 100),
                    PhoneNumber = c.String(),
                    TownOfBirth = c.String(),
                    StateOfOrigin = c.String(),
                    Nationality = c.String(),
                    CountryOfBirth = c.String(),
                    IsAcctive = c.Boolean(nullable: false),
                    DateOfBirth = c.DateTime(),
                    Age = c.Int(),
                    Passport = c.Binary(),
                    FileLocation = c.String(),
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
                "dbo.TutorCourses",
                c => new
                {
                    TutorCoursesId = c.Int(nullable: false, identity: true),
                    TutorId = c.String(maxLength: 128),
                    CourseId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.TutorCoursesId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Tutors", t => t.TutorId)
                .Index(t => t.TutorId)
                .Index(t => t.CourseId);

            CreateTable(
                "dbo.QuizRules",
                c => new
                {
                    QuizRuleId = c.Int(nullable: false, identity: true),
                    ModuleId = c.Int(nullable: false),
                    ScorePerQuestion = c.Double(nullable: false),
                    TotalQuestion = c.Int(nullable: false),
                    MaximumTime = c.Int(nullable: false),
                    Topic_TopicId = c.Int(),
                })
                .PrimaryKey(t => t.QuizRuleId)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Topics", t => t.Topic_TopicId)
                .Index(t => t.ModuleId)
                .Index(t => t.Topic_TopicId);

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
                    ModuleId = c.Int(nullable: false),
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
                    Topic_TopicId = c.Int(),
                })
                .PrimaryKey(t => t.TopicQuizId)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Topics", t => t.Topic_TopicId)
                .Index(t => t.ModuleId)
                .Index(t => t.Topic_TopicId);

            CreateTable(
                "dbo.Enrollments",
                c => new
                {
                    EnrollmentID = c.Int(nullable: false, identity: true),
                    CourseID = c.Int(nullable: false),
                    StudentID = c.Int(nullable: false),
                    Grade = c.Int(),
                    Student_StudentId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.EnrollmentID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .Index(t => t.CourseID)
                .Index(t => t.Student_StudentId);

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
                "dbo.StudentPayments",
                c => new
                {
                    StudentPaymentId = c.Int(nullable: false, identity: true),
                    OrderId = c.String(),
                    CourseCategoryId = c.Int(nullable: false),
                    StudentId = c.String(maxLength: 128),
                    PaymentDateTime = c.DateTime(nullable: false),
                    Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsPayed = c.Boolean(nullable: false),
                    ReferenceNo = c.String(),
                    PaymentStatus = c.String(),
                })
                .PrimaryKey(t => t.StudentPaymentId)
                .ForeignKey("dbo.CourseCategories", t => t.CourseCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.CourseCategoryId)
                .Index(t => t.StudentId);

            CreateTable(
                "dbo.StudentPaypalPayments",
                c => new
                {
                    StudentPaypalPaymentId = c.Int(nullable: false, identity: true),
                    PaymentStatus = c.String(),
                    PayerFirstName = c.String(),
                    PayerLastName = c.String(),
                    Amount = c.String(),
                    TxToken = c.String(),
                    ReceiverEmail = c.String(),
                    ItemName = c.String(),
                    Currency = c.String(),
                    PayerEmail = c.String(),
                    PaymentDate = c.String(),
                    CourseCategoryId = c.Int(nullable: false),
                    StudentId = c.String(maxLength: 128),
                    PayerId = c.String(),
                })
                .PrimaryKey(t => t.StudentPaypalPaymentId)
                .ForeignKey("dbo.CourseCategories", t => t.CourseCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.CourseCategoryId)
                .Index(t => t.StudentId);

            CreateTable(
                "dbo.CorperEnrolledCourses",
                c => new
                {
                    CorperEnrolledCoursesId = c.Int(nullable: false, identity: true),
                    CourseId = c.Int(nullable: false),
                    CorperCallUpNumber = c.String(),
                    StudentId = c.String(maxLength: 128),
                    CourseCategoryId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.CorperEnrolledCoursesId)
                .ForeignKey("dbo.CourseCategories", t => t.CourseCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.CourseCategoryId);

            CreateTable(
                "dbo.CourseRatings",
                c => new
                {
                    CourseRatingId = c.Int(nullable: false, identity: true),
                    CourseId = c.Int(nullable: false),
                    Rating = c.Int(nullable: false),
                    Dislike = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.CourseRatingId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);

            CreateTable(
                "dbo.Files",
                c => new
                {
                    FileId = c.Int(nullable: false, identity: true),
                    FileName = c.String(maxLength: 355),
                    ContentType = c.String(maxLength: 100),
                    Content = c.Binary(),
                    FileType = c.Int(nullable: false),
                    TutorId = c.Int(nullable: false),
                    Tutor_TutorId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.Tutors", t => t.Tutor_TutorId)
                .Index(t => t.Tutor_TutorId);

            CreateTable(
                "dbo.PaymentTypes",
                c => new
                {
                    PaymentTypeId = c.Int(nullable: false, identity: true),
                    PaymentName = c.String(),
                    Amount = c.Int(nullable: false),
                    PaymentTypeValue = c.String(),
                })
                .PrimaryKey(t => t.PaymentTypeId);

            CreateTable(
                "dbo.ProfessionalPayments",
                c => new
                {
                    ProfessionalPaymentId = c.Int(nullable: false, identity: true),
                    UserId = c.String(),
                    ProfessionalWorkerId = c.String(),
                    PaymentDateTime = c.DateTime(nullable: false),
                    Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsPayed = c.Boolean(nullable: false),
                    Email = c.String(),
                    CoursePayedFor = c.String(),
                    PaymentDate = c.String(),
                    PayStackCustomerId = c.String(),
                })
                .PrimaryKey(t => t.ProfessionalPaymentId);

            CreateTable(
                "dbo.QuizLogs",
                c => new
                {
                    QuizLogId = c.Int(nullable: false, identity: true),
                    StudentId = c.String(maxLength: 128),
                    ModuleId = c.Int(nullable: false),
                    LevelId = c.Int(nullable: false),
                    SemesterId = c.Int(nullable: false),
                    SessionId = c.Int(nullable: false),
                    ExamTypeId = c.Int(nullable: false),
                    Score = c.Double(nullable: false),
                    TotalScore = c.Double(nullable: false),
                    ExamTaken = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.QuizLogId)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.ModuleId);

            CreateTable(
                "dbo.RegularStudentVms",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(),
                    LastName = c.String(),
                    Email = c.String(),
                    Address = c.String(),
                    PhoneNumber = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.RemitaPaymentLogs",
                c => new
                {
                    RemitaPaymentLogId = c.Int(nullable: false, identity: true),
                    OrderId = c.String(),
                    StatusCode = c.String(),
                    TransactionMessage = c.String(),
                    Rrr = c.String(),
                    PaymentName = c.String(),
                    PaymentDate = c.DateTime(nullable: false),
                    Amount = c.String(),
                    PayerName = c.String(),
                })
                .PrimaryKey(t => t.RemitaPaymentLogId);

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
                "dbo.StudentTopicQuizs",
                c => new
                {
                    StudentTopicQuizId = c.Int(nullable: false, identity: true),
                    StudentQuestionId = c.Int(nullable: false),
                    StudentId = c.String(maxLength: 128),
                    ModuleId = c.Int(nullable: false),
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
                .PrimaryKey(t => t.StudentTopicQuizId)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.ModuleId);

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

            CreateTable(
                "dbo.UserCourseDetails",
                c => new
                {
                    UserCourseDetailId = c.String(nullable: false, maxLength: 128),
                    UserCourseName = c.String(),
                    CourseCategory = c.String(),
                    CourseId = c.Int(nullable: false),
                    PaymentDate = c.String(),
                })
                .PrimaryKey(t => t.UserCourseDetailId);

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
                "dbo.VoteForumAnswerForumAnswers",
                c => new
                {
                    VoteForumAnswer_VoteForumAnswerId = c.Int(nullable: false),
                    ForumAnswer_ForumAnswerId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.VoteForumAnswer_VoteForumAnswerId, t.ForumAnswer_ForumAnswerId })
                .ForeignKey("dbo.VoteForumAnswers", t => t.VoteForumAnswer_VoteForumAnswerId, cascadeDelete: true)
                .ForeignKey("dbo.ForumAnswers", t => t.ForumAnswer_ForumAnswerId, cascadeDelete: true)
                .Index(t => t.VoteForumAnswer_VoteForumAnswerId)
                .Index(t => t.ForumAnswer_ForumAnswerId);

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
            DropForeignKey("dbo.TestAnswers", "TestQuestionId", "dbo.TestQuestions");
            DropForeignKey("dbo.TestQuestions", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.TestAnswers", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentTopicQuizs", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentTopicQuizs", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.QuizLogs", "StudentId", "dbo.Students");
            DropForeignKey("dbo.QuizLogs", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Files", "Tutor_TutorId", "dbo.Tutors");
            DropForeignKey("dbo.CourseRatings", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CorperEnrolledCourses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Courses", "CorperEnrolledCourses_CorperEnrolledCoursesId", "dbo.CorperEnrolledCourses");
            DropForeignKey("dbo.CorperEnrolledCourses", "CourseCategoryId", "dbo.CourseCategories");
            DropForeignKey("dbo.AssignCourseCategories", "StudentAssignedCourse_StudentAssignedCourseId", "dbo.StudentAssignedCourses");
            DropForeignKey("dbo.AssignCourseCategories", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.AssignCourseCategories", "CourseCategoryId", "dbo.CourseCategories");
            DropForeignKey("dbo.StudentPaypalPayments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentPaypalPayments", "CourseCategoryId", "dbo.CourseCategories");
            DropForeignKey("dbo.StudentPayments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentPayments", "CourseCategoryId", "dbo.CourseCategories");
            DropForeignKey("dbo.StudentAssesments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentAssesments", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.Enrollments", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.Enrollments", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.EnrollForCourses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentAssignedCourses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentAssignedCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.TopicQuizs", "Topic_TopicId", "dbo.Topics");
            DropForeignKey("dbo.TopicQuizs", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.TopicAssignments", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.SubmitAssignments", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.SubmitAssignments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.AssignmentReviews", "SubmitAssignmentId", "dbo.SubmitAssignments");
            DropForeignKey("dbo.StudentQuestions", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.StudentQuestions", "StudentId", "dbo.Students");
            DropForeignKey("dbo.QuizRules", "Topic_TopicId", "dbo.Topics");
            DropForeignKey("dbo.QuizRules", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Topics", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.TopicMaterialUploads", "MaterialByTutor_TutorId", "dbo.Tutors");
            DropForeignKey("dbo.TutorCourses", "TutorId", "dbo.Tutors");
            DropForeignKey("dbo.TutorCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Posts", "TutorId", "dbo.Tutors");
            DropForeignKey("dbo.Posts", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.TagPosts", "Post_PostId", "dbo.Posts");
            DropForeignKey("dbo.TagPosts", "Tag_TagId", "dbo.Tags");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.TopicMaterialUploads", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.StudentTestLogs", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentTestLogs", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Modules", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.ForumViews", "ContentViewId", "dbo.Fora");
            DropForeignKey("dbo.ForumQuestions", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ForumQuestionViews", "ForumQuestionId", "dbo.ForumQuestions");
            DropForeignKey("dbo.VoteForumAnswerForumAnswers", "ForumAnswer_ForumAnswerId", "dbo.ForumAnswers");
            DropForeignKey("dbo.VoteForumAnswerForumAnswers", "VoteForumAnswer_VoteForumAnswerId", "dbo.VoteForumAnswers");
            DropForeignKey("dbo.ForumAnswers", "ForumQuestionId", "dbo.ForumQuestions");
            DropForeignKey("dbo.ForumQuestions", "CourseId", "dbo.Fora");
            DropForeignKey("dbo.Fora", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseEnrollments", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.CourseEnrollments", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.AssesmentQuestionAnswers", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.EnrollForCourses", "CourseCategoryId", "dbo.CourseCategories");
            DropIndex("dbo.TagPosts", new[] { "Post_PostId" });
            DropIndex("dbo.TagPosts", new[] { "Tag_TagId" });
            DropIndex("dbo.VoteForumAnswerForumAnswers", new[] { "ForumAnswer_ForumAnswerId" });
            DropIndex("dbo.VoteForumAnswerForumAnswers", new[] { "VoteForumAnswer_VoteForumAnswerId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.TestQuestions", new[] { "CourseId" });
            DropIndex("dbo.TestAnswers", new[] { "StudentId" });
            DropIndex("dbo.TestAnswers", new[] { "TestQuestionId" });
            DropIndex("dbo.StudentTopicQuizs", new[] { "ModuleId" });
            DropIndex("dbo.StudentTopicQuizs", new[] { "StudentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.QuizLogs", new[] { "ModuleId" });
            DropIndex("dbo.QuizLogs", new[] { "StudentId" });
            DropIndex("dbo.Files", new[] { "Tutor_TutorId" });
            DropIndex("dbo.CourseRatings", new[] { "CourseId" });
            DropIndex("dbo.CorperEnrolledCourses", new[] { "CourseCategoryId" });
            DropIndex("dbo.CorperEnrolledCourses", new[] { "StudentId" });
            DropIndex("dbo.StudentPaypalPayments", new[] { "StudentId" });
            DropIndex("dbo.StudentPaypalPayments", new[] { "CourseCategoryId" });
            DropIndex("dbo.StudentPayments", new[] { "StudentId" });
            DropIndex("dbo.StudentPayments", new[] { "CourseCategoryId" });
            DropIndex("dbo.StudentAssesments", new[] { "Course_CourseId" });
            DropIndex("dbo.StudentAssesments", new[] { "StudentId" });
            DropIndex("dbo.Enrollments", new[] { "Student_StudentId" });
            DropIndex("dbo.Enrollments", new[] { "CourseID" });
            DropIndex("dbo.TopicQuizs", new[] { "Topic_TopicId" });
            DropIndex("dbo.TopicQuizs", new[] { "ModuleId" });
            DropIndex("dbo.TopicAssignments", new[] { "TopicId" });
            DropIndex("dbo.AssignmentReviews", new[] { "SubmitAssignmentId" });
            DropIndex("dbo.SubmitAssignments", new[] { "StudentId" });
            DropIndex("dbo.SubmitAssignments", new[] { "TopicId" });
            DropIndex("dbo.StudentQuestions", new[] { "TopicId" });
            DropIndex("dbo.StudentQuestions", new[] { "StudentId" });
            DropIndex("dbo.QuizRules", new[] { "Topic_TopicId" });
            DropIndex("dbo.QuizRules", new[] { "ModuleId" });
            DropIndex("dbo.TutorCourses", new[] { "CourseId" });
            DropIndex("dbo.TutorCourses", new[] { "TutorId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropIndex("dbo.Posts", new[] { "TutorId" });
            DropIndex("dbo.Posts", new[] { "TopicId" });
            DropIndex("dbo.TopicMaterialUploads", new[] { "MaterialByTutor_TutorId" });
            DropIndex("dbo.TopicMaterialUploads", new[] { "TopicId" });
            DropIndex("dbo.Topics", new[] { "ModuleId" });
            DropIndex("dbo.StudentTestLogs", new[] { "ModuleId" });
            DropIndex("dbo.StudentTestLogs", new[] { "StudentId" });
            DropIndex("dbo.Modules", new[] { "CourseId" });
            DropIndex("dbo.ForumViews", new[] { "ContentViewId" });
            DropIndex("dbo.ForumQuestionViews", new[] { "ForumQuestionId" });
            DropIndex("dbo.ForumAnswers", new[] { "ForumQuestionId" });
            DropIndex("dbo.ForumQuestions", new[] { "StudentId" });
            DropIndex("dbo.ForumQuestions", new[] { "CourseId" });
            DropIndex("dbo.Fora", new[] { "CourseId" });
            DropIndex("dbo.CourseEnrollments", new[] { "Student_StudentId" });
            DropIndex("dbo.CourseEnrollments", new[] { "CourseId" });
            DropIndex("dbo.AssesmentQuestionAnswers", new[] { "CourseId" });
            DropIndex("dbo.Courses", new[] { "CorperEnrolledCourses_CorperEnrolledCoursesId" });
            DropIndex("dbo.StudentAssignedCourses", new[] { "CourseId" });
            DropIndex("dbo.StudentAssignedCourses", new[] { "StudentId" });
            DropIndex("dbo.EnrollForCourses", new[] { "StudentId" });
            DropIndex("dbo.EnrollForCourses", new[] { "CourseCategoryId" });
            DropIndex("dbo.AssignCourseCategories", new[] { "StudentAssignedCourse_StudentAssignedCourseId" });
            DropIndex("dbo.AssignCourseCategories", new[] { "CourseCategoryId" });
            DropIndex("dbo.AssignCourseCategories", new[] { "CourseId" });
            DropTable("dbo.TagPosts");
            DropTable("dbo.VoteForumAnswerForumAnswers");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserCourseDetails");
            DropTable("dbo.TestQuestions");
            DropTable("dbo.TestAnswers");
            DropTable("dbo.StudentTopicQuizs");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RemitaPaymentLogs");
            DropTable("dbo.RegularStudentVms");
            DropTable("dbo.QuizLogs");
            DropTable("dbo.ProfessionalPayments");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.Files");
            DropTable("dbo.CourseRatings");
            DropTable("dbo.CorperEnrolledCourses");
            DropTable("dbo.StudentPaypalPayments");
            DropTable("dbo.StudentPayments");
            DropTable("dbo.StudentAssesments");
            DropTable("dbo.Enrollments");
            DropTable("dbo.TopicQuizs");
            DropTable("dbo.TopicAssignments");
            DropTable("dbo.AssignmentReviews");
            DropTable("dbo.SubmitAssignments");
            DropTable("dbo.StudentQuestions");
            DropTable("dbo.QuizRules");
            DropTable("dbo.TutorCourses");
            DropTable("dbo.Tags");
            DropTable("dbo.Comments");
            DropTable("dbo.Posts");
            DropTable("dbo.Tutors");
            DropTable("dbo.TopicMaterialUploads");
            DropTable("dbo.Topics");
            DropTable("dbo.StudentTestLogs");
            DropTable("dbo.Modules");
            DropTable("dbo.ForumViews");
            DropTable("dbo.ForumQuestionViews");
            DropTable("dbo.VoteForumAnswers");
            DropTable("dbo.ForumAnswers");
            DropTable("dbo.ForumQuestions");
            DropTable("dbo.Fora");
            DropTable("dbo.CourseEnrollments");
            DropTable("dbo.AssesmentQuestionAnswers");
            DropTable("dbo.Courses");
            DropTable("dbo.StudentAssignedCourses");
            DropTable("dbo.Students");
            DropTable("dbo.EnrollForCourses");
            DropTable("dbo.CourseCategories");
            DropTable("dbo.AssignCourseCategories");
        }
    }
}