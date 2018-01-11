﻿namespace CodedenimWebApp.ViewModels
{
    public class RemitaPostVm
    {
        public string payerName { get; set; }
        public string payerEmail { get; set; }
        public string payerPhone { get; set; }
        public string orderId { get; set; }
        public string merchantId { get; set; }
        public string serviceTypeId { get; set; }
        public string responseurl { get; set; }
        public string amt { get; set; }
        public string hash { get; set; }
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