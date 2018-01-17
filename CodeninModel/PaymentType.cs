using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace CodeninModel
{
    public class PaymentType : Entity<int>
    {
        //public int PaymentTypeId { get; set; }
        

        public string PaymentName { get; set; }
        public int Amount { get; set; }
        public string PaymentTypeValue { get; set; }

      
        

    }
}
