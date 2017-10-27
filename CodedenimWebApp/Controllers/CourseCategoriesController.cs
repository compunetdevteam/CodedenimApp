using CodedenimWebApp.Models;
using CodeninModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
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
using CodedenimWebApp.Service;
using CodeninModel.Abstractions;

namespace CodedenimWebApp.Controllers
{

    public class CourseCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.CourseCategories.ToListAsync());
        }

        public async Task<ActionResult> CourseCategoryPayment()
        {
            var userId = User.Identity.GetUserId();
            var student = await db.Students.FindAsync(userId);
                
            var model = new List<CourseCategory>();

            var assignedCourse = await db.AssignCourseCategories.Include(i => i.CourseCategory).Include(i => i.Courses)
                .AsNoTracking().Where(x => x.CourseCategory.StudentType.Equals(student.AccountType)).ToListAsync();

            var paymentList = await db.StudentPayments.AsNoTracking().Where(x => x.StudentId.Equals(student.StudentId)
                                    && x.IsPayed.Equals(true)).ToListAsync();
            if (paymentList.Any())
            {
                foreach (var courseCategory in assignedCourse)
                {
                    var checkpayement = paymentList.FirstOrDefault(x => x.CourseCategoryId.Equals(courseCategory.CourseCategoryId));
                    if (checkpayement == null)
                    {
                        model.Add(courseCategory.CourseCategory);
                    }
                }
            }
            else
            {
                model.AddRange(assignedCourse.Select(s => s.CourseCategory));
            }

            return View(model);
        }
        public async Task<ActionResult> StartPayment(int? id)
        {

            var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
            var api = new PayStackApi(testOrLiveSecret);
            var userId = User.Identity.GetUserId();
            bool isPayAll = false;
            decimal amount = 0;
            // var userRole = User.Identity;
            if (userId != null)
            {
                var student = await db.Students.AsNoTracking().Where(x => x.StudentId.Equals(userId)).FirstOrDefaultAsync();

                if (id == null)
                {
                    amount = await db.AssignCourseCategories.Include(i => i.CourseCategory).AsNoTracking()
                         .Where(x => x.CourseCategory.StudentType.Equals(student.AccountType))
                         .SumAsync(s => s.CourseCategory.Amount);
                    isPayAll = true;
                }
                else
                {
                    amount = db.CourseCategories.Where(x => x.CourseCategoryId.Equals((int)id)).Select(x => x.Amount).FirstOrDefault();
                    id = (int) id;
                }


                var convertedamount = KoboToNaira.ConvertKoboToNaira(amount);
                var transactionInitializaRequest = new TransactionInitializeRequest
                {
                    //Reference = "SwifKampus",
                    AmountInKobo = convertedamount,
                    CallbackUrl = "http://localhost:64301/CourseCategories/ConfrimPayment",
                    Email = student.Email,
                    Bearer = "Application fee",

                    CustomFields = new List<CustomField>
                    {
                        new  CustomField("coursecategoryid","coursecategoryid", id.ToString()),
                        new  CustomField("studentid","studentid", student.StudentId),
                        new  CustomField("ispayedall","ispayedall", isPayAll.ToString()),
                    }

                };
                var response = api.Transactions.Initialize(transactionInitializaRequest);

                if (response.Status)
                {
                    //redirect to authorization url
                    return RedirectPermanent(response.Data.AuthorizationUrl);
                    // return Content("Successful");
                }
                return Content("An error occurred");
            }
            return RedirectToAction("Login", "Account");

        }

        public async Task<ActionResult> ConfrimPayment(string reference)
        {
            var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
            var api = new PayStackApi(testOrLiveSecret);
            //Verifying a transaction
            var verifyResponse = api.Transactions.Verify(reference); // auto or supplied when initializing;
            if (verifyResponse.Status)
            {
                var convertedValues = new List<SelectableEnumItem>();
                var valuepair = verifyResponse.Data.Metadata.Where(x => x.Key.Contains("custom")).Select(s => s.Value);

                foreach (var item in valuepair)
                {
                    convertedValues = ((JArray)item).Select(x => new SelectableEnumItem
                    {
                        key = (string)x["display_name"],
                        value = (string)x["value"]
                    }).ToList();
                }
                var userId = User.Identity.GetUserId();
                var student = await db.Students.FindAsync(userId);
                var ispayedAll = convertedValues.Where(x => x.key.Equals("ispayedall")).Select(s => s.value)
                                .FirstOrDefault();
                if (ispayedAll.ToUpper().Equals("TRUE"))
                {
                    var courseCategory = await db.AssignCourseCategories.Include(i => i.CourseCategory).AsNoTracking()
                        .Where(x => x.CourseCategory.StudentType.Equals(student.AccountType))
                        .ToListAsync();
                    foreach (var coureCat in courseCategory)
                    {
                        var studentPayments = new StudentPayment()
                        {
                            PaymentDateTime = DateTime.Now,
                            CourseCategoryId = coureCat.CourseCategoryId,
                            StudentId = convertedValues.Where(x => x.key.Equals("studentid")).Select(s => s.value)
                                .FirstOrDefault(),
                            ReferenceNo = reference,
                            Amount = KoboToNaira.ConvertKoboToNaira(verifyResponse.Data.Amount),
                            IsPayed = true,
                            AmountPaid = KoboToNaira.ConvertKoboToNaira(verifyResponse.Data.Amount),

                        };
                        db.StudentPayments.Add(studentPayments);

                    }
                    await db.SaveChangesAsync();
                    return RedirectToAction("DashBoard", "Students");
                }
                var studentPayment = new StudentPayment()
                {
                    PaymentDateTime = DateTime.Now,
                    CourseCategoryId = Convert.ToInt32(convertedValues.Where(x => x.key.Equals("coursecategoryid")).Select(s => s.value)
                        .FirstOrDefault()),
                    StudentId = convertedValues.Where(x => x.key.Equals("studentid")).Select(s => s.value).FirstOrDefault(),
                    ReferenceNo = reference,
                    Amount = KoboToNaira.ConvertKoboToNaira(verifyResponse.Data.Amount),
                    IsPayed = true,
                    AmountPaid = KoboToNaira.ConvertKoboToNaira(verifyResponse.Data.Amount),

                };
                db.StudentPayments.Add(studentPayment);
                await db.SaveChangesAsync();
                return RedirectToAction("MyCoursesAsync", "Courses");
            }
            return RedirectToAction("ListCourses", "Courses");
        }



        public static class KoboToNaira
        {
            public static int ConvertKoboToNaira(decimal naira)
            {

                var kobo = naira * 100;
                return (int)kobo;
            }
        }

        // GET: CourseCategories/Details/5
        public async Task<ActionResult> Details(int? id)
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
            var fp = new UploadedFileProcessor();

            var path = fp.ProcessFilePath(File);
            if (ModelState.IsValid)
            {
                courseCategory.ImageLocation = path.Path;
                db.CourseCategories.Add(courseCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //  ViewBag.StudentType = new SelectList(studentType, "Name", "Name");
            return View(courseCategory);
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
        public async Task<ActionResult> Edit([Bind(Include = "CourseCategoryId,CategoryName,Amount,StudentType")] CourseCategory courseCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(courseCategory);
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
    }
}
