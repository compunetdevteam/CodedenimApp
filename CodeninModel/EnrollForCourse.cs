using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    public class EnrollForCourse
    {
        public int EnrollForCourseId { get; set; }
        public int CourseCategoryId { get; set; }
        public string StudentId { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }
        public virtual Student Student { get; set; }
    }
}
