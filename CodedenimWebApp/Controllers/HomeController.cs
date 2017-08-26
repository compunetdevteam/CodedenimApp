using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;

namespace CodedenimWebApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string id )
        {
            var viewModel = new TutorsIndexVm();

            viewModel.Tutors = db.Tutors
                .Include(i => i.Courses.Select(c => c.CourseCategory))
                .OrderBy(i => i.LastName);

            if (!String.IsNullOrEmpty(id))
            {
                ViewBag.TutorId = id;
                viewModel.Courses = viewModel.Tutors.Single(i => i.TutorId == id).Courses;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}