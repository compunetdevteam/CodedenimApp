using System;
using System.ComponentModel.DataAnnotations.Schema;
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
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //Soring direction(either desending or ascending)
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            //var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            var v = db.Tutors.Select(s => new { s.TutorId, s.FirstName, s.MiddleName,s.LastName,s.Email }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = db.Tutors.Where(x =>  (x.TutorId.Equals(search) || x.FirstName.Equals(search) || x.MiddleName.Equals(search) || x.LastName.Equals(search) || x.Email.Equals(search)))
                    .Select(s => new { s.TutorId, s.FirstName, s.MiddleName, s.LastName,s.Email }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }



        public async Task<PartialViewResult> CreateTutorPartial() 
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<PartialViewResult> CreateTutorPartial(Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                db.Tutors.Add(tutor);
                await db.SaveChangesAsync();
               // return RedirectToAction("Index");
            }

            //return RedirectToAction("Index");

             return PartialView();
        }
        public  ActionResult DashBoard()
        {
            var tutor = User.Identity.GetUserId();
            var tutorCourses = db.TutorCourses.AsNoTracking().Where(t => t.TutorId.Equals(tutor))
                                                                .Select(c => c.Courses).ToList();

            return View();
        }

        public ActionResult TutorDashboard()
        {

            var userId = User.Identity.GetUserId();
          
            var tutorCourses = db.TutorCourses.FirstOrDefault(x => x.TutorId.Equals(userId));
            var courses = db.TutorCourses.Where(x => x.TutorId.Equals(userId)).ToList();

            var courseId = db.TutorCourses.Where(x => x.TutorId.Equals(userId)).Select(x => x.CourseId).ToList();

            //if the tutor has not  been assigned a course it shouldnt give and error....that logic goes here
            if (tutorCourses != null)
            {
                var question = db.ForumQuestions.Where(x => x.CourseId.Equals(tutorCourses.CourseId)).ToList();

                ViewBag.ForumQuestions = question;
            }
           //var tutorInfo = new TutorDashboardVm();
            ViewBag.TutorProfile = db.Tutors.Where(x => x.TutorId.Equals(userId)).Select(x => x.ImageLocation).FirstOrDefault();
            ViewBag.TutorCourses = courses;
      
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
        
        public async Task<ActionResult> Create(TutorCreateVm model)
        {
            var sameTutorId = db.Tutors.AsNoTracking().SingleOrDefault(x => x.TutorId.Equals(model.TutorId));
            if (sameTutorId == null)
            {
                if (ModelState.IsValid)
                {
                    var tutor = new Tutor
                    {
                            
                        TutorId = model.TutorId,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                };
                   
                    db.Tutors.Add(tutor);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Create","TutorCourses");
                }
            }
            else
            {
                //wanted to send to the view the information to the existing tutor that wanted to be added again
                var tutorInfo = new TutorCreateVm();
                var tutorExist = db.Tutors.AsNoTracking().FirstOrDefault(x => x.TutorId.Equals(model.TutorId));
                tutorInfo.TutorId = tutorExist.TutorId;
                tutorInfo.LastName = tutorExist.LastName;
                tutorInfo.FirstName = tutorExist.FirstName;
               

                return View("TutorExist" ,tutorInfo);
            }


            return View();
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
                
                return View("NotExist");
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
