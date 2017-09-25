using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using Newtonsoft.Json.Linq;
using PagedList;
using PayStack.Net;

namespace CodedenimWebApp.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = from s in db.Students
                select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                               || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));

            // return View(await db.Students.ToListAsync());
        }
        
        public ActionResult DashBoard(string reference, string trxref)
            {
            if ((reference != null) && (trxref != null))
            {
                var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
                var api = new PayStackApi(testOrLiveSecret);
               // Verifying a transaction
                var verifyResponse = api.Transactions.Verify(reference); // auto or supplied when initializing;
                if (verifyResponse.Status)
                {
                    /* 
                       You can save the details from the json object returned above so that the authorization code 
                       can be used for charging subsequent transactions

                       // var authCode = verifyResponse.Data.Authorization.AuthorizationCode
                       // Save 'authCode' for future charges!

                   */
                    //var customfieldArray = verifyResponse.Data.Metadata.CustomFields.A

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
                    //var studentid = _db.Users.Find(id);
                    var professionalPayment = new ProfessionalPayment()
                    {
                        //FeeCategoryId = Convert.ToInt32(verifyResponse.Data.Metadata.CustomFields[3].Value),
                        ProfessionalWorkerId = convertedValues.Where(x => x.key.Equals("professionalworkerid")).Select(s => s.value).FirstOrDefault(),
                        PaymentDateTime = DateTime.Now,
                        Amount = Convert.ToDecimal(convertedValues.Where(x => x.key.Equals("amount")).Select(s => s.value).FirstOrDefault()),
                        IsPayed = true,
                        //StudentId = "HAS-201",
                        AmountPaid = PaymentTypesController.KoboToNaira.ConvertKoboToNaira(verifyResponse.Data.Amount),

                    };
                    db.ProfessionalPayments.Add(professionalPayment);
                   db.SaveChangesAsync();
                }
                return RedirectToAction("DashBoard");
            }
            var courses = db.Courses.ToList();
            return View(courses);
        }

        // GET: Students/Details/5

        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public async Task<ActionResult> CreateCorper()
        {
            return View();
        }

        public async Task<ActionResult> CreateUnderGrad()
        {
            return View();
        }

        public ActionResult RegularStudent()
        {
            return View();
        }

        public async Task<ActionResult> RegularStudent(RegularStudentVm regularStudentVm)
        {
            if (ModelState.IsValid)
            {
                var student = new Student();
                student.FirstName = regularStudentVm.FirstName;
                student.LastName = regularStudentVm.LastName;
                student.Email = regularStudentVm.Email;
                student.PhoneNumber = regularStudentVm.PhoneNumber;
                
                db.Students.Add(student);
            }
            return View();
        }
        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(Student student)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        db.Students.Add(student);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(student);

        //}


        [Authorize]
        public ActionResult AssignedCourseView()
        {
            var student = new Student();
            student.Enrollments = new List<Enrollment>();
            PopulateAssignedCourseData(student);
            return View();
        }





        /// <summary>
        /// Method to map the assigned courses to the student
        /// </summary>
        /// <param name="student"></param>
        private void PopulateAssignedCourseData(Student student)
        {
            var allCourses = db.Courses;
            var studentCourses = new HashSet<int>(student.Enrollments.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourses>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourses
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    Assigned = studentCourses.Contains(course.CourseId)
                });
            }
            ViewBag.Courses = viewModel;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if (upload != null && upload.ContentLength > 0)
                    //{
                    //    var avatar = new File
                    //    {
                    //        FileName = System.IO.Path.GetFileName(upload.FileName),
                    //        FileType = FileType.Photo,
                    //        ContentType = upload.ContentType
                    //    };
                    //    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    //    {
                    //        avatar.Content = reader.ReadBytes(upload.ContentLength);
                    //    }
                    //    student.Files = new List<File> { avatar };
                    //}
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Student student = db.Students.Include(s => s.Files).SingleOrDefault(s => s.StudentId == id);
            //if (student == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "StudentId,EnrollmentDate,ProgrammeId,Active,IsGraduated,FirstName,MiddleName,LastName,Gender,Email,PhoneNumber,TownOfBirth,StateOfOrigin,Nationality,CountryOfBirth,IsAcctive,DateOfBirth,Age,Passport")] Student student, HttpPostedFileBase upload)
        {
            if (student.StudentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = db.Students.Find(student.StudentId);
            if (TryUpdateModel(studentToUpdate, "",
                new string[] { "LastName", "FirstMidName", "EnrollmentDate" }))
            {
                try
                {
                    //if (upload != null && upload.ContentLength > 0)
                    //{
                    //    if (studentToUpdate.Files.Any(f => f.FileType == FileType.Photo))
                    //    {
                    //        db.Files.Remove(studentToUpdate.Files.First(f => f.FileType == FileType.Photo));
                    //    }
                    //    var avatar = new File
                    //    {
                    //        FileName = System.IO.Path.GetFileName(upload.FileName),
                    //        FileType = FileType.Photo,
                    //        ContentType = upload.ContentType
                    //    };
                    //    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    //    {
                    //        avatar.Content = reader.ReadBytes(upload.ContentLength);
                    //    }
                    //    studentToUpdate.Files = new List<File> { avatar };
                    //}
                    db.Entry(studentToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(studentToUpdate);

            //if (ModelState.IsValid)
            //{
            //    db.Entry(student).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //return View(student);
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Student student = await db.Students.FindAsync(id);
            db.Students.Remove(student);
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

    public class ProfessionalPayment
    {
       public int ProfessionalPaymentId { get; set; }
        public string ProfessionalWorkerId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public decimal Amount { get; set; }
        public bool IsPayed { get; set; }
        public object AmountPaid { get; set; }
    }

    public class SelectableEnumItem
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
