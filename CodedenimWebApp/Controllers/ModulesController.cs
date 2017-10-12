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
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modules
        public async Task<ActionResult> Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(await modules.ToListAsync());
        }

        public async Task<ActionResult> MyCourses(string id)
        {
            var tutorCourses = await db.TutorCourses.Where(x => x.TutorId.Equals(id)).ToListAsync();
            return View(tutorCourses);
        }
        // GET: Modules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        //GET: Modules/Create
        public ActionResult Create(int? id)
        {
            if (id != null)
            {
                ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.CourseId.Equals(id.Value)).ToList(), "CourseId", "CourseName");
                // ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode");

            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            //  ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.CourseId.Equals(id.Value)).ToList(), "CourseId", "CourseCode");


            return View();
        }

        //POST: Modules/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ModuleId,CourseId,ModuleName,ModuleDescription,ExpectedTime")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", module.CourseId);
            return View(module);
        }


        public PartialViewResult CreatePartial(int? id)
        {
            if (id != null)
            {
                ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.CourseId.Equals(id.Value)).ToList(), "CourseId", "CourseName");
                /// ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode");

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

        public ActionResult Redirect()
        {
            return RedirectToAction("TutorDashBoard","Tutors");
        }
        // GET: Modules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", module.CourseId);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ModuleId,CourseId,ModuleName,ModuleDescription,ExpectedTime")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", module.CourseId);
            return View(module);
        }

        // GET: Modules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Module module = await db.Modules.FindAsync(id);
            db.Modules.Remove(module);
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
