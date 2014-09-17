using System;
using System.Collections.Generic;
using System.Web;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserPatientCareInfo : TestUnit
    {
        private IEnumerable<BoardCertification> newDataBoardCertification;
        private PatientCare newDataPatientCare;

        public TestUnitUserPatientCareInfo(TestSuite parent, IEnumerable<BoardCertification> newDataBoardCertification, PatientCare newDataPatientCare)
            : base(parent)
        {
            this.newDataBoardCertification = newDataBoardCertification;
            this.newDataPatientCare = newDataPatientCare;
        }

        protected override void RunAllSingleTests()
        {
            UserPatientCareInfo_PhysicianBio();
            UserPatientCareInfo_AcceptedReferral();
            UserPatientCareInfo_MyChart();
            UserPatientCareInfo_IsSeeingNewPatients();
            UserPatientCareInfo_IsSeeingPatientType();
            UserEducationTrainingInfo_BoardCertifications();
            UserEducationTrainingInfo_CancersTreated();

        }

        private void UserPatientCareInfo_PhysicianBio()
        {
            string newValue = string.Empty;

            if (this.newDataPatientCare != null && this.newDataPatientCare.PhysicianBio != null)
            {
                newValue = this.newDataPatientCare.PhysicianBio;
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_PhysicianBio, 
                "Comparing Physician Bio", 
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.physicianBio.ToString(), 
                newValue);
        }

        private void UserPatientCareInfo_MyChart()
        {
            string newValue = string.Empty;

            // convert to values returned by the old service
            if (this.newDataPatientCare != null && this.newDataPatientCare.IsMyChartAvailable != null)
            {
                if (this.newDataPatientCare.IsMyChartAvailable)
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
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.myChart.ToString(), 
                newValue);
        }

        private void UserPatientCareInfo_IsSeeingNewPatients()
        {
            string newValue = string.Empty;

            if (this.newDataPatientCare != null && this.newDataPatientCare.IsSeeingPatients != null)
            {
                if (this.newDataPatientCare.IsSeeingPatients)
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
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.newPatients.ToString(), 
                newValue);
        }

        private void UserPatientCareInfo_IsSeeingPatientType()
        {
            var oldValuesMerged = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.patientsGroups.ToString()));
            var oldValues = ParsingHelper.StringToList(oldValuesMerged, ',');

            var newValues = new HashSet<string>();

            if (this.newDataPatientCare != null && this.newDataPatientCare.IsSeeingAdults != null)
            {
                if (this.newDataPatientCare.IsSeeingAdults)
                {
                    newValues.Add("Adult");
                }
            }

            if (this.newDataPatientCare != null && this.newDataPatientCare.IsSeeingChild != null)
            {
                if (this.newDataPatientCare.IsSeeingChild)
                {
                    newValues.Add("Child");
                }
            }

            if (this.newDataPatientCare != null && this.newDataPatientCare.IsSeeingAdolescent != null)
            {
                if (this.newDataPatientCare.IsSeeingAdolescent)
                {
                    newValues.Add("Adolescent");
                }
            }

            if (this.newDataPatientCare != null && this.newDataPatientCare.IsSeeingGeriatric != null)
            {
                if (this.newDataPatientCare.IsSeeingGeriatric)
                {
                    newValues.Add("Geriatric");
                }
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_IsSeeingPatientType, 
                "Comparing Patient Type(s)", 
                oldValues, 
                newValues);
        }

        private void UserPatientCareInfo_AcceptedReferral()
        {
            string value = string.Empty;

            if (this.newDataPatientCare != null && !string.IsNullOrEmpty(this.newDataPatientCare.AcceptedReferral))
            {
                value = this.newDataPatientCare.AcceptedReferral;
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_AcceptedReferral, 
                "Comparing AcceptedReferral", 
                this.OldDataNodes,
                EnumOldServiceFieldsAsKeys.acceptReferrals.ToString(), 
                value);
        }

        private void UserEducationTrainingInfo_BoardCertifications()
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

            if (this.newDataBoardCertification != null)
            {
                foreach (var newValue in this.newDataBoardCertification)
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
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_BoardCertifications, "Comparing Board Certification(s)", oldValues, newValues);
        }

        private void UserEducationTrainingInfo_CancersTreated()
        {
            var oldValuesMerged = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.cancersTreated.ToString()));
            var oldValues = ParsingHelper.StringToList(oldValuesMerged, ',');

            var newValues = new HashSet<string>();
            if (this.newDataPatientCare != null && this.newDataPatientCare.CancersTreated != null)
            {
                foreach (var newValueEntry in this.newDataPatientCare.CancersTreated)
                {
                    if (!string.IsNullOrEmpty(newValueEntry.Name))
                    {
                        newValues.Add(newValueEntry.Name);
                    }
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_CancersTreated, "Comparing Cancer(s) Treated", oldValues, newValues);
        }
    }
}