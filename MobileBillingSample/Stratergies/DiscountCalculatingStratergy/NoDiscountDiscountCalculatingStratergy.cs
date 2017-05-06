using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// The discount calcluation stratergy that does not give any discount
    /// </summary>
    public class NoDiscountDiscountCalculatingStratergy : IDiscountCalculatingStratergy
    {
        public decimal GetDiscountAmount(Bill bill)
        {
            return 0;
        }
    }
}
