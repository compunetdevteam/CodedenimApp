using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class MyCoursesVm
    {
        public int CourseId { get; set; }
        public int CourseCategoryId { get; set; }
        public string CourseName { get; set; }
        public string CategoryName { get; set; }
        public string CourseDescription { get; set; }
        public int ExpectedTime { get; set; }
        public string FileLocation { get; set; }
        public int ProgressCount { get; set; }
        public int StudentPaymentId { get; set; }
        public string StudentId { get; set; }
        public DateTime PaymentDate { get; set; }
       
        public IEnumerable<CoursesVm> Courses { get; set; }

    }
}