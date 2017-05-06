using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// Per second call charge calculation stratergy
    /// </summary>
    public class PerSecondCallChargeCalculationStratergy : CallChargeCalculationStratergy
    {
        protected override decimal GetCharegesForTheCall(CallCharge charge, int durationInSeconds)
        {
            return (charge.PerMinuteCharge * durationInSeconds) / 60;
        }
    }
}
