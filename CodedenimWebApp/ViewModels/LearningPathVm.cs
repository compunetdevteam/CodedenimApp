using CodeninModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.ViewModels
{
    public class LearningPathVm
    {
        public List<AssignCourseCategory> AssignCourseCategory { get; set; }
        public List<Course> Courses { get; set; }
        public List<CourseCategory> CourseCategory { get; set; }
    }
}