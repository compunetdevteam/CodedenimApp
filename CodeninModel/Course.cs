using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using CodeninModel.Assesment;

namespace CodeninModel
{
    public class Course
    {
        public int CourseId { get; set; }
        public int CourseCategoryId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int ExpectedTime { get; set; }

        [NotMapped]
        public HttpPostedFileBase upload { get; set; }
        public DateTime? DateAdded { get; set; }
        [Range(0, 5)]
        public int Points { get; set; }
        public byte[] CourseImage { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Tutor> Instructors { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<AssesmentQuestionAnswer> AssesmentQuestionAnswers { get; set; }
    }
}   