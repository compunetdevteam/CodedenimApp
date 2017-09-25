using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;
using CodeninModel;

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

            IEnumerable<Course> courses = db.Courses
                .Where(c => !SelectedCategory.HasValue || c.CourseCategoryId == categoryId)
                .OrderBy(d => d.CourseId)
                .Include(d => d.CourseCategory);
            //var courses = db.Courses.Include(c => c.CourseCategory);
            return View(courses.ToList());
        }

        public  async Task<ActionResult> GetIndex()
        {
            #region Server Side filtering
            //Get parameter for sorting from grid table
            // get Start (paging start index) and length (page size for paging)
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns values when we click on Header Name of column
            //getting column name
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //Soring direction(either desending or ascending)
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            //var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            var v = db.Courses.Select(s => new {s.CourseCategoryId, s.CourseCode, s.CourseName, s.CourseDescription, s.ExpectedTime,s.DateAdded,s.Points, s.CourseImage }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = db.Courses.Where(x => (x.CourseCategoryId.ToString().Equals(search) || x.CourseCode.Equals(search) || x.CourseName.Equals(search) || x.CourseDescription.Equals(search) || x.ExpectedTime.Equals(search) || x.DateAdded.Equals(search) || x.Points.Equals(search)))
                    .Select(s => new {s.CourseCategoryId, s.CourseCode, s.CourseName, s.CourseDescription, s.ExpectedTime, s.DateAdded, s.Points, s.CourseImage }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
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

        // GET: Courses/Details/5

        public async Task<ActionResult> Details(int? id)
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
        public async Task<ActionResult> Create([Bind(Include = "CourseId,CourseCategoryId,CourseCode,CourseName,CourseDescription,ExpectedTime,DateAdded,Points")] Course course, HttpPostedFileBase File)
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
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", course.CourseCategoryId);
            return View(course);
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
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", course.CourseCategoryId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseId,CourseCategoryId,CourseCode,CourseName,CourseDescription,ExpectedTime,DateAdded,Points")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", course.CourseCategoryId);
            return View(course);
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
