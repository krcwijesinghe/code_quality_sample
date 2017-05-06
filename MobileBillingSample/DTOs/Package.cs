using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// This class contains the settings and stratergies for Mobile billing packages
    /// </summary>
    public class Package
    {
        public decimal Rental { get; set; }
        public IList<CallCharge> CallCharges { get; set; }
        public IList<BillingPeriod> BillingPeriods { get; set; }
        public IList<FreeCallDuration> FreeCallDurations { get; set; } = new List<FreeCallDuration>();
        public CallChargeCalculationStratergy CallChargeCalcluationStratergy { get; set; }
        public IDiscountCalculatingStratergy DiscountCalculationStratergy { get; set; } = new NoDiscountDiscountCalculatingStratergy();
    }

    public enum CallType
    {
        Local,
        LongDistance
    }

    public enum BillingPeriodType
    {
        Peak,
        OffPeak
    }

    public class CallCharge
    {
        public BillingPeriodType PeriodType { get; set; }
        public CallType CallType { get; set; }
        public decimal PerMinuteCharge { get; set; }
    }

    public class FreeCallDuration
    {
        public BillingPeriodType PeriodType { get; set; }
        public CallType CallType { get; set; }
        public int DurationInSeconds { get; set; }
    }

    public class BillingPeriod
    {
        public BillingPeriodType Type { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
