using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// Monthly mobile bill issued for a customer
    /// </summary>
    public class Bill
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string BillingAddress { get; set; }
        public decimal TotalCallChareges { get; set; }
        public decimal Discounts { get; set; }
        public decimal Tax { get; set; }
        public decimal Rental { get; set; }
        public decimal BillAmount { get; set; }
        public IList<CallDetails> CallList { get; set; } = new List<CallDetails>();
    }

    /// <summary>
    /// Call details as required to be included in monthly bill
    /// </summary>
    public class CallDetails
    {
        public DateTime StartTime { get; set; }
        public int DurationInSeconds { get; set; }
        public string DestinationNumber { get; set; }
        public decimal Charge { get; set; }
    }
}
