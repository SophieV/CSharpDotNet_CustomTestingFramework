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
            UserGeneralInfo_LanguageUsers_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_AltFirstName_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_AltMiddleName_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_AltLastName_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_AltSuffix_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_SuffixNames_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_CountCVs_Test(newUserGeneralInfo, oldServiceData);

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

        private void UserGeneralInfo_AltLastName_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_AltLastName_Test", "Comparing AltLastName");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/lastname");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.AltLastName, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserGeneralInfo_AltFirstName_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_AltFirstName_Test", "Comparing AltFirstName");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/firstname");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.AltFirstName, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserGeneralInfo_AltSuffix_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_AltSuffix_Test", "Comparing AltSuffix");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/Suffix");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.AltSuffix, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserGeneralInfo_AltMiddleName_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_AltMiddleName_Test", "Comparing AltMiddleName");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/middle");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.AltMiddleName, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserGeneralInfo_Titles_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();

            List<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/title", "titleName");

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

        private void UserGeneralInfo_CountCVs_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();

            List<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/cv", "fileName");
            string oldValue = oldValues.Count().ToString();

            string newValue = newServiceData.CVs.Count().ToString();

            var resultReport = new ResultReport("UserGeneralInfo_CountCVs_Test", "Count CVs listed");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newValue, resultReport);
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

        private void UserGeneralInfo_SuffixNames_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();

            string oldValuePart1 = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/degree");
            string oldValuePart2 = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/professionalSuffix");

            List<string> oldValues = new List<string>() { oldValuePart1 };
            var valuesPart2 = oldValuePart2.Split(',');
            foreach(string value in valuesPart2)
            {
                oldValues.Add(value.Trim());
            }

            List<string> newValues = new List<string>();
            if (newServiceData.Titles.Count() > 0)
            {
                foreach (var suffix in newServiceData.Suffixes)
                {
                    newValues.Add(suffix.SuffixName);
                }
            }

            var resultReport = new ResultReport("UserGeneralInfo_SuffixNames_Test", "Comparing SuffixNames");
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

        private void UserGeneralInfo_LanguageUsers_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();

            List<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/language", "languageName");

            List<string> newValues = new List<string>();
            if (newServiceData.LanguageUsers.Count() > 0)
            {
                foreach (var language in newServiceData.LanguageUsers)
                {
                    newValues.Add(language.LanguageName);
                }
            }

            var resultReport = new ResultReport("UserGeneralInfo_LanguageUsers_Test", "Comparing LanguageUser(s)");
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