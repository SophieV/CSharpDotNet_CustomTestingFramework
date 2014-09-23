using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Web.UI;
using TestMVC4App.Templates;
using TestMVC4ConsoleApp.Templates;

namespace TestMVC4App.Models
{
    public sealed class LogManager : IDisposable
    {
        private const string SUMMARY_BY_PROFILE_FILENAME = "QA_Reporting_Summary_UPIs.htm";
        private const string SUMMARY_FILENAME = "index.html";
        private const string HTM_EXTENSION = ".htm";

        private static volatile LogManager instance;
        private static object syncRoot = new Object();

        private static object lockLogResult = new object();
        private static object lockProfileOverview = new object();

        public SortedSet<EnumTestUnitNames> AllTestNames {get; private set;}
        private StreamWriter streamWriter;
        private Dictionary<EnumTestUnitNames, HtmlTextWriter> htmlWritersForDetailedReports_ByTestName;
        private HtmlTextWriter htmlWriterForSummaryReport;
        private HtmlTextWriter htmlWriterForProfileReport;

        private static Dictionary<EnumTestUnitNames, Dictionary<EnumResultSeverityType, int>> countSeverityResults_ByTestName;
        private static Dictionary<EnumTestUnitNames, Dictionary<EnumIdentifiedDataBehavior, int>> countDataBehaviors_ByTestName;

        private static Dictionary<EnumTestUnitNames, HashSet<TimeSpan>> duration_ByTestName;
        private static HashSet<TimeSpan> durationByProfile;

        private static Dictionary<int, bool> NoWarningNorErrorHappenedFlag_ByUpi;

        private static Dictionary<EnumTestUnitNames, string> sampleDataByTestName = new Dictionary<EnumTestUnitNames,string>();

        public static Dictionary<EnumIdentifiedDataBehavior, string> IdentifiedBehaviorsDescriptions { get; private set; }

        public double StatsCountProfilesIgnored { get; set; }
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
            AllTestNames = new SortedSet<EnumTestUnitNames>();
            foreach (var testName in (EnumTestUnitNames[])Enum.GetValues(typeof(EnumTestUnitNames)))
            {
                AllTestNames.Add(testName);
            }

            IdentifiedBehaviorsDescriptions = new Dictionary<EnumIdentifiedDataBehavior, string>();
            foreach (var behavior in (EnumIdentifiedDataBehavior[]) Enum.GetValues(typeof(EnumIdentifiedDataBehavior)))
            {
                IdentifiedBehaviorsDescriptions.Add(behavior, ParsingHelper.GetDescription(behavior));
            }

            NoWarningNorErrorHappenedFlag_ByUpi = new Dictionary<int, bool>();
            countSeverityResults_ByTestName = new Dictionary<EnumTestUnitNames, Dictionary<EnumResultSeverityType, int>>();
            countDataBehaviors_ByTestName = new Dictionary<EnumTestUnitNames, Dictionary<EnumIdentifiedDataBehavior, int>>();

            duration_ByTestName = new Dictionary<EnumTestUnitNames, HashSet<TimeSpan>>();
            foreach(var testName in AllTestNames)
            {
                duration_ByTestName.Add(testName, new HashSet<TimeSpan>());
            }

            durationByProfile = new HashSet<TimeSpan>();

            htmlWritersForDetailedReports_ByTestName = new Dictionary<EnumTestUnitNames, HtmlTextWriter>();

            StatsCountTotalUpis = 0;
            StatsCountProfilesIgnored = 0;
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
                if (TestSuiteUser.IsDebugMode || (resultReport.Result != EnumResultSeverityType.SUCCESS && resultReport.Result != EnumResultSeverityType.WARNING_NO_DATA))
                {

                    var detailedReportData = new SharedDetailedReportData(resultReport)
                    {
                        UserId = userId,
                        UPI = upi,
                        OldUrl = oldUrl,
                        NewUrl = newServiceUrl
                    };

                    switch (resultReport.DisplayFormat)
                    {
                        case EnumResultDisplayFormat.ListOfValues:
                            var templateListValues = new TestNameDetailedReport_ListValues();
                            templateListValues.Session = new Dictionary<string, object>()
                            {
                                { "DetailedReportDataObject", detailedReportData }
                            };

                            templateListValues.Initialize();
                            if (htmlWritersForDetailedReports_ByTestName.ContainsKey(resultReport.TestName))
                            {
                                htmlWritersForDetailedReports_ByTestName[resultReport.TestName].WriteLine(templateListValues.TransformText());
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("no writer for " + resultReport.TestName);
                            }
                            break;
                        case EnumResultDisplayFormat.OrganizationTree:
                            var templateOrgTree = new TestNameDetailedReport_OrganizationTree();
                            templateOrgTree.Session = new Dictionary<string, object>()
                            {
                                { "DetailedReportDataObject", detailedReportData }
                            };

                            templateOrgTree.Initialize();
                            if (htmlWritersForDetailedReports_ByTestName.ContainsKey(resultReport.TestName))
                            {
                                htmlWritersForDetailedReports_ByTestName[resultReport.TestName].WriteLine(templateOrgTree.TransformText());
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("no writer for " + resultReport.TestName);
                            }
                            break;
                        case EnumResultDisplayFormat.StructureOfValues:
                            var templateListStructures = new TestNameDetailedReport_ListStructures();
                            templateListStructures.Session = new Dictionary<string, object>()
                            {
                                { "DetailedReportDataObject", detailedReportData }
                            };

                            templateListStructures.Initialize();
                            if (htmlWritersForDetailedReports_ByTestName.ContainsKey(resultReport.TestName))
                            {
                                htmlWritersForDetailedReports_ByTestName[resultReport.TestName].WriteLine(templateListStructures.TransformText());
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("no writer for " + resultReport.TestName);
                            }
                            break;
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
                    FileLinkEnd = "_" + countFilesGenerated + HTM_EXTENSION,
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

            if (resultReport.Result != EnumResultSeverityType.SUCCESS)
            {
                NoWarningNorErrorHappenedFlag_ByUpi[upi] = true;
            }

            if (!countSeverityResults_ByTestName.ContainsKey(resultReport.TestName))
            {
                countSeverityResults_ByTestName.Add(resultReport.TestName, new Dictionary<EnumResultSeverityType, int>());

                // initialize all the possible combinations for the given test name
                foreach (var severity in (EnumResultSeverityType[])Enum.GetValues(typeof(EnumResultSeverityType)))
                {
                    countSeverityResults_ByTestName[resultReport.TestName].Add(severity, 0);
                }
            }

            // increase call counter
            countSeverityResults_ByTestName[resultReport.TestName][resultReport.Result]++;

            if (!countDataBehaviors_ByTestName.ContainsKey(resultReport.TestName))
            {
                // initialize all the possible combinations for the given test name
                countDataBehaviors_ByTestName.Add(resultReport.TestName, new Dictionary<EnumIdentifiedDataBehavior, int>());

                foreach (var behavior in (EnumIdentifiedDataBehavior[])Enum.GetValues(typeof(EnumIdentifiedDataBehavior)))
                {
                    countDataBehaviors_ByTestName[resultReport.TestName].Add(behavior, 0);
                }
            }

            // increase call counter
            foreach (EnumIdentifiedDataBehavior label in resultReport.IdentifedDataBehaviors)
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

                htmlWritersForDetailedReports_ByTestName = new Dictionary<EnumTestUnitNames, HtmlTextWriter>();
                countFilesGenerated++;

                foreach (EnumTestUnitNames testName in AllTestNames)
                {
                    filePath = testName + "_" + countFilesGenerated + HTM_EXTENSION;

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

                filePath = SUMMARY_BY_PROFILE_FILENAME;

                streamWriter = new StreamWriter(filePath);
                htmlWriterForProfileReport = new HtmlTextWriter(streamWriter);

                var sharedHeaderProfileData = new SharedHeaderProfileReportData() { AllTestNames = AllTestNames };

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
            string filePath = SUMMARY_FILENAME;
            // System.Diagnostics.Debug.WriteLine(filePath);
            
            streamWriter = new StreamWriter(filePath);
            htmlWriterForSummaryReport = new HtmlTextWriter(streamWriter);

            int countTroubleFreeUpis = 0;
            TimeSpan averageDurationPerUpi = TimeSpan.Zero;
            var countByDataBehavior = new Dictionary<EnumIdentifiedDataBehavior, int>();
            var averageDuration_ByTestName = new Dictionary<EnumTestUnitNames, TimeSpan>();
            var frequencySuccess_ByTestName = new Dictionary<EnumTestUnitNames, double>();
            double countSuccessResults = 0;

            var countSeverityResults = new Dictionary<EnumResultSeverityType, int>();
            foreach (var severity in (EnumResultSeverityType[])Enum.GetValues(typeof(EnumResultSeverityType)))
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
                // deduce ignored profiles from statistics, to still reach 100%
                this.StatsCountTotalUpis -= this.StatsCountProfilesIgnored;

                foreach (var testName in AllTestNames)
                {
                    countSuccessResults = 0;

                    foreach (var countSeverityResultsPair in countSeverityResults_ByTestName[testName])
                    {
                        switch (countSeverityResultsPair.Key)
                        {
                            case EnumResultSeverityType.SUCCESS:
                            case EnumResultSeverityType.WARNING_NO_DATA:
                            case EnumResultSeverityType.WARNING_ONLY_NEW:
                            case EnumResultSeverityType.WARNING:
                            case EnumResultSeverityType.FALSE_POSITIVE:
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
                CountBySeverity = countSeverityResults.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value),
                CountByIdentifiedDataBehavior = countByDataBehavior.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value),
                TestNames = countSeverityResults_ByTestName.Keys.ToList(),
                CountBySeverity_ByTestName = countSeverityResults_ByTestName,
                CountByIdentifiedDataBehavior_ByTestName = countDataBehaviors_ByTestName,
                CountTestsRun = StatsCountTotalUpis * countSeverityResults_ByTestName.Keys.Count(),
                CountTestsPerUser = countSeverityResults_ByTestName.Keys.Count(),
                FrequencySuccess_ByTestName = frequencySuccess_ByTestName,
                SampleData_ByTestName = sampleDataByTestName,
                Duration = duration,
                FileLinkEnd = "_" + countFilesGenerated + HTM_EXTENSION,
                AverageDurationPerProfile = averageDurationPerUpi,
                AverageDuration_ByTestName = averageDuration_ByTestName,
                ErrorHappened = errorHappened,
                ErrorMessage = errorMessage,
                FileByProfileLink = SUMMARY_BY_PROFILE_FILENAME,
                CountProfilesIgnored = this.StatsCountProfilesIgnored
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