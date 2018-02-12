using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodedenimWebApp.ViewModels
{
    public class AdminDashboard
    {
        public List<StudentPaymentVm> PaymentDetails { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public int TotalCourses { get; set; }
        public int TotalTopics { get; set; }
        public int TotalStudents { get; set; }
        public int TotalTutors { get; set; }

    }

    public class StudentPaymentVm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PaymentStatus { get; set; }
        public string ReferenceNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public string PaymentDate { get; set; }
    }
}