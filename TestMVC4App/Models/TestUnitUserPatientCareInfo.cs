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
    public class TestUnitUserPatientCareInfo : TestUnit
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
            get { return "/PatientCare"; }
        }

        public TestUnitUserPatientCareInfo(TestSuite parent)
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
            UserPatientCareInfo newServiceInfo = newServiceAccessor.GetUserPatientCareById(userId);

            string value = string.Empty;

            if (newServiceInfo.PatientCare != null && !string.IsNullOrEmpty(newServiceInfo.PatientCare.AcceptedReferral))
            {
                value = newServiceInfo.PatientCare.AcceptedReferral;
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_AcceptedReferral, "Comparing AcceptedReferral", this.userId, this.upi, oldServiceData, "acceptReferrals", value);
            UserEducationTrainingInfo_BoardCertifications(newServiceInfo);
            UserEducationTrainingInfo_CancersTreated(newServiceInfo);

        }

        private void UserEducationTrainingInfo_BoardCertifications(UserPatientCareInfo newServiceInfo)
        {
            var oldValues = ParsingHelper.ParseListSimpleValuesStructure(oldServiceData, EnumOldServiceFieldsAsKeys.boardCertification.ToString(), new EnumOldServiceFieldsAsKeys[] { EnumOldServiceFieldsAsKeys.specialty,
                                                                                                                                        EnumOldServiceFieldsAsKeys.certificationYear});
            // TODO: Location belongs in a dedicated test
            var newValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();

            Dictionary<EnumOldServiceFieldsAsKeys, string> properties;

            foreach (var newValue in newServiceInfo.BoardCertifications)
            {
                properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.specialty, newValue.SpecialtyName);
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.specialty, string.Empty);
                }

                try
                {
                    properties.Add(EnumOldServiceFieldsAsKeys.certificationYear, String.Format("{0:yyyy}", newValue.OriginalDate));
                }
                catch (Exception)
                {
                    // make sure a value is present for each index
                    properties.Add(EnumOldServiceFieldsAsKeys.certificationYear, string.Empty);
                }

                newValues.Add(properties);
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_BoardCertifications, "Comparing Board Certification(s)", userId, upi, oldValues, newValues);
        }

        private void UserEducationTrainingInfo_CancersTreated(UserPatientCareInfo newServiceInfo)
        {
            var oldValuesMerged = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(oldServiceData, "cancersTreated"));
            var oldValues = ParsingHelper.StringToList(oldValuesMerged, ',');

            var newValues = new HashSet<string>();
            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.CancersTreated != null)
            {
                foreach (var newValueEntry in newServiceInfo.PatientCare.CancersTreated)
                {
                    if (!string.IsNullOrEmpty(newValueEntry.Name))
                    {
                        newValues.Add(newValueEntry.Name);
                    }
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_CancersTreated, "Comparing Cancer(s) Treated", userId, upi, oldValues, newValues);
        }
    }
}