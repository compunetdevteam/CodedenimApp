using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;
using CodeninModel.Forums;

namespace CodedenimWebApp.ViewModels
{
    public class DashboardVm
    {
        public List<AssignCourseCategory> AssignCourseCategories { get; set; }
        public List<StudentPayment> Student { get; set; }
        public List<CourseCategory> CourseCategories { get; set; }
        public List<Student> StudentInfo { get; set; }
        public List<ForumQuestion> ForumQuestion { get; set; }
            
    }
}