using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    public class TutorCourses
    {
        [Key]
        public int TutorCoursesId { get; set; }
        public string TutorId { get; set; }
        public int CourseId { get; set; }
        public virtual  Tutor Tutor { get; set; }
        public virtual Course Courses { get; set; }
    }

}
