using System;
using System.Collections.Generic;
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
        public DateTime? DateAdded { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<AssesmentQuestionAnswer> AssesmentQuestionAnswers { get; set; }
    }
}