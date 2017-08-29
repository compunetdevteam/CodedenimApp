using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;

using System.Web.Mvc;
using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using Microsoft.AspNet.Identity.EntityFramework;
using File = CodeninModel.File;

namespace CodedenimWebApp.Controllers
{
    public class TutorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tutors
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.Tutors.ToListAsync());
        //}



        public ActionResult Index(string id, int? courseId)
        {
            var viewModel = new TutorsIndexVm();

            viewModel.Tutors = db.Tutors
                .Include(i => i.Courses.Select(c => c.CourseCategory))
                .OrderBy(i => i.LastName);

            if (id != null)
            {
                ViewBag.TutorId = id;
                viewModel.Courses = viewModel.Tutors.Single(i => i.TutorId == id).Courses;
            }

            if (courseId != null)
            {
                ViewBag.CourseId = courseId.Value;
                // Lazy loading
                //viewModel.Enrollments = viewModel.Courses.Where(
                //    x => x.CourseID == courseID).Single().Enrollments;
                // Explicit loading
                var selectedCourse = viewModel.Courses.Where(x => x.CourseId == courseId).Single();
                db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    db.Entry(enrollment).Reference(x => x.Student).Load();
                }

                viewModel.Enrollments = selectedCourse.Enrollments;
            }

            return View(viewModel);
        }



        /// <summary>
        /// This Method Get the Details of the Current logged in Tutors Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<ActionResult> TutorDashboard()
        {
            var tutorId = User.Identity.GetUserId();
            Tutor tutor = db.Tutors.FirstOrDefault(t => t.TutorId == tutorId);
            return View();
        }

        public async Task<ActionResult> RenderImage(string TutorId)
        {
            var tutor = await db.Tutors.FindAsync(TutorId);

            byte[] photoBack = tutor.Passport;

            return File(photoBack, "image/png");
        }


        /// <summary>
        /// this method is to assign  subjects to
        /// the Various tutors.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<ActionResult> AssignCourseToTutor(string tutorId, string[] courses)
        {
            var tutorCourses = new TutorCourses
            {
               
            };
           
             
            //var tutors = new HashSet<string>(courses);
          
            return View();
        }


        // GET: Tutors/Details/5
        public async Task<ActionResult> Details(string id)
        {
           // var username = User.Identity.GetUserId();
            //var user = await Db.Users.AsNoTracking().Where(c => c.Id.Equals(username)).Select(c => c.Email).FirstOrDefaultAsync();
            //if (id == null)
            //{
            //    id = username;
            //}

            Tutor tutor = await db.Tutors.FindAsync(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }



            // Tutor tutor =   db.Tutors.Include(s => s.Files).SingleOrDefault(s => s.TutorId == id);
            // Tutor tutor = await db.Tutors.FindAsync(id);
            //if (tutor == null)
            //{
            //    return HttpNotFound();
            //}
            return View(tutor);
        }

        //GET: Tutors/Create
        public ActionResult Create()
        {
            return View();
        }

        //method to populate the assigned course in the the create view
        //private void PopulateAssignedCourseData(Tutor tutor)
        //{
        //    var allCourses = db.Courses;
        //    var instructorCourses = new HashSet<int>(tutor.Courses.Select(c => c.CourseId));
        //    var viewModel = new List<AssignedCourses>();
        //    foreach (var course in allCourses)
        //    {
        //        viewModel.Add(new AssignedCourses
        //        {
        //            CourseId = course.CourseId,
        //            CourseName = course.CourseName,
        //            Assigned = instructorCourses.Contains(course.CourseId)
        //        });
        //    }
        //    ViewBag.Courses = viewModel;
        //}


        // POST: Tutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tutor tutor)
        {

            //if (selectedCourses != null)
            //{
            //    tutor.Courses = new List<Course>();
            //    foreach (var course in selectedCourses)
            //    {
            //        var courseToAdd = db.Courses.Find(int.Parse(course));
            //        if (courseToAdd == null) throw new ArgumentNullException(nameof(courseToAdd));
            //        tutor.Courses.Add(courseToAdd);
            //    }
            //}
            if (ModelState.IsValid)
            {
                tutor = new Tutor
                {
                    TutorId = tutor.TutorId,
                    FirstName = tutor.FirstName,
                    LastName = tutor.LastName,
                    DateOfBirth = tutor.DateOfBirth,
                    Passport = tutor.Passport,

                    
                };

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

            
           var tutor = db.Tutors.AsNoTracking( )
            .FirstOrDefault(t => t.TutorId.Equals(tutorId));

            if (tutor == null)
            {
                ViewBag.Message = "This Id Does Not Exist";
                return View();
            }
            var tutorVm = new TutorRegisterVm();
            tutorVm.FirstName = tutor.FirstName ;
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
