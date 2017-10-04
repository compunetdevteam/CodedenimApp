using CodeninModel.Assesment;
using CodeninModel.CBTE;
using CodeninModel.Forums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeninModel
{
    public class Student : Person
    {
        [Key]
        public string StudentId { get; set; }

        //[Index(IsUnique = true)]
        //[MaxLength(35)]
        public string MatricNo { get; set; }

        //[Index(IsUnique = true)]
        //[MaxLength(15)]
        public string CallUpNo { get; set; }

        // [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime? EnrollmentDate { get; set; }

        // public string GuardianEmail { get; set; }

        public bool Active { get; set; }
        public string Title { get; set; }
        public string AccountType { get; set; }

        public bool IsGraduated { get; set; }
        public string Institution { get; set; }
        public string StateOfService { get; set; }
        public string Batch { get; set; }
        public string Discpline { get; set; }



        public virtual ICollection<Enrollment> Enrollments { get; set; }
        //  public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<SubmitAssignment> SubmitAssignments { get; set; }
        public virtual ICollection<StudentTestLog> StudentTestLogs { get; set; }
        public virtual ICollection<StudentQuestion> StudentQuestions { get; set; }
        public virtual ICollection<StudentAssesment> StudentAssesments { get; set; }
        public virtual ICollection<StudentAssignedCourse> AssignedCourses { get; set; }
        public virtual ICollection<ForumQuestion> ForumQuestions { get; set; }
    }
}