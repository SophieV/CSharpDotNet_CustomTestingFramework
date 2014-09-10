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
        private IEnumerable<XElement> oldServiceData;
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
            var newServiceInfo = newServiceAccessor.GetUserGeneralInfoById(userId);

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Bio, "Comparing Bio", this.userId, this.upi, oldServiceData, "biography", newServiceInfo.Bio);
            UserGeneralInfo_Titles_Test(newServiceInfo, oldServiceData);
            UserGeneralInfo_LanguageUsers_Test(newServiceInfo, oldServiceData);
            UserGeneralInfo_AltLastName_Test(newServiceInfo, oldServiceData);
            UserGeneralInfo_AltFirstName_Test(newServiceInfo, oldServiceData);
            UserGeneralInfo_AltMiddleName_Test(newServiceInfo, oldServiceData);
            UserGeneralInfo_AltSuffix_Test(newServiceInfo, oldServiceData);
            UserGeneralInfo_AltMiddleNameDisplayed_Test(newServiceInfo, oldServiceData);
            UserGeneralInfo_SuffixNames_Test(newServiceInfo, oldServiceData);
            UserGeneralInfo_CountCVs_Test(newServiceInfo, oldServiceData);
            UserGeneralInfo_Organizations_Test(newServiceInfo, oldServiceData);

            ComputeOverallSeverity();
        }

        public void ProvideData(IEnumerable<XElement> oldData, int upi, UsersClient newDataAccessor, int userId)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.userId = userId;
            this.upi = upi;
        }

        #region Field Comparison Tests

        private void UserGeneralInfo_AltLastName_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(EnumTestUnitNames.UserGeneralInfo_AltLastName, "Comparing AltLastName (if needed)");

            string oldValue = ParsingHelper.ParseSingleValue(oldServiceData, "lastname");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.LastName, resultReport);
            compareStrategy.Investigate();

            if(resultReport.Result != ResultSeverityType.SUCCESS)
            {
                resultReport.ResetForReTesting();

                oldValue = ParsingHelper.ParseSingleValue(oldServiceData, "lastname");
                compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.AltLastName, resultReport);
                compareStrategy.Investigate();
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(upi),
                                              resultReport);
        }

        private void UserGeneralInfo_AltFirstName_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(EnumTestUnitNames.UserGeneralInfo_AltFirstName, "Comparing AltFirstName (if needed)");

            string oldValue = ParsingHelper.ParseSingleValue(oldServiceData, "firstname");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.FirstName, resultReport);
            compareStrategy.Investigate();

            if (resultReport.Result != ResultSeverityType.SUCCESS)
            {
                resultReport.ResetForReTesting();

                oldValue = ParsingHelper.ParseSingleValue(oldServiceData, "firstname");
                compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.AltFirstName, resultReport);
                compareStrategy.Investigate();
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(upi),
                                              resultReport);
        }

        private void UserGeneralInfo_AltMiddleNameDisplayed_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(EnumTestUnitNames.UserGeneralInfo_AltMiddleNameDisplayed, "Comparing AltMiddleNameDisplayed");

            string oldValue = ParsingHelper.ParseSingleValue(oldServiceData, "MiddleName");
            var compareStrategy = new CompareStrategyContextSwitcher(string.IsNullOrEmpty(oldValue).ToString(), newServiceData.IsAltMiddleNameDisplayed.ToString(), resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(upi),
                                              resultReport);
        }

        private void UserGeneralInfo_AltSuffix_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(EnumTestUnitNames.UserGeneralInfo_AltSuffix, "Comparing AltSuffix (if needed)");

            string oldValue = ParsingHelper.ParseSingleValue(oldServiceData, "Suffix");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.Suffix, resultReport);
            compareStrategy.Investigate();

            if (resultReport.Result != ResultSeverityType.SUCCESS)
            {
                resultReport.ResetForReTesting();

                oldValue = ParsingHelper.ParseSingleValue(oldServiceData, "Suffix");
                compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.AltSuffix, resultReport);
                compareStrategy.Investigate();
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(upi),
                                              resultReport);
        }

        private void UserGeneralInfo_AltMiddleName_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(EnumTestUnitNames.UserGeneralInfo_AltMiddleName, "Comparing AltMiddleName (if needed)");

            string oldValue = ParsingHelper.ParseSingleValue(oldServiceData, "middle");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.MiddleName, resultReport);
            compareStrategy.Investigate();

            if (resultReport.Result != ResultSeverityType.SUCCESS)
            {
                resultReport.ResetForReTesting();

                oldValue = ParsingHelper.ParseSingleValue(oldServiceData, "middle");
                compareStrategy = new CompareStrategyContextSwitcher(oldValue, newServiceData.AltMiddleName, resultReport);
                compareStrategy.Investigate();
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(upi),
                                              resultReport);
        }

        private void UserGeneralInfo_Titles_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            HashSet<string> oldValues = ParsingHelper.ParseListSimpleValues(oldServiceData, "title", "titleName");

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

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Titles, "Comparing Title(s)", this.userId, this.upi, oldValues, newValues);
        }

        private void UserGeneralInfo_CountCVs_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            HashSet<string> oldValues = ParsingHelper.ParseListSimpleValues(oldServiceData, "cv", "fileName");
            string oldValue = oldValues.Count().ToString();

            string newValue = newServiceData.CVs.Count().ToString();

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_CVs_Count, "Count CVs listed", this.userId, this.upi, oldValue, newValue);
        }

        private void UserGeneralInfo_SuffixNames_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            string oldValuePart1 = ParsingHelper.ParseSingleValue(oldServiceData, "degree");
            string oldValuePart2 = ParsingHelper.ParseSingleValue(oldServiceData, "professionalSuffix");

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

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Suffixes, "Comparing SuffixNames", this.userId, this.upi, oldValues, newValues);
        }

        private void UserGeneralInfo_LanguageUsers_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            var oldValues = ParsingHelper.ParseListSimpleValues(oldServiceData, "language", "languageName");

            var newValues = new HashSet<string>();
            if (newServiceData.LanguageUsers.Count() > 0)
            {
                foreach (var language in newServiceData.LanguageUsers)
                {
                    newValues.Add(language.LanguageName);
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Languages, "Comparing LanguageUser(s)", this.userId, this.upi, oldValues, newValues);
        }

        private void UserGeneralInfo_Organizations_Test(UserGeneralInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            var departments = ParsingHelper.ParseListNode(oldServiceData,"department");
            var departmentTree = ParsingHelper.ParseListNode(oldServiceData, "treeDepartments");

            var organizationTest = new TestUnitUserOrganization(this.Container, this);
            this.Children.Add(organizationTest);

            organizationTest.ProvideData(userId, 
                                                     upi, 
                                                     departments,
                                                     departmentTree, 
                                                     newServiceData.Organizations);
            organizationTest.RunAllTests();
        }

        #endregion
    }
}