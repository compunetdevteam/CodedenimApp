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

    public class ConfirmTutorVm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public ICollection<Course> Courses { get; set; }



    }
}