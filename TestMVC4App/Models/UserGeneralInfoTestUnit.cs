using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using TestMVC4App.Templates;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class UserGeneralInfoTestUnit : TestUnit
    {
        private UsersClient newServiceAccessor;
        private XDocument oldServiceData;
        private int userId;
        private int upi;

        public override string newServiceURLExtensionBeginning
        {
            get { return "Users/"; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return "/GeneralInfo"; }
        }

        public UserGeneralInfoTestUnit(TestSuite parent) : base(parent)
        {

        }

        public override void RunAllTests()
        {
            var newUserGeneralInfo = newServiceAccessor.GetUserGeneralInfoById(userId);

            UserGeneralInfo_Bio_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_Titles_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_Organizations_Test(newUserGeneralInfo, oldServiceData);

            ComputeOverallSeverity();
        }

        public void ProvideUserData(XDocument oldData, int upi, UsersClient newDataAccessor, int userId)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.userId = userId;
            this.upi = upi;
        }

        #region Field Comparison Tests

        private void UserGeneralInfo_Bio_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();

            var resultReport = new ResultReport("UserGeneralInfo_Bio_Test", "Comparing Bio");
            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/biography");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.Bio, resultReport);
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

        private void UserGeneralInfo_Titles_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();

            List<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/title");

            List<string> newValues = new List<string>();
            if(newServiceData.Titles.Count() > 0)
            {
                foreach(var title in newServiceData.Titles)
                {
                    newValues.Add(title.TitleName);
                }
            }

            var resultReport = new ResultReport("UserGeneralInfo_Titles_Test", "Comparing Title(s)");
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

        private void UserGeneralInfo_Organizations_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var organizationTest = new OrganizationTestUnit(this.Master, this);
            this.Children.Add(organizationTest);
            organizationTest.ProvideOrganizationData(userId, upi, oldServiceData.XPathSelectElements("/Faculty/facultyMember/department"), newServiceData.Organizations);
            organizationTest.RunAllTests();
        }

        #endregion
    }
}