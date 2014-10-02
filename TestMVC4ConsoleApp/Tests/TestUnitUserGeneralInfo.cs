using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserGeneralInfo : TestUnit
    {
        private UserCompleteInfo newData;

        public TestUnitUserGeneralInfo(TestSuite parent, UserCompleteInfo newData) : base(parent)
        {
            this.newData = newData;
        }

        protected override void RunAllSingleTests()
        {
            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Bio, "Comparing Bio", this.OldDataNodes, EnumOldServiceFieldsAsKeys.biography.ToString(), this.newData.Bio);
            UserGeneralInfo_Titles_Test();
            UserGeneralInfo_LanguageUsers_Test();
            UserGeneralInfo_AltLastName_Test();
            UserGeneralInfo_AltFirstName_Test();
            UserGeneralInfo_AltMiddleName_Test();
            UserGeneralInfo_AltSuffixName_Test();
            UserGeneralInfo_AltMiddleNameDisplayed_Test();
            UserGeneralInfo_All_EduProfSuffixes();
            UserGeneralInfo_CountCVs_Test();

            ComputeOverallSeverity();
        }

        private void UserGeneralInfo_AltLastName_Test()
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_AltLastName, "Comparing AltLastName (if needed)");

            string oldValue = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.lastname.ToString());
            var compareStrategy = new CompareStrategyFactory(oldValue, newData.LastName, resultReport);
            compareStrategy.Investigate();

            if(resultReport.Severity != EnumResultSeverityType.SUCCESS)
            {
                resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_AltLastName, "Comparing AltLastName (if needed)");

                oldValue = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.lastname.ToString());
                compareStrategy = new CompareStrategyFactory(oldValue, newData.AltLastName, resultReport);
                compareStrategy.Investigate();
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserGeneralInfo_AltFirstName_Test()
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_AltFirstName, "Comparing AltFirstName (if needed)");

            string oldValue = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.firstname.ToString());
            var compareStrategy = new CompareStrategyFactory(oldValue, newData.FirstName, resultReport);
            compareStrategy.Investigate();

            if (resultReport.Severity != EnumResultSeverityType.SUCCESS)
            {
                resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_AltFirstName, "Comparing AltFirstName (if needed)");

                oldValue = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.firstname.ToString());
                compareStrategy = new CompareStrategyFactory(oldValue, newData.AltFirstName, resultReport);
                compareStrategy.Investigate();
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserGeneralInfo_AltMiddleNameDisplayed_Test()
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_AltMiddleNameDisplayed, "Comparing AltMiddleNameDisplayed");

            string oldValue = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.middle.ToString());
            var compareStrategy = new CompareStrategyFactory((!string.IsNullOrEmpty(oldValue)).ToString(), newData.IsAltMiddleNameDisplayed.ToString(), resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserGeneralInfo_AltSuffixName_Test()
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_AltSuffixName, "Comparing Alt SuffixName (if needed)");

            string oldValue = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.suffix.ToString());
            var compareStrategy = new CompareStrategyFactory(oldValue, newData.Suffix, resultReport);
            compareStrategy.Investigate();

            if (resultReport.Severity != EnumResultSeverityType.SUCCESS)
            {
                resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_AltSuffixName, "Comparing Alt SuffixName (if needed)");

                oldValue = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.suffix.ToString());
                compareStrategy = new CompareStrategyFactory(oldValue, newData.AltSuffix, resultReport);
                compareStrategy.Investigate();
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserGeneralInfo_AltMiddleName_Test()
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_AltMiddleName, "Comparing AltMiddleName (if needed)");

            string oldValue = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.middle.ToString());
            if (string.IsNullOrEmpty(oldValue))
            {
                oldValue = "False";
            }
            var compareStrategy = new CompareStrategyFactory(oldValue, newData.MiddleName, resultReport);
            compareStrategy.Investigate();

            if (resultReport.Severity != EnumResultSeverityType.SUCCESS)
            {
                resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_AltMiddleName, "Comparing AltMiddleName (if needed)");

                oldValue = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.middle.ToString());
                compareStrategy = new CompareStrategyFactory(oldValue, newData.AltMiddleName, resultReport);
                compareStrategy.Investigate();
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserGeneralInfo_Titles_Test()
        {
            HashSet<string> oldValues = ParsingHelper.ParseUnstructuredListOfValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.title.ToString(), EnumOldServiceFieldsAsKeys.titleName.ToString());

            HashSet<string> newValues = new HashSet<string>();
            if(newData.Titles.Count() > 0)
            {
                foreach(var title in newData.Titles)
                {
                    if (!string.IsNullOrEmpty(title.Name))
                    {
                        newValues.Add(title.Name);
                    }
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Titles, "Comparing Title(s)", oldValues, newValues);
        }

        private void UserGeneralInfo_CountCVs_Test()
        {
            HashSet<string> oldValues = ParsingHelper.ParseUnstructuredListOfValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.cv.ToString(), EnumOldServiceFieldsAsKeys.fileName.ToString());
            string oldValue = oldValues.Count().ToString();

            string newValue = newData.CVs.Count().ToString();

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_CVs_Count, "Count CVs listed", oldValue, newValue);
        }

        private void UserGeneralInfo_All_EduProfSuffixes()
        {
            string oldValuePart1 = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.degree.ToString());
            string oldValuePart2 = ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.professionalSuffix.ToString());

            HashSet<string> oldValues = ParsingHelper.StringToList(oldValuePart1, ',');
            oldValues = ParsingHelper.StringToList(oldValuePart2, ',', oldValues);
            
            HashSet<string> newValues = new HashSet<string>();
            if (newData.Titles.Count() > 0)
            {
                foreach (var suffix in newData.Suffixes)
                {
                    newValues.Add(suffix.Name);
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_All_EduProfSuffixes, "Comparing list of Suffix(es)", oldValues, newValues);
        }

        private void UserGeneralInfo_LanguageUsers_Test()
        {
            var oldValues = ParsingHelper.ParseUnstructuredListOfValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.language.ToString(), EnumOldServiceFieldsAsKeys.languageName.ToString());

            var newValues = new HashSet<string>();
            if (newData.LanguageUsers.Count() > 0)
            {
                foreach (var language in newData.LanguageUsers)
                {
                    newValues.Add(language.LanguageName);
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Languages, "Comparing LanguageUser(s)", oldValues, newValues);
        }

    }
}