using GenericDataRepository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    public class CourseCategory : Entity<int>
    {
        //public int CourseCategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public string StudentType { get;set; }
        public string CategoryDescription { get; set; }
        public string ImageLocation { get; set; }
        public virtual ICollection<StudentPayment> StudentPayments { get; set; }
        public virtual ICollection<EnrollForCourse> EnrollForCourse { get; set; }
       }
}