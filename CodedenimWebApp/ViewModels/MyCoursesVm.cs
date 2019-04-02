using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodedenimWebApp.Controllers.Api.ApiViewModel;
using CodeninModel;

namespace CodedenimWebApp.ViewModels
{
    public class MyCoursesVm
    {
        public List<MyCourseCategoryVm> StudentCourses { get; set; }
       // public List<CorperEnrolledCourses> CorperCourses { get; set; }
    }

    public class MyCourseCategoryVm
    {
        public int CourseCategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public string StudentType { get; set; }
        public string ImageLocation { get; set; }
        public string CategoryDescription { get; set; }
        public IEnumerable<CoursesVm> Courses { get; set; }
    }
}