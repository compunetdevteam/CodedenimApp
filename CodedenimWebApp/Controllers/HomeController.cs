using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

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
            var paymentDetails = _db.StudentPayments
                                    .Select(x => new StudentPaymentVm
                                    {
                                        FullName = x.Student.FirstName + " " + x.Student.LastName,
                                        PaymentStatus = x.PaymentStatus,
                                        PaymentDate = x.PaymentDateTime.ToString(),
                                        ReferenceNo = x.ReferenceNo
                                    }).ToList();

            var payment = new AdminDashboard
            {
                PaymentDetails = paymentDetails.Take(4).ToList(),
                Likes = _db.CourseRatings.Count(x => x.Rating == 1),
                DisLikes = _db.CourseRatings.Count(x => x.Dislike == 1),
                TotalCourses = _db.Courses.Count(),
                TotalTopics = _db.Topics.Count(),
                TotalStudents = _db.Students.Count(),
                TotalTutors = _db.Tutors.Count()
            };
          //  ViewBag.AdminProfile = _db.
            return View(payment);
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

        public ActionResult AllStudent()
        {
            return View();
        }

        public ActionResult PaymentDetails()
        {
            return View();
        }


        /// <summary>
        /// this is a datatable code to return all student in the database
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult GetData()
        {
           string draw, search;
           int pageSize, skip, totalRecord;

           //string draw = Request
           //            .Form
           //            .GetValues("draw")
           //            .FirstOrDefault();
           // var start = Request
           //             .Form
           //             .GetValues("start")
           //             .FirstOrDefault();

           // var length = Request
           //              .Form
           //              .GetValues("length")
           //              .FirstOrDefault();

           // //Get sort column value
           // var sortColumn = Request
           //                  .Form
           //                  .GetValues("columns["
           //                  + Request.Form.GetValues("order[0][column]")
           //                  .FirstOrDefault() + "][name]")
           //                  .FirstOrDefault();


           // var sortColumnDir = Request
           //                     .Form.GetValues("order[0][dir]")
           //                      .FirstOrDefault();
           // string search = Request
           //              .Form
           //              .GetValues("search[value]")
           //              .FirstOrDefault();
           //int  pageSize = length != null ? Convert.ToInt32(length) : 0;
           // int  skip = start != null ? Convert.ToInt32(start) : 0;
           //int  totalRecord = 0;

            DataTableInputs(out draw, out search, out pageSize, out skip, out totalRecord);

            //below code is to get the exact properties to return to the view
            //if not an error will be genereated
            var v = _db.Students.Select(x => new {
                x.FirstName,
                x.LastName,
                x.Email,
                x.AccountType,
                x.PhoneNumber }).ToList();

            // Verification.
            if (!string.IsNullOrEmpty(search.Trim()) && !string.IsNullOrWhiteSpace(search.Trim()))
            {
                // Apply search   
                // v = v.OrderBy(sortColumn + ""+sortColumnDir);
                v = v.Where(p => p.FirstName.ToString().ToLower().Contains(search.ToLower())
                                // ||
                                //p.LastName.ToString().ToLower().Contains(search.ToLower())
                                //||
                                //p.Email.ToString().ToLower().Contains(search.ToLower())
                                ||
                                p.AccountType.ToString().ToLower().Contains(search.ToLower())).ToList();
            }

      
            totalRecord = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(
                new
                {
                    draw = draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = data
                },
               JsonRequestBehavior.AllowGet
                );

        }

        /// <summary>
        /// Datatable code to return all the Payment Transaction in the Platform
        /// </summary>
        /// <returns>Json of all student List</returns>
        public ActionResult GetData1()
        {
               string draw, search;
                int pageSize, skip, totalRecord;

           

            DataTableInputs(out draw, out search, out pageSize, out skip, out totalRecord);

            //below code is to get the exact properties to return to the view
            //if not an error will be genereated
            var v = _db.StudentPayments.Select(x => new StudentPaymentVm  {
                PaymentDate = x.PaymentDateTime.ToString(),
                LastName = x.Student.LastName,
               FirstName =  x.Student.FirstName ,
                ReferenceNo = x.ReferenceNo,
                PaymentStatus =  x.PaymentStatus,
            
            }).ToList();

            // Verification.
            if (!string.IsNullOrEmpty(search.Trim()) && !string.IsNullOrWhiteSpace(search.Trim()))
            {
                // Apply search   
                // v = v.OrderBy(sortColumn + ""+sortColumnDir);
                v = v.Where(p => p.PaymentDate.ToString().ToLower().Contains(search.ToLower())
                                ||
                                p.FirstName.ToString().ToLower().Contains(search.ToLower())
                                ||
                                 p.LastName.ToString().ToLower().Contains(search.ToLower())
                                ||
                                p.ReferenceNo.ToString().ToLower().Contains(search.ToLower())
                                ||
                                p.PaymentStatus.ToString().ToLower().Contains(search.ToLower())).ToList();
            }


            totalRecord = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(
                new
                {
                    draw = draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = data
                },
               JsonRequestBehavior.AllowGet
                );

        }

        /// <summary>
        /// return all the Undergraduate student on the platform
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUndergradData()
        {
            string draw, search;
            int pageSize, skip, totalRecord;



            DataTableInputs(out draw, out search, out pageSize, out skip, out totalRecord);

            //below code is to get the exact properties to return to the view
            //if not an error will be genereated
            var v = _db.Students.Where(x => x.AccountType.Equals(RoleName.UnderGraduate)).Select(x => new StudentListVm
            {
               FirstName = x.FirstName,
               LastName =x.LastName,
               Email = x.Email,
               Mobile = x.PhoneNumber
           
               }).ToList();

            // Verification.
            if (!string.IsNullOrEmpty(search.Trim()) && !string.IsNullOrWhiteSpace(search.Trim()))
            {
                // Apply search   
                // v = v.OrderBy(sortColumn + ""+sortColumnDir);
                v = v.Where(p => 
                                p.FirstName.ToString().ToLower().Contains(search.ToLower())
                                ||
                                 p.LastName.ToString().ToLower().Contains(search.ToLower())
                               
                                // p.Mobile.ToString().ToLower().Contains(search.ToLower())
                                ||
                                p.Email.ToString().ToLower().Contains(search.ToLower())
                               ).ToList();
            }


            totalRecord = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(
                new
                {
                    draw = draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = data
                },
               JsonRequestBehavior.AllowGet
                );

        }

        /// <summary>
        /// return all the corpers on the platform in as datatable
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCorperData()
        {
            string draw, search;
            int pageSize, skip, totalRecord;



            DataTableInputs(out draw, out search, out pageSize, out skip, out totalRecord);

            //below code is to get the exact properties to return to the view
            //if not an error will be genereated
            var v = _db.Students.Where(x => x.AccountType.Equals(RoleName.Corper)).Select(x => new StudentListVm
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Mobile = x.PhoneNumber

            }).ToList();

            // Verification.
            if (!string.IsNullOrEmpty(search.Trim()) && !string.IsNullOrWhiteSpace(search.Trim()))
            {
                // Apply search   
                // v = v.OrderBy(sortColumn + ""+sortColumnDir);
                v = v.Where(p =>
                                p.FirstName.ToString().ToLower().Contains(search.ToLower())
                                ||
                                 p.LastName.ToString().ToLower().Contains(search.ToLower())
                                  ||
                                // p.Mobile.ToString().ToLower().Contains(search.ToLower())
                                //||
                                p.Email.ToString().ToLower().Contains(search.ToLower())
                               ).ToList();
            }


            totalRecord = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(
                new
                {
                    draw = draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = data
                },
               JsonRequestBehavior.AllowGet
                );

        }

        public ActionResult GetProfsData()
        {
            string draw, search;
            int pageSize, skip, totalRecord;



            DataTableInputs(out draw, out search, out pageSize, out skip, out totalRecord);

            //below code is to get the exact properties to return to the view
            //if not an error will be genereated
            var v = _db.Students.Where(x => x.AccountType.Equals(RoleName.RegularStudent)).Select(x => new StudentListVm
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Mobile = x.PhoneNumber

            }).ToList();

            // Verification.
            if (!string.IsNullOrEmpty(search.Trim()) && !string.IsNullOrWhiteSpace(search.Trim()))
            {
                // Apply search   
                // v = v.OrderBy(sortColumn + ""+sortColumnDir);
                v = v.Where(p =>
                                p.FirstName.ToString().ToLower().Contains(search.ToLower())
                                ||
                                 p.LastName.ToString().ToLower().Contains(search.ToLower())

                                // p.Mobile.ToString().ToLower().Contains(search.ToLower())
                                ||
                                p.Email.ToString().ToLower().Contains(search.ToLower())
                               ).ToList();
            }


            totalRecord = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(
                new
                {
                    draw = draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = data
                },
               JsonRequestBehavior.AllowGet
                );

        }

        private void DataTableInputs(out string draw, out string search, out int pageSize, out int skip, out int totalRecord)
        {
            draw = Request
                       .Form
                       .GetValues("draw")
                       .FirstOrDefault();
            var start = Request
                        .Form
                        .GetValues("start")
                        .FirstOrDefault();

            var length = Request
                         .Form
                         .GetValues("length")
                         .FirstOrDefault();

            //Get sort column value
            var sortColumn = Request
                             .Form
                             .GetValues("columns["
                             + Request.Form.GetValues("order[0][column]")
                             .FirstOrDefault() + "][name]")
                             .FirstOrDefault();


            var sortColumnDir = Request
                                .Form.GetValues("order[0][dir]")
                                 .FirstOrDefault();
            search = Request
                         .Form
                         .GetValues("search[value]")
                         .FirstOrDefault();
            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            totalRecord = 0;
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