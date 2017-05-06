using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// A hard-coded implementation of Package repository
    /// </summary>
    public class PackageRepository : IPackageRepository
    {
        private IDictionary<string, Package> _packages = new Dictionary<string, Package>()
        {
            {
                "Package A",
                new Package()
                {
                    Rental = 100,
                    CallChargeCalcluationStratergy = new PerMinuteCallChargeCalculationStratergy(),
                    DiscountCalculationStratergy = new PercentageDiscountCalculationStratergy()
                    {
                        TotalCallChargeLimit = 1000,
                        DiscountPercentage = 40
                    },
                    BillingPeriods = new List<BillingPeriod>()
                    {
                        new BillingPeriod() { Type = BillingPeriodType.OffPeak, StartTime = TimeSpan.FromHours(0), EndTime = TimeSpan.FromHours(10) },
                        new BillingPeriod() { Type = BillingPeriodType.Peak, StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(18) },
                        new BillingPeriod() { Type = BillingPeriodType.OffPeak, StartTime = TimeSpan.FromHours(18), EndTime = TimeSpan.FromHours(24) },
                    },
                    CallCharges = new List<CallCharge>()
                    {
                        new CallCharge() { PeriodType = BillingPeriodType.OffPeak, CallType = CallType.Local, PerMinuteCharge = 2 },
                        new CallCharge() { PeriodType = BillingPeriodType.OffPeak, CallType = CallType.LongDistance, PerMinuteCharge = 4 },
                        new CallCharge() { PeriodType = BillingPeriodType.Peak, CallType = CallType.Local, PerMinuteCharge = 3 },
                        new CallCharge() { PeriodType = BillingPeriodType.Peak, CallType = CallType.LongDistance, PerMinuteCharge = 5 },
                    }
                }
            },
            {
                "Package B",
                new Package()
                {
                    Rental = 100,
                    CallChargeCalcluationStratergy = new PerSecondCallChargeCalculationStratergy(),
                    FreeCallDurations = new List<FreeCallDuration>()
                    {
                        new FreeCallDuration() { CallType = CallType.Local, PeriodType = BillingPeriodType.OffPeak, DurationInSeconds= 60 },
                    },
                    DiscountCalculationStratergy = new PercentageDiscountCalculationStratergy()
                    {
                        TotalCallChargeLimit = 1000,
                        DiscountPercentage = 40
                    },
                    BillingPeriods = new List<BillingPeriod>()
                    {
                        new BillingPeriod() { Type = BillingPeriodType.OffPeak, StartTime = TimeSpan.FromHours(0), EndTime = TimeSpan.FromHours(8) },
                        new BillingPeriod() { Type = BillingPeriodType.Peak, StartTime = TimeSpan.FromHours(8), EndTime = TimeSpan.FromHours(20) },
                        new BillingPeriod() { Type = BillingPeriodType.OffPeak, StartTime = TimeSpan.FromHours(20), EndTime = TimeSpan.FromHours(24) },
                    },
                    CallCharges = new List<CallCharge>()
                    {
                        new CallCharge() { PeriodType = BillingPeriodType.OffPeak, CallType = CallType.Local, PerMinuteCharge = 3 },
                        new CallCharge() { PeriodType = BillingPeriodType.OffPeak, CallType = CallType.LongDistance, PerMinuteCharge = 5 },
                        new CallCharge() { PeriodType = BillingPeriodType.Peak, CallType = CallType.Local, PerMinuteCharge = 4 },
                        new CallCharge() { PeriodType = BillingPeriodType.Peak, CallType = CallType.LongDistance, PerMinuteCharge = 6 },
                    }
                }
            },
            {
                "Package C",
                new Package()
                {
                    Rental = 300,
                    CallChargeCalcluationStratergy = new PerMinuteCallChargeCalculationStratergy(),
                    FreeCallDurations = new List<FreeCallDuration>()
                    {
                        new FreeCallDuration() { CallType = CallType.Local, PeriodType = BillingPeriodType.Peak, DurationInSeconds= 60 },
                        new FreeCallDuration() { CallType = CallType.Local, PeriodType = BillingPeriodType.OffPeak, DurationInSeconds= 60 },
                    },
                    BillingPeriods = new List<BillingPeriod>()
                    {
                        new BillingPeriod() { Type = BillingPeriodType.OffPeak, StartTime = TimeSpan.FromHours(0), EndTime = TimeSpan.FromHours(9) },
                        new BillingPeriod() { Type = BillingPeriodType.Peak, StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(20) },
                        new BillingPeriod() { Type = BillingPeriodType.OffPeak, StartTime = TimeSpan.FromHours(20), EndTime = TimeSpan.FromHours(24) },
                    },
                    CallCharges = new List<CallCharge>()
                    {
                        new CallCharge() { PeriodType = BillingPeriodType.OffPeak, CallType = CallType.Local, PerMinuteCharge = 1 },
                        new CallCharge() { PeriodType = BillingPeriodType.OffPeak, CallType = CallType.LongDistance, PerMinuteCharge = 2 },
                        new CallCharge() { PeriodType = BillingPeriodType.Peak, CallType = CallType.Local, PerMinuteCharge = 2 },
                        new CallCharge() { PeriodType = BillingPeriodType.Peak, CallType = CallType.LongDistance, PerMinuteCharge = 3 },
                    }
                }
            },
            {
                "Package D",
                new Package()
                {
                    Rental = 300,
                    CallChargeCalcluationStratergy = new PerSecondCallChargeCalculationStratergy(),
                    BillingPeriods = new List<BillingPeriod>()
                    {
                        new BillingPeriod() { Type = BillingPeriodType.OffPeak, StartTime = TimeSpan.FromHours(0), EndTime = TimeSpan.FromHours(8) },
                        new BillingPeriod() { Type = BillingPeriodType.Peak, StartTime = TimeSpan.FromHours(8), EndTime = TimeSpan.FromHours(20) },
                        new BillingPeriod() { Type = BillingPeriodType.OffPeak, StartTime = TimeSpan.FromHours(20), EndTime = TimeSpan.FromHours(24) },
                    },
                    CallCharges = new List<CallCharge>()
                    {
                        new CallCharge() { PeriodType = BillingPeriodType.OffPeak, CallType = CallType.Local, PerMinuteCharge = 2 },
                        new CallCharge() { PeriodType = BillingPeriodType.OffPeak, CallType = CallType.LongDistance, PerMinuteCharge = 4 },
                        new CallCharge() { PeriodType = BillingPeriodType.Peak, CallType = CallType.Local, PerMinuteCharge = 3 },
                        new CallCharge() { PeriodType = BillingPeriodType.Peak, CallType = CallType.LongDistance, PerMinuteCharge = 5 },
                    }
                }
            }
        };

        public Package GetPackage(string packageCode)
        {
            Package package;
            if (!_packages.TryGetValue(packageCode, out package))
                throw new BillingEngineException($"No package found with package code '{packageCode}'");
            return package;
        }
    }

}
