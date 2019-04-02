using CodedenimWebApp.Models;
using CodedenimWebApp.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodedenimWebApp
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            //// Plug in your email service here to send an email.
            //string schoolName = ConfigurationManager.AppSettings["CODEDENIM"];
            //string emailsetting = ConfigurationManager.AppSettings["GmailUserName"];
            //MailMessage email = new MailMessage(new MailAddress($"noreply{emailsetting}", "(Codedenin Registration, do not reply)"),
            //    new MailAddress(message.Destination));

            //email.Subject = message.Subject;
            //email.Body = message.Body;

            //email.IsBodyHtml = true;

            //using (var mailClient = new EmailSetUpServices())
            //{
            //    //In order to use the original from email address, uncomment this line:
            //    email.From = new MailAddress(mailClient.UserName, $"(do not reply)@{schoolName}");

            //    await mailClient.SendMailAsync(email);
            //}
            // Plug in your email service here to send an email.          
            using (var db = new ApplicationDbContext())
            {
                // var query = new QueryCommand(db);
                var student = db.Students.FirstOrDefault(x => x.Email.Trim().ToUpper().Equals(message.Destination));

                var username = student?.FullName ?? message.Destination;

                // ***Begin Sendgrid Implementation *******//
                //var apiKey = "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";
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress($"noreply@Codedenim.com", "CODEDENIM");
                var subject = !string.IsNullOrEmpty(message.Subject) ? message.Subject : "CODEDENIM NOTIFICATION";
                var to = new EmailAddress(message.Destination, username);
                var plainTextContent = message.Body;
                var htmlContent = message.Body;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                try
                {
                    var response = await client.SendEmailAsync(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                // ***End Sendgrid Implementation *******//              


            }

        }

       
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {

            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager, "");
        }


        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
