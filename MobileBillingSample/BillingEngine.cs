using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// This class is responsible for mobile bill generation
    /// </summary>
    public class BillingEngine
    {
        //Government tax percentage (currently hardcoded here)
        private const decimal _taxPercentage = 0.2M;

        //Dependent contracts
        private IPackageRepository _packageRepository;

        /// <summary>
        /// Default constructor with hardcoded default dependencies
        /// </summary>
        public BillingEngine()
            :this(new PackageRepository()) 
        {
        }

        /// <summary>
        /// Injection constructor
        /// </summary>
        /// <param name="packageRepository"></param>
        public BillingEngine(IPackageRepository packageRepository) 
        {
            _packageRepository = packageRepository;
        }

        /// <summary>
        /// Generate monthly bills
        /// </summary>
        /// <param name="customerList">List of all customers</param>
        /// <param name="cdrLists">List of CDRs for that (for all customers)</param>
        /// <returns>Bills for  each customer in customer list</returns>
        public IList<Bill> Generate(IEnumerable<Customer> customerList, IEnumerable<CallDetailsRecord> cdrLists)
        {
            var billsList = new List<Bill>();

            foreach (var customer in customerList)
            {
                var cdrsForCustomer = cdrLists.Where(c => string.Equals(c.OriginatingPhoneNumber, customer.PhoneNumber, StringComparison.Ordinal));
                var bill = GenerateBillForCustomer(customer, cdrsForCustomer);
                billsList.Add(bill);
            }

            return billsList;
        }

        /// <summary>
        /// Generate the bill for the given customer
        /// </summary>
        /// <param name="customer">Customer to generate the bill</param>
        /// <param name="cdrsForCustomer">CDRs of that customer</param>
        /// <returns></returns>
        private Bill GenerateBillForCustomer(Customer customer, IEnumerable<CallDetailsRecord> cdrsForCustomer)
        {
            var package = _packageRepository.GetPackage(customer.PackageCode);

            var bill = new Bill();
            FillCustomerDetails(bill, customer);

            foreach (var cdr in cdrsForCustomer)
            {
                var callDetails = GetCallDetailsFromCdr(cdr, customer, package);
                bill.CallList.Add(callDetails);
            }

            CacluateSummery(bill, package);
            return bill;
        }

        /// <summary>
        /// Fill customer details to bill object
        /// </summary>
        /// <param name="bill">the bill object to fill data</param>
        /// <param name="customer">tustomer object</param>
        private void FillCustomerDetails(Bill bill, Customer customer)
        {
            bill.PhoneNumber = customer.PhoneNumber;
            bill.FullName = customer.FullName;
            bill.BillingAddress = customer.BillingAddress;
        }

        /// <summary>
        /// Convert a CallDetailsRecord (CDR) object to CallDetails object (call details objects in mobile bills)
        /// </summary>
        /// <param name="cdr">CDR to convert</param>
        /// <param name="customer">Customer who owns the CDR</param>
        /// <param name="package">Customer Package</param>
        /// <returns></returns>
        private CallDetails GetCallDetailsFromCdr(CallDetailsRecord cdr, Customer customer, Package package)
        {
            var callDetails = new CallDetails();
            callDetails.StartTime = cdr.StartTime;
            callDetails.DestinationNumber = cdr.RecievingPhoneNumber;
            callDetails.DurationInSeconds = cdr.DurationInSeconds;
            callDetails.Charge = package.CallChargeCalcluationStratergy.CalcluateCallCharge(customer, cdr, package);
            return callDetails;
        }

        /// <summary>
        /// Calculate the summery of the bill. This includes monthly rental, government tax, any discounts and the total amount
        /// </summary>
        /// <param name="bill">Bill to calculate the summery</param>
        /// <param name="package">Package of the use who owns this bill</param>
        private void CacluateSummery(Bill bill, Package package)
        {
            bill.TotalCallChareges = bill.CallList.Sum(c => c.Charge);
            bill.Rental = package.Rental;
            bill.Tax = (bill.TotalCallChareges + bill.Rental) * _taxPercentage;
            bill.Discounts = package.DiscountCalculationStratergy.GetDiscountAmount(bill);
            bill.BillAmount = bill.TotalCallChareges + bill.Rental + bill.Tax - bill.Discounts;
        }
    }

}
