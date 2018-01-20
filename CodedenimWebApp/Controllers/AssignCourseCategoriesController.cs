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
using CodeninModel;
using CodedenimWebApp.ViewModels;

namespace CodedenimWebApp.Controllers
{
    public class AssignCourseCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AssignCourseCategories
        public async Task<ActionResult> Index()
        {
            var assignCourseCategories = db.AssignCourseCategories.Include(a => a.CourseCategory).Include(a => a.Courses);
            return View(await assignCourseCategories.ToListAsync());
        }

        // GET: AssignCourseCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignCourseCategory assignCourseCategory = await db.AssignCourseCategories.FindAsync(id);
            if (assignCourseCategory == null)
            {
                return HttpNotFound();
            }
            return View(assignCourseCategory);
        }

        // GET: AssignCourseCategories/Create
        public ActionResult Create()
        {
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName");
            ViewBag.CourseId = new MultiSelectList(db.Courses.ToList(), "CourseId", "CourseName");
            return View();
        }

        // POST: AssignCourseCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignCourseToCategory assignCourseCategory)
        {
           
            if (ModelState.IsValid)
            {
                foreach (var item in assignCourseCategory.CourseId)
                {
                    var assignedCourses = new AssignCourseCategory();
                    assignedCourses.CourseCategoryId = assignCourseCategory.CourseCategoryId;
                    assignedCourses.CourseId = item;
                    db.AssignCourseCategories.Add(assignedCourses);
                }
                //assignedCourses.CourseCategoryId = assignCourseCategory.CourseCategoryId;
                //assignedCourses.CourseId = assignCourseCategory.CourseId;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", assignCourseCategory.CourseCategoryId);
            ViewBag.CourseId = new MultiSelectList(db.Courses.ToList(), "CourseId", "CourseCode");
            return View();
        }

        // GET: AssignCourseCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignCourseCategory assignCourseCategory = await db.AssignCourseCategories.FindAsync(id);
            if (assignCourseCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", assignCourseCategory.CourseCategoryId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", assignCourseCategory.CourseId);
            return View(assignCourseCategory);
        }

        // POST: AssignCourseCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AssignCourseCategoryId,CourseId,CourseCategoryId")] AssignCourseCategory assignCourseCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignCourseCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "CourseCategoryId", "CategoryName", assignCourseCategory.CourseCategoryId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", assignCourseCategory.CourseId);
            return View(assignCourseCategory);
        }

        // GET: AssignCourseCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignCourseCategory assignCourseCategory = await db.AssignCourseCategories.FindAsync(id);
            if (assignCourseCategory == null)
            {
                return HttpNotFound();
            }
            return View(assignCourseCategory);
        }

        // POST: AssignCourseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignCourseCategory assignCourseCategory = await db.AssignCourseCategories.FindAsync(id);
            db.AssignCourseCategories.Remove(assignCourseCategory);
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
