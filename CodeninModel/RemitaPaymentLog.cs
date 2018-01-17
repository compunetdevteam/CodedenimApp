using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeninModel
{
    public class RemitaPaymentLog : Entity<int>
    {
        //public int RemitaPaymentLogId { get; set; }
        public string OrderId { get; set; }
        public string StatusCode { get; set; }
        public string TransactionMessage { get; set; }
        public string Rrr { get; set; }
        public string PaymentName { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Amount { get; set; }
        public string PayerName { get; set; }
    }
}
