using System;
using System.Collections.Generic;
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
                    properties.Add(EnumOldServiceFieldsAsKeys.category, newValue.Category);
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
    }
}