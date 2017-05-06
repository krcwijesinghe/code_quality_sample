using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// The interface for discount calcluation stratergy classes of monthly mobile bills
    /// </summary>
    public interface IDiscountCalculatingStratergy
    {
        decimal GetDiscountAmount(Bill bill);
    }
}
