using GenericDataRepository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    public class CourseEnrollment : Entity<int>
    {
        //public int CourseEnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public bool HasStartCourse { get; set; }
        public bool HasEndCourse { get; set; }
        public virtual Student Student { get;set; }
        public virtual Course Course { get; set; }

        
    }
}
