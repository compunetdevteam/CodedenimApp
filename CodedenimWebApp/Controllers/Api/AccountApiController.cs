using CodedenimWebApp.Models;
using CodedenimWebApp.Providers;
using CodedenimWebApp.Results;
using CodedenimWebApp.Service;
using CodeninModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.Authorize]
    [System.Web.Http.RoutePrefix("api/AccountApi")]
    public class AccountApiController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _db;

        public AccountApiController()
        {
            _db = new ApplicationDbContext();
        }

        public AccountApiController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [System.Web.Http.Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [System.Web.Http.Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [System.Web.Http.Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [System.Web.Http.Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [System.Web.Http.Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                                                              && ticket.Properties.ExpiresUtc.HasValue
                                                              && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [System.Web.Http.Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [System.Web.Http.OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }


        // POST api/Account/Register
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }



            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> RegisterCorper()
        {
            return Ok();
        }


   // POST api/Account/RegisterStudent
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("RegisterCorper")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> RegisterCorper(RegisterCorperModel model)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           

            var user = new ApplicationUser()
            {
                //Id = model.CallUpNumber,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.MobileNumber
            };

   

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //Persisting the  Student Redord 
            var corper = new Student()
            {
                StudentId = user.Id,
                CallUpNo = model.CallUpNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Email = model.Email,
                PhoneNumber = model.MobileNumber,
                StateOfService = model.NyscState.ToString(),
               Institution = model.Institution,
               AccountType = "Corper",
               Batch = model.NyscBatch.ToString(),
               Discpline = model.Discpline
            };

            _db.Students.Add(corper);
            await _db.SaveChangesAsync();
            await this.UserManager.AddToRoleAsync(user.Id, RoleName.Corper);

       //    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            ////var callbackUrl = Url.Link("ConfirmEmail", "Account", new { userId = user.Id, code = code }/*, protocol: Request.Url.Scheme*/);
            ////var callbackUrl = EmailLink(user.Id, code);

           // await UserManager.SendEmailAsync(user.Id, "Code-denim Mobile Registration", "Welcome to Code-dedenim...");
            ////ViewBag.Link = callbackUrl;
            //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your Tutor account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
            //ViewBag.Link = callbackUrl;
            //  TempData["UserMessage"] = $"Registration is Successful for {user.UserName}, Please Confirm Your Email to Login.";
            return Ok("Registration was successful");

        }

        public string EmailLink(string userId, string code)
        {
           var url = this.Url.Link("Default", new { Controller = "Account" , Action = "ConfirmEmail", userId, code} );
            return url;
        }

        // POST api/Account/RegisterUnderGraduate
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("RegisterUnderGraduate")]
        public async Task<IHttpActionResult> RegisterUnderGraduate(RegisterUnderGrad model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                //Id = model.MatNumber,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.MobileNumber
            };
         

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var student = new Student()
            {
                StudentId = user.Id,
                MatricNo = model.MatNumber,
                Title = model.Title.ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                AccountType = "UnderGraduate",
                PhoneNumber = model.MobileNumber,
                Institution = model.Institution,
                Discpline = model.Discpline,
                Email = model.Email

            };
            _db.Students.Add(student);


           await _db.SaveChangesAsync();
           await this.UserManager.AddToRoleAsync(user.Id, RoleName.UnderGraduate);

            //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            // var callbackUrl = Url.Link("ConfirmEmail", "Account", new { userId = user.Id, code = code }/*, protocol: Request.Url.Scheme*/);
            //var callbackUrl = EmailLink(user.Id, code);

           // await UserManager.SendEmailAsync(user.Id, "Code-denim Mobile Registration", "Thank you for creating your account of mobile");


            return Ok("Student Created Successfully ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> OtherStudent()
        {
            return Ok();
        }



        /// <summary>
        /// method to register student from the mobile app
        /// </summary>
        /// <param name="model"></param>
        /// <returns>HttpStatus Code</returns>
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("OtherStudent")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> OtherStudent(RegisterOtherStudentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {

                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.MobileNumber
            };

       

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            var modelTitle = "";
            if (model.Gender == "Male" || model.Gender == "male")
            {
                modelTitle = "Mr";
            }
            else { modelTitle = "Mrs/Miss"; }
                var student = new Student()
                {
                    StudentId = user.Id,
                    Title = modelTitle,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    AccountType = "RegularStudent",
                    PhoneNumber = model.MobileNumber,
                    Email = model.Email,
                    


                };
            
            _db.Students.Add(student);
           await _db.SaveChangesAsync();

           await this.UserManager.AddToRoleAsync(user.Id, RoleName.RegularStudent);
        
       
          

            return Ok("Student Created Successfully ");
         
        }
        //my custom Registration controller
        // POST api/Account/InstructorRegister
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("InstructorRegister")]
        public async Task<IHttpActionResult> InstructorRegister(InstructorRegModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = model.Mobile };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Email Confirmation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [System.Web.Http.AllowAnonymous]
        public async Task<IHttpActionResult> SendEmail()
        {
            var message = new IdentityMessage
            {
                Subject = "Confirm Email",
                Destination = "davidzagi93@gmail.com",
                Body = "this is to Confirm Password",

            };

            var send = new EmailService();
            await send.SendAsync(message);
           // ViewBag.Success = "Success";
            return Ok();

            // return await message;
        }



        [System.Web.Http.AllowAnonymous]
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
        //// GET: /Account/ConfirmEmail
        //[System.Web.Http.AllowAnonymous]
        //public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return Ok("Error");
        //    }
        //    var result = await UserManager.ConfirmEmailAsync(userId, code);
        //    return Ok(result.Succeeded ? "ConfirmEmail" : "Error");
        //}




        // POST api/Account/RegisterExternal    
        [System.Web.Http.OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
