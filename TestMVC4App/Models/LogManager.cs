using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Web.UI;
using TestMVC4App.Templates;

namespace TestMVC4App.Models
{
    public sealed class LogManager
    {
        private const string SUMMARY_BY_PROFILE_FILENAME = "QA_Reporting_Summary_User_PerProfile.html";
        private const string SUMMARY_FILENAME = "QA_Reporting_Summary_User_MAIN.html";

        private static volatile LogManager instance;
        private static object syncRoot = new Object();

        private static object lockLogResult = new object();
        private static object lockProfileOverview = new object();

        private List<string> allTestNames;
        private StreamWriter streamWriter;
        private Dictionary<string, HtmlTextWriter> htmlWritersForDetailedReports_ByTestName;
        private HtmlTextWriter htmlWriterForSummaryReport;
        private HtmlTextWriter htmlWriterForProfileReport;

        private static Dictionary<string, Dictionary<ResultSeverityType, int>> countSeverityTypes_ByTestName;
        private static Dictionary<string, Dictionary<IdentifiedDataBehavior, int>> countIdentifiedDataBehaviors_ByTestName;

        private static Dictionary<string, List<TimeSpan>> duration_ByTestName;
        private static List<TimeSpan> durationByProfile;

        private static Dictionary<int, bool> checkNoWarningNorError_ByUPI;

        private static Dictionary<string, string> sampleDataByTestName = new Dictionary<string,string>();

        public int StatsCountProfilesProcessed { get;set; }

        private static int countFilesGenerated = 0;

