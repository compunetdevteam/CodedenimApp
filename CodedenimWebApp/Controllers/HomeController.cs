using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodedenimWebApp.Controllers
{
	public class HomeController : Controller
	{

		public ActionResult Index()
		{
			return View();
		}
		public ActionResult GeneralDashboard()
		{
			return View();
		}
		public ActionResult StudentDashboard()
		{
			return View();
		}
		public ActionResult CorpsDashboard()
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