using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using Microsoft.AspNet.Identity;

namespace CodedenimWebApp.Controllers
{
    public class TutorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tutors
        public async Task<ActionResult> Index()
        {
            return View(await db.Tutors.ToListAsync());
        }

        public async Task<ActionResult> TutorDashboard()
        {
            var tutor = User.Identity.GetUserId();



            var tutorCourses = db.TutorCourses.AsNoTracking().Where(t => t.TutorId.Equals(tutor))
                                                                .Select(c => c.Courses).ToList();

            return View();
        }

        // GET: Tutors/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = await db.Tutors.Include(i => i.TutorCourses).Where(x => x.TutorId.Equals(id))
                .FirstOrDefaultAsync();
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // GET: Tutors/Create
        public ActionResult Create()
        {
           // ViewBag.Gender = new SelectList(Enum.GetValues(typeof(Gender), "Gender", "Gender");
            return View();
        }

        // POST: Tutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                db.Tutors.Add(tutor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tutor);
        }

        public async Task<ActionResult> ConfirmTutor()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmTutor(string tutorId)
        {
            // bool tutorIsExist = false;
            if (tutorId == null)
            {

            }


            var tutor = db.Tutors.AsNoTracking()
                .FirstOrDefault(t => t.TutorId.Equals(tutorId));

            if (tutor == null)
            {
                ViewBag.Message = "This Id Does Not Exist";
                return View();
            }
            var tutorVm = new TutorRegisterVm();
            tutorVm.FirstName = tutor.FirstName;
            tutorVm.TutorId = tutor.TutorId;
            tutorVm.LastName = tutor.LastName;
            return View("info", tutorVm);
        }

        // GET: Tutors/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = await db.Tutors.FindAsync(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // POST: Tutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tutor);
        }

        // GET: Tutors/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = await db.Tutors.FindAsync(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // POST: Tutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Tutor tutor = await db.Tutors.FindAsync(id);
            db.Tutors.Remove(tutor);
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
