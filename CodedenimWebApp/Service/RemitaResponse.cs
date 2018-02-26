using System;
using System.Runtime.Serialization;

namespace CodedenimWebApp.Service
{

    public class RemitaResponse
    {
        [DataMember(Name = "orderId")]
        public string OrderId { get; set; }

        [DataMember(Name = "RRR")]
        public string Rrr { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }




        [DataMember(Name = "Name")]
        public string PayerName { get; set; }

        public DateTime PaymentDate { get; set; }
        public string CourseCategory  { get; set; }
    }

}