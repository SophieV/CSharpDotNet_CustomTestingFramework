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
            UserPatientCareInfo_PhysicianBio(newServiceInfo);
            UserPatientCareInfo_AcceptedReferral(newServiceInfo);
            UserPatientCareInfo_MyChart(newServiceInfo);
            UserPatientCareInfo_IsSeeingNewPatients(newServiceInfo);
            UserPatientCareInfo_IsSeeingPatientType(newServiceInfo);
            UserEducationTrainingInfo_BoardCertifications(newServiceInfo);
            UserEducationTrainingInfo_CancersTreated(newServiceInfo);

        }

        private void UserPatientCareInfo_PhysicianBio(UserPatientCareInfo newServiceInfo)
        {
            string value = string.Empty;

            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.PhysicianBio != null)
            {
                value = newServiceInfo.PatientCare.PhysicianBio;
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_PhysicianBio, "Comparing Physician Bio", this.userId, this.upi, oldServiceData, EnumOldServiceFieldsAsKeys.physicianBio.ToString(), value);
        }

        private void UserPatientCareInfo_MyChart(UserPatientCareInfo newServiceInfo)
        {
            string value = string.Empty;

            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.IsMyChartAvailable != null)
            {
                if (newServiceInfo.PatientCare.IsMyChartAvailable)
                {
                    value = "1";
                }
                else
                {
                    value = "0";
                }
            }
            else
            {
                value = "0";
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_MyChart, "Comparing myChart", this.userId, this.upi, oldServiceData, EnumOldServiceFieldsAsKeys.myChart.ToString(), value);
        }

        private void UserPatientCareInfo_IsSeeingNewPatients(UserPatientCareInfo newServiceInfo)
        {
            string value = string.Empty;

            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.IsSeeingPatients != null)
            {
                if (newServiceInfo.PatientCare.IsSeeingPatients)
                {
                    value = "Yes";
                }
                else
                {
                    value = "No";
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_IsSeeingNewPatients, "Comparing Seeing New Patients", this.userId, this.upi, oldServiceData, EnumOldServiceFieldsAsKeys.newPatients.ToString(), value);
        }

        private void UserPatientCareInfo_IsSeeingPatientType(UserPatientCareInfo newServiceInfo)
        {
            var oldValuesMerged = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(oldServiceData, EnumOldServiceFieldsAsKeys.patientsGroups.ToString()));
            var oldValues = ParsingHelper.StringToList(oldValuesMerged, ',');

            var newValues = new HashSet<string>();

            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.IsSeeingAdults != null)
            {
                if (newServiceInfo.PatientCare.IsSeeingAdults)
                {
                    newValues.Add("Adult");
                }
            }

            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.IsSeeingChild != null)
            {
                if (newServiceInfo.PatientCare.IsSeeingChild)
                {
                    newValues.Add("Child");
                }
            }

            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.IsSeeingAdolescent != null)
            {
                if (newServiceInfo.PatientCare.IsSeeingAdolescent)
                {
                    newValues.Add("Adolescent");
                }
            }

            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.IsSeeingGeriatric != null)
            {
                if (newServiceInfo.PatientCare.IsSeeingGeriatric)
                {
                    newValues.Add("Geriatric");
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_IsSeeingPatientType, "Comparing Patient Type(s)", this.userId, this.upi, oldValues, newValues);
        }

        private void UserPatientCareInfo_AcceptedReferral(UserPatientCareInfo newServiceInfo)
        {
            string value = string.Empty;

            if (newServiceInfo.PatientCare != null && !string.IsNullOrEmpty(newServiceInfo.PatientCare.AcceptedReferral))
            {
                value = newServiceInfo.PatientCare.AcceptedReferral;
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_AcceptedReferral, "Comparing AcceptedReferral", this.userId, this.upi, oldServiceData, "acceptReferrals", value);
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