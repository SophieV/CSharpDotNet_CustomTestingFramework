using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class TestUnitUserHonorServiceInfo : TestUnit
    {
        private UsersClient newServiceAccessor;
        private IEnumerable<XElement> oldServiceData;
        private int upi;
        private int userId;

        public override string newServiceURLExtensionBeginning
        {
            get { return "Users/"; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return "/HonorService"; }
        }

        public TestUnitUserHonorServiceInfo(TestSuite parent)
            : base(parent)
        {
        }

        public void ProvideData(IEnumerable<XElement> oldData, UsersClient newDataAccessor, int upi, int userId)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.upi = upi;
            this.userId = userId;
        }

        protected override void RunAllSingleTests()
        {
            UserEducationTrainingInfo newServiceInfo = newServiceAccessor.GetUserHonorServiceById(userId);

            UserEducationTrainingInfo_Honors(newServiceInfo);
            UserEducationTrainingInfo_Services(newServiceInfo);

        }

        private void UserEducationTrainingInfo_Honors(UserEducationTrainingInfo newServiceInfo)
        {
            var oldValues = ParsingHelper.ParseListSimpleValuesStructure(oldServiceData, EnumOldServiceFieldsAsKeys.professionalHonor.ToString(), new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.award,
                                                                                                                                        EnumOldServiceFieldsAsKeys.organization,
                                                                                                                                        EnumOldServiceFieldsAsKeys.presentationDate,
                                                                                                                                        EnumOldServiceFieldsAsKeys.category});
            // TODO: Location belongs in a dedicated test
            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            foreach (var newValue in newServiceInfo.Honors)
            {
                properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.award, newValue.AwardName);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.award, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.organization, newValue.AwardingOrganization);
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
                    if (newValue.Category == "Unknown")
                    {
                        // keep coherent with old service for testing
                        properties.Add(EnumOldServiceFieldsAsKeys.category, string.Empty);
                    }
                    else
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.category, newValue.Category);
                    }
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.category, string.Empty);
                }

                newValues.Add(properties);
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserHonorServiceInfo_Honors, "Comparing Honor(s)", userId, upi, oldValues, newValues);
        }

        private void UserEducationTrainingInfo_Services(UserEducationTrainingInfo newServiceInfo)
        {
            var oldValues = ParsingHelper.ParseListSimpleValuesStructure(oldServiceData, EnumOldServiceFieldsAsKeys.professionalService.ToString(), new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.role,
                                                                                                                                        EnumOldServiceFieldsAsKeys.organization,
                                                                                                                                        EnumOldServiceFieldsAsKeys.startDate,
                                                                                                                                        EnumOldServiceFieldsAsKeys.endDate,
                                                                                                                                        EnumOldServiceFieldsAsKeys.category,
                                                                                                                                        EnumOldServiceFieldsAsKeys.description});

            foreach (var structure in oldValues)
            {
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
                    structure[EnumOldServiceFieldsAsKeys.endDate] = string.Format("{0:yyyy}", DateTime.Parse(structure[EnumOldServiceFieldsAsKeys.endDate], CultureInfo.CurrentCulture));
                }
                catch (Exception) { }
            }

            // TODO: Location belongs in a dedicated test
            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            foreach (var newValue in newServiceInfo.Services)
            {
                properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.role, newValue.ServiceType);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.role, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.organization, newValue.ServiceOrganization);
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
                    if (newValue.ServiceCategory == "Unknown")
                    {
                        // keep coherent with old service for testing
                        properties.Add(EnumOldServiceFieldsAsKeys.category, string.Empty);
                    }
                    else
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.category, newValue.ServiceCategory);
                    }
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.category, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.description, newValue.Description);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.description, string.Empty);
                }

                newValues.Add(properties);
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserHonorServiceInfo_Services, "Comparing Service(s)", userId, upi, oldValues, newValues);
        }
    }
}