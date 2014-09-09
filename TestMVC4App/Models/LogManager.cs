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
    public sealed class LogManager : IDisposable
    {
        private readonly bool isDebugMode = true;

        private const string SUMMARY_BY_PROFILE_FILENAME = "QA_Reporting_Summary_User_PerProfile.html";
        private const string SUMMARY_FILENAME = "QA_Reporting_Summary_User_MAIN.html";

        private static volatile LogManager instance;
        private static object syncRoot = new Object();

        private static object lockLogResult = new object();
        private static object lockProfileOverview = new object();

        private SortedSet<TestUnitNames> allTestNames;
        private StreamWriter streamWriter;
        private Dictionary<TestUnitNames, HtmlTextWriter> htmlWritersForDetailedReports_ByTestName;
        private HtmlTextWriter htmlWriterForSummaryReport;
        private HtmlTextWriter htmlWriterForProfileReport;

        private static Dictionary<TestUnitNames, Dictionary<ResultSeverityType, int>> countSeverityResults_ByTestName;
        private static Dictionary<TestUnitNames, Dictionary<IdentifiedDataBehavior, int>> countDataBehaviors_ByTestName;

        private static Dictionary<TestUnitNames, HashSet<TimeSpan>> duration_ByTestName;
        private static HashSet<TimeSpan> durationByProfile;

        private static Dictionary<int, bool> NoWarningNorErrorHappenedFlag_ByUpi;

        private static Dictionary<string, string> sampleDataByTestName = new Dictionary<string,string>();

        public static Dictionary<IdentifiedDataBehavior, string> IdentifiedBehaviorsDescriptions { get; private set; }

        public double StatsCountTotalUpis { get;set; }

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
            allTestNames = new SortedSet<TestUnitNames>();
            foreach (var testName in (TestUnitNames[])Enum.GetValues(typeof(TestUnitNames)))
            {
                allTestNames.Add(testName);
            }

            IdentifiedBehaviorsDescriptions = new Dictionary<IdentifiedDataBehavior, string>();
            foreach (var behavior in (IdentifiedDataBehavior[]) Enum.GetValues(typeof(IdentifiedDataBehavior)))
            {
                IdentifiedBehaviorsDescriptions.Add(behavior, ParsingHelper.GetDescription(behavior));
            }

            NoWarningNorErrorHappenedFlag_ByUpi = new Dictionary<int, bool>();
            countSeverityResults_ByTestName = new Dictionary<TestUnitNames, Dictionary<ResultSeverityType, int>>();
            countDataBehaviors_ByTestName = new Dictionary<TestUnitNames, Dictionary<IdentifiedDataBehavior, int>>();

            duration_ByTestName = new Dictionary<TestUnitNames, HashSet<TimeSpan>>();
            foreach(var testName in allTestNames)
            {
                duration_ByTestName.Add(testName, new HashSet<TimeSpan>());
            }

            durationByProfile = new HashSet<TimeSpan>();

            htmlWritersForDetailedReports_ByTestName = new Dictionary<TestUnitNames, HtmlTextWriter>();

            StatsCountTotalUpis = 0;
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

                // log only if added value
                if (isDebugMode || (resultReport.Result != ResultSeverityType.SUCCESS && resultReport.Result != ResultSeverityType.WARNING_NO_DATA))
                {

                    var detailedReportData = new SharedDetailedReportData(resultReport)
                    {
                        UserId = userId,
                        UPI = upi,
                        OldUrl = oldUrl,
                        NewUrl = newServiceUrl
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
                        System.Diagnostics.Debug.WriteLine("no writer for " + resultReport.TestName);
                    }
                }

                UpdateStatistics(upi, resultReport);
            }
        }

        public void LogProfileResult(int upi, HashSet<ResultReport> allTheResults, TimeSpan duration)
        {
            lock (lockProfileOverview)
            {
                durationByProfile.Add(duration);

                var resultByTestName = allTheResults.Select(x => new { x.TestName, x.Result }).OrderBy(z => z.TestName).ToDictionary(x => x.TestName, x => x.Result);
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
            if (!NoWarningNorErrorHappenedFlag_ByUpi.ContainsKey(upi))
            {
                NoWarningNorErrorHappenedFlag_ByUpi.Add(upi, false);
            }

            if (resultReport.Result != ResultSeverityType.SUCCESS)
            {
                NoWarningNorErrorHappenedFlag_ByUpi[upi] = true;
            }

            if (!countSeverityResults_ByTestName.ContainsKey(resultReport.TestName))
            {
                countSeverityResults_ByTestName.Add(resultReport.TestName, new Dictionary<ResultSeverityType, int>());

                // initialize all the possible combinations for the given test name
                foreach (var severity in (ResultSeverityType[])Enum.GetValues(typeof(ResultSeverityType)))
                {
                    countSeverityResults_ByTestName[resultReport.TestName].Add(severity, 0);
                }
            }

            // increase call counter
            countSeverityResults_ByTestName[resultReport.TestName][resultReport.Result]++;

            if (!countDataBehaviors_ByTestName.ContainsKey(resultReport.TestName))
            {
                // initialize all the possible combinations for the given test name
                countDataBehaviors_ByTestName.Add(resultReport.TestName, new Dictionary<IdentifiedDataBehavior, int>());

                foreach (var behavior in (IdentifiedDataBehavior[])Enum.GetValues(typeof(IdentifiedDataBehavior)))
                {
                    countDataBehaviors_ByTestName[resultReport.TestName].Add(behavior, 0);
                }
            }

            // increase call counter
            foreach (IdentifiedDataBehavior label in resultReport.IdentifedDataBehaviors)
            {
                countDataBehaviors_ByTestName[resultReport.TestName][label]++;
            }
        }

        #region Set Up Output Files

        public void StartWritingDetailedReports()
        {
            try
            {
                string filePath;

                StopWritingDetailedReports();

                htmlWritersForDetailedReports_ByTestName = new Dictionary<TestUnitNames, HtmlTextWriter>();
                countFilesGenerated++;

                foreach (TestUnitNames testName in allTestNames)
                {
                    filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + testName + "_" + countFilesGenerated + ".html");

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
            // System.Diagnostics.Debug.WriteLine(filePath);
            
            streamWriter = new StreamWriter(filePath);
            htmlWriterForSummaryReport = new HtmlTextWriter(streamWriter);

            int countTroubleFreeUpis = 0;
            TimeSpan averageDurationPerUpi = TimeSpan.Zero;
            var countByDataBehavior = new Dictionary<IdentifiedDataBehavior, int>();
            var averageDuration_ByTestName = new Dictionary<TestUnitNames, TimeSpan>();
            var frequencySuccess_ByTestName = new Dictionary<TestUnitNames, double>();
            double countSuccessResults = 0;

            var countSeverityResults = new Dictionary<ResultSeverityType, int>();
            foreach (var severity in (ResultSeverityType[])Enum.GetValues(typeof(ResultSeverityType)))
            {
                countSeverityResults.Add(severity, 0);
            }

            foreach (var upiPair in NoWarningNorErrorHappenedFlag_ByUpi)
            {
                if (upiPair.Value == false)
                {
                    countTroubleFreeUpis++;
                }
            }

            // make sure no division by zero
            if (StatsCountTotalUpis > 0)
            {
                foreach (var testName in allTestNames)
                {
                    countSuccessResults = 0;

                    foreach (var countSeverityResultsPair in countSeverityResults_ByTestName[testName])
                    {
                        switch (countSeverityResultsPair.Key)
                        {
                            case ResultSeverityType.SUCCESS:
                            case ResultSeverityType.WARNING_NO_DATA:
                            case ResultSeverityType.WARNING:
                            case ResultSeverityType.FALSE_POSITIVE:
                                // define overall success % for given test
                                countSuccessResults += countSeverityResultsPair.Value;
                            break;
                        }

                        // add count of results to the overall total by severity, without considering the test name
                        countSeverityResults[countSeverityResultsPair.Key] += countSeverityResultsPair.Value;
                    }

                    frequencySuccess_ByTestName.Add(testName, countSuccessResults / this.StatsCountTotalUpis);

                    foreach (var countDataBehaviorPair in countDataBehaviors_ByTestName[testName])
                    {
                        if (!countByDataBehavior.ContainsKey(countDataBehaviorPair.Key))
                        {
                            countByDataBehavior.Add(countDataBehaviorPair.Key, 0);
                        }
                        countByDataBehavior[countDataBehaviorPair.Key] += countDataBehaviorPair.Value;
                    }

                    averageDuration_ByTestName.Add(testName, TimeSpan.FromMilliseconds(duration_ByTestName[testName].Average(t => t.TotalMilliseconds) / this.StatsCountTotalUpis));

                }

                averageDurationPerUpi = TimeSpan.FromMilliseconds(durationByProfile.Average(t => t.TotalMilliseconds) / this.StatsCountTotalUpis);
            }

            var summaryReportData = new SharedSummaryReportData
            {
                CountProfilesTested = StatsCountTotalUpis,
                CountProfilesWithoutWarnings = countTroubleFreeUpis,
                CountBySeverity = countSeverityResults,
                CountByIdentifiedDataBehavior = countByDataBehavior.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value),
                TestNames = countSeverityResults_ByTestName.Keys.ToList(),
                CountBySeverity_ByTestName = countSeverityResults_ByTestName,
                CountByIdentifiedDataBehavior_ByTestName = countDataBehaviors_ByTestName,
                CountTestsRun = StatsCountTotalUpis * countSeverityResults_ByTestName.Keys.Count(),
                CountTestsPerUser = countSeverityResults_ByTestName.Keys.Count(),
                FrequencySuccess_ByTestName = frequencySuccess_ByTestName,
                SampleData_ByTestName = sampleDataByTestName,
                Duration = duration,
                FileLinkEnd = "_" + countFilesGenerated + ".html",
                AverageDurationPerProfile = averageDurationPerUpi,
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
            htmlWriterForSummaryReport.Dispose();
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


        public void Dispose()
        {
            if (this.streamWriter != null)
            {
                this.streamWriter.Dispose();
                this.streamWriter = null;
            }

            if (this.htmlWriterForSummaryReport != null)
            {
                this.htmlWriterForSummaryReport.Dispose();
                this.htmlWriterForSummaryReport = null;
            }

            if (this.htmlWriterForProfileReport != null)
            {
                this.htmlWriterForProfileReport.Dispose();
                this.htmlWriterForProfileReport = null;
            }
        }
    }
}