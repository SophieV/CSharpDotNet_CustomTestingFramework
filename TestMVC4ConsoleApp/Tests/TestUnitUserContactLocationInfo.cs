using System;
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
            var oldDataLabWebsites = ParsingHelper.ParseListNodes(this.OldDataNodes, EnumOldServiceFieldsAsKeys.labWebsite.ToString());

            UserContactLocationInfo_LabWebsites_Names_Test(oldDataLabWebsites, new HashSet<string>(this.newDataLabWebsite.Select(x => x.LabName)));
            UserContactLocationInfo_LabWebsites_Links_Test(oldDataLabWebsites, new HashSet<string>(this.newDataLabWebsite.Select(x => x.LabUrl)));

            UserContactLocationInfo_Addresses_Test();

            var oldAddressesWithoutMailing = ParsingHelper.ParseListNodes(this.OldDataNodes, EnumOldServiceFieldsAsKeys.location.ToString());
            var oldAddresses = ParsingHelper.ParseListNodes(this.OldDataNodes, EnumOldServiceFieldsAsKeys.mailing.ToString(), oldAddressesWithoutMailing.ToList(), true);

            UserContactLocationInfo_Addresses_Geo(oldAddressesWithoutMailing, new HashSet<GeoPoint>(this.newDataUserAddress.Select(x => x.Address.BaseAddress.Location.GeoPoint)));
            UserContactLocationInfo_UserAddress_StreetAddress_Test(oldAddresses, new HashSet<string>(this.newDataUserAddress.Select(x => x.Address.BaseAddress.StreetAddress)));
            UserContactLocationInfo_UserAddress_ZipCodes_Test(oldAddresses, new HashSet<string>(this.newDataUserAddress.Select(x => x.Address.BaseAddress.Zip)));
            UserContactLocationInfo_UserAddress_IsMailing_Test(oldAddresses, new HashSet<string>(this.newDataUserAddress.Where(x => x.IsMailingAddress == true).Select(x => x.Address.BaseAddress.StreetAddress)));

            ComputeOverallSeverity();
        }

        private void UserContactLocationInfo_Addresses_Test()
        {
            var oldValues = ParsingHelper.ParseStructuredListOfValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.location.ToString(), new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.locationName,
                                                                                                                                        EnumOldServiceFieldsAsKeys.building,
                                                                                                                                        EnumOldServiceFieldsAsKeys.addressLine1,
                                                                                                                                        EnumOldServiceFieldsAsKeys.suite,
                                                                                                                                        EnumOldServiceFieldsAsKeys.city,
                                                                                                                                        EnumOldServiceFieldsAsKeys.state,
                                                                                                                                        EnumOldServiceFieldsAsKeys.zipCode,
                                                                                                                                        EnumOldServiceFieldsAsKeys.type});

            // add mailing address manually
            if (this.OldDataNodes != null)
            {
                var mailingAddress = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                try
                {
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.locationName, ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.mailingAddressName.ToString()));
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.locationName, string.Empty);
                }

                mailingAddress.Add(EnumOldServiceFieldsAsKeys.building, string.Empty);


                try
                {
                    StringBuilder value = new StringBuilder();
                    value.Append(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.mailingAddress1.ToString()));
                    value.Append(" ");
                    value.Append(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.mailingAddress2.ToString()));
                        
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.addressLine1, value.ToString().Trim());
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.addressLine1, string.Empty);
                }
                
                mailingAddress.Add(EnumOldServiceFieldsAsKeys.suite, string.Empty);


                try
                {
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.city, ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.mailingAddressCity.ToString()));
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.city, string.Empty);
                }

                try
                {
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.state, ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.mailingAddressState.ToString()));
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.state, string.Empty);
                }

                try
                {
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.zipCode, ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.mailingAddressZip.ToString()));
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    mailingAddress.Add(EnumOldServiceFieldsAsKeys.zipCode, string.Empty);
                }

                bool atLeastOneFieldPopulated = mailingAddress.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Count() > 0;

                if (atLeastOneFieldPopulated)
                {
                    oldValues.Add(mailingAddress);
                }
            }


            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            if (this.newDataUserAddress != null)
            {
                foreach (var newValue in this.newDataUserAddress)
                {
                    properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.locationName, newValue.Address.OfficeName);
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.locationName, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.building, newValue.Address.BaseAddress.Building.Name);
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.building, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.addressLine1, newValue.Address.BaseAddress.StreetAddress);
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.addressLine1, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.suite, newValue.Address.Detail);
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.suite, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.city, newValue.Address.BaseAddress.Location.City);
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.city, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.state, newValue.Address.BaseAddress.Location.StateRegionProvince);
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.state, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.zipCode, newValue.Address.BaseAddress.Zip);
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.zipCode, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.type, newValue.Address.AddressType.Name);
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.type, string.Empty);
                    }

                    newValues.Add(properties);
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_Addresses, "Comparing Address(es)", oldValues, newValues);
        }

        private void UserContactLocationInfo_Addresses_Geo(IEnumerable<XElement> oldServiceNodes, HashSet<GeoPoint> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            List<string> oldValues = new List<string>();
            oldValues.AddRange(ParsingHelper.ParseUnstructuredListOfValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.location.ToString(), EnumOldServiceFieldsAsKeys.latitude.ToString()));
            oldValues.AddRange(ParsingHelper.ParseUnstructuredListOfValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.location.ToString(), EnumOldServiceFieldsAsKeys.longitude.ToString()));

            HashSet<string> roundedOldValues = new HashSet<string>(oldValues.Where(x=>!string.IsNullOrEmpty(x)).Select(x=>Math.Round(Double.Parse(x),0).ToString()));

            HashSet<string> roundedNewValues = new HashSet<string>();
            if (newValues != null)
            {
                foreach (var value in newValues)
                {
                    try
                    {
                        roundedNewValues.Add(Math.Round((double)value.Latitude, 0).ToString());
                    }
                    catch (Exception) { }

                    try
                    {
                        roundedNewValues.Add(Math.Round((double)value.Longitude, 0).ToString());
                    }
                    catch (Exception) { }
                }
            }

            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserContactLocationInfo_Addresses_Geo, "Comparing Geo Location Data");
            var compareStrategy = new CompareStrategyFactory(roundedOldValues, roundedNewValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserContactLocationInfo_LabWebsites_Names_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseUnstructuredListOfValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.titleName.ToString());

            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserContactLocationInfo_LabWebsites_Names, "Comparing LabWebsite Name(s)");
            var compareStrategy = new CompareStrategyFactory(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserContactLocationInfo_LabWebsites_Links_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseUnstructuredListOfValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.link.ToString());

            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserContactLocationInfo_LabWebsites_Links, "Comparing LabWebsite Link(s)");
            var compareStrategy = new CompareStrategyFactory(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserContactLocationInfo_UserAddress_StreetAddress_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseUnstructuredListOfValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.addressLine1.ToString());

            StringBuilder mailingAddress = new StringBuilder();
            mailingAddress.Append(ParsingHelper.ParseSingleValue(oldServiceNodes, EnumOldServiceFieldsAsKeys.mailingAddress1.ToString()));
            mailingAddress.Append(" ");
            mailingAddress.Append(ParsingHelper.ParseSingleValue(oldServiceNodes, EnumOldServiceFieldsAsKeys.mailingAddress2.ToString()));

            if (!string.IsNullOrWhiteSpace(mailingAddress.ToString()))
            {
                oldValues.Add(mailingAddress.ToString());
            }

            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserContactLocationInfo_Addresses_StreetAddress, "Comparing Address StreetInfo(s)");
            var compareStrategy = new CompareStrategyFactory(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
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

            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserContactLocationInfo_Addresses_IsMailing, "Comparing Mailing Address");
            var compareStrategy = new CompareStrategyFactory(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserContactLocationInfo_UserAddress_ZipCodes_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = ParsingHelper.ParseUnstructuredListOfValues(oldServiceNodes, EnumOldServiceFieldsAsKeys.zipCode.ToString());

            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserContactLocationInfo_Addresses_ZipCodes, "Comparing Address Zip Code(s)");
            var compareStrategy = new CompareStrategyFactory(new HashSet<string>(oldValues.Distinct()), new HashSet<string>(newValues.Distinct()), resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }
    }
}