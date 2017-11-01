using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;

namespace CodedenimWebApp.ViewModels
{
    public class MyCoursesVm
    {
        public List<StudentPayment> StudentCourses { get; set; }
        public List<CorperEnrolledCourses> CorperCourses { get; set; } 
    }
}