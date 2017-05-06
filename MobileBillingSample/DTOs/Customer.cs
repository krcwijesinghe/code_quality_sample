using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// Mobile customer details
    /// </summary>
    public class Customer
    {
        public string FullName { get; set; }
        public string BillingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PackageCode { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}
