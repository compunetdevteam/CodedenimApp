using CodeninModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.ViewModels
{
    public class CourseCategoryDetailVm
    {
      
        public CourseCategory CourseCategory { get; set; }
        public List<AssignCourseCategory> AssignedCourses { get; set; }
        public RemitaPaymentType RemitaPaymentType { get; set; }
        public string orderId { get; set; }
        public string responseurl { get; set; }
        public string StudentId { get; set; }
        public string payerEmail { get; set; }
        public string payerName { get; set; }
        public int CourseCategoryId { get; set; }
        public string amt { get; set; }
        public string payerPhone { get; set; }
        public string merchantId { get; set; }
        public string serviceTypeId { get; set; }
        public string paymenttype { get; set; }
        public string hash { get; set; }
    }

    public enum RemitaPaymentType
    {
        VISA = 1, MASTERCARD, Verve, PocketMoni, POS, BANK_BRANCH, BANK_INTERNET, REMITA_PAY, RRRGEN
    }
}