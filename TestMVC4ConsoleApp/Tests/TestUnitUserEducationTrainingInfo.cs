using System;
using System.Collections.Generic;
using System.Web;
using YSM.PMS.Web.Service.DataTransfer.Models;

namespace TestMVC4App.Models
{
    public class TestUnitUserEducationTrainingInfo : TestUnit
    {
        IEnumerable<Education> newDataEducation;
        IEnumerable<Training> newDataTraining;

        public TestUnitUserEducationTrainingInfo(TestSuite parent, IEnumerable<Education> newDataEducation, IEnumerable<Training> newDataTraining)
            : base(parent)
        {
            this.newDataEducation = newDataEducation;
            this.newDataTraining = newDataTraining;
        }

        protected override void RunAllSingleTests()
        {
            UserEducationTrainingInfo_Trainings();
            UserEducationTrainingInfo_Education();

        }

        private void UserEducationTrainingInfo_Education()
        {
            var oldValues = ParsingHelper.ParseStructuredListOfValues(
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.education.ToString(), 
                new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.degree,
                                                    EnumOldServiceFieldsAsKeys.institution,
                                                    EnumOldServiceFieldsAsKeys.gradYear});

            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            if (this.newDataEducation != null)
            {
                foreach (var newValue in this.newDataEducation)
                {
                    properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.degree, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.DegreeAwarded)));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.degree, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.institution, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.Institution)));
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
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserEducationTrainingInfo_Education, "Comparing Education(s)", oldValues, newValues);
        }

        private void UserEducationTrainingInfo_Trainings()
        {
            var oldValues = ParsingHelper.ParseStructuredListOfValues(
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.training.ToString(), new 
                    EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.departmentName, 
                                                    EnumOldServiceFieldsAsKeys.startYear, 
                                                    EnumOldServiceFieldsAsKeys.endYear, 
                                                    EnumOldServiceFieldsAsKeys.position, 
                                                    EnumOldServiceFieldsAsKeys.locationName });
            // TODO: Location belongs in a dedicated test
            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            if (this.newDataTraining != null)
            {
                foreach (var newValue in this.newDataTraining)
                {
                    properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.departmentName, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.Department)));
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
                        properties.Add(EnumOldServiceFieldsAsKeys.position, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.Position)));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.position, string.Empty);
                    }

                    try
                    {
                        properties.Add(EnumOldServiceFieldsAsKeys.locationName, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.Institution)));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.locationName, string.Empty);
                    }

                    newValues.Add(properties);
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserEducationTrainingInfo_Trainings, "Comparing Training(s)", oldValues, newValues);
        }
    }
}