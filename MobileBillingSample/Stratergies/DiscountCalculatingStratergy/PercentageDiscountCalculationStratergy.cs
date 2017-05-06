using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// Discount calculation stratergy that gives a percentage discount for bills that exceed the given total call charge limit
    /// </summary>
    public class PercentageDiscountCalculationStratergy : IDiscountCalculatingStratergy
    {
        public decimal TotalCallChargeLimit { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal GetDiscountAmount(Bill bill)
        {
            //check if the total call charge limit is exceeded
            if (TotalCallChargeLimit <= bill.TotalCallChareges)
                return bill.TotalCallChareges * DiscountPercentage / 100; //if so give a percentage discount
            else
                return 0; //otherwise give zero discount
        }
    }
}
