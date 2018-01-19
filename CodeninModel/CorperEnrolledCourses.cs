using GenericDataRepository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    //this table is for the enrollment of students and
    // not only corpers
    public class CorperEnrolledCourses : Entity<int>
    {
        //public int CorperEnrolledCoursesId { get; set; }
        public int CourseId { get; set; }
        public string CorperCallUpNumber { get; set; }
        public string StudentId { get; set; }
        public int CourseCategoryId { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual Student Student { get; set; }
        public virtual CourseCategory Category { get; set; }
    }
}
