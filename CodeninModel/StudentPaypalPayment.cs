using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    public class StudentPaypalPayment
    {
        [Key]
        public int StudentPaypalPaymentId { get; set; }

        public string PaymentStatus { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerLastName { get; set; }
        public string Amount { get; set; }
        public string TxToken { get; set; } 
        public string ReceiverEmail { get; set; }
        public string ItemName { get; set; }
        public string Currency { get; set; }    
        public string PayerEmail { get; set; }
        public string PaymentDate { get; set; }
        public int CourseCategoryId { get; set; }
        public string StudentId { get; set; }
        public string PayerId { get; set; }
        public virtual Student Student { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }

    }
}
