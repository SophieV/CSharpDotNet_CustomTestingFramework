using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserContactLocationInfo : TestUnit
    {
        private IEnumerable<LabWebsite> newDataLabWebsite;
        private IEnumerable<UserAddress> newDataUserAddress;

        public TestUnitUserContactLocationInfo(TestSuite parent, IEnumerable<LabWebsite> newDataLabWebsite, IEnumerable<UserAddress> newDataUserAddresses) 
            : base(parent)
        {
            this.newDataLabWebsite = newDataLabWebsite;
            this.newDataUserAddress = newDataUserAddresses;
        }

        protected override void RunAllSingleTests()
        {
            // TODO : not exposed yet
            // UserContactLocationInfo_Assistants_Test();
            UserContactLocationInfo_LabWebsites_Test();
            UserContactLocationInfo_Addresses_Test();

            ComputeOverallSeverity();
        }

        /// <summary>
        /// The Assistant Name is a combination of the lastname and firstname.
        /// We are interested in checking :
        /// - whether the amount of persons listed as assistants is a match
        /// - whether the name returned by the new service contains the firstname returned by the old service (defined as good enough match)
        /// </summary>
        /// <param name="newServiceData"></param>
        /// <param name="this.OldDataNodes"></param>
        /// <remarks>Special syntax.</remarks>
        private void UserContactLocationInfo_Assistants_Test()
        {
            //HashSet<string> oldValues = ParsingHelper.ParseListSimpleValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.assistant.ToString(), EnumOldServiceFieldsAsKeys.fname.ToString());

            //HashSet<string> newValues = new HashSet<string>();
            //if(newServiceData.Assistants.Count() > 0)
            //{
            //    foreach(var assistant in newServiceData.Assistants)
            //    {
            //        if (!string.IsNullOrEmpty(assistant.UserMinimalInfo.Name))
            //        {
            //            newValues.Add(assistant.UserMinimalInfo.Name);
            //        }
            //    }
            //}

            //var pairs = new Dictionary<HashSet<string>, HashSet<string>>();
            //if (oldValues.Count() > 0 || newValues.Count() > 0)
            //{
            //    pairs.Add(oldValues,newValues);
            //}

            //this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_Assistants, "Comparing Assistant Name(s)", this.UserId, this.Upi, pairs,true);
        }

        private void UserContactLocationInfo_LabWebsites_Test()
        {
            var oldDataLabWebsites = ParsingHelper.ParseListNodes(this.OldDataNodes, EnumOldServiceFieldsAsKeys.labWebsite.ToString());

            UserContactLocationInfo_LabWebsites_Names_Test(oldDataLabWebsites, new HashSet<string>(this.newDataLabWebsite.Select(x => x.LabName)));
            UserContactLocationInfo_LabWebsites_Links_Test(oldDataLabWebsites, new HashSet<string>(this.newDataLabWebsite.Select(x => x.LabUrl)));
        }

        private void UserContactLocationInfo_Addresses_Test()
        {
            var oldAddresses = ParsingHelper.ParseListNodes(this.OldDataNodes,EnumOldServiceFieldsAsKeys.location.ToString());
            var allOldAddresses = ParsingHelper.ParseListNodes(this.OldDataNodes, EnumOldServiceFieldsAsKeys.mailing.ToString(),oldAddresses.ToList(), true);

            UserContactLocationInfo_UserAddress_StreetAddress_Test(allOldAddresses, new HashSet<string>(this.newDataUserAddress.Select(x => x.Address.BaseAddress.StreetAddress)));
            UserContactLocationInfo_UserAddress_ZipCodes_Test(allOldAddresses, new HashSet<string>(this.newDataUserAddress.Select(x => x.Address.BaseAddress.Zip)));
            UserContactLocationInfo_UserAddress_IsMailing_Test(allOldAddresses, new HashSet<string>(this.newDataUserAddress.Where(x => x.IsMailingAddress == true).Select(x => x.Address.BaseAddress.StreetAddress)));
        }

        private void UserContactLocationInfo_LabWebsites_Names_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseListSimpleValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.titleName.ToString());

            var resultReport = new ResultReport(EnumTestUnitNames.UserContactLocationInfo_LabWebsites_Names, "Comparing LabWebsite Name(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.UserId,
                                              this.Upi,
                                              this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserContactLocationInfo_LabWebsites_Links_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseListSimpleValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.link.ToString());

            var resultReport = new ResultReport(EnumTestUnitNames.UserContactLocationInfo_LabWebsites_Links, "Comparing LabWebsite Link(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.UserId,
                                              this.Upi,
                                              this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }


        private void UserContactLocationInfo_UserAddress_StreetAddress_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseListSimpleValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.addressLine1.ToString());

            StringBuilder mailingAddress = new StringBuilder();
            mailingAddress.Append(ParsingHelper.ParseSingleValue(oldServiceNodes, EnumOldServiceFieldsAsKeys.mailingAddress1.ToString()));
            mailingAddress.Append(" ");
            mailingAddress.Append(ParsingHelper.ParseSingleValue(oldServiceNodes, EnumOldServiceFieldsAsKeys.mailingAddress2.ToString()));
            oldValues.Add(mailingAddress.ToString());

            var resultReport = new ResultReport(EnumTestUnitNames.UserContactLocationInfo_Addresses_StreetAddress, "Comparing Address StreetInfo(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.UserId,
                                              this.Upi,
                                              this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserContactLocationInfo_UserAddress_IsMailing_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            StringBuilder mailingAddress = new StringBuilder();
            mailingAddress.Append(ParsingHelper.ParseSingleValue(oldServiceNodes, EnumOldServiceFieldsAsKeys.mailingAddress1.ToString()));
            mailingAddress.Append(" ");
            mailingAddress.Append(ParsingHelper.ParseSingleValue(oldServiceNodes, EnumOldServiceFieldsAsKeys.mailingAddress2.ToString()));
            var oldValues = new HashSet<string>();
            oldValues.Add(mailingAddress.ToString());

            var resultReport = new ResultReport(EnumTestUnitNames.UserContactLocationInfo_Addresses_IsMailing, "Comparing Mailing Address");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.UserId,
                                              this.Upi,
                                              this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserContactLocationInfo_UserAddress_ZipCodes_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseListSimpleValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.zipCode.ToString());

            var resultReport = new ResultReport(EnumTestUnitNames.UserContactLocationInfo_Addresses_ZipCodes, "Comparing Address Zip Code(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(new HashSet<string>(oldValues.Distinct()), new HashSet<string>(newValues.Distinct()), resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.UserId,
                                              this.Upi,
                                              this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }
    }
}