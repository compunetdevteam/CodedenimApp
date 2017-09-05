using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodedenimWebApp.Controllers
{
	public class HomeController : Controller
	{
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