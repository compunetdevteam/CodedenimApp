using CodedenimWebApp.Models;
using CodedenimWebApp.Service;
using CodedenimWebApp.Services;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;

using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace CodedenimWebApp.Controllers
{
     public class CoursesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(int? SelectedCategory)
        {
            var categories = await db.CourseCategories.OrderBy(c => c.CategoryName).ToListAsync();
            ViewBag.SelectedCategory = new SelectList(categories, "CourseCategoryId", "CategoryName", SelectedCategory);
            int categoryId = SelectedCategory.GetValueOrDefault();

            //IEnumerable<Course> courses = db.Courses
            //    .Where(c => !SelectedCategory.HasValue || c.CourseCategoryId == categoryId)
            //    .OrderBy(d => d.CourseId)
            //    .Include(d => d.CourseCategory);
            //var courses = db.Courses.Include(c => c.CourseCategory);
            return View( db.Courses.AsNoTracking().Include(x => x.Modules).ToList());
        }

        public async Task<ActionResult> ListCourses()
        {

            var courses = db.Courses.ToList();
            return View(courses);
        }

        public async Task<ActionResult> _ListCoursesPartial(int? categoryId)
        {
            var corperCourses = await db.AssignCourseCategories.Include(x => x.CourseCategory).Include(x => x.Courses)
                                        //.Where(x =>x.CourseCategoryId.Equals((int)categoryId))
                                        .ToListAsync();

            if (categoryId != null)
            {
                corperCourses = corperCourses.Where(x => x.CourseCategoryId.Equals((int)categoryId)).ToList();
            }

            if (User.IsInRole(RoleName.Corper))
            {

                corperCourses = corperCourses.Where(x => x.CourseCategory.StudentType.Equals(RoleName.Corper))
                    .ToList();
            }
            if (User.IsInRole(RoleName.UnderGraduate))
            {

                corperCourses = corperCourses.Where(x => x.CourseCategory.StudentType.Equals(RoleName.UnderGraduate))
                    .ToList();

            }
            if (User.IsInRole(RoleName.OtherStudent))
            {

                corperCourses = corperCourses.Where(x => x.CourseCategory.StudentType.Equals(RoleName.OtherStudent))
                    .ToList();
            }

            return PartialView(corperCourses);
        }

        /// <summary>
        /// course Category is what comes into these method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CategoryContent(int id)
        {

            var userId = User.Identity.GetUserId();

            var categoryVm = new CategoryVm();
            if (id != null)
            {
                categoryVm.CourseCategoryId = db.CorperEnrolledCourses.FirstOrDefault(x => x.CourseCategoryId.Equals(id));

            }
            var corperEnrollment = new CorperEnrolledCourses();
            var hasEnrolled = db.CorperEnrolledCourses.Any(x => x.CourseCategoryId.Equals(id));
            if (!hasEnrolled == true)
            {


                corperEnrollment.StudentId = userId;
                corperEnrollment.CourseCategoryId = id;
                db.CorperEnrolledCourses.Add(corperEnrollment);
                db.SaveChanges();

            }

            var categoriesPaidFor = db.StudentPayments.Where(x => x.StudentId.Equals(userId))
                .Select(x => x.CourseCategoryId).FirstOrDefault();
            //foreach (var category in categoriesPaidFor)
            //{
            //   course.Add(category); 
            //}
            //categoryVm.courseContentVm = GetModules()
            categoryVm.Courses = db.AssignCourseCategories.Include(x => x.CourseCategory).Include(x => x.Courses)
                .Where(x => x.CourseCategoryId.Equals(categoriesPaidFor)).ToList();
            categoryVm.CourseCategory = db.CourseCategories.Where(x => x.CourseCategoryId.Equals(categoriesPaidFor))
                .Select(x => x.CategoryName).FirstOrDefault();

         


            return View(categoryVm);
        }

        /// <summary>
        /// course id is what come into these  method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Content(int? id)
        {
            if (id != null)
            {
                //this method is used to get and the modules based on the course
                CourseContentVm viewModel = GetModules(id);

                RedirectToAction("CalculatePercentage", "Students", new
                {
                    courseId = id,

                });
                //viewModel.Modules =  db.Modules.Include(x => x.Topics).Include(x => x.Course).Where(x => x.CourseId.Equals((int)id)).ToListAsync();


                //viewModel.Topics =  db.Topics.Include(x => x.MaterialUploads).ToListAsync();
                //var courseList = new CourseContentVm();
                //courseList.Modules = courses
                return View(viewModel);
            }
            return View();
        }


        //the method take in the course id and get the total number of module per couses
        private CourseContentVm GetModules(int? id)
        {
            var userId = User.Identity.GetUserId();
            var viewModel = new CourseContentVm();
            var myVar = new List<ModulesVm>();

            var modules = db.Modules.Include(x => x.Topics).Include(x => x.Course)
                .Where(x => x.CourseId.Equals((int)id)).ToList();

            foreach (var item in modules)
            {
                var modulesVm = new ModulesVm();
                modulesVm.ModuleId = item.ModuleId;
                modulesVm.ModuleName = item.ModuleName;
                modulesVm.ExpectedTime = item.ExpectedTime;
                modulesVm.ModuleDescription = item.ModuleDescription;
                modulesVm.IsModuleTaken = db.StudentTopicQuizs.Where(x => x.ModuleId.Equals(item.ModuleId) && x.StudentId.Equals(userId)).Any();
                myVar.Add(modulesVm);
            }
            viewModel.Module = myVar;
            viewModel.Topics = db.Topics.Include(x => x.MaterialUploads).ToList();
            viewModel.CourseId = (int)id;
            return viewModel;
        }

        public async Task<ActionResult> GetIndex()
        {
            #region Server Side filtering

            //Get parameter for sorting from grid table
            // get Start (paging start index) and length (page size for paging)
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns values when we click on Header Name of column
            //getting column name
            var sortColumn = Request.Form
                .GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]")
                .FirstOrDefault();
            //Soring direction(either desending or ascending)
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            //var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            var v = db.Courses.Select(s => new
            {
             
                s.CourseCode,
                s.CourseName,
                s.CourseDescription,
                s.ExpectedTime,
                s.DateAdded,
                s.Points,
               
            }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = db.Courses.Where(x => (x.CourseCode.Equals(search) || x.CourseName.Equals(search) ||
                                           x.CourseDescription.Equals(search) || x.ExpectedTime.Equals(search) ||
                                           x.DateAdded.Equals(search) || x.Points.Equals(search)))
                    .Select(s => new
                    {
                      
                        s.CourseCode,
                        s.CourseName,
                        s.CourseDescription,
                        s.ExpectedTime,
                        s.DateAdded,
                        s.Points
                     
                    }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data },
                JsonRequestBehavior.AllowGet);

            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }


        public async Task<PartialViewResult> CreateCoursePartial()
        {
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName");
            return PartialView();
        }

        [HttpPost]
        public async Task<PartialViewResult> CreateCoursePartial(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();
                // return RedirectToAction("Index");
            }


            return PartialView();
        }


        /// <summary>
        /// Partial Views For Creating Modules
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult CreatePartial(int? id)
        {
            if (id != null)
            {
                ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.CourseId.Equals((int)id)).ToList(), "CourseId", "CourseName");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            //  ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.CourseId.Equals(id.Value)).ToList(), "CourseId", "CourseCode");


            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> CreatePartial([Bind(Include = "ModuleId,CourseId,ModuleName,ModuleDescription,ExpectedTime")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                await db.SaveChangesAsync();
                // Redirect();
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", module.CourseId);
            return PartialView(module);
        }

        // GET: Courses/Details/5

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseInfo = new CourseContentVm();
            var course = await db.Courses.FindAsync(id);
            var modeules = await db.Modules.Where(x => x.CourseId.Equals((int)id)).ToListAsync();
            courseInfo.CoursesAD = course;
            courseInfo.Modules = modeules;


            if (course == null)
            {
                return HttpNotFound();
            }
            return View(courseInfo);
        }



        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include =
                "CourseId,CourseCategoryId,CourseCode,CourseName,CourseDescription,ExpectedTime,DateAdded,Points")]
            Course course, HttpPostedFileBase File, HttpPostedFileBase VideoFile)
        {
            if (ModelState.IsValid)
            {

                string _FileName = String.Empty;
                string _VideoFileName = String.Empty;
                if (File.ContentLength > 0)
                {
                    _FileName = Path.GetFileName(File.FileName);
                    _VideoFileName = Path.GetFileName(VideoFile.FileName);
                    string path = HostingEnvironment.MapPath("~/MaterialUpload/") + _FileName;
                    string Videopath = HostingEnvironment.MapPath("~/MaterialUpload/") + _VideoFileName;
                    course.FileLocation = path;
                    course.VideoLocation = _VideoFileName;
                    var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/MaterialUpload/"));
                    if (directory.Exists == false)
                    {
                        directory.Create();
                    }
                    File.SaveAs(path);
                    File.SaveAs(Videopath);
                }
                course.DateAdded = DateTime.Now;
                course.FileLocation = _FileName;


                db.Courses.Add(course);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId",
                "CategoryName" /*, course.CourseCategoryId*/);
            return View();
        }


        /// <summary>
        /// this method is to create the courses partial from the coursecategory
        /// index by passing a couses id
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult CreateCoursePartial1(int id)
        {
            var courseCategory = db.CourseCategories.FirstOrDefault(x => x.CourseCategoryId.Equals(id));
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName");
            return PartialView(courseCategory);
        }
        [HttpPost]
        public PartialViewResult CreateCoursePartial1(Course course, HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {

                string _FileName = String.Empty;
                if (File.ContentLength > 0)
                {
                    _FileName = Path.GetFileName(File.FileName);
                    string path = HostingEnvironment.MapPath("~/MaterialUpload/") + _FileName;
                    course.FileLocation = path;
                    var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/MaterialUpload/"));
                    if (directory.Exists == false)
                    {
                        directory.Create();
                    }
                    File.SaveAs(path);
                }
                course.FileLocation = _FileName;


                db.Courses.Add(course);
                 db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName");
            return PartialView();
        }



        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", course.CourseCategoryId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include =
                "CourseId,CourseCategoryId,CourseCode,CourseName,CourseDescription,ExpectedTime,DateAdded,Points")]
            Course course, HttpPostedFileBase File, HttpPostedFileBase video)
        {
            if (ModelState.IsValid)
            {
                //var imageFromDB = db.Courses.Where(x => x.CourseId.Equals(course.CourseId)).Select(x => x.FileLocation).ToString();
             
               
                
                var fp = new UploadedFileProcessor();
                if (File != null)
                {
                    DeletePhoto(course);
                    var path = fp.ProcessFilePath(File);
                    course.FileLocation = path.Path;
                }
                if (video != null)
                {
                    DeleteVideo(course);
                    var videoPath = fp.ProcessFilePath(video);


                    course.VideoLocation = videoPath.Path;
                }
               
               

                db.Entry(course).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", course.CourseCategoryId);
            return View(course);
        }


        /// <summary>
        /// this method delete the photo from the file location on the server
        /// </summary>
        /// <param name="course"></param>
        private void DeletePhoto(Course course)
        {
            if (course != null)
            {
                var photoName = "";
                photoName = course.FileLocation;
                string fullPath = Request.MapPath("~/MaterialUpload/" + photoName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    //Session["DeleteSuccess"] = "Yes";
                }
            }
           
        }

        /// <summary>
        /// this method delete the video from the file location on the server
        /// </summary>
        /// <param name="course"></param>
        private void DeleteVideo(Course course)
        {

            if (course != null)
            {
                var videoName = "";
                videoName = course.VideoLocation;
                string fullPath = Request.MapPath("~/MaterialUpload/" + videoName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    //Session["DeleteSuccess"] = "Yes";
                }
            }
        }



        /// <summary>
        /// Partial View for editing courses on the course index page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", course.CourseCategoryId);
            return PartialView(course);
        }



        /// <summary>
        /// Post Version of the Partial Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPartial([Bind(Include ="CourseId,CourseCategoryId,CourseCode,CourseName,CourseDescription,ExpectedTime,DateAdded,Points")]
            Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", course.CourseCategoryId);
            return PartialView(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Course course = await db.Courses.FindAsync(id);
            db.Courses.Remove(course);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete Partial for the partial view of course
        /// </summary>
        /// <param name="id"></param>
        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return PartialView(course);
        }
        /// <summary>
        /// Confirm delete Partial 
        /// </summary>
        /// <param name="id"></param>
        // POST: Courses/Delete/5
        [HttpPost, ActionName("DeletePartial")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedPartial(int id)
        {
            Course course = await db.Courses.FindAsync(id);
            db.Courses.Remove(course);
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

        public async Task<ActionResult> MyCoursesAsync()
        {
            var userId = User.Identity.GetUserId();
            var assinedCourseCategory = new List<AssignCourseCategory>();
            //var mycourses = await db.StudentPayments.Where(x => x.StudentId.Equals(userId)).ToArrayAsync();
            var mycourses = await db.StudentPayments.Distinct()
                                    .AsQueryable()
                                    .Where(x => x.StudentId.Equals(userId)&& x.IsPayed.Equals(true))
                                    .Select(x => x.CourseCategoryId)
                                    .ToListAsync();

            //list of all courses on the sidebar
            // ViewBag.ListCourses = db.Courses.ToList();
            foreach (var ListCourse in db.Courses.ToList())
            {
                ViewBag.ListCourses = ListCourse;
            }
            foreach (var course in mycourses)
            {

                var assignedCourses = await db.AssignCourseCategories
                                              .Include(i => i.CourseCategory)
                                              .Include(i => i.Courses)
                                              .AsNoTracking()
                                              .Where(x => x.CourseCategoryId.Equals(course))
                                              .Distinct()
                                              .AsQueryable()
                                              .FirstOrDefaultAsync();
                //var assignedCourses = await  db.AssignCourseCategories.Where(x => x.CourseCategoryId.Equals(course)).DistinctBy(s => s.CourseCategoryId).AsQueryable().ToListAsync();
                assinedCourseCategory.Add(assignedCourses);
            }
         
            return View(assinedCourseCategory);
        }
        public async Task<ActionResult> MyCourses()
        {
            var userId = User.Identity.GetUserId();
            var mycourses = await db.CorperEnrolledCourses.Where(x => x.StudentId.Equals(userId)).Select(x => x.CourseCategoryId).ToListAsync();

            return View(mycourses);
        }


        [HttpPost]
        public async Task<ActionResult> ExcelUpload(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View();
            }
            HttpPostedFileBase file = Request.Files["excelfile"];
            if(excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string lastrecord = "";
                int recordCount = 0;
                string message = "";
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
                        int requiredField = 7;

                        string validCheck = myExcel.ValidateExcel(noOfRow, sheet, requiredField);
                        if(!validCheck.Equals("Success"))
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
                            string CourseCode = sheet.Cells[row, 1].Value.ToString().ToUpper().Trim();
                            string CourseName = sheet.Cells[row, 2].Value.ToString().ToUpper().Trim();
                            string CourseDescription = sheet.Cells[row, 3].Value.ToString().Trim().ToUpper();
                            int ExpectedTime = Int32.Parse(sheet.Cells[row, 4].Value.ToString().Trim().ToUpper());
                            var DateAdded = DateTime.Parse(sheet.Cells[row, 5].Value.ToString().Trim());
                            int Points = Int32.Parse(sheet.Cells[row, 6].Value.ToString().ToUpper().Trim());
                            string FileName = sheet.Cells[row, 7].Value.ToString().Trim();

                            //var subjectName = db.Subjects.Where(x => x.SubjectCode.Equals(subjectValue))
                            //    .Select(c => c.SubjectId).FirstOrDefault();

                            var courses = db.Courses.Where(x => x.CourseCode.Equals(CourseCode));
                            var countFromDb = await courses.CountAsync();
                            if (countFromDb >= 1)
                            {
                                return View("Error2");
                            }
                            var course = new Course
                            {
                                CourseCode = CourseCode,
                                CourseName = CourseName,
                                CourseDescription = CourseDescription,
                                ExpectedTime = ExpectedTime,
                                DateAdded = DateAdded,
                                Points = Points,

                                FileLocation = FileName,

                            };
                            db.Courses.Add(course);

                            recordCount++;
                            //lastrecord = $"The last Updated record has the Student Id {studentId} and Subject Name is {subjectName}. Please Confirm!!!";
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
            return View("Index");
        }
    }   

    //public class MyCourses
    //{
    //    public List<int> Courses { get; set; }
    //}
}
