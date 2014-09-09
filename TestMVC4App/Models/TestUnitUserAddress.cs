using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserAddress : TestUnit
    {
        private IEnumerable<UserAddress> newServiceAddresses = new HashSet<UserAddress>();
        private IEnumerable<XElement> oldServiceAddresses;
        private IEnumerable<XElement> oldServiceMailingInfo;
        private int userId;
        private int upi;

        public void ProvideOrganizationData(int userId,
                                            int upi,
                                            IEnumerable<XElement> oldServiceAddresses,
                                            IEnumerable<XElement> oldServiceMailingInfo,
                                            IEnumerable<UserAddress> newServiceAddresses)
        {
            this.userId = userId;
            this.upi = upi;

            if (newServiceAddresses != null)
            {
                this.newServiceAddresses = newServiceAddresses;
            }

            if (oldServiceAddresses != null)
            {
                this.oldServiceAddresses = oldServiceAddresses;
            }

            if (oldServiceMailingInfo != null)
            {
                this.oldServiceMailingInfo = oldServiceMailingInfo;
            }
        }

        public override string newServiceURLExtensionBeginning
        {
            get { return Parent.newServiceURLExtensionBeginning; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return Parent.newServiceURLExtensionEnding; }
        }

        public TestUnitUserAddress(TestSuite parent, TestUnit bigBrother) 
        : base (parent,bigBrother)
        {

        }

        protected override void RunAllSingleTests()
        {
           UserContactLocationInfo_UserAddress_StreetAddress_Test(this.oldServiceAddresses, new HashSet<string>(this.newServiceAddresses.Select(x=>x.Address.BaseAddress.StreetAddress)));
           UserContactLocationInfo_UserAddress_ZipCodes_Test(this.oldServiceAddresses, new HashSet<string>(this.newServiceAddresses.Select(x => x.Address.BaseAddress.Zip)));
           UserContactLocationInfo_UserAddress_IsMailing_Test(this.oldServiceAddresses, new HashSet<string>(this.newServiceAddresses.Where(x=>x.IsMailingAddress==true).Select(x => x.Address.BaseAddress.StreetAddress)));

            ComputeOverallSeverity();
        }

        private void UserContactLocationInfo_UserAddress_StreetAddress_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseListSimpleOldValues(oldServiceNodes, "addressLine1");

            var oldValue2 = ParsingHelper.ParseSingleOldValue(oldServiceMailingInfo, "mailingAddress2");

            if (!string.IsNullOrEmpty(oldValue2))
            {
                oldValues.Add(oldValue2);
            }

            var resultReport = new ResultReport("UserContactLocationInfo_UserAddress_StreetAddress_Test", "Comparing Address StreetInfo(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(userId),
                                              resultReport);
        }

        private void UserContactLocationInfo_UserAddress_IsMailing_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            var oldValue2 = ParsingHelper.ParseSingleOldValue(oldServiceMailingInfo, "mailingAddress2");
            var oldValues = new HashSet<string>();
            oldValues.Add(oldValue2);

            var resultReport = new ResultReport("UserContactLocationInfo_UserAddress_IsMailing_Test", "Comparing Mailing Address");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(userId),
                                              resultReport);
        }

        private void UserContactLocationInfo_UserAddress_ZipCodes_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseListSimpleOldValues(oldServiceNodes, "zipCode");

            var resultReport = new ResultReport("UserContactLocationInfo_UserAddress_ZipCodes_Test", "Comparing Address Zip Code(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(new HashSet<string>(oldValues.Distinct()), new HashSet<string>(newValues.Distinct()), resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(userId),
                                              resultReport);
        }
    }
}