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
        public string TutorName { get; set; }
        public string Course { get; set; }
        public virtual  Tutor Tutor { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }

}
