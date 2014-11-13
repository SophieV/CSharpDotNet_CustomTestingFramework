using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using YSM.PMS.Web.Service.DataTransfer.Models;

namespace TestMVC4App.Models
{
    public class TestUnitUserHonorServiceInfo : TestUnit
    {
        IEnumerable<Honor> newDataHonor;
        IEnumerable<Service> newDataService;

        public TestUnitUserHonorServiceInfo(TestSuite parent, IEnumerable<Honor> newDataHonor, IEnumerable<Service> newDataService)
            : base(parent)
        {
            this.newDataHonor = newDataHonor;
            this.newDataService = newDataService;
        }

        protected override void RunAllSingleTests()
        {
            UserEducationTrainingInfo_Honors();
            UserEducationTrainingInfo_Services();

        }

        private void UserEducationTrainingInfo_Honors()
        {
            var oldValues = ParsingHelper.ParseStructuredListOfValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.professionalHonor.ToString(), new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.award,
                                                                                                                                        EnumOldServiceFieldsAsKeys.organization,
                                                                                                                                        EnumOldServiceFieldsAsKeys.presentationDate,
                                                                                                                                        EnumOldServiceFieldsAsKeys.category});

            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            if (this.newDataHonor != null)
            {
                foreach (var newValue in this.newDataHonor)
                {
                    properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.award, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.AwardName)));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.award, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.organization, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.AwardingOrganization)));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.organization, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.presentationDate, String.Format("{0:yyyy}", newValue.AwardDate));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.presentationDate, string.Empty);
                    }

                    try
                    {
                        if (newValue.HonorCategory.Name == "Unknown")
                        {
                            // keep coherent with old service for testing
                            properties.Add(EnumOldServiceFieldsAsKeys.category, string.Empty);
                        }
                        else
                        {
                            properties.Add(EnumOldServiceFieldsAsKeys.category, newValue.HonorCategory.Name);
                        }
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.category, string.Empty);
                    }

                    newValues.Add(properties);
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserHonorServiceInfo_Honors, "Comparing Honor(s)", oldValues, newValues);
        }

        /// <summary>
        /// This is an example where the data is compared as a structure.
        /// </summary>
        private void UserEducationTrainingInfo_Services()
        {
            var oldValues = ParsingHelper.ParseStructuredListOfValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.professionalService.ToString(), new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.role,
                                                                                                                                        EnumOldServiceFieldsAsKeys.organization,
                                                                                                                                        EnumOldServiceFieldsAsKeys.startDate,
                                                                                                                                        EnumOldServiceFieldsAsKeys.endDate,
                                                                                                                                        EnumOldServiceFieldsAsKeys.category,
                                                                                                                                        EnumOldServiceFieldsAsKeys.description});

            foreach (var structure in oldValues)
            {
                // metadata is added for comparison with more complete new data
                structure[EnumOldServiceFieldsAsKeys.role] = "Professional Organization";

                if (structure[EnumOldServiceFieldsAsKeys.endDate] == "present")
                {
                    structure[EnumOldServiceFieldsAsKeys.endDate] = string.Empty;
                }

                try
                {
                    structure[EnumOldServiceFieldsAsKeys.startDate] = string.Format("{0:yyyy}", DateTime.Parse(structure[EnumOldServiceFieldsAsKeys.startDate], CultureInfo.CurrentCulture));
                }
                catch (Exception) { }

                try {
                    if (structure[EnumOldServiceFieldsAsKeys.endDate].ToLower().Contains("now") || structure[EnumOldServiceFieldsAsKeys.endDate].ToLower().Contains("curr"))
                    {
                        // YMPS-506 : endDate ongoing <=> null
                        structure[EnumOldServiceFieldsAsKeys.endDate] = null;
                    }
                    else
                    {
                        structure[EnumOldServiceFieldsAsKeys.endDate] = string.Format("{0:yyyy}", DateTime.Parse(structure[EnumOldServiceFieldsAsKeys.endDate], CultureInfo.CurrentCulture));
                    }
                }
                catch (Exception) { }
            }

            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            if (this.newDataService != null)
            {
                foreach (var newValue in newDataService)
                {
                    properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.role, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.ServiceType.Name)));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.role, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.organization, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.ServiceOrganization)));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.organization, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.startDate, newValue.StartYear.ToString());
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.startDate, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.endDate, newValue.EndYear.ToString());
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.endDate, string.Empty);
                    }

                    try
                    {
                        if (newValue.ServiceCategory.Name == "Unknown")
                        {
                            // keep coherent with old service for testing
                            properties.Add(EnumOldServiceFieldsAsKeys.category, string.Empty);
                        }
                        else
                        {
                            properties.Add(EnumOldServiceFieldsAsKeys.category, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.ServiceCategory.Name)));
                        }
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.category, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.description, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.Description)));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.description, string.Empty);
                    }

                    newValues.Add(properties);
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserHonorServiceInfo_Services, "Comparing Service(s)", oldValues, newValues);
        }
    }
}