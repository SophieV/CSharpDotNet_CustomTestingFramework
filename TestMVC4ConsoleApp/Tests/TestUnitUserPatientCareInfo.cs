using System;
using System.Collections.Generic;
using System.Web;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserPatientCareInfo : TestUnit
    {
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

        protected override void RunAllSingleTests()
        {
            UserPatientCareInfo newServiceInfo = this.NewDataAccessor.GetUserPatientCareById(this.UserId);
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
            string newValue = string.Empty;

            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.PhysicianBio != null)
            {
                newValue = newServiceInfo.PatientCare.PhysicianBio;
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_PhysicianBio, 
                "Comparing Physician Bio", 
                this.UserId, 
                this.Upi, 
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.physicianBio.ToString(), 
                newValue);
        }

        private void UserPatientCareInfo_MyChart(UserPatientCareInfo newServiceInfo)
        {
            string newValue = string.Empty;

            // convert to values returned by the old service
            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.IsMyChartAvailable != null)
            {
                if (newServiceInfo.PatientCare.IsMyChartAvailable)
                {
                    newValue = "1";
                }
                else
                {
                    newValue = "0";
                }
            }
            else
            {
                newValue = "0";
            }
            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_MyChart, 
                "Comparing myChart", 
                this.UserId, 
                this.Upi, 
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.myChart.ToString(), 
                newValue);
        }

        private void UserPatientCareInfo_IsSeeingNewPatients(UserPatientCareInfo newServiceInfo)
        {
            string newValue = string.Empty;

            if (newServiceInfo.PatientCare != null && newServiceInfo.PatientCare.IsSeeingPatients != null)
            {
                if (newServiceInfo.PatientCare.IsSeeingPatients)
                {
                    newValue = "Yes";
                }
                else
                {
                    newValue = "No";
                }
            }
            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_IsSeeingNewPatients, 
                "Comparing Seeing New Patients", 
                this.UserId, 
                this.Upi, 
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.newPatients.ToString(), 
                newValue);
        }

        private void UserPatientCareInfo_IsSeeingPatientType(UserPatientCareInfo newServiceInfo)
        {
            var oldValuesMerged = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.patientsGroups.ToString()));
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

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_IsSeeingPatientType, 
                "Comparing Patient Type(s)", 
                this.UserId, 
                this.Upi, 
                oldValues, 
                newValues);
        }

        private void UserPatientCareInfo_AcceptedReferral(UserPatientCareInfo newServiceInfo)
        {
            string value = string.Empty;

            if (newServiceInfo.PatientCare != null && !string.IsNullOrEmpty(newServiceInfo.PatientCare.AcceptedReferral))
            {
                value = newServiceInfo.PatientCare.AcceptedReferral;
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_AcceptedReferral, 
                "Comparing AcceptedReferral", 
                this.UserId, 
                this.Upi, 
                this.OldDataNodes,
                EnumOldServiceFieldsAsKeys.acceptReferrals.ToString(), 
                value);
        }

        private void UserEducationTrainingInfo_BoardCertifications(UserPatientCareInfo newServiceInfo)
        {
            var oldValues = ParsingHelper.ParseListSimpleValuesStructure(
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.boardCertification.ToString(), 
                new EnumOldServiceFieldsAsKeys[] { 
                    EnumOldServiceFieldsAsKeys.specialty, 
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

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_BoardCertifications, "Comparing Board Certification(s)", this.UserId, this.Upi, oldValues, newValues);
        }

        private void UserEducationTrainingInfo_CancersTreated(UserPatientCareInfo newServiceInfo)
        {
            var oldValuesMerged = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.cancersTreated.ToString()));
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

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_CancersTreated, "Comparing Cancer(s) Treated", this.UserId, this.Upi, oldValues, newValues);
        }
    }
}