using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
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
        public async Task<ActionResult> Create([Bind(Include = "CourseId,CourseCategoryId,CourseCode,CourseName,CourseDescription,ExpectedTime,DateAdded,Points")] Course course, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
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
