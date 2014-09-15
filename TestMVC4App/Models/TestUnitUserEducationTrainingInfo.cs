using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public enum OldServiceFieldsAsKeys
    {
        departmentName, 
        startYear, 
        endYear, 
        position, 
        locationName
    }

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

            var trainingsOld = ParsingHelper.ParseListSimpleValuesStructure(oldServiceData, "training", new OldServiceFieldsAsKeys[] { OldServiceFieldsAsKeys.departmentName, 
                                                                                                                                       OldServiceFieldsAsKeys.startYear, 
                                                                                                                                       OldServiceFieldsAsKeys.endYear, 
                                                                                                                                       OldServiceFieldsAsKeys.position, 
                                                                                                                                       OldServiceFieldsAsKeys.locationName });

            // TODO: Location belongs in a dedicated test
            var trainingsNew = new HashSet<Dictionary<OldServiceFieldsAsKeys, string>>();

            Dictionary<OldServiceFieldsAsKeys, string> properties;

            foreach (var training in newServiceInfo.Trainings)
            {
                properties = new Dictionary<OldServiceFieldsAsKeys, string>();

                try
                {
                    properties.Add(OldServiceFieldsAsKeys.departmentName, training.Department);
                } catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(OldServiceFieldsAsKeys.departmentName, string.Empty);
                }

                try
                {
                    properties.Add(OldServiceFieldsAsKeys.startYear,training.StartYear.ToString());
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(OldServiceFieldsAsKeys.startYear, string.Empty);
                }

                try
                {
                    properties.Add(OldServiceFieldsAsKeys.endYear, training.EndYear.ToString());
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(OldServiceFieldsAsKeys.endYear, string.Empty);
                }

                try
                {
                    properties.Add(OldServiceFieldsAsKeys.position, training.Position);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(OldServiceFieldsAsKeys.position, string.Empty);
                }

                try
                {
                    properties.Add(OldServiceFieldsAsKeys.locationName, training.Institution);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(OldServiceFieldsAsKeys.locationName, string.Empty);
                }

                trainingsNew.Add(properties);
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserEducationTrainingInfo_Trainings, "Comparing Training(s)", userId,upi, trainingsOld, trainingsNew);
        }
    }
}