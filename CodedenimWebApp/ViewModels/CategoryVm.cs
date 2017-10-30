using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;

namespace CodedenimWebApp.ViewModels
{
    public class CategoryVm
    {
       public List<AssignCourseCategory> Courses { get; set; }
       public string CourseCategory { get; set; } 
        public CorperEnrolledCourses CourseCategoryId { get; set; }
    }
}