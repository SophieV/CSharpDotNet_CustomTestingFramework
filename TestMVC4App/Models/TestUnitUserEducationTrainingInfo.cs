using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class TestUnitUserEducationTrainingInfo : TestUnit
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
            get { return "/EducationTraining"; }
        }

        public TestUnitUserEducationTrainingInfo(TestSuite parent)
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
            UserEducationTrainingInfo newServiceInfo = newServiceAccessor.GetUserEducationTrainingById(userId);

            UserEducationTrainingInfo_Trainings(newServiceInfo);
            UserEducationTrainingInfo_Education(newServiceInfo);
        }

        private void UserEducationTrainingInfo_Education(UserEducationTrainingInfo newServiceInfo)
        {
            var oldValues = ParsingHelper.ParseListSimpleValuesStructure(oldServiceData, EnumOldServiceFieldsAsKeys.education.ToString(), new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.degree,
                                                                                                                                        EnumOldServiceFieldsAsKeys.institution,
                                                                                                                                        EnumOldServiceFieldsAsKeys.gradYear});
            // TODO: Location belongs in a dedicated test
            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            foreach (var newValue in newServiceInfo.Educations)
            {
                properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.degree, newValue.DegreeAwarded);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.degree, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.institution, newValue.Institution);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.institution, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.gradYear, String.Format("{0:yyyy}", newValue.DateAwarded));
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.gradYear, string.Empty);
                }

                newValues.Add(properties);
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserEducationTrainingInfo_Education, "Comparing Education(s)", userId, upi, oldValues, newValues);
        }

        private void UserEducationTrainingInfo_Trainings(UserEducationTrainingInfo newServiceInfo)
        {
            var oldValues = ParsingHelper.ParseListSimpleValuesStructure(oldServiceData, EnumOldServiceFieldsAsKeys.training.ToString(), new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.departmentName, 
                                                                                                                                       EnumOldServiceFieldsAsKeys.startYear, 
                                                                                                                                       EnumOldServiceFieldsAsKeys.endYear, 
                                                                                                                                       EnumOldServiceFieldsAsKeys.position, 
                                                                                                                                       EnumOldServiceFieldsAsKeys.locationName });
            // TODO: Location belongs in a dedicated test
            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            foreach (var newValue in newServiceInfo.Trainings)
            {
                properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.departmentName, newValue.Department);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.departmentName, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.startYear, newValue.StartYear.ToString());
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.startYear, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.endYear, newValue.EndYear.ToString());
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.endYear, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.position, newValue.Position);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.position, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.locationName, newValue.Institution);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.locationName, string.Empty);
                }

                newValues.Add(properties);
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserEducationTrainingInfo_Trainings, "Comparing Training(s)", userId, upi, oldValues, newValues);
        }
    }
}