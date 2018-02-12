using CodeninModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Email")]
        public string payerEmail { get; set; }

        [Display(Name = "Name")]
        public string payerName { get; set; }
        public int CourseCategoryId { get; set; }
        [Display(Name ="Amount")]
        public string amt { get; set; }

        [Display(Name = "Mobile No")]
        public string payerPhone { get; set; }
        public string merchantId { get; set; }
        public string serviceTypeId { get; set; }

        [Display(Name = "Payment Type")]
        public string paymenttype { get; set; }
        public string hash { get; set; }
    }

    public enum RemitaPaymentType
    {
        VISA = 1, MASTERCARD, Verve, PocketMoni, POS, BANK_BRANCH, BANK_INTERNET, REMITA_PAY, RRRGEN
    }
}