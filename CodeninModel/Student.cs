﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CodeninModel.Assesment;
using CodeninModel.CBTE;
using CodeninModel.Forums;

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

        // public string GuardianEmail { get; set; }

        public bool Active { get; set; }
        public Title Title { get; set; }

        public bool IsGraduated { get; set; }
        public string Institution { get; set; }
        public State StateOfService { get; set; }
        public Batch Batch { get; set; }
        public string Discpline { get; set; }

        public string FullName
        {
            get
            {
                var name = FirstName + " " + LastName;
                return FullName = name;
            }
            set { }
        }

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