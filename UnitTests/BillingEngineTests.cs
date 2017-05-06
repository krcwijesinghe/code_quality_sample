using System;
using System.Collections.Generic;
using System.Linq;
using MobileBillingSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Billing_Engine_Should
    {
        [TestMethod]
        public void Generate_Bills_For_Each_Customer()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    FullName = "Customer Name 1",
                    BillingAddress = "Billing Address 1",
                    PackageCode = "Package A"
                },
                new Customer() {
                    PhoneNumber = "077-2345434",
                    FullName = "Customer Name 2",
                    BillingAddress = "Billing Address 2",
                    PackageCode = "Package B"
                },
                new Customer() {
                    PhoneNumber = "077-2345343",
                    FullName = "Customer Name 3",
                    BillingAddress = "Billing Address 3",
                    PackageCode = "Package A"
                },
            };

            var cdrs = new List<CallDetailsRecord>(){};

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);

            Assert.AreEqual(3, bills.Count);

            var bill1 = bills.FirstOrDefault(b => b.PhoneNumber == "077-7342345");
            Assert.IsNotNull(bill1);
            Assert.AreEqual("Customer Name 1", bill1.FullName);
            Assert.AreEqual("Billing Address 1", bill1.BillingAddress);

            var bill2 = bills.FirstOrDefault(b => b.PhoneNumber == "077-2345434");
            Assert.IsNotNull(bill2);
            Assert.AreEqual("Customer Name 2", bill2.FullName);
            Assert.AreEqual("Billing Address 2", bill2.BillingAddress);
        }

        [TestMethod]
        public void Generate_Bills_For_Each_Customer_With_Correct_CDRs()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    FullName = "Customer Name 1",
                    BillingAddress = "Billing Address 1",
                    PackageCode = "Package A"
                },
                new Customer() {
                    PhoneNumber = "077-2345434",
                    FullName = "Customer Name 2",
                    BillingAddress = "Billing Address 2",
                    PackageCode = "Package B"
                },
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-2345434",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 10
                },
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 2, 8, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 20
                },
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-2345434",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 3, 10, 00, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 30
                },
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 4, 20, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 40
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);

            var bill1 = bills.FirstOrDefault(b => b.PhoneNumber == "077-7342345");
            Assert.IsNotNull(bill1);
            Assert.AreEqual(2, bill1.CallList.Count);

            var call1 = bill1.CallList.FirstOrDefault(c => c.StartTime == new DateTime(2017, 1, 2, 8, 30, 0));
            Assert.IsNotNull(call1);
            Assert.AreEqual(20, call1.DurationInSeconds);

            var call2 = bill1.CallList.FirstOrDefault(c => c.StartTime == new DateTime(2017, 1, 4, 20, 30, 0));
            Assert.IsNotNull(call2);
            Assert.AreEqual(40, call2.DurationInSeconds);
        }

        [TestMethod]
        public void Calculate_Peak_Billing_Charges_for_PerMinute_Local_Calls_For_Full_Minutes_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package A"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 120
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);
            Assert.AreEqual(1, bills[0].CallList.Count);
            Assert.AreEqual(6, bills[0].CallList[0].Charge);
        }

        [TestMethod]
        public void Calculate_Peak_Billing_Charges_for_PerMinute_Local_Calls_For_Partial_Minutes_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package A"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 30
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);
            Assert.AreEqual(1, bills[0].CallList.Count);
            Assert.AreEqual(3, bills[0].CallList[0].Charge);
        }

        [TestMethod]
        public void Calculate_Peak_Billing_Charges_for_PerMinute_Local_Calls_For_Durations_More_Than_One_Minutes_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package A"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 70
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);
            Assert.AreEqual(1, bills[0].CallList.Count);
            Assert.AreEqual(6, bills[0].CallList[0].Charge);
        }

        [TestMethod]
        public void Calculate_Peak_Billing_Charges_for_PerSecond_Local_Calls_For_Partial_Minutes_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package D"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 30
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);
            Assert.AreEqual(1, bills[0].CallList.Count);
            Assert.AreEqual(1.5M, bills[0].CallList[0].Charge);
        }

        [TestMethod]
        public void Calculate_Peak_Billing_Charges_for_Long_Distance_Calls_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package A"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "071-2342345", //Long distance call (different extensions)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 120
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);
            Assert.AreEqual(1, bills[0].CallList.Count);
            Assert.AreEqual(10, bills[0].CallList[0].Charge);
        }

        [TestMethod]
        public void Calculate_OffPeak_Billing_Charges_for_Local_Calls_Before_Peak_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package A"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 7, 30, 0), // 01-01-2017  7:30 AM (Off-peak call)
                    DurationInSeconds = 120
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);
            Assert.AreEqual(1, bills[0].CallList.Count);
            Assert.AreEqual(4, bills[0].CallList[0].Charge);
        }

        [TestMethod]
        public void Calculate_OffPeak_Billing_Charges_for_Local_Calls_After_Peak_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package A"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 21, 30, 0), // 01-01-2017  9:30 PM (Off-peak call)
                    DurationInSeconds = 120
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);
            Assert.AreEqual(1, bills[0].CallList.Count);
            Assert.AreEqual(4, bills[0].CallList[0].Charge);
        }

        [TestMethod]
        public void Calculate_Free_Minutes_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package C"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 PM (Peak call)
                    DurationInSeconds = 120
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);
            Assert.AreEqual(1, bills[0].CallList.Count);
            Assert.AreEqual(2, bills[0].CallList[0].Charge);
        }

        [TestMethod]
        public void Calculate_Summery_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package A"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 120
                },
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 30
                },
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 70
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);

            Assert.AreEqual(1, bills.Count);
            Assert.AreEqual(3, bills[0].CallList.Count);
            Assert.AreEqual(100, bills[0].Rental);
            Assert.AreEqual(15, bills[0].TotalCallChareges);
            Assert.AreEqual(0, bills[0].Discounts);
            Assert.AreEqual(23, bills[0].Tax);
            Assert.AreEqual(138, bills[0].BillAmount);
        }

        [TestMethod]
        public void Calculate_Summery_With_Discount_Correctly()
        {
            var customers = new List<Customer>() {
                new Customer() {
                    PhoneNumber = "077-7342345",
                    PackageCode = "Package A"
                }
            };

            var cdrs = new List<CallDetailsRecord>()
            {
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 12000
                },
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 3000
                },
                new CallDetailsRecord() {
                    OriginatingPhoneNumber = "077-7342345",
                    RecievingPhoneNumber = "077-2342345", //Local call (same extension)
                    StartTime = new DateTime(2017, 1, 1, 10, 30, 0), // 01-01-2017  10:30 AM (Peak call)
                    DurationInSeconds = 7000
                }
            };

            var target = new BillingEngine();
            var bills = target.Generate(customers, cdrs);

            Assert.AreEqual(1, bills.Count);
            Assert.AreEqual(3, bills[0].CallList.Count);
            Assert.AreEqual(100, bills[0].Rental);
            Assert.AreEqual(1101, bills[0].TotalCallChareges);
            Assert.AreEqual(440.4M, bills[0].Discounts);
            Assert.AreEqual(240.2M, bills[0].Tax);
            Assert.AreEqual(1000.8M, bills[0].BillAmount);
        }
    }

}
