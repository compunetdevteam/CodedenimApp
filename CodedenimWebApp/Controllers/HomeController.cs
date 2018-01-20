using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;

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
  //      //CodeDenim/TutorDashboard
	 //   [Authorize(Roles = "Admin")]
	 //   [Authorize(Roles = "Tutor")]
  //      public ActionResult TutorDashboard()
		//{
		//	return View();
		//}

        //CodeDenim/AdminDashboard
	    [Authorize(Roles = "Admin")]
        public ActionResult AdminDashboard()
		{
            //get rating for specific courses
            ViewBag.Likes = _db.CourseRatings.Count(x => x.Rating == 1);
            ViewBag.Dislikes = _db.CourseRatings.Count(x => x.Dislike == 1);
            ViewBag.TotalCourses = _db.Courses.Count();
		    ViewBag.TotalTopics = _db.Topics.Count();
		    ViewBag.TotalStudents = _db.Students.Count();
		    ViewBag.TotalTutors = _db.Tutors.Count();
          //  ViewBag.AdminProfile = _db.
            return View();
		}


	    public ActionResult DisplayStudentType()
	    {
	       
	        var corper = _db.Students.Where(x => x.AccountType.Equals(RoleName.Corper)).ToList();
            var undergraduate = _db.Students.Where(x => x.AccountType.Equals(RoleName.UnderGraduate)).ToList();
            var otherStudent = _db.Students.Where(x => x.AccountType.Equals(RoleName.OtherStudent)).ToList();
	        var studenType = new StudentTypeVm
	        {
	            Corpers = corper,
	            Undergraduate = undergraduate,
                OtherStudent = otherStudent
	        };
	        
            return View(studenType);
	    }

	    public ActionResult ListCorper()
	    {
	        var corper = _db.Students.Where(x => x.AccountType.Equals(RoleName.Corper)).ToList();
	        var studenType = new StudentTypeVm
	        {
	            Corpers = corper,	           
	        };

	        return View(studenType);
	    }

	    public ActionResult ListUnderGrad()
	    {
            
	        var undergraduate = _db.Students.Where(x => x.AccountType.Equals(RoleName.UnderGraduate)).ToList();
	        var studenType = new StudentTypeVm
	        {
	            Undergraduate = undergraduate
	        };
	        return View(studenType);
	    }

	    public ActionResult OtherStudent()
	    {

	  
	        var otherStudent = _db.Students.Where(x => x.AccountType.Equals(RoleName.Student)).ToList();
	        var studenType = new StudentTypeVm
	        {
	            OtherStudent = otherStudent
	        };

	        return View(studenType);
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