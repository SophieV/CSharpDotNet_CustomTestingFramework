using System;
using System.Collections.Generic;
using System.Web;
using YSM.PMS.Web.Service.DataTransfer.Models;

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
            UserPatientCareInfo_ContainsData();
            UserPatientCareInfo_PhysicianBio();
            UserPatientCareInfo_AcceptedReferral();
            UserPatientCareInfo_MyChart();
            UserPatientCareInfo_IsAcceptingNewPatients();
            UserPatientCareInfo_IsSeeingPatientType();
            UserEducationTrainingInfo_BoardCertifications();
            UserEducationTrainingInfo_CancersTreated();
            UserPatientCareInfo_StaywellsDepecatedKeywords();
        }

        private void UserPatientCareInfo_PhysicianBio()
        {
            string newValue = string.Empty;

            if (this.newDataPatientCare != null && this.newDataPatientCare.PhysicianBio != null)
            {
                newValue = HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(this.newDataPatientCare.PhysicianBio));
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_PhysicianBio, 
                "Comparing Physician Bio", 
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.physicianBio.ToString(), 
                newValue);
        }

        private void UserPatientCareInfo_StaywellsDepecatedKeywords()
        {
            string newValue = string.Empty;

            if (this.newDataPatientCare != null && this.newDataPatientCare.DeprecatedKeywordList != null)
            {
                newValue = HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(this.newDataPatientCare.DeprecatedKeywordList));
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_StaywellsDeprecatedKeywords,
                "Comparing Staywells deprecated keywords",
                this.OldDataNodes,
                EnumOldServiceFieldsAsKeys.clinicalInterests.ToString(),
                newValue);
        }

        private void UserPatientCareInfo_MyChart()
        {
            string newValue = string.Empty;

            // convert to values returned by the old service
            if (this.newDataPatientCare != null)
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

        private void UserPatientCareInfo_ContainsData()
        {
            var oldValuePopulated = !string.IsNullOrWhiteSpace(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.newPatients.ToString()));
            var oldValuePopulated2 = !string.IsNullOrWhiteSpace(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.patientsGroups.ToString()));
            var oldValuePopulated3 = !string.IsNullOrWhiteSpace(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.cancersTreated.ToString()));

            var oldPatientCarePopulated = oldValuePopulated || oldValuePopulated2 || oldValuePopulated3;

            var newPatientCarePopulated = (this.newDataPatientCare != null);

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_IsPopulated,
                "Comparing PatientCare data populated",
                oldPatientCarePopulated.ToString(),
                newPatientCarePopulated.ToString());
        }

        private void UserPatientCareInfo_IsAcceptingNewPatients()
        {
            var oldValue = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.newPatients.ToString()));

            if (!string.IsNullOrEmpty(oldValue))
            {
                if (oldValue.ToUpper() == "NA")
                {
                    // See ticket #568
                    oldValue = "No";
                }
                else if (oldValue.ToUpper() == "ICO")
                {
                    // ticket #612 - InPatients Only - restricted yes
                    oldValue = "Yes";
                }

            }

            var newValue = string.Empty;

            if (this.newDataPatientCare != null && this.newDataPatientCare.AcceptingNewPatientsStatus != null)
            {
                newValue = this.newDataPatientCare.AcceptingNewPatientsStatus.Name;
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserPatientCareInfo_IsAcceptingNewPatients, 
                "Comparing Accepting New Patients", 
                oldValue,
                newValue);
        }

        private void UserPatientCareInfo_IsSeeingPatientType()
        {
            var oldValuesMerged = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.patientsGroups.ToString()));
            var oldValues = ParsingHelper.StringToList(oldValuesMerged, ',');

            var newValues = new HashSet<string>();

            if (this.newDataPatientCare != null)
            {
                if (this.newDataPatientCare.IsSeeingAdults)
                {
                    newValues.Add("Adult");
                }
            }

            if (this.newDataPatientCare != null)
            {
                if (this.newDataPatientCare.IsSeeingChildren)
                {
                    newValues.Add("Child");
                }
            }

            if (this.newDataPatientCare != null)
            {
                if (this.newDataPatientCare.IsSeeingAdolescents)
                {
                    newValues.Add("Adolescent");
                }
            }

            if (this.newDataPatientCare != null)
            {
                if (this.newDataPatientCare.IsSeeingGeriatrics)
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

        /// <summary>
        /// Case where the data from the new service is converted into values returned by the old service.
        /// </summary>
        private void UserPatientCareInfo_AcceptedReferral()
        {
            string value = string.Empty;

            if (this.newDataPatientCare != null && this.newDataPatientCare.AcceptedReferral != null && !string.IsNullOrEmpty(this.newDataPatientCare.AcceptedReferral.Option))
            {
                switch (this.newDataPatientCare.AcceptedReferral.Option)
                {
                    case "From patients or physicians":
                        value = "Accepts referrals from patients";
                        break;
                    case "From physicians only":
                        value = "Requires referral from a physician";
                        break;
                    case "Not Applicable":
                        value = "NA";
                        break;
                    case "MD to MD Consult":
                        value = "MD";
                        break;
                    default:
                        // make sure the value received is printed
                        value = this.newDataPatientCare.AcceptedReferral.Option;
                        break;
                }
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
            var oldValues = ParsingHelper.ParseStructuredListOfValues(
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.boardCertification.ToString(), 
                new EnumOldServiceFieldsAsKeys[] { 
                    EnumOldServiceFieldsAsKeys.specialty,
                    EnumOldServiceFieldsAsKeys.organization,
                    EnumOldServiceFieldsAsKeys.certificationYear});

            foreach (var oldValue in oldValues)
            {
                if (oldValue[EnumOldServiceFieldsAsKeys.organization] != null && oldValue[EnumOldServiceFieldsAsKeys.organization].ToLower().Trim() == "none")
                {
                    oldValue[EnumOldServiceFieldsAsKeys.organization] = string.Empty;
                }
            }

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
                        properties.Add(EnumOldServiceFieldsAsKeys.specialty, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.SpecialtyName)));
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.specialty, string.Empty);
                    }

                    try
                    {
                        if (!newValue.CertificationOrganization.ToLower().Contains("none"))
                        {
                            properties.Add(EnumOldServiceFieldsAsKeys.organization, HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValue.CertificationOrganization)));
                        }
                        else
                        {
                            properties.Add(EnumOldServiceFieldsAsKeys.organization, string.Empty);
                        }
                    }
                    catch (Exception)
                    {
                        // make sure a value is present for each index
                        properties.Add(EnumOldServiceFieldsAsKeys.organization, string.Empty);
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
            var oldValuesMerged = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.cancersTreated.ToString());
            var oldValues = ParsingHelper.StringToList(oldValuesMerged, ',');

            var newValues = new HashSet<string>();
            if (this.newDataPatientCare != null && this.newDataPatientCare.CancersTreated != null)
            {
                foreach (var newValueEntry in this.newDataPatientCare.CancersTreated)
                {
                    if (!string.IsNullOrEmpty(newValueEntry.Name))
                    {
                        newValues.Add(HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(newValueEntry.Name)));
                    }
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserPatientCareInfo_CancersTreated, "Comparing Cancer(s) Treated", oldValues, newValues);
        }
    }
}