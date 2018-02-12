using System.ComponentModel.DataAnnotations;

namespace CodedenimWebApp.ViewModels
{
    public class RemitaPostVm
    {
        public string payerName { get; set; }

        [Display(Name = "Payer Email")]
        public string payerEmail { get; set; }

        [Display(Name = "Mobile No")]
        public string payerPhone { get; set; }
        public string orderId { get; set; }
        public string merchantId { get; set; }
        public string serviceTypeId { get; set; }
        public string responseurl { get; set; }

        [Display(Name ="Amount")]
        public string amt { get; set; }
        public string hash { get; set; }

        [Display(Name = "Payment Type")]
        public string paymenttype { get; set; }

    }

    public class ConfirmRrr
    {
        public string rrr { get; set; }

        public ServiceType ServiceType { get; set; }
    }

    public enum ServiceType
    {

    }


}