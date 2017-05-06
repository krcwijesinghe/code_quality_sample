using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// Base class for all call charge calculation stratergies
    /// </summary>
    public abstract class CallChargeCalculationStratergy
    {
        /// <summary>
        /// Calculate call chareg
        /// </summary>
        /// <param name="customer">Customer who made the call</param>
        /// <param name="cdr">CDR record of the call to calculate the chareg</param>
        /// <param name="package">Package of the customer</param>
        /// <returns>Charge for the given call (using CDR)</returns>
        public decimal CalcluateCallCharge(Customer customer, CallDetailsRecord cdr, Package package)
        {
            var billingPeriodType = GetBillingPeriodType(cdr.StartTime, package);
            var callType = GetCallType(customer.PhoneNumber, cdr.RecievingPhoneNumber);
            var charge = package.CallCharges.Where(c => c.PeriodType == billingPeriodType && c.CallType == callType).First();
            var paidDurationInSeconds = GetPaidCallDuration(customer, cdr, package);
            return GetCharegesForTheCall(charge, paidDurationInSeconds);
        }

        /// <summary>
        /// When overriden by the derived classes, this method caclulate the call charges for the given duration
        /// </summary>
        /// <param name="charge">Call charge object that represent charding details for the call</param>
        /// <param name="durationInSeconds">call duration</param>
        /// <returns>call charge</returns>
        protected abstract decimal GetCharegesForTheCall(CallCharge charge, int durationInSeconds);

        /// <summary>
        /// Get paid call duration (i.e. total call duration in seconds - no of free seconds)
        /// </summary>
        /// <returns>Call duration that requires to be paied</returns>
        private int GetPaidCallDuration(Customer customer, CallDetailsRecord cdr, Package package)
        {
            var freeCallDuration = package.FreeCallDurations
                            .Where(d => d.CallType == GetCallType(customer.PhoneNumber, cdr.OriginatingPhoneNumber) &&
                                        d.PeriodType == GetBillingPeriodType(cdr.StartTime, package))
                            .FirstOrDefault()?.DurationInSeconds ?? 0;

            if (cdr.DurationInSeconds > freeCallDuration)
                return cdr.DurationInSeconds - freeCallDuration;
            else
                return 0;
        }

        private static CallType GetCallType(string originatingPhoneNumber, string recievingPhoneNumber)
        {
            //check if the extensions of originating phone number and the recieving phone number are the same
            return string.Equals(GetExtension(recievingPhoneNumber), GetExtension(originatingPhoneNumber), StringComparison.Ordinal) ?
                        CallType.Local :
                        CallType.LongDistance;
        }

        /// <summary>
        /// Extreact the extension from a phone number in "999-9999999" format (first 3 digits are the extension)
        /// </summary>
        /// <param name="number">Phone number to extract the extension</param>
        /// <returns></returns>
        protected static string GetExtension(string number)
        {
            return number.Substring(0, 3);
        }

        protected BillingPeriodType GetBillingPeriodType(DateTime startTime, Package package)
        {
            var timeOfTheDay = startTime - startTime.Date;
            foreach (var period in package.BillingPeriods)
            {
                if (period.StartTime <= timeOfTheDay && timeOfTheDay < period.EndTime)
                {
                    return period.Type;
                }
            }

            //no proper billing period is found for the given start time (this is a configuration error, throw an exception)
            throw new BillingEngineException($"Billing Period is not defined for the given start time { startTime }");
        }
    }

}
