using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class TestUnitUserGeneralInfo : TestUnit
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

        public TestUnitUserGeneralInfo(TestSuite parent) : base(parent)
        {

        }

        protected override void RunAllSingleTests()
        {
            var newUserGeneralInfo = newServiceAccessor.GetUserGeneralInfoById(userId);

            UserGeneralInfo_Bio_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_Titles_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_LanguageUsers_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_AltLastName_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_AltFirstName_Test(newUserGeneralInfo, oldServiceData);
            UserGeneralInfo_AltMiddleName_Test(newUserGeneralInfo, oldServiceData);
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
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.Bio, resultReport);
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
            var resultReport = new ResultReport("UserGeneralInfo_AltLastName_Test", "Comparing AltLastName (if needed)");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/lastname");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.LastName, resultReport);
            compareStrategy.Investigate();

            if(resultReport.Result != ResultSeverityType.SUCCESS)
            {
                resultReport.ResetForReTesting();

                oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/lastname");
                compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.AltLastName, resultReport);
                compareStrategy.Investigate();
            }

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
            var resultReport = new ResultReport("UserGeneralInfo_AltFirstName_Test", "Comparing AltFirstName (if needed)");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/firstname");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.FirstName, resultReport);
            compareStrategy.Investigate();

            if (resultReport.Result != ResultSeverityType.SUCCESS)
            {
                resultReport.ResetForReTesting();

                oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/firstname");
                compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.AltFirstName, resultReport);
                compareStrategy.Investigate();
            }

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
            var resultReport = new ResultReport("UserGeneralInfo_AltSuffix_Test", "Comparing AltSuffix (if needed)");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/Suffix");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.Suffix, resultReport);
            compareStrategy.Investigate();

            if (resultReport.Result != ResultSeverityType.SUCCESS)
            {
                resultReport.ResetForReTesting();

                oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/Suffix");
                compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.AltSuffix, resultReport);
                compareStrategy.Investigate();
            }

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
            var resultReport = new ResultReport("UserGeneralInfo_AltMiddleName_Test", "Comparing AltMiddleName (if needed)");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/middle");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.MiddleName, resultReport);
            compareStrategy.Investigate();

            if (resultReport.Result != ResultSeverityType.SUCCESS)
            {
                resultReport.ResetForReTesting();

                oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/middle");
                compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.AltMiddleName, resultReport);
                compareStrategy.Investigate();
            }

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

            HashSet<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/title", "titleName");

            HashSet<string> newValues = new HashSet<string>();
            if(newServiceData.Titles.Count() > 0)
            {
                foreach(var title in newServiceData.Titles)
                {
                    if (!string.IsNullOrEmpty(title.TitleName))
                    {
                        newValues.Add(title.TitleName);
                    }
                }
            }

            var resultReport = new ResultReport("UserGeneralInfo_Titles_Test", "Comparing Title(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
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

            HashSet<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/cv", "fileName");
            string oldValue = oldValues.Count().ToString();

            string newValue = newServiceData.CVs.Count().ToString();

            var resultReport = new ResultReport("UserGeneralInfo_CountCVs_Test", "Count CVs listed");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newValue, resultReport);
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

            string entry;
            HashSet<string> oldValues = new HashSet<string>();
            var valuesPart1 = oldValuePart1.Split(',');
            foreach (string value in valuesPart1)
            {
                entry = value.Trim();
                if(!string.IsNullOrEmpty(entry))
                {
                    oldValues.Add(entry);
                };
            }
            var valuesPart2 = oldValuePart2.Split(',');
            foreach(string value in valuesPart2)
            {
                entry = value.Trim();
                if (!string.IsNullOrEmpty(entry))
                {
                   oldValues.Add(entry);
                };
            }

            HashSet<string> newValues = new HashSet<string>();
            if (newServiceData.Titles.Count() > 0)
            {
                foreach (var suffix in newServiceData.Suffixes)
                {
                    newValues.Add(suffix.SuffixName);
                }
            }

            var resultReport = new ResultReport("UserGeneralInfo_SuffixNames_Test", "Comparing SuffixNames");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
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

            HashSet<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/language", "languageName");

            HashSet<string> newValues = new HashSet<string>();
            if (newServiceData.LanguageUsers.Count() > 0)
            {
                foreach (var language in newServiceData.LanguageUsers)
                {
                    newValues.Add(language.LanguageName);
                }
            }

            var resultReport = new ResultReport("UserGeneralInfo_LanguageUsers_Test", "Comparing LanguageUser(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
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
            IEnumerable<XElement> departments;
            IEnumerable<XElement> departmentTree;

            var organizationTest = new TestUnitUserOrganization(this.Master, this);
            this.Children.Add(organizationTest);

            try
            {
                departments = oldServiceData.XPathSelectElements("/Faculty/facultyMember/department");
            }
            catch (Exception)
            {
                departments = new List<XElement>();
            }

            try
            {
                departmentTree = oldServiceData.XPathSelectElements("/Faculty/facultyMember/treeDepartments");
            }
            catch (Exception)
            {
                departmentTree = new List<XElement>();
            }

            organizationTest.ProvideOrganizationData(userId, 
                                                     upi, 
                                                     departments,
                                                     departmentTree, 
                                                     newServiceData.Organizations);
            organizationTest.RunAllTests();
        }

        #endregion
    }
}