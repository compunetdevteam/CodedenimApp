//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Net.Mail;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Helpers;
//using System.Web.Http;
//using System.Web.Mvc;
//using CodedenimWebApp.Models;
//using CodedenimWebApp.Service;
//using CodeninModel;
//using Microsoft.AspNet.Identity;

//namespace CodedenimWebApp.Controllers
//{
//    public class AccountJson
//    {

//        private const string LocalLoginProvider = "Local";
//        private ApplicationUserManager _userManager;
//        private readonly ApplicationDbContext _db;

//        public AccountJson()
//        {
//            _db = new ApplicationDbContext();
//        }


//        [System.Web.Http.AllowAnonymous]
//        [System.Web.Http.Route("RegisterCorper")]
//        public async Task<JsonResult> RegisterCorper([FromBody] RegisterCorperModel model)
//        {
//            //if (model == null)
//            //{
//            //    return Json(new{data = model}, JsonRequestBehavior.AllowGet);
//            //}

//            var user = new ApplicationUser()
//            {
//                Id = model.CallUpNumber,
//                UserName = model.Email,
//                Email = model.Email
//            };


//            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            //Persisting the  Student Redord 
//            var corper = new Student
//            {
//                StudentId = model.CallUpNumber,
//                FirstName = model.FirstName,
//                LastName = model.LastName,
//                DateOfBirth = model.DateOfBirth,
//                Gender = model.Gender,
//                Email = model.Email,
//                PhoneNumber = model.MobileNumber,
//                StateOfService = model.NyscState,
//                Institution = model.Institution,
//                Batch = model.NyscBatch,
//                Discpline = model.Discpline
//            };

//            _db.Students.Add(corper);
//            await _db.SaveChangesAsync();
//            await this.UserManager.AddToRoleAsync(user.Id, "Corper");

//            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
//            // var callbackUrl = Url.Link("ConfirmEmail", "Account", new { userId = user.Id, code = code }/*, protocol: Request.Url.Scheme*/);
//            var callbackUrl = EmailLink(user.Id, code);

//            await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
//            //         ViewBag.Link = callbackUrl;
//            //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your Tutor account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
//            //ViewBag.Link = callbackUrl;
//            //  TempData["UserMessage"] = $"Registration is Successful for {user.UserName}, Please Confirm Your Email to Login.";
//            return Ok("ConfirmRegistration");

//        }

//        public string EmailLink(string userId, string code)
//        {
//            var url = this.Url.Link("Default", new { Controller = "Account", Action = "ConfirmEmail", userId, code });
//            return url;
//        }

//        // POST api/Account/RegisterCorper
//        [System.Web.Http.AllowAnonymous]
//        [System.Web.Http.Route("RegisterUnderGraduate")]
//        public async Task<IHttpActionResult> RegisterUnderGraduate([FromBody] RegisterStudentModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

//            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            var student = new Student
//            {
//                StudentId = model.AdmissionNumber,
//                Title = model.Title,
//                FirstName = model.FirstName,
//                LastName = model.LastName,
//                DateOfBirth = model.DateOfBirth,
//                Gender = model.Gender,
//                PhoneNumber = model.MobileNumber,
//                Institution = model.Institution,
//                Discpline = model.Discpline

//            };
//            _db.Students.Add(student);
//            await _db.SaveChangesAsync();
//            await this.UserManager.AddToRoleAsync(user.Id, "UnderGraduate");
//            return Ok();
//        }


//        //my custom Registration controller
//        // POST api/Account/InstructorRegister
//        [System.Web.Http.AllowAnonymous]
//        [System.Web.Http.Route("InstructorRegister")]
//        public async Task<IHttpActionResult> InstructorRegister(InstructorRegModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = model.Mobile };

//            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }

//        /// <summary>
//        /// Email Confirmation
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>

//        [System.Web.Http.AllowAnonymous]
//        public async Task<IHttpActionResult> SendEmail()
//        {
//            var message = new IdentityMessage
//            {
//                Subject = "Confirm Email",
//                Destination = "davidzagi93@gmail.com",
//                Body = "this is to Confirm Password",

//            };

//            var send = new EmailService();
//            await send.SendAsync(message);
//            // ViewBag.Success = "Success";
//            return Ok();

//            // return await message;
//        }



//        [System.Web.Http.AllowAnonymous]
//        public async Task SendAsync(EmailSender message)
//        {
//            // Plug in your email service here to send an email.
//            string schoolName = ConfigurationManager.AppSettings["CODEDENIM"];
//            string emailsetting = ConfigurationManager.AppSettings["GmailUserName"];
//            MailMessage email = new MailMessage(new MailAddress($"noreply{emailsetting}", "(Codedenin Registration, do not reply)"),
//                new MailAddress(message.Destination));

//            email.Subject = message.Subject;
//            email.Body = message.Body;

//            email.IsBodyHtml = true;

//            using (var mailClient = new EmailSetUpServices())
//            {
//                //In order to use the original from email address, uncomment this line:
//                email.From = new MailAddress(mailClient.UserName, $"(do not reply)@{schoolName}");

//                await mailClient.SendMailAsync(email);
//            }
//        }

//    }
//}