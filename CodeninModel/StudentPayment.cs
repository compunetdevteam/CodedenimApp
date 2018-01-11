using System;

namespace CodeninModel
{
    public class StudentPayment
    {
        public int StudentPaymentId { get; set; }
        public string OrderId { get; set; }
        public int CourseCategoryId { get; set; }
        public string StudentId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public decimal Amount { get; set; }
        public bool IsPayed { get; set; }
        public object AmountPaid { get; set; }
        public string ReferenceNo { get; set; }

        public string PaymentStatus { get; set; }
        public virtual Student Student { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }


    }
}
