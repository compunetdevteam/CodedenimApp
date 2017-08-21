using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CodeninModel.Assesment;
using CodeninModel.CBTE;

namespace CodeninModel
{
    public class Student : Person
    {
        [Key]
        public string StudentId { get; set; }

        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime? EnrollmentDate { get; set; }

        public int? ProgrammeId { get; set; }

        // public string GuardianEmail { get; set; }

        public bool Active { get; set; }

        public bool IsGraduated { get; set; }

        public virtual ICollection<SubmitAssignment> SubmitAssignments { get; set; }
        public virtual ICollection<StudentTestLog> StudentTestLogs { get; set; }
        public virtual ICollection<StudentQuestion> StudentQuestions { get; set; }
        public virtual ICollection<StudentAssesment> StudentAssesments { get; set; }
    }
}