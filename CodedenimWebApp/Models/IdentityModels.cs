using CodedenimWebApp.Controllers;
using CodeninModel;
using CodeninModel.Quiz;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodedenimWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            if (String.IsNullOrEmpty(authenticationType))
            {
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                return userIdentity;

            }
            else
            {
                var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
                return userIdentity;
            }
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            // Add custom user claims here

        }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<CourseCategory> CourseCategories { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<TopicMaterialUpload> TopicMaterialUploads { get; set; }
        public DbSet<File> Files { get; set; }

        public System.Data.Entity.DbSet<CodeninModel.Tutor> Tutors { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<TutorCourse> TutorCourses { get; set; }

        public DbSet<CodeninModel.CBTE.QuizRule> QuizRules { get; set; }

        public DbSet<TopicQuiz> TopicQuizs { get; set; }

        public System.Data.Entity.DbSet<StudentTopicQuiz> StudentTopicQuizs { get; set; }
        public DbSet<QuizLog> QuizLogs { get; set; }

        public DbSet<CodeninModel.StudentAssignedCourse> StudentAssignedCourses { get; set; }

        public DbSet<CodeninModel.Forums.Forum> Fora { get; set; }

        public DbSet<CodeninModel.Forums.ForumView> ForumViews { get; set; }

        public DbSet<CodeninModel.Forums.ForumQuestion> ForumQuestions { get; set; }

        public DbSet<CodeninModel.Forums.ForumQuestionView> ForumQuestionViews { get; set; }

        public DbSet<CodeninModel.Forums.ForumAnswer> ForumAnswers { get; set; }

        public DbSet<CodeninModel.CourseRating> CourseRatings { get; set; }

        public System.Data.Entity.DbSet<CodedenimWebApp.ViewModels.RegularStudentVm> RegularStudentVms { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<ProfessionalPayment> ProfessionalPayments { get; set; }

        public DbSet<ViewModels.UserCourseDetail> UserCourseDetails { get; set; }
        public DbSet<CorperEnrolledCourses> CorperEnrolledCourses { get; set; }

        public DbSet<CodeninModel.AssignCourseCategory> AssignCourseCategories { get; set; }
        public DbSet<StudentPayment> StudentPayments { get; set; }

        public DbSet<RemitaPaymentLog> RemitaPaymentLogs { get; set; }

        public DbSet<CodeninModel.EnrollForCourse> EnrollForCourses { get; set; }
        public DbSet<StudentPaypalPayment> StudentPaypalPayments { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<TestAnswer> TestAnswers { get; set; }
        public DbSet<StudentCourseTrack> StudentCourseTracks { get; set; }
        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        // Your code...
        //        // Could also be before try if you know the exception occurs in SaveChanges

        //        base.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine(
        //                "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        throw;

        //    }
        //    return 0;
        //}
    }
}