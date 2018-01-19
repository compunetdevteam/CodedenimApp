using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using CodedenimWebApp.Abstractions;
using GenericDataRepository.Abstractions;

namespace CodedenimWebApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IRepository _repo;
        private readonly ICodedenimUserMgr _user;
        private double _progress;

        public StudentsController(ICodedenimUserMgr user, ApplicationDbContext db, IRepository repo)
        {
            _user = user;
            _db = db;
            _repo = repo;
        }
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

            //var students = from s in _db.Students
            //               select s;

            var students = await _repo.GetAllAsync<Student>(null, null, null, null);



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
        }

        public ActionResult DashBoard()
        {
            var userId = _user.GetUser();
            var email = _db.Students.Where(x => x.Id.Equals(userId)).Select(x => x.Email).FirstOrDefault();
            // var studentType = _db.Students.Where(x => x.StudentId.Equals(userId)).Select(x => x.AccountType).FirstOrDefault();
            var paymentRecord = _db.StudentPayments.FirstOrDefault(x => x.StudentId.Equals(userId) && x.IsPayed.Equals(true));

            //if (User.IsInRole(RoleName.Corper))
            //{

            //    return RedirectToAction("CorperDashboard", "Students");


            //}
            if (User.IsInRole(RoleName.UnderGraduate) || User.IsInRole(RoleName.Student) || User.IsInRole(RoleName.Corper))
            {
                if (paymentRecord == null)
                {
                    return RedirectToAction("LearningPathDetails", "CourseCategories");
                }

                var student = _db.AssignCourseCategories.Include(x => x.CourseCategory)
                    .Include(x => x.Courses)
                    .Where(x => x.CourseCategory.StudentType.Equals(RoleName.UnderGraduate))
                    .ToList();
                return RedirectToAction("MyCoursesAsync", "Courses", student);
            }


            //ViewBag.UserCourseName = _db.StudentPayments.Where(x => x.IsPayed == true && x.StudentId.Equals(userId))
            //    .Select(x => x.).FirstOrDefault();
            //return View(_db.Courses.ToList());
            return View();
        }

        /// <summary>
        /// the list of courses that a corper has enrolled
        /// </summary>
        /// <returns></returns>
        public ActionResult CorperCourses(int? courseId)
        {
            var userId = User.Identity.GetUserId();
            //   var course = db.db.ProfessionalPayments.Where(x => x.Email.Equals(vUserCourseName)).Select(x => x.CoursePayedFor).FirstOrDefault(),.Where(x => x.Email.Equals(email)).Select(x => x.CoursePayedFor).FirstOrDefault(),
            var email = _db.Students.Where(x => x.Id.Equals(userId)).Select(x => x.Email).SingleOrDefault();
            var studentId = _db.Students.Where(x => x.Id.Equals(userId)).Select(x => x.Id).FirstOrDefault();
            var callUpNumber = _db.Students.Where(x => x.Id.Equals(userId)).Select(x => x.CallUpNo).FirstOrDefault();

            if ((courseId != null) && (callUpNumber != null))
            {

                var corperEnrolleCourses = new CodeninModel.CorperEnrolledCourses
                {
                    StudentId = studentId,
                    CorperCallUpNumber = callUpNumber,
                    CourseId = (int)courseId,
                };

                _db.CorperEnrolledCourses.Add(corperEnrolleCourses);
                _db.SaveChanges();

            }
            else if (callUpNumber == null)
            {
                return RedirectToAction("Edit", "Students");
            }

            ViewBag.UserCourseName = _db.CorperEnrolledCourses.Where(x => x.StudentId.Equals(userId)).Select(x => x.CourseId);
            //  ViewBag.CourseId = db.Courses.Where(x => x.CourseName.Equals(UserCourseName)).Select(x => x.CourseId);
            //var userCourseDetail = new List<UserCourseDetail>
            //{
            //   UserCourseName = db.ProfessionalPayments.Where(x =>  x.Email.Equals(email)).Select(x => x.CoursePayedFor).FirstOrDefault(),
            //   CourseId = db.Courses.Where(x => x.CourseName.Equals(UserCourseName)).Select(x => x.CourseId )
            //};

            return View();

        }


        /// <summary>
        /// This dashbaord is used for all students not only corpers
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CorperDashboard()
        {
            var user = _user.GetUser();
            var quizTaken = _progress;
            var assignCourseCategories = new List<AssignCourseCategory>();

            var payedCourses = await _db.StudentPayments.AsNoTracking()
                                        .Where(x => x.StudentId.Equals(user) && 
                                        x.IsPayed.Equals(true)).Select(x => x.CourseCategoryId).ToListAsync();


            foreach (var item in payedCourses)
            {
                var corperCourses = await _db.AssignCourseCategories.Include(x => x.CourseCategory).Include(x => x.Courses)
              .Where(x => x.CourseCategoryId.Equals(item))
              .FirstOrDefaultAsync();
                assignCourseCategories.Add(corperCourses);
            }

            

            var courseCategory = await _db.CourseCategories.Where(x => x.StudentType.Equals(RoleName.Corper))
                .ToListAsync();
            var student = await _db.Students.Where(x => x.Id.Equals(user)).ToListAsync();

            var forumQuestion = _db.ForumQuestions.Where(x => x.StudentId.Equals(user))
                             .ToList();
            var quiz = _db.QuizLogs.Where(x => x.StudentId.Equals(user)).Take(1).ToList();
            var quizTakenList = _db.QuizLogs.Where(x => x.StudentId.Equals(user)).AsEnumerable().DistinctBy(x => x.ModuleId).Count();
            var moduleParCourseList = new List<Module>();
            foreach (var module in assignCourseCategories.DistinctBy(x => x.CourseCategoryId))
            {
                foreach (var singleModule in module.Courses.Modules)
                {
                    
                    var  moduleParCourseList1 = _db.Modules.Where(x => x.Id.Equals(singleModule.Id) && x.CourseId.Equals(singleModule.CourseId)).FirstOrDefault();

                    moduleParCourseList.Add(moduleParCourseList1);
                }
            }
            ProgressBar(quizTakenList, moduleParCourseList.Count());
            // var profilePics = GetImage();
            List<Course> certificate = GenerateCertificate(user, assignCourseCategories);
            var model = new DashboardVm()
            {
                AssignCourseCategories = assignCourseCategories,
                CourseCategories = courseCategory,
                StudentInfo = student,
                ForumQuestion = forumQuestion,
                StudentQuiz = quiz,
                Progress = _progress,
                CourseCertificate = certificate,

                // Profile = profilePics


            };

            return View(model);
        }

        private void ProgressBar(int quizTakenList,int moduleNo)
        {
           
               // var moduleNumber = _db.StudentTopicQuizs.Where(x => x.ModuleId.Equals(module.ModuleId)).FirstOrDefault();

                if((quizTakenList != 0) && (moduleNo != 0))
                {
                    _progress = (quizTakenList * 100) / moduleNo;
                }
                else
                {
                    _progress = 0;
                }
            
        }

        /// <summary>
        /// this is refactored method to generate certificate of co
        /// </summary>
        /// <param name="user"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private List<Course> GenerateCertificate(string user, List<AssignCourseCategory> categoryId)
        {
            var certificate = new List<Course>();
            foreach (var item in categoryId)
            {
                var myCourse = _db.Courses.Where(x => x.Id.Equals(item.CourseId)).FirstOrDefault();
                //  var myModule = _db.Modules.Where(x => x.CourseId.Equals(item.CourseId)).ToList();
                List<Module> myModule = ModulesNumber(myCourse);
                var numberOfModules = myModule.Count();
                foreach (var modules in item.Courses.Modules)
                {
                    //need to fix the certificate it generates
                    var NumberOfmoduleQuizTaken = _db.StudentTopicQuizs.Where(x => x.StudentId.Equals(user)).DistinctBy(x => x.ModuleId).Count();
                    var moduleQuizTaken = _db.StudentTopicQuizs.Include(x => x.Module).Where(x => x.ModuleId.Equals(modules.Id) && x.StudentId.Equals(user)).Count();
                    if (numberOfModules == NumberOfmoduleQuizTaken)
                    {
                        certificate.Add(item.Courses);
                    }
                }

            }

            return certificate.DistinctBy(x => x.Id).ToList();
        }

        private List<Module> ModulesNumber(Course myCourse)
        {
            return _db.Modules.Where(x => x.CourseId.Equals(myCourse.Id)).ToList();
        }


        //
        public ActionResult Certificate()
        {
            return View();
        }

        //this is a method that prints the certificate
        public PartialViewResult GenerateCertificate()
        {
          
            return PartialView();
        }

        //public byte[] GetImage()
        //{
        //    var user = User.Identity.GetUserId();
        //    var profilePics = _db.Students.Where(x => x.StudentId.Equals(user)).Select(x => x.Passport).FirstOrDefault();

        //    string path = Server.MapPath("~/Profile_Pics/" + profilePics);
        //    byte[] imageByteData = System.IO.File.ReadAllBytes(path);
        //    return imageByteData ;
        //}

        /// <summary>
        /// CorperDashboard1 is the First Dashboard 
        /// that a corper see when they login
        /// can view Courses based on Categories,
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> CorperDashboard1()
        {
            var categories = await _db.CourseCategories.ToListAsync();

            var corperDashboard1 = new CorperDashboard1Vm
            {
                CourseCategories = categories,
            };
            return View(corperDashboard1);
        }



        // GET: Students/Details/5

        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                id = User.Identity.GetUserId();
            }
            Student student = await _db.Students.FindAsync(id);
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
            var allCourses = _db.Courses;
            var studentCourses = new HashSet<int>(student.Enrollments.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourses>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourses
                {
                    CourseId = course.Id,
                    CourseName = course.CourseName,
                    Assigned = studentCourses.Contains(course.Id)
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
                string _FileName = String.Empty;
                if (upload.ContentLength > 0)
                {
                    _FileName = Path.GetFileName(upload.FileName);
                    string path = HostingEnvironment.MapPath("~/ProfilePics/") + _FileName;
                    student.FileLocation = path;
                    var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/ProfilePics/"));
                    if (directory.Exists == false)
                    {
                        directory.Create();
                    }
                    upload.SaveAs(path);
                }
                student.FileLocation = _FileName;
                _db.Students.Add(student);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View();
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                id = User.Identity.GetUserId();
            }
            Student student = await _db.Students.FindAsync(id);
            if (student != null)
            {
                var model = new StudentVm
                {
                    StudentId = student.Id,
                    AccountType = student.AccountType,
                    CountryOfBirth = student.CountryOfBirth,
                    DateOfBirth = student.DateOfBirth,
                    Discpline = student.Discpline,
                    Passport = student.Passport,
                    Email = student.Email,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    MiddleName = student.MiddleName,
                    TownOfBirth = student.TownOfBirth,
                    Institution = student.Institution,
                    PhoneNumber = student.PhoneNumber,
                    MatricNo = student.MatricNo,
                    CallUpNo = student.CallUpNo

                };
                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StudentVm student)
        {
            if (ModelState.IsValid)
            {
                var model = await _db.Students.FindAsync(student.StudentId);

                if (model != null)
                {
                    model.Id = student.StudentId;
                    model.AccountType = student.AccountType;
                    model.CountryOfBirth = student.CountryOfBirth;
                    model.DateOfBirth = student.DateOfBirth;
                    model.Discpline = student.Discpline;
                    model.Passport = student.Passport;
                    model.Email = student.Email;
                    model.FirstName = student.FirstName;
                    model.LastName = student.LastName;
                    model.MiddleName = student.MiddleName;
                    model.Institution = student.Institution;
                    model.EnrollmentDate = DateTime.Now;
                    model.PhoneNumber = student.PhoneNumber;
                    model.StateOfService = student.StateOfService.ToString();
                    model.StateOfOrigin = student.StateOfOrigin.ToString();
                    model.Batch = student.Batch.ToString();
                    model.Title = student.Title.ToString();
                    model.TownOfBirth = student.TownOfBirth;
                    model.Gender = student.Gender.ToString();
                    model.MatricNo = student.MatricNo;
                    model.CallUpNo = student.CallUpNo;

                    _db.Entry(model).State = EntityState.Modified;
                    await _db.SaveChangesAsync();

                    return RedirectToAction("Details");
                }
            }
            return View(student);

        }

        [AllowAnonymous]
        public async Task<ActionResult> RenderImage(string studentId)
        {
            Student student = await _db.Students.FindAsync(studentId);

            byte[] photoBack = student.Passport;

            return File(photoBack, "image/png");
        }


        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _db.Students.FindAsync(id);
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
            Student student = await _db.Students.FindAsync(id);
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class ProfessionalPayment
    {
        public int ProfessionalPaymentId { get; set; }
        public string UserId { get; set; }
        public string ProfessionalWorkerId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public decimal Amount { get; set; }
        public bool IsPayed { get; set; }
        public object AmountPaid { get; set; }
        public string Email { get; set; }
        public string CoursePayedFor { get; set; }
        public string PaymentDate { get; set; }
        public string PayStackCustomerId { get; set; }
    }

    public class SelectableEnumItem
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
