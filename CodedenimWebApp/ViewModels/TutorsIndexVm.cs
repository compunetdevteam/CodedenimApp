using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;

namespace CodedenimWebApp.ViewModels
{
    public class TutorsIndexVm
    {
        public IEnumerable<Tutor> Tutors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}