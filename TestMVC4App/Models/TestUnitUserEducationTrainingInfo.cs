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

            var trainingsOld = ParsingHelper.ParseListSimpleValuesStructure(oldServiceData, "training", new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.departmentName, 
                                                                                                                                       EnumOldServiceFieldsAsKeys.startYear, 
                                                                                                                                       EnumOldServiceFieldsAsKeys.endYear, 
                                                                                                                                       EnumOldServiceFieldsAsKeys.position, 
                                                                                                                                       EnumOldServiceFieldsAsKeys.locationName });

            // TODO: Location belongs in a dedicated test
            var trainingsNew = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            foreach (var training in newServiceInfo.Trainings)
            {
                properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.departmentName, training.Department);
                } catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.departmentName, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.startYear,training.StartYear.ToString());
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.startYear, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.endYear, training.EndYear.ToString());
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.endYear, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.position, training.Position);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.position, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.locationName, training.Institution);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.locationName, string.Empty);
                }

                trainingsNew.Add(properties);
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserEducationTrainingInfo_Trainings, "Comparing Training(s)", userId,upi, trainingsOld, trainingsNew);
        }
    }
}