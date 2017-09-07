using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.Models;

namespace CodedenimWebApp.Controllers
{
	public class HomeController : Controller
	{
	    private ApplicationDbContext _db;

	    public HomeController()
	    {
            _db = new ApplicationDbContext();
        }
        //CodeDenim/
        public ActionResult Index()
		{
			return View();
		}
		//CodeDenim/TutorDashboard
		public ActionResult TutorDashboard()
		{
			return View();
		}
		
		//CodeDenim/AdminDashboard
		public ActionResult AdminDashboard()
		{
		    ViewBag.TotalCourses = _db.Courses.Count();
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