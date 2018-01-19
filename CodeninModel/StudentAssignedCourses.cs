using GenericDataRepository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    public class StudentAssignedCourse : Entity<int>
    {
        //public int StudentAssignedCourseId { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public virtual Student Student { get; set; }
        public virtual Course Courses { get; set; }
    }
}
