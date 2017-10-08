using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    public class AssignCourseCategory
    {

        public int AssignCourseCategoryId { get; set; }
        public int CourseId { get; set; }
        public int CourseCategoryId { get; set; }
        public virtual Course Courses { get; set; } 
        public virtual CourseCategory CourseCategory { get; set; }



    }
}
