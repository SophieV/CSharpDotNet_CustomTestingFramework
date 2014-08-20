using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class OrganizationTest : TestUnit
    {
        private IEnumerable<Organization> newServiceOrganizations = new List<Organization>();
        private IEnumerable<XElement> oldServiceOrganizations;

        private int userId;
        private int upi;

        public override string newServiceURLExtensionBeginning
        {
            get { return Parent.newServiceURLExtensionBeginning; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return Parent.newServiceURLExtensionEnding; }
        }

        public OrganizationTest(TestSuite parent, TestUnit bigBrother) 
            : base (parent,bigBrother)
        {

        }

        public void ProvideOrganizationData(int userId, int upi, IEnumerable<XElement> oldServiceOrganizations,IEnumerable<Organization> newServiceOrganizations)
        {
            this.userId = userId;
            this.upi = upi;

            if (newServiceOrganizations != null)
            {
                this.newServiceOrganizations = newServiceOrganizations;
            }

            if(oldServiceOrganizations != null)
            {
                this.oldServiceOrganizations = oldServiceOrganizations;
            }
        }

        public override void RunAllTests()
        {
            List<string> oldOrganizationIdValues;
            List<string> oldOrganizationNameValues;
            ParseOldServiceData(out oldOrganizationIdValues, out oldOrganizationNameValues);

            List<string> newOrganizationIdValues;
            List<string> newOrganizationNameValues;
            ParseNewServiceData(out newOrganizationIdValues, out newOrganizationNameValues);

            UserGeneralInfo_Organization_Id_Test(oldOrganizationIdValues, newOrganizationIdValues);
            UserGeneralInfo_Organization_Name_Test(oldOrganizationNameValues, newOrganizationNameValues);

            ComputeOverallSeverity();
        }

        private void ParseNewServiceData(out List<string> newOrganizationIdValues, out List<string> newOrganizationNameValues)
        {
            newOrganizationIdValues = new List<string>();
            newOrganizationNameValues = new List<string>();

            if (this.newServiceOrganizations.Count() > 0)
            {
                foreach (var organization in this.newServiceOrganizations)
                {
                    newOrganizationIdValues.Add(organization.OrganizationId.ToString());
                    newOrganizationNameValues.Add(organization.Name);
                }
            }
        }

        private void ParseOldServiceData(out List<string> oldOrganizationIdValues, out List<string> oldOrganizationNameValues)
        {
            oldOrganizationIdValues = new List<string>();
            oldOrganizationNameValues = new List<string>();
            try
            {
                //oldServiceXMLContent.XPathSelectElements("/Faculty/facultyMember/department");

                foreach (XElement el in oldServiceOrganizations)
                {
                    try
                    {
                        oldOrganizationIdValues.Add(el.Element("OrgID").Value);
                    }
                    catch (Exception)
                    {
                        // no value to parse
                    }

                    try
                    {
                        oldOrganizationNameValues.Add(el.Element("departmentName").Value);
                    }
                    catch (Exception)
                    {
                        // no value to parse
                    }
                }
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }
        }

        private void UserGeneralInfo_Organization_Id_Test(List<string> oldValues, List<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_Organization_Id_Test", "Comparing Organization Ids");
            var compareStrategy = new SimpleCollectionCompareStrategy(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(userId),
                                              resultReport);
        }

        private void UserGeneralInfo_Organization_Name_Test(List<string> oldValues, List<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_Organization_Name_Test", "Comparing Organization Names");
            var compareStrategy = new SimpleCollectionCompareStrategy(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(userId),
                                              resultReport);
        }
    }
}