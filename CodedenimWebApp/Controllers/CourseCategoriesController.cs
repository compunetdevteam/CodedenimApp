using CodedenimWebApp.Controllers.Api;
using CodedenimWebApp.Models;
using CodedenimWebApp.Service;
using CodedenimWebApp.Services;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OfficeOpenXml;
using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CodedenimWebApp.Controllers
{

    public class CourseCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RemitaHash myHash = new RemitaHash();
        private ConvertEmail converter = new ConvertEmail();

        // GET: CourseCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.CourseCategories.ToListAsync());
        }

        public PartialViewResult CategoryPartial(int? id)
        {
            var courseCategories = db.CourseCategories.ToList();
            if (id != null)
            {
                courseCategories = db.CourseCategories.Where(x => x.CourseCategoryId.Equals((int) id)).ToList();
                return PartialView(courseCategories);
            }
            return PartialView(courseCategories);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> CourseCategoryPayment()
        {
            var userId = User.Identity.GetUserId();
            var student = await db.Students.FindAsync(userId);

            var model = new List<CourseCategory>();
            var listOfCourses = new LearningPathVm();
            var assignedCourse = await db.AssignCourseCategories.Include(i => i.CourseCategory).Include(i => i.Courses)
                .AsNoTracking().Where(x => x.CourseCategory.StudentType.Equals(student.AccountType)).ToListAsync();

            var paymentList = await db.StudentPayments.AsNoTracking().Where(x => x.StudentId.Equals(student.StudentId)
                                    && x.IsPayed.Equals(true)).ToListAsync();

            //list of all the course
          //  var allCourses = db.Courses.ToList();
            var assigedCourses = db.AssignCourseCategories
                                   .Include(x => x.Courses)
                                   .DistinctBy(x => x.CourseId)
                                   .ToList();
            if (paymentList.Any())
            {
                foreach (var courseCategory in assignedCourse)
                {
                    var checkpayement = paymentList.FirstOrDefault(x => x.CourseCategoryId.Equals(courseCategory.CourseCategoryId));
                    if (checkpayement == null)
                    {
                        model.Add(courseCategory.CourseCategory);
                    }

                    //listOfCourses.AssignCourseCategory = ;
                    
                   
                }
            }
            else
            {
                model.AddRange(assignedCourse.Select(s => s.CourseCategory));
            }
            listOfCourses.Payment = paymentList;
            listOfCourses.AssignCourseCategory = assigedCourses;
            listOfCourses.CourseCategory = model;
            //  return View(model.DistinctBy(x => x.CourseCategoryId));
            return View(listOfCourses);
        }


        /// <summary>
        /// using express paypal checkout to make international payment
        /// </summary>
        /// <returns></returns>
        public ActionResult PaypalCheckout(CourseCategoryDetailVm model)
        {
            model.amt = GetCurrency(model.amt);
            if (model.amt != null)
            {
                var convertAmount = decimal.Parse(model.amt) / 360;
                model.amt = convertAmount.ToString();
            }
           
            return View(model);
        }

        public string GetCurrency(string value)
        {
            return value;
        }

        /// <summary>
        /// Return Url for paypal
        /// </summary>
        /// <param name="pal"></param>
        /// <returns></returns>
        public ActionResult PayPalSuccess()
        {

            var getData = new GetDataPayPal();
            var order = getData.InformationOrder(getData.GetPayPalResponse(Request.QueryString["tx"]));

            //var newOrder = new StudentPaypalPayment
            //{
            //    PayerEmail = order.PayerEmail,
            //    PayerFirstName = order.PayerFirstName,
            //    PayerLastName = order.PayerLastName,
            //    Currency = order.Currency,
            //    Amount = order.Amount,
            //    ItemName = order.ItemName,
            //    TxToken = order.TxToken,
            //    ReceiverEmail = order.ReceiverEmail,
            //    PaymentStatus = order.PaymentStatus,
            //    CourseCategoryId = order.CourseCategoryId,
            //    PaymentDate = order.PaymentDate,
            //    PayerId = order.PayerId


            //};

            //db.StudentPaypalPayments.Add(newOrder);
            //db.SaveChanges();

            // ViewBag.Tx = tx;
            return View(order);
        }

        public ActionResult PayPalConfirmPayment()
        {
            var getData = new GetDataPayPal();
            var order = getData.InformationOrder(getData.GetPayPalResponse(Request.QueryString["tx"]));

            //object to save to the studentpayment where the query for showing the paid course is performed
            var studentPayment = new StudentPayment();

            try
            {
                var newOrder = new StudentPaypalPayment
                {
                    PayerEmail = order.PayerEmail,
                    PayerFirstName = order.PayerFirstName,
                    PayerLastName = order.PayerLastName,
                    Currency = order.Currency,
                    Amount = order.Amount,
                    ItemName = order.ItemName,
                    TxToken = order.TxToken,
                    ReceiverEmail = order.ReceiverEmail,
                    PaymentStatus = order.PaymentStatus,
                    CourseCategoryId = order.CourseCategoryId,
                    PaymentDate = order.PaymentDate,
                    PayerId = order.PayerId


                };

                if (order.PaymentStatus == "Completed")
                {




                    studentPayment.CourseCategoryId = order.CourseCategoryId;
                    studentPayment.StudentId = order.StudentId;
                    studentPayment.PaymentDateTime = DateTime.Now;
                    studentPayment.Amount = Decimal.Parse(order.Amount);
                    studentPayment.IsPayed = true;




                }
                else
                {

                    studentPayment.CourseCategoryId = order.CourseCategoryId;
                    studentPayment.StudentId = order.StudentId;
                    studentPayment.PaymentDateTime = DateTime.Now;
                    studentPayment.Amount = Decimal.Parse(order.Amount);
                    studentPayment.IsPayed = false;




                };
                db.StudentPayments.Add(studentPayment);
                db.StudentPaypalPayments.Add(newOrder);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

          

            return View(order);
        }

        /// <summary>
        /// method to convert currency
        /// </summary>
        /// <param name="model"></param>
        /// <returns>decimal of monet</returns>
        public decimal CurrecyConverter(decimal amount)
        {
            return 0;
        }

        //public async Task<ActionResult> StartPayment(int? id)
        //{

        //    var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
        //    var api = new PayStackApi(testOrLiveSecret);
        //    var userId = User.Identity.GetUserId();
        //    bool isPayAll = false;
        //    decimal amount = 0;
        //    var userRole = User.Identity;
        //    if (userId != null)
        //    {
        //        var student = await db.Students.AsNoTracking().Where(x => x.StudentId.Equals(userId)).FirstOrDefaultAsync();

        //        if (id == null)
        //        {
        //            amount = await db.AssignCourseCategories.Include(i => i.CourseCategory).AsNoTracking()
        //                 .Where(x => x.CourseCategory.StudentType.Equals(student.AccountType))
        //                 .SumAsync(s => s.CourseCategory.Amount);
        //            isPayAll = true;
        //        }
        //        else
        //        {
        //            amount = db.CourseCategories.Where(x => x.CourseCategoryId.Equals((int)id)).Select(x => x.Amount).FirstOrDefault();
        //            id = (int)id;
        //        }


        //        var convertedamount = KoboToNaira.ConvertKoboToNaira(amount);
        //        var transactionInitializaRequest = new TransactionInitializeRequest
        //        {
        //            //Reference = "SwifKampus",
        //            AmountInKobo = convertedamount,
        //            CallbackUrl = "http://localhost:64301/CourseCategories/ConfirmPaystackPayment",
        //            // CallbackUrl = "http://codedenim.azurewebsites.net/CourseCategories/ConfrimPayment",
        //            Email = student.Email,
        //            Bearer = "Application fee",

        //            CustomFields = new List<CustomField>
        //            {
        //                new  CustomField("coursecategoryid","coursecategoryid", id.ToString()),
        //                new  CustomField("studentid","studentid", student.StudentId),
        //                new  CustomField("ispayedall","ispayedall", isPayAll.ToString()),
        //            }

        //        };
        //        var response = api.Transactions.Initialize(transactionInitializaRequest);

        //        if (response.Status)
        //        {
        //            //redirect to authorization url
        //            return RedirectPermanent(response.Data.AuthorizationUrl);
        //            // return Content("Successful");
        //        }
        //        return Content("An error occurred");
        //    }
        //    return RedirectToAction("Login", "Account");

        //}

        public async Task<ActionResult> ConfirmPaystackPayment(object[] values)
        {
            return RedirectToAction("ConfrimRrrPayment", "RemitaServices");
        }

        //public async Task<ActionResult> ConfrimPayment(string reference)
        //{
        //    var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
        //    var api = new PayStackApi(testOrLiveSecret);
        //    //Verifying a transaction
        //    var verifyResponse = api.Transactions.Verify(reference); // auto or supplied when initializing;
        //    if (verifyResponse.Status)
        //    {
        //        var convertedValues = new List<SelectableEnumItem>();
        //        var valuepair = verifyResponse.Data.Metadata.Where(x => x.Key.Contains("custom")).Select(s => s.Value);

        //        foreach (var item in valuepair)
        //        {
        //            convertedValues = ((JArray)item).Select(x => new SelectableEnumItem
        //            {
        //                key = (string)x["display_name"],
        //                value = (string)x["value"]
        //            }).ToList();
        //        }
        //        var userId = User.Identity.GetUserId();
        //        var student = await db.Students.FindAsync(userId);
        //        var ispayedAll = convertedValues.Where(x => x.key.Equals("ispayedall")).Select(s => s.value)
        //                        .FirstOrDefault();
        //        if (ispayedAll.ToUpper().Equals("TRUE"))
        //        {
        //            var courseCategory = await db.AssignCourseCategories.Include(i => i.CourseCategory).AsNoTracking()
        //                .Where(x => x.CourseCategory.StudentType.Equals(student.AccountType))
        //                .ToListAsync();
        //            foreach (var coureCat in courseCategory)
        //            {
        //                var studentPayments = new StudentPayment()
        //                {
        //                    PaymentDateTime = DateTime.Now,
        //                    CourseCategoryId = coureCat.CourseCategoryId,
        //                    StudentId = convertedValues.Where(x => x.key.Equals("studentid")).Select(s => s.value).FirstOrDefault(),
        //                    Amount = KoboToNaira.ConvertKoboToNaira(verifyResponse.Data.Amount),
        //                    IsPayed = true,
        //                    AmountPaid = KoboToNaira.ConvertKoboToNaira(verifyResponse.Data.Amount),

        //                };
        //                db.StudentPayments.Add(studentPayments);

        //            }
        //            await db.SaveChangesAsync();
        //            return RedirectToAction("DashBoard", "Students");
        //        }
        //        var studentPayment = new StudentPayment()
        //        {
        //            PaymentDateTime = DateTime.Now,
        //            CourseCategoryId = Convert.ToInt32(convertedValues.Where(x => x.key.Equals("coursecategoryid")).Select(s => s.value)
        //                .FirstOrDefault()),
        //            StudentId = convertedValues.Where(x => x.key.Equals("studentid")).Select(s => s.value).FirstOrDefault(),
        //            ReferenceNo = reference,
        //            Amount = KoboToNaira.ConvertKoboToNaira(verifyResponse.Data.Amount),
        //            IsPayed = true,
        //            AmountPaid = KoboToNaira.ConvertKoboToNaira(verifyResponse.Data.Amount),

        //        };
        //        db.StudentPayments.Add(studentPayment);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("MyCoursesAsync", "Courses");
        //    }
        //    return RedirectToAction("ListCourses", "Courses");
        //}



        public static class KoboToNaira
        {
            public static int ConvertKoboToNaira(decimal naira)
            {

                var kobo = naira * 100;
                return (int)kobo;
            }
        }
        /// <summary>
        /// this method  takes in a category Id and and displays all the courses
        /// associated with that category
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> CategoryDetailsMobile(int id, string email)
        {
            var courseDetails = new CourseCategoryDetailVm();
            if (email != null)
            {
                var userId = converter.ConvertEmailToId(email);

                var hasPayed = await db.StudentPayments
                                        .AsNoTracking()
                                         .Where(x => x.StudentId.Equals(userId)
                                         &&
                                           x.CourseCategoryId.Equals((int)id) && x.IsPayed.Equals(true))
                                           .FirstOrDefaultAsync();
                if (hasPayed != null)
                {
                    if (hasPayed.IsPayed.Equals(false))
                    {
                        string serviceTypeId = string.Empty;


                        var hashed = myHash.HashRemitedValidate(hasPayed.OrderId, RemitaConfigParams.APIKEY, RemitaConfigParams.MERCHANTID);
                        string checkurl = RemitaConfigParams.CHECKSTATUSURL + "/" + RemitaConfigParams.MERCHANTID + "/" + hasPayed.OrderId + "/" + hashed + "/" + "orderstatus.reg";
                        string jsondata = new WebClient().DownloadString(checkurl);
                        var result = JsonConvert.DeserializeObject<RemitaResponse>(jsondata);
                        if (string.IsNullOrEmpty(result.Rrr))
                        {
                            var entry = db.Entry(hasPayed);
                            if (entry.State == EntityState.Detached)
                                db.StudentPayments.Attach(hasPayed);
                            db.StudentPayments.Remove(hasPayed);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            return RedirectToAction("ConfrimPayment", new { orderID = hasPayed.OrderId });
                        }



                    }
                    else
                    {
                        //return to the receipt page
                    }
                }

                System.Threading.Thread.Sleep(1);
                long milliseconds = DateTime.Now.Ticks;
                var url = Url.Action("ConfrimPayment", "CourseCategories", new { }, protocol: Request.Url.Scheme);


                var student = db.Students.Where(x => x.StudentId.Equals(userId)).Select(s => new {
                    s.AccountType,
                    s.Email,
                    s.PhoneNumber,
                    s.FirstName,
                    s.LastName,
                    s.MiddleName
                }).SingleOrDefault();
                var fullName = $"{student.LastName} {student.FirstName} {student.MiddleName}";

                var model = new List<CourseCategory>();

                var assignedCourse = db.AssignCourseCategories.Include(i => i.CourseCategory).Include(i => i.Courses)
                    .AsNoTracking().Where(x => x.CourseCategory.StudentType.Equals(student.AccountType) && x.CourseCategoryId.Equals(id)).ToList();

                var categories = db.CourseCategories.Find(id);
                var assingeCourses = db.AssignCourseCategories.Where(x => x.CourseCategoryId.Equals(id)).ToList();


                var currency = from Currency s in Enum.GetValues(typeof(Currency))
                               select new { Id = (int)s, Name = s.ToString() };
                ViewBag.Currency = new SelectList(currency.ToList(), "Name", "Name");

                courseDetails.CourseCategory = categories;
                courseDetails.AssignedCourses = assingeCourses;
                courseDetails.orderId = milliseconds.ToString();
                courseDetails.responseurl = url;
                courseDetails.StudentId = userId;
                courseDetails.payerEmail = student.Email;
                courseDetails.payerName = fullName;
                courseDetails.CourseCategoryId = id;
                courseDetails.payerPhone = student.PhoneNumber;

                courseDetails.amt = categories.Amount.ToString();

                RedirectToAction("PaypalCheckout", courseDetails);

            }

            return View(courseDetails);
        }

        [Authorize]
        public async Task<ActionResult> CategoryDetails(int id)
        {
            var userId = User.Identity.GetUserId();

            var hasPayed = await db.StudentPayments
                                    .AsNoTracking()
                                     .Where(x => x.StudentId.Equals(userId)
                                     &&
                                       x.CourseCategoryId.Equals((int)id) && x.IsPayed.Equals(true))
                                       .FirstOrDefaultAsync();
            if (hasPayed != null)
            {
                if (hasPayed.IsPayed.Equals(false))
                {
                    string serviceTypeId = string.Empty;


                    var hashed = myHash.HashRemitedValidate(hasPayed.OrderId, RemitaConfigParams.APIKEY, RemitaConfigParams.MERCHANTID);
                    string checkurl = RemitaConfigParams.CHECKSTATUSURL + "/" + RemitaConfigParams.MERCHANTID + "/" + hasPayed.OrderId + "/" + hashed + "/" + "orderstatus.reg";
                    string jsondata = new WebClient().DownloadString(checkurl);
                    var result = JsonConvert.DeserializeObject<RemitaResponse>(jsondata);
                    if (string.IsNullOrEmpty(result.Rrr))
                    {
                        var entry = db.Entry(hasPayed);
                        if (entry.State == EntityState.Detached)
                            db.StudentPayments.Attach(hasPayed);
                        db.StudentPayments.Remove(hasPayed);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        return RedirectToAction("ConfrimPayment", new { orderID = hasPayed.OrderId });
                    }



                }
                else
                {
                    //return to the receipt page
                }
            }

            System.Threading.Thread.Sleep(1);
            long milliseconds = DateTime.Now.Ticks;
            var url = Url.Action("ConfrimPayment", "CourseCategories", new { }, protocol: Request.Url.Scheme);


            var student = db.Students.Where(x => x.StudentId.Equals(userId)).Select(s => new {
                s.AccountType,
                s.Email,
                s.PhoneNumber,
                s.FirstName,
                s.LastName,
                s.MiddleName
            }).SingleOrDefault();
            var fullName = $"{student.LastName} {student.FirstName} {student.MiddleName}";

            var model = new List<CourseCategory>();

            var assignedCourse = db.AssignCourseCategories.Include(i => i.CourseCategory).Include(i => i.Courses)
                .AsNoTracking().Where(x => x.CourseCategory.StudentType.Equals(student.AccountType) && x.CourseCategoryId.Equals(id)).ToList();
            var courseDetails = new CourseCategoryDetailVm();
            var categories = db.CourseCategories.Find(id);
            var assingeCourses = db.AssignCourseCategories.Where(x => x.CourseCategoryId.Equals(id)).ToList();


            var currency = from Currency s in Enum.GetValues(typeof(Currency))
                           select new { Id = (int)s, Name = s.ToString() };
            ViewBag.Currency = new SelectList(currency.ToList(), "Name", "Name");

            courseDetails.CourseCategory = categories;
            courseDetails.AssignedCourses = assingeCourses;
            courseDetails.orderId = milliseconds.ToString();
            courseDetails.responseurl = url;
            courseDetails.StudentId = userId;
            courseDetails.payerEmail = student.Email;
            courseDetails.payerName = fullName;
            courseDetails.CourseCategoryId = id;
            courseDetails.payerPhone = student.PhoneNumber;

            courseDetails.amt = categories.Amount.ToString();

            RedirectToAction("PaypalCheckout", courseDetails);

            return View(courseDetails);
        }




        [HttpPost]
        public async Task<ActionResult> CreatePayment(CourseCategoryDetailVm model)
        {
            if (ModelState.IsValid)
            {
                //var hasTransaction = await db.SchoolFeePayments.AsNoTracking().Where(x => x.StudentId.Equals(model.StudentId)
                //                                && x.SessionId.Equals(model.SessionId)
                //                                && x.FeeCategory.Equals(model.FeeCategory))
                //                                .ToListAsync();
                model.paymenttype = model.RemitaPaymentType.ToString().Replace("_", " ").ToLower();             
                
                if (string.IsNullOrEmpty(model.payerEmail))
                {
                    model.payerEmail = $"{model.payerName}@codednim.com";
                }


                var studentPayment = new StudentPayment
                {
                    OrderId = model.orderId,
                    PaymentDateTime = DateTime.Now,

                    StudentId = model.StudentId,
                    CourseCategoryId = model.CourseCategoryId,
                    Amount = Convert.ToDecimal(model.amt),
                
                };
                db.StudentPayments.Add(studentPayment);
                var log = new RemitaPaymentLog
                {
                    OrderId = model.orderId,
                    PaymentName = "Course Category Payment",
                    PaymentDate = DateTime.Now,
                    Amount = model.amt.ToString(),
                    PayerName = model.payerName


                };
                db.RemitaPaymentLogs.Add(log);
                await db.SaveChangesAsync();
                model.serviceTypeId = RemitaConfigParams.SERVICETYPEID;
                model.merchantId = RemitaConfigParams.MERCHANTID;
                
                model.hash = myHash.HashRemitaRequest(RemitaConfigParams.MERCHANTID, RemitaConfigParams.SERVICETYPEID
                    , model.orderId, model.amt, model.responseurl, RemitaConfigParams.APIKEY);
                return RedirectToAction("SubmitRemita", model);
            }
            return View(model);

          
        }


        [AllowAnonymous]
        public ActionResult SubmitRemita(CourseCategoryDetailVm model)
        {
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RetrySchoolFeePayment(string rrr)
        {

            var hashrrr = myHash.HashRrrQuery(rrr, RemitaConfigParams.APIKEY, RemitaConfigParams.MERCHANTID);
            string posturl = RemitaConfigParams.CHECKSTATUSURL + "/" + RemitaConfigParams.MERCHANTID + "/" + rrr + "/" + hashrrr + "/" + "status.reg";
            string jsondata = new WebClient().DownloadString(posturl);
            var result = JsonConvert.DeserializeObject<RemitaResponse>(jsondata);
            if (result.Status.Equals("00") || result.Status.Equals("01"))
            {
                return RedirectToAction("ConfrimPayment", "CourseCategories", new { RRR = result.Rrr, orderID = result.OrderId });
            }
            var url = Url.Action("ConfrimPayment", "CourseCategories", new { }, protocol: Request.Url.Scheme);
            var hash = myHash.HashRemitedRePost(RemitaConfigParams.MERCHANTID, rrr, RemitaConfigParams.APIKEY);

            var model = new RemitaRePostVm
            {
                rrr = rrr,
                merchantId = RemitaConfigParams.MERCHANTID,
                hash = hash,
                responseurl = url
            };
            return View(model);
        }



        [AllowAnonymous]
        public async Task<ActionResult> ConfrimPayment(string RRR, string orderID)
        {
            StudentPayment studentPayment;
            RemitaResponse result = new RemitaResponse();
            if (string.IsNullOrEmpty(orderID))
            {
                studentPayment = await db.StudentPayments.AsNoTracking()
                    .Where(x => x.ReferenceNo.Equals(RRR))
                    .FirstOrDefaultAsync();
            }
            else
            {
                studentPayment = await db.StudentPayments.AsNoTracking()
                    .Where(x => x.OrderId.Equals(orderID.Trim()))
                    .FirstOrDefaultAsync();
            }
            if (studentPayment != null)
            {
                if (studentPayment.IsPayed.Equals(true))
                {
                    result.Message = studentPayment.PaymentStatus;
                    result.OrderId = studentPayment.OrderId;
                    result.Rrr = studentPayment.ReferenceNo;
                    result.Status = studentPayment.IsPayed.ToString();
                    result.PayerName = studentPayment.Student.FullName;
                    result.Price = studentPayment.Amount;
                    result.PaymentDate = studentPayment.PaymentDateTime;
                    result.CourseCategory = studentPayment.CourseCategory.CategoryName;
                    return RedirectToAction("ConfrimRrrPayment", "RemitaServices", result);
                }
                var log = await db.RemitaPaymentLogs.AsNoTracking().Where(x => x.OrderId.Equals(studentPayment.OrderId))
                                            .FirstOrDefaultAsync();

                var hashed = myHash.HashRemitedValidate(studentPayment.OrderId, RemitaConfigParams.APIKEY, RemitaConfigParams.MERCHANTID);
                string url = RemitaConfigParams.CHECKSTATUSURL + "/" + RemitaConfigParams.MERCHANTID + "/" + studentPayment.OrderId + "/" + hashed + "/" + "orderstatus.reg";
                string jsondata = new WebClient().DownloadString(url);
                result = JsonConvert.DeserializeObject<RemitaResponse>(jsondata);

                if (result.Status.Equals("00") || result.Status.Equals("01"))
                {
                    studentPayment.IsPayed = true;
                    studentPayment.PaymentStatus = result.Message;
                    studentPayment.ReferenceNo = result.Rrr;
                    db.Entry(studentPayment).State = EntityState.Modified;

                    log.Rrr = result.Rrr;
                    log.StatusCode = result.Status;
                    log.TransactionMessage = result.Message;
                    db.Entry(log).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                else
                {
                    studentPayment.IsPayed = false;
                    studentPayment.PaymentStatus = result.Message;
                    studentPayment.ReferenceNo = result.Rrr;
                    db.Entry(studentPayment).State = EntityState.Modified;

                    log.Rrr = result.Rrr;
                    log.StatusCode = result.Status;
                    log.TransactionMessage = result.Message;
                    db.Entry(log).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("RetrySchoolFeePayment", new { rrr = result.Rrr });

                }

                return RedirectToAction("ConfrimRrrPayment", "RemitaServices", result);
            }
            var message = $"There is no payment that has either the RRR {RRR} or" +
                            $" Order Id {orderID} for School fee or Acceptance Fee";
            return RedirectToAction("GetPaymentStatus", "RemitaServices", new { message = message });

        }

        // GET: CourseCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var userId = User.Identity.GetUserId();
            var userType = db.Students.Where(x => x.StudentId.Equals(userId)).Select(x => x.AccountType).FirstOrDefault();
            // var userRole = User.IsInRole(userType);
            var enrolledCourses = db.AssignCourseCategories.Where(x => x.CourseCategory.StudentType.Equals(userType)).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var categoryVm = new CategoryVm
            {
                Courses = enrolledCourses
            };
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return View(categoryVm);
        }
        [Authorize]
        public async Task<ActionResult> LearningPathDetails()
        {
            var userId = User.Identity.GetUserId();


            var student = await db.Students.Where(x => x.StudentId.Equals(userId)).Select(x => x.AccountType).SingleOrDefaultAsync();

            var model = new List<CourseCategory>();

            LearningPathVm learningPathVm = CategoriesAndCoursesList(student);
            //return View(assignedCourse);
            return View(learningPathVm);
        }

        private LearningPathVm CategoriesAndCoursesList(string student)
        {
            var learningPathVm = new LearningPathVm();
            //list of assigned courses
            var assignedCourse = db.AssignCourseCategories.Include(i => i.CourseCategory).Include(i => i.Courses)
                .AsNoTracking().Where(x => x.CourseCategory.StudentType.Equals(student)).DistinctBy(s => s.CourseCategoryId).AsQueryable().ToList();
            //list of all courses
            var allCourses = db.Courses.ToList();
            //assigning course and course category to Learning path
            learningPathVm.AssignCourseCategory = assignedCourse;
            learningPathVm.Courses = allCourses;
            return learningPathVm;
        }

        // GET: CourseCategories/Create
        public ActionResult Create()
        {

            var studentType = from StudentTypes s in Enum.GetValues(typeof(StudentTypes))
                              select new { Id = s, Name = s.ToString() };
            ViewBag.StudentType = new SelectList(studentType, "Name", "Name");
            return View();
        }

        // POST: CourseCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseCategoryId,CategoryName,Amount,StudentType")] CourseCategory courseCategory, HttpPostedFileBase File)
        {
            //var fp = new UploadedFileProcessor();

            //var path = fp.ProcessFilePath(File);
            if (ModelState.IsValid)
            {
                // courseCategory.ImageLocation = path.Path;
             
                db.CourseCategories.Add(courseCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //  ViewBag.StudentType = new SelectList(studentType, "Name", "Name");
            return View(courseCategory);
        }

        /// <summary>
        /// the create category partial
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CreateCategoryPartial(int categoryId)
        {

            return PartialView();
        }
        [HttpPost]
        public PartialViewResult CreateCategoryPartial(Course course)
        {

            return PartialView();
        }

        // GET: CourseCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentType = from StudentTypes s in Enum.GetValues(typeof(StudentTypes))
                              select new { Id = s, Name = s.ToString() };
            ViewBag.StudentType = new SelectList(studentType, "Name", "Name");
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return View(courseCategory);
        }

        // POST: CourseCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseCategoryId,CategoryName,Amount,StudentType,ImageLocation")] CourseCategory courseCategory,HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {


                var imageFromDB = db.CourseCategories.Where(x => x.CourseCategoryId.Equals(courseCategory.CourseCategoryId)).Select(x => x.ImageLocation).ToString();

                DeletePhoto(courseCategory);
                var fp = new UploadedFileProcessor();

                var path = fp.ProcessFilePath(File);
                courseCategory.ImageLocation = path.Path;
                db.Entry(courseCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(courseCategory);
        }
        /// <summary>
        /// this method delete the photo from the file location on the server
        /// </summary>
        /// <param name="courseCategory"></param>
        private void DeletePhoto(CourseCategory courseCategory)
        {
            var photoName = "";
            photoName = courseCategory.ImageLocation;
            string fullPath = Request.MapPath("~/MaterialUpload/" + photoName);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                //Session["DeleteSuccess"] = "Yes";
            }
        }

        // GET: CourseCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return View(courseCategory);
        }

        // POST: CourseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            db.CourseCategories.Remove(courseCategory);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Action to Upload course categories using excel files.
        /// </summary>
        /// <param name="excelfile">HttpPostedFieBase</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public async Task<ActionResult> ExcelUpload(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View();
            }
            HttpPostedFileBase file = Request.Files["excelfile"];
            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string lastrecord = string.Empty;
                int recordCount = 0;
                string message = string.Empty;
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                // Read data from excel file
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    foreach (var sheet in currentSheet)
                    {
                        ExcelValidation myExcel = new ExcelValidation();
                        //var workSheet = currentSheet.First();
                        var noOfCol = sheet.Dimension.End.Column;
                        var noOfRow = sheet.Dimension.End.Row;
                        int requiredField = 5;

                        string validCheck = myExcel.ValidateExcel(noOfRow, sheet, requiredField);
                        if (!validCheck.Equals("Success"))
                        {

                            string[] ssizes = validCheck.Split(' ');
                            string[] myArray = new string[2];
                            for (int i = 0; i < ssizes.Length; i++)
                            {
                                myArray[i] = ssizes[i];
                            }
                            string lineError =
                                $"Please Check sheet {sheet}, Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                            //ViewBag.LineError = lineError;
                            TempData["UserMessage"] = lineError;
                            TempData["Title"] = "Error.";
                            return View();
                        }

                        for (int row = 2; row <= noOfRow; row++)
                        {
                            string studentType = sheet.Cells[row, 1].Value.ToString().Trim();
                            string categoryName = sheet.Cells[row, 2].Value.ToString().ToUpper().Trim();
                            string courseDescription = sheet.Cells[row, 3].Value.ToString().Trim().ToUpper();
                            int amount = Int32.Parse(sheet.Cells[row, 4].Value.ToString().ToUpper().Trim());
                            string imageLocation = sheet.Cells[row, 5].Value.ToString().Trim();

                            //var subjectName = db.Subjects.Where(x => x.SubjectCode.Equals(subjectValue))
                            //    .Select(c => c.SubjectId).FirstOrDefault();

                            var category = db.CourseCategories.Where(x => x.CategoryName.Equals(categoryName) && x.StudentType.Equals(studentType));
                            var countFromDb = await category.CountAsync();
                            if (countFromDb >= 1)
                            {
                                return View("Error2");
                            }
                            var mycategory = new CourseCategory()
                            {
                                CategoryName = categoryName,

                                Amount = amount,
                                StudentType = studentType,
                                CategoryDescription = courseDescription,
                                ImageLocation = imageLocation,


                            };
                            db.CourseCategories.Add(mycategory);

                            recordCount++;
                          //  return View("Index",mycategory);
                            //lastrecord = $"The last Updated record has the Student Id {studentId} and Subject Name is {subjectName}. Please Confirm!!!";
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
            return View("ExcelUpload");
        }
    }
}
