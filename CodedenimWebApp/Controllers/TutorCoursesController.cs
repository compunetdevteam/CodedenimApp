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

namespace CodedenimWebApp.Controllers
{
    public class TutorCoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TutorCourses
        public async Task<ActionResult> Index()
        {
            var tutorCourses = db.TutorCourses.Include(t => t.Courses).Include(t => t.Tutor);
            return View(await tutorCourses.ToListAsync());
        }

        // GET: TutorCourses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorCourse tutorCourses = await db.TutorCourses.FindAsync(id);
            if (tutorCourses == null)
            {
                return HttpNotFound();
            }
            return View(tutorCourses);
        }

        // GET: TutorCourses/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new MultiSelectList(db.Courses, "CourseId", "CourseCode");
            ViewBag.TutorId = new SelectList(db.Tutors, "TutorId", "UserName");
            return View();
        }

        // POST: TutorCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TutorCoursesId,TutorId,CourseId")] TutorCourse tutorCourses)
        {
            if (ModelState.IsValid)
            {
                db.TutorCourses.Add(tutorCourses);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", tutorCourses.CourseId);
            ViewBag.TutorId = new SelectList(db.Tutors, "TutorId", "UserName", tutorCourses.TutorId);
            return View(tutorCourses);
        }

        // GET: TutorCourses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorCourse tutorCourses = await db.TutorCourses.FindAsync(id);
            if (tutorCourses == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", tutorCourses.CourseId);
            ViewBag.TutorId = new SelectList(db.Tutors, "TutorId", "UserName", tutorCourses.TutorId);
            return View(tutorCourses);
        }

        // POST: TutorCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TutorCoursesId,TutorId,CourseId")] TutorCourse tutorCourses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutorCourses).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", tutorCourses.CourseId);
            ViewBag.TutorId = new SelectList(db.Tutors, "TutorId", "UserName", tutorCourses.TutorId);
            return View(tutorCourses);
        }

        // GET: TutorCourses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorCourse tutorCourses = await db.TutorCourses.FindAsync(id);
            if (tutorCourses == null)
            {
                return HttpNotFound();
            }
            return View(tutorCourses);
        }

        // POST: TutorCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TutorCourse tutorCourses = await db.TutorCourses.FindAsync(id);
            db.TutorCourses.Remove(tutorCourses);
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
