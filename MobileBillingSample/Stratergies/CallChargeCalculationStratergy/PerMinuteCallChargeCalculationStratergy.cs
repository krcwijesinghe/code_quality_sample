using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// Per minute call charege calcluation startergy
    /// </summary>
    public class PerMinuteCallChargeCalculationStratergy : CallChargeCalculationStratergy
    {
        protected override decimal GetCharegesForTheCall(CallCharge charge, int durationInSeconds)
        {
            return charge.PerMinuteCharge * (durationInSeconds / 60 + (durationInSeconds % 60 != 0 ? 1 : 0));
        }
    }
}