        public static LogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LogManager();
                    }
                }

                return instance;
            }
        }

        private LogManager() {
            // the order here is quite important because this is implicitely the expectation of the templates
            allTestNames = new List<string>();
            allTestNames.Add("UserBasicInfo_LastName_Test");
            allTestNames.Add("UserBasicInfo_FirstName_Test");
            allTestNames.Add("UserBasicInfo_MiddleName_Test");
            allTestNames.Add("UserBasicInfo_Email_Test");
            allTestNames.Add("UserBasicInfo_NetId_Test");
            allTestNames.Add("UserBasicInfo_PageName_Test");
            allTestNames.Add("UserBasicInfo_Suffix_Test");
            allTestNames.Add("UserBasicInfo_Idx_Test");
            allTestNames.Add("UserBasicInfo_LicenseNumber_Test");
            allTestNames.Add("UserBasicInfo_Npi_Test");
            allTestNames.Add("UserBasicInfo_Gender_Test");
            allTestNames.Add("UserBasicInfo_UPI_Test");

            allTestNames.Add("UserGeneralInfo_Bio_Test");
            allTestNames.Add("UserGeneralInfo_Titles_Test");
            allTestNames.Add("UserGeneralInfo_LanguageUsers_Test");
            allTestNames.Add("UserGeneralInfo_AltFirstName_Test");
            allTestNames.Add("UserGeneralInfo_AltMiddleName_Test");
            allTestNames.Add("UserGeneralInfo_AltLastName_Test");
            allTestNames.Add("UserGeneralInfo_AltSuffix_Test");
            allTestNames.Add("UserGeneralInfo_SuffixNames_Test");
            allTestNames.Add("UserGeneralInfo_CountCVs_Test");
            allTestNames.Add("UserGeneralInfo_Organization_Id_Test");
            allTestNames.Add("UserGeneralInfo_Organization_Name_Test");
            allTestNames.Add("UserGeneralInfo_Organization_CheckTreeDepthCoherence_Test");
            allTestNames.Add("UserGeneralInfo_Organization_CheckIsPrimary_Test");

            checkNoWarningNorError_ByUPI = new Dictionary<int, bool>();
            countSeverityTypes_ByTestName = new Dictionary<string, Dictionary<ResultSeverityType, int>>();
            countIdentifiedDataBehaviors_ByTestName = new Dictionary<string, Dictionary<IdentifiedDataBehavior, int>>();

            duration_ByTestName = new Dictionary<string, List<TimeSpan>>();
            foreach(var name in allTestNames)
            {
                duration_ByTestName.Add(name, new List<TimeSpan>());
            }

            durationByProfile = new List<TimeSpan>();

            htmlWritersForDetailedReports_ByTestName = new Dictionary<string, HtmlTextWriter>();

            StatsCountProfilesProcessed = 0;
        }

        /// <summary>
        /// Populates the template based on the provided data.
        /// Writes the entry to the log.
        /// </summary>
        /// <param name="errorMessage">Description of the failure.</param>
        /// <param name="userId">User Unique Identifier in the new service.</param>
        /// <param name="upi">User Unique Identifier in the old service.</param>
        /// <param name="severity">Type of failure.</param>
        /// <param name="newServiceUrl">URL of the new service being called (for the specific User Identifier).</param>
        /// <param name="memberName">Test method generated name.</param>
        /// <param name="taskDescription">Human-readable description of the test.</param>
        /// <param name="optionalExplanation">Hint on what caused the issue.</param>
        public void LogTestResult(int userId, int upi, string oldUrl,string newServiceUrl, ResultReport resultReport)
        {
            lock (lockLogResult)
            {
                duration_ByTestName[resultReport.TestName].Add(resultReport.Duration);

                var detailedReportData = new SharedDetailedReportData
                {
                    ErrorMessage = resultReport.ErrorMessage,
                    TestName = resultReport.TestName,
                    UserId = userId,
                    UPI = upi,
                    Result = resultReport.Result,
                    OldUrl = oldUrl,
                    NewUrl = newServiceUrl,
                    TestDescription = resultReport.TestDescription,
                    IdentifiedDataBehaviors = resultReport.IdentifedDataBehaviors,
                    OldValues = resultReport.OldValues,
                    NewValues = resultReport.NewValues,
                    Duration = resultReport.Duration
                };

                var template = new TestNameDetailedReport();
                template.Session = new Dictionary<string, object>()
            {
                { "DetailedReportDataObject", detailedReportData }
            };

                template.Initialize();

                if (htmlWritersForDetailedReports_ByTestName.ContainsKey(resultReport.TestName))
                {
                    htmlWritersForDetailedReports_ByTestName[resultReport.TestName].WriteLine(template.TransformText());
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(resultReport.TestName);
                }

                UpdateStatistics(upi, resultReport);
            }
        }

        public void LogProfileResult(int upi, List<ResultReport> allTheResults, TimeSpan duration)
        {
            lock (lockProfileOverview)
            {
                durationByProfile.Add(duration);

                var resultByTestName = allTheResults.Select(x => new { x.TestName, x.Result }).ToDictionary(x => x.TestName, x => x.Result);
                var summaryProfileData = new SharedProfileReportData()
                {
                    UPI = upi,
                    ResultSeverity_ByTestName = resultByTestName,
                    FileLinkEnd = "_" + countFilesGenerated + ".html",
                    Duration = duration
                };
                var template = new ProfileReport();
                template.Session = new Dictionary<string, object>()
            {
                { "ProfileReportDataObject", summaryProfileData }
            };

                template.Initialize();
                htmlWriterForProfileReport.WriteLine(template.TransformText());
            }
        }

        private static void UpdateStatistics(int upi, ResultReport resultReport)
        {
            // keeping track of profiles without failures by logging any failure happening
            if (!checkNoWarningNorError_ByUPI.ContainsKey(upi))
            {
                checkNoWarningNorError_ByUPI.Add(upi, false);
            }

            if (resultReport.Result != ResultSeverityType.SUCCESS)
            {
                checkNoWarningNorError_ByUPI[upi] = true;
            }

            if (!countSeverityTypes_ByTestName.ContainsKey(resultReport.TestName))
            {
                // initialize all the possible combinations for the given test name
                countSeverityTypes_ByTestName.Add(resultReport.TestName, new Dictionary<ResultSeverityType, int>());
                countSeverityTypes_ByTestName[resultReport.TestName].Add(ResultSeverityType.ERROR, 0);
                countSeverityTypes_ByTestName[resultReport.TestName].Add(ResultSeverityType.ERROR_WITH_EXPLANATION, 0);
                countSeverityTypes_ByTestName[resultReport.TestName].Add(ResultSeverityType.FALSE_POSITIVE, 0);
                countSeverityTypes_ByTestName[resultReport.TestName].Add(ResultSeverityType.WARNING, 0);
                countSeverityTypes_ByTestName[resultReport.TestName].Add(ResultSeverityType.SUCCESS, 0);
            }

            // increase call counter
            countSeverityTypes_ByTestName[resultReport.TestName][resultReport.Result]++;

            if (!countIdentifiedDataBehaviors_ByTestName.ContainsKey(resultReport.TestName))
            {
                // initialize all the possible combinations for the given test name
                countIdentifiedDataBehaviors_ByTestName.Add(resultReport.TestName, new Dictionary<IdentifiedDataBehavior, int>());
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.DUPLICATED_VALUES_ON_NEW_SERVICE, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.MORE_VALUES_ON_OLD_SERVICE_ALL_DUPLICATES, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.MORE_VALUES_ON_NEW_SERVICE, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.MISMATCH_DUE_TO_TRAILING_WHITE_SPACES, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.VALUE_POPULATED_WITH_WHITE_SPACE_ON_NEW_SERVICE, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.VALUES_NOT_POPULATED, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.MISSING_VALUES_ON_NEW_SERVICE, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.WRONG_VALUE, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.OLD_TREE_HAS_MORE_CHILDREN, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.OLD_TREE_NON_EXHAUSTIVE, 0);
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName].Add(IdentifiedDataBehavior.NEW_TREE_COUNT_CONSISTENT, 0);
            }

            // increase call counter
            foreach (IdentifiedDataBehavior label in resultReport.IdentifedDataBehaviors)
            {
                countIdentifiedDataBehaviors_ByTestName[resultReport.TestName][label]++;
            }
        }

        #region Set Up Output Files

        public void StartWritingDetailedReports()
        {
            try
            {
                string filePath;

                StopWritingDetailedReports();

                htmlWritersForDetailedReports_ByTestName = new Dictionary<string, HtmlTextWriter>();
                countFilesGenerated++;

                foreach (string testName in allTestNames)
                {
                    filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + testName + "_" + countFilesGenerated + ".html");
                    System.Diagnostics.Debug.WriteLine(filePath);
                    streamWriter = new StreamWriter(filePath);
                    htmlWritersForDetailedReports_ByTestName.Add(testName, new HtmlTextWriter(streamWriter));
                }

                TestNameDetailedReport_Header headerTemplate = null;

                foreach (HtmlTextWriter htmlWriter in htmlWritersForDetailedReports_ByTestName.Values)
                {
                    // if the header template is created out of the loop, its content duplicates itself...
                    headerTemplate = new TestNameDetailedReport_Header();
                    htmlWriter.WriteLine(headerTemplate.TransformText());
                }

                filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + SUMMARY_BY_PROFILE_FILENAME);
                System.Diagnostics.Debug.WriteLine(filePath);

                streamWriter = new StreamWriter(filePath);
                htmlWriterForProfileReport = new HtmlTextWriter(streamWriter);

                var sharedHeaderProfileData = new SharedHeaderProfileReportData() { AllTestNames = allTestNames };

                var headerProfileTemplate = new ProfileReport_Header();
                headerProfileTemplate.Session = new Dictionary<string, object>()
                    {
                        { "ProfileHeaderReportDataObject", sharedHeaderProfileData }
                    };

                headerProfileTemplate.Initialize();
                htmlWriterForProfileReport.WriteLine(headerProfileTemplate.TransformText());

            }
            catch (IOException ioe)
            {
                System.Diagnostics.Debug.WriteLine(ioe.StackTrace);
            }
        }

        public void StopWritingDetailedReports()
        {
            try
            {
                if (htmlWritersForDetailedReports_ByTestName.Count > 0)
                {
                    // finish the HTML syntax and cleanup resource
                    TestNameDetailedReport_Footer jsTemplate = null;

                    foreach (HtmlTextWriter htmlTestWriter in htmlWritersForDetailedReports_ByTestName.Values)
                    {
                        // if the template is created out of the loop, its content duplicates itself...
                        jsTemplate = new TestNameDetailedReport_Footer();
                        htmlTestWriter.WriteLine(jsTemplate.TransformText());
                        htmlTestWriter.Close();
                    }
                }

                if (htmlWriterForProfileReport != null)
                {
                    var footerTemplate = new ProfileReport_Footer();
                    htmlWriterForProfileReport.WriteLine(footerTemplate.TransformText());
                    htmlWriterForProfileReport.Close();
                    htmlWriterForProfileReport = null;
                }
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        public void WriteSummaryReport(TimeSpan duration, string errorHappened, string errorMessage)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + SUMMARY_FILENAME);
            System.Diagnostics.Debug.WriteLine(filePath);
            
            streamWriter = new StreamWriter(filePath);
            htmlWriterForSummaryReport = new HtmlTextWriter(streamWriter);

            int countProfileNoWarningNorError = 0;
            foreach (var entry in checkNoWarningNorError_ByUPI)
            {
                if (entry.Value == false)
                {
                    countProfileNoWarningNorError++;
                }
            }

            var countBySeverity = new Dictionary<ResultSeverityType, int>();
            foreach (KeyValuePair<string, Dictionary<ResultSeverityType, int>> entry in countSeverityTypes_ByTestName)
            {
                foreach (KeyValuePair<ResultSeverityType, int> subEntry in entry.Value)
                {
                    if (!countBySeverity.ContainsKey(subEntry.Key))
                    {
                        countBySeverity.Add(subEntry.Key, 0);
                    }
                    countBySeverity[subEntry.Key] += subEntry.Value;
                }
            }

            var countByIdentifiedDataBehavior = new Dictionary<IdentifiedDataBehavior, int>();
            foreach (var entry in countIdentifiedDataBehaviors_ByTestName)
            {
                foreach (var subEntry in entry.Value)
                {
                    if (!countByIdentifiedDataBehavior.ContainsKey(subEntry.Key))
                    {
                        countByIdentifiedDataBehavior.Add(subEntry.Key, 0);
                    }
                    countByIdentifiedDataBehavior[subEntry.Key] += subEntry.Value;

                }
            }

            TimeSpan averageDurationPerProfile = TimeSpan.Zero;
            Dictionary<string, TimeSpan> averageDuration_ByTestName = new Dictionary<string, TimeSpan>();

            if (StatsCountProfilesProcessed > 0)
            {
                TimeSpan.FromMilliseconds(durationByProfile.Average(t => t.TotalMilliseconds) / StatsCountProfilesProcessed);

                foreach(var testNameEntry in duration_ByTestName)
                {
                    if (testNameEntry.Value.Count() > 0)
                    {
                        averageDuration_ByTestName.Add(testNameEntry.Key, TimeSpan.FromMilliseconds(testNameEntry.Value.Average(t => t.TotalMilliseconds) / StatsCountProfilesProcessed));
                    }
                }
            }

            var summaryReportData = new SharedSummaryReportData
            {
                CountProfilesTested = StatsCountProfilesProcessed,
                CountProfilesWithoutWarnings = countProfileNoWarningNorError,
                CountBySeverity = countBySeverity,
                CountByIdentifiedDataBehavior = countByIdentifiedDataBehavior.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value),
                TestNames = countSeverityTypes_ByTestName.Keys.ToList(),
                CountBySeverity_ByTestName = countSeverityTypes_ByTestName,
                CountByIdentifiedDataBehavior_ByTestName = countIdentifiedDataBehaviors_ByTestName,
                CountTestsRun = StatsCountProfilesProcessed * countSeverityTypes_ByTestName.Keys.ToList().Count(),
                CountTestsPerUser = countSeverityTypes_ByTestName.Keys.ToList().Count(),
                SampleData_ByTestName = sampleDataByTestName,
                Duration = duration,
                FileLinkEnd = "_" + countFilesGenerated + ".html",
                AverageDurationPerProfile = averageDurationPerProfile,
                AverageDuration_ByTestName = averageDuration_ByTestName,
                ErrorHappened = errorHappened,
                ErrorMessage = errorMessage,
                FileByProfileLink = SUMMARY_BY_PROFILE_FILENAME
            };

            var template = new SummaryReport();
            template.Session = new Dictionary<string, object>()
                    {
                        { "SummaryReportDataObject", summaryReportData }
                    };

            template.Initialize();
            htmlWriterForSummaryReport.WriteLine(template.TransformText());
            htmlWriterForSummaryReport.Close();
            htmlWriterForSummaryReport = null;
        }

        public void CleanUpResources()
        {
            if (htmlWritersForDetailedReports_ByTestName.Count > 0)
            {
                foreach (HtmlTextWriter htmlTestWriter in htmlWritersForDetailedReports_ByTestName.Values)
                {
                    htmlTestWriter.Close();
                }
            }

            htmlWritersForDetailedReports_ByTestName = null;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Allows to display the string text associated with an enum entry.
        /// </summary>
        /// <param name="value">Enum type from which we want the description.</param>
        /// <returns>Description text.</returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        #endregion

    }
}