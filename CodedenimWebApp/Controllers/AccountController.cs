using System;
using System.Collections.Generic;
using CodedenimWebApp.Models;
using CodedenimWebApp.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.ViewModels;
using CodeninModel;

namespace CodedenimWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _db;

        public AccountController()
        {
            _db = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationDbContext db)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            this._db = db;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }



            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: 
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(c => c.Email.ToUpper().Equals(model.Email.ToUpper())
                                                                               || c.UserName.ToUpper().Equals(model.Email.ToUpper())
                                                                               || c.Id.Equals(model.Email));

            if (user == null)
            {
                ViewBag.Message = "Incorrect UserName or Password, Please try again!!!";
                return View(model);
            }
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //  var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("CustomDashborad", new { username = user.UserName });
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        public ActionResult CustomDashborad(string username)
        {
            if (User.IsInRole(RoleName.Admin))
            {
                TempData["UserMessage"] = $"Login Successful, Welcome {username}";
                TempData["Title"] = "Success.";
                return RedirectToAction("AdminDashBoard", "Home");
                // return RedirectToAction("AdminDashboard", "Home");
            }

            if (User.IsInRole(RoleName.Student))
            {
                //var model = await Db.Students.Where(x => x.StudentId.Equals(username)).FirstOrDefaultAsync();
                //model.IsLogin = true;
                //Db.Entry(model).State = EntityState.Modified;
                //await Db.SaveChangesAsync();

                //IdentityResult result = await UserManager.UpdateAsync(model);
                TempData["UserMessage"] = $"Login Successful, Welcome {username}";
                TempData["Title"] = "Success.";
                return RedirectToAction("Dashboard", "Students");
            }
           
            return RedirectToAction("Index", "Home");
        }



        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }


        //Confirm if the tutor has been registered
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult ConfirmTutorReg()
        {
            //  var tutor = new ConfirmTutorVm();
           // var tutor = ConfirmTutor(id);
            return View();
        }

        //
        // Post: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult ConfirmTutorReg(string id)
        //{
        //    //  var tutor = new ConfirmTutorVm();
        //    var tutor = ConfirmTutor(id);
        //    return View(tutor);
        //}


        //method to confirm if a tutor Id exist
        //private RegisterViewModel ConfirmTutor(string id)
        //{

        //    var tutorDetail = new RegisterViewModel();
        //    var tutor = _db.Tutors.Where(t => t.TutorId.Equals(id)).Select(t => new
        //    {
        //        t.FirstName,
        //        t.LastName,
        //        t.Email,
        //        t.Courses

        //    }).FirstOrDefault();

        //   // Debug.Assert(tutor != null, "tutor != null");
        //    tutorDetail.FirstName = tutor.FirstName;
        //    tutorDetail.LastName = tutor.LastName;
        //    tutorDetail.Email = tutor.Email;
        //    tutorDetail.TutorsCourses = tutor.Courses;


        //   // my.FirstName = tutor.FirstName;

        //    return tutorDetail;
        //}



        //private void SendEmail(string mail, string subject, string body)
        //{

        //    SmtpClient ss = new SmtpClient("smtp.gmail.com", 587);
        //    ss.EnableSsl = true;
        //    ss.Timeout = 10000;
        //    ss.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    ss.UseDefaultCredentials = true;
        //    ss.Credentials = new System.Net.NetworkCredential("davidzagi93@gmail.com", "19920107");


        //    MailMessage message = new MailMessage();
        //    //Setting From , To
        //    message.From = new MailAddress("Codedenim@gmail.com", "Course");
        //    message.To.Add(new MailAddress(mail));
        //    message.Subject = subject;
        //    message.Body = body;

        //    ss.Send(message);

        //}


        //private void SendEmailConfirmation(string to, string username, string confirmationToken)
        //{
        //    string link = Url.Action("RegisterConfirmation", "Account", new { Id = confirmationToken }, "http");
        //    SendEmail(to, "Confrimation of Registration", link);
        //}

        //private bool ConfirmAccount(string confirmationToken)
        //{
        //    ApplicationUser user = _db.Users.First(u => u.ConfirmationToken == confirmationToken);
        //    if (user != null)
        //    {
        //        user.IsConfirmed = true;
        //        DbSet<ApplicationUser> dbSet = _db.Set<ApplicationUser>();
        //        dbSet.Attach(user);
        //        _db.Entry(user).State = EntityState.Modified;
        //        _db.SaveChanges();

        //        return true;
        //    }
        //    return false;
        //}

        //[AllowAnonymous]
        //public ActionResult RegisterConfirmation(string Id)
        //{
        //    if (ConfirmAccount(Id))
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    return RedirectToAction("Login");
        //}


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //  var tutor = new ConfirmTutorVm();
           // var tutor = ConfirmTutor(id);
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email + model.Email };
                // var result = await UserManager.CreateAsync(user, model.Password);

                // create a tutor from admin without a password
                var result = await UserManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {

                    await this.UserManager.AddToRoleAsync(user.Id, "Tutor");

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                  


                    //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your Tutor account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    //ViewBag.Link = callbackUrl;
                    ////await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your Tutor account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    ////ViewBag.Link = callbackUrl;
                    //TempData["UserMessage"] = $"Registration is Successful for {user.UserName}, Please Confirm Your Email to Login.";
                    //return View("ConfirmRegistration");

                    // await this.UserManager.AddToRoleAsync(user.Id, "Admin");
                    // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                   return RedirectToAction("Index", "Home");

                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult TutorRegistration()
        {
            //var tutor = new Tutor();
            //ViewBag.Roles = new SelectList(_db.Roles.ToList(), "Id", "Name");
            //tutor.Courses = new List<Course>();
            //PopulateAssignedCourseData(tutor);
            return View();
        }


        //method to populate the assigned course in the the create view
        //private void PopulateAssignedCourseData(TutorCourses tutor)
        //{
        //    var allCourses = _db.Courses;
        //    var instructorCourses = new HashSet<int>(tutor.Where())
        //   // var instructorCourses = new HashSet<int>(tutor.Courses.Select(c => c.CourseId));
        //    var viewModel = new List<AssignedCourses>();
        //    foreach (var course in allCourses)
        //    {
        //        viewModel.Add(new AssignedCourses
        //        {
        //            CourseId = course.CourseId,
        //            CourseName = course.CourseName,
        //            Assigned = instructorCourses.Contains(course.CourseId)
        //        });
        //    }
        //    ViewBag.Courses = viewModel;
        //}


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TutorRegistration(TutorRegisterVm model)
        {
            var tutorInDb = _db.Tutors.Find(model.TutorId);

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Id = model.TutorId,
                    UserName = model.FirstName + " " + model.LastName,
                    Email = model.Email,
                  
                    PhoneNumber = model.PhoneNumber.Trim(),
                  
                };

                var result = await UserManager.CreateAsync(user,model.Password);
                   
                if (result.Succeeded && (tutorInDb != null))
                {
                 
                
                    tutorInDb.Email = model.Email;

                    tutorInDb.DateOfBirth = model.DateOfBirth;
                    tutorInDb.PhoneNumber = model.PhoneNumber;
                  
                    _db.Entry(tutorInDb).State = EntityState.Modified;
                   
                    await _db.SaveChangesAsync();
                    await this.UserManager.AddToRoleAsync(user.Id, "Tutor");

                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your Tutor account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    ViewBag.Link = callbackUrl;
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your Tutor account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    //ViewBag.Link = callbackUrl;
                    TempData["UserMessage"] = $"Registration is Successful for {user.UserName}, Please Confirm Your Email to Login.";
                    return View("ConfirmRegistration");

                  

                    RedirectToAction("Index", "Tutors");

                   
                };
                   
                 

     
                AddErrors(result);
            }
                return View();
        }



        [AllowAnonymous]
        public async Task<ActionResult> SendEmail()
        {
            var message = new IdentityMessage
            {
                Subject = "Confirm Email",
                Destination = "davidzagi93@gmail.com",
                Body = "this is to Confirm Password",

            };

            var send = new EmailService();
            await send.SendAsync(message);
            ViewBag.Success = "Success";
            return View();

            // return await message;
        }



        [AllowAnonymous]
        public async Task SendAsync(EmailSender message)
        {
            // Plug in your email service here to send an email.
            string schoolName = ConfigurationManager.AppSettings["CODEDENIM"];
            string emailsetting = ConfigurationManager.AppSettings["GmailUserName"];
            MailMessage email = new MailMessage(new MailAddress($"noreply{emailsetting}", "(Codedenin Registration, do not reply)"),
                new MailAddress(message.Destination));

            email.Subject = message.Subject;
            email.Body = message.Body;

            email.IsBodyHtml = true;

            using (var mailClient = new EmailSetUpServices())
            {
                //In order to use the original from email address, uncomment this line:
                email.From = new MailAddress(mailClient.UserName, $"(do not reply)@{schoolName}");

                await mailClient.SendMailAsync(email);
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }

    public class EmailSender
    {
        //public string Sender { get; set; }
        public string Destination { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}