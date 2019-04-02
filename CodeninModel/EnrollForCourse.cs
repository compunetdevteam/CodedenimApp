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
        public int CourseId { get; set; }
        public string StudentId { get; set; }
        public DateTime DateStarted { get; set; }
        public bool hasCompleted { get; set; }
        public Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}
