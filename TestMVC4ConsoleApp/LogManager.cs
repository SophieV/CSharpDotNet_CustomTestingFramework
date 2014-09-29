using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.UI;
using TestMVC4App.Templates;
using TestMVC4ConsoleApp.Templates;

namespace TestMVC4App.Models
{
    public sealed class LogManager : IDisposable
    {
        public static SortedSet<EnumTestUnitNames> AllTestNames { get; private set; }
        public static Dictionary<EnumIdentifiedDataBehavior, string> IdentifiedBehaviorsDescriptions { get; private set; }
        public double StatsCountProfilesIgnored { get; set; }
        public double StatsCountTotalUpis { get; set; }

        private const string SUMMARY_BY_PROFILE_FILENAME = "QA_Reporting_Summary_UPIs.htm";
        private const string SUMMARY_FILENAME = "index.html";
        private const string HTM_EXTENSION = ".htm";

        private static volatile LogManager instance;
        private static object syncRoot = new Object();
        private static object lockLogResult = new object();
        private static object lockProfileOverview = new object();

        private static Dictionary<EnumTestUnitNames, HtmlTextWriter> detailedReportsWriters;
        private static HtmlTextWriter profileReportWriter;
        private static Dictionary<EnumTestUnitNames, Dictionary<EnumResultSeverityType, int>> countSeverityTypeOccurences;
        private static Dictionary<EnumTestUnitNames, Dictionary<EnumIdentifiedDataBehavior, int>> countDataBehaviorOccurences;
        private static Dictionary<EnumTestUnitNames, HashSet<TimeSpan>> durations;
        private static HashSet<TimeSpan> durationByProfile;
        private static Dictionary<int, bool> NoWarningNorErrorHappenedFlag_ByUpi;
        private static Dictionary<EnumTestUnitNames, string> sampleDataByTestName = new Dictionary<EnumTestUnitNames,string>();
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
            // the order here is quite important because this is implicitly the expectation of the templates
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
            countSeverityTypeOccurences = new Dictionary<EnumTestUnitNames, Dictionary<EnumResultSeverityType, int>>();
            countDataBehaviorOccurences = new Dictionary<EnumTestUnitNames, Dictionary<EnumIdentifiedDataBehavior, int>>();

            durations = new Dictionary<EnumTestUnitNames, HashSet<TimeSpan>>();
            foreach(var testName in AllTestNames)
            {
                durations.Add(testName, new HashSet<TimeSpan>());
            }

            durationByProfile = new HashSet<TimeSpan>();

            detailedReportsWriters = new Dictionary<EnumTestUnitNames, HtmlTextWriter>();

            StatsCountTotalUpis = 0;
            StatsCountProfilesIgnored = 0;
        }

        /// <summary>
        /// Start writing Profile Overview and Detailed Reports.
        /// </summary>
        public void StartWritingReports()
        {
            StartWritingDetailedReports();
            StartWritingProfileReport();
        }

        public void LogSummary(TimeSpan duration, string errorHappened, string errorMessage)
        {
            StopWritingReports();
            var summaryReportData = ComputeSummaryStatistics(duration, errorHappened, errorMessage);
            WriteSummaryReport(summaryReportData);
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
        /// <remarks>If the test result is SUCCESS, the entry is logged only if DEBUG mode specified.</remarks>
        public void LogTestResult(string oldUrl,string newServiceUrl, ResultReport resultReport)
        {
            UpdateStatistics(resultReport);

            if (TestSuiteUser.IsDebugMode || (resultReport.Result != EnumResultSeverityType.SUCCESS && resultReport.Result != EnumResultSeverityType.WARNING_NO_DATA))
            {
                var detailedReportData = new SharedDetailedReportData(resultReport)
                {
                    OldUrl = oldUrl,
                    NewUrl = newServiceUrl
                };

                WriteDetailedEntry(detailedReportData, resultReport.DisplayFormat);
            }
        }

        /// <summary>
        /// Writes Results to File for a full Profile tested.
        /// </summary>
        /// <param name="upi"></param>
        /// <param name="allTheResults"></param>
        /// <param name="profileProcessingDuration"></param>
        /// <param name="oldDataRQDuration"></param>
        /// <param name="newDataRQDuration"></param>
        public void LogProfileResult(int upi, HashSet<ResultReport> allTheResults, TimeSpan profileProcessingDuration, TimeSpan oldDataRQDuration, TimeSpan newDataRQDuration)
        {
            lock (lockProfileOverview)
            {
                durationByProfile.Add(profileProcessingDuration);

                var resultByTestName = allTheResults.Select(x => new { x.TestName, x.Result }).OrderBy(z => z.TestName).ToDictionary(x => x.TestName, x => x.Result);
                var summaryProfileData = new SharedProfileReportData()
                {
                    UPI = upi,
                    ResultSeverity_ByTestName = resultByTestName,
                    FileLinkEnd = "_" + countFilesGenerated + HTM_EXTENSION,
                    ProfileProcessingDuration = profileProcessingDuration,
                     OldServiceDuration = oldDataRQDuration,
                     NewServiceDuration = newDataRQDuration
                };

                WriteProfileEntry(summaryProfileData);
            }
        }

        /// <summary>
        ///  Updates the Statistics with the information from the Test Result provided.
        /// </summary>
        /// <param name="upi"></param>
        /// <param name="resultReport"></param>
        private void UpdateStatistics(ResultReport resultReport)
        {
            // keeping track of profiles without failures by logging any failure happening
            if (!NoWarningNorErrorHappenedFlag_ByUpi.ContainsKey(resultReport.Upi))
            {
                NoWarningNorErrorHappenedFlag_ByUpi.Add(resultReport.Upi, false);
            }

            if (resultReport.Result != EnumResultSeverityType.SUCCESS)
            {
                NoWarningNorErrorHappenedFlag_ByUpi[resultReport.Upi] = true;
            }

            if (!countSeverityTypeOccurences.ContainsKey(resultReport.TestName))
            {
                countSeverityTypeOccurences.Add(resultReport.TestName, new Dictionary<EnumResultSeverityType, int>());

                // initialize all the possible combinations for the given test name
                foreach (var severity in (EnumResultSeverityType[])Enum.GetValues(typeof(EnumResultSeverityType)))
                {
                    countSeverityTypeOccurences[resultReport.TestName].Add(severity, 0);
                }
            }

            // increase call counter
            countSeverityTypeOccurences[resultReport.TestName][resultReport.Result]++;

            if (!countDataBehaviorOccurences.ContainsKey(resultReport.TestName))
            {
                // initialize all the possible combinations for the given test name
                countDataBehaviorOccurences.Add(resultReport.TestName, new Dictionary<EnumIdentifiedDataBehavior, int>());

                foreach (var behavior in (EnumIdentifiedDataBehavior[])Enum.GetValues(typeof(EnumIdentifiedDataBehavior)))
                {
                    countDataBehaviorOccurences[resultReport.TestName].Add(behavior, 0);
                }
            }

            // increase call counter
            foreach (EnumIdentifiedDataBehavior label in resultReport.IdentifedDataBehaviors)
            {
                countDataBehaviorOccurences[resultReport.TestName][label]++;
            }

            durations[resultReport.TestName].Add(resultReport.Duration);
        }

        private SharedSummaryReportData ComputeSummaryStatistics(TimeSpan duration, string errorHappened, string errorMessage)
        {
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

                    foreach (var countSeverityResultsPair in countSeverityTypeOccurences[testName])
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

                    foreach (var countDataBehaviorPair in countDataBehaviorOccurences[testName])
                    {
                        if (!countByDataBehavior.ContainsKey(countDataBehaviorPair.Key))
                        {
                            countByDataBehavior.Add(countDataBehaviorPair.Key, 0);
                        }
                        countByDataBehavior[countDataBehaviorPair.Key] += countDataBehaviorPair.Value;
                    }

                    averageDuration_ByTestName.Add(testName, TimeSpan.FromMilliseconds(durations[testName].Average(t => t.TotalMilliseconds) / this.StatsCountTotalUpis));

                }

                averageDurationPerUpi = TimeSpan.FromMilliseconds(durationByProfile.Average(t => t.TotalMilliseconds) / this.StatsCountTotalUpis);
            }

            var summaryReportData = new SharedSummaryReportData
            {
                CountProfilesTested = StatsCountTotalUpis,
                CountProfilesWithoutWarnings = countTroubleFreeUpis,
                CountBySeverity = countSeverityResults.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value),
                CountByIdentifiedDataBehavior = countByDataBehavior.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value),
                TestNames = countSeverityTypeOccurences.Keys.ToList(),
                CountBySeverity_ByTestName = countSeverityTypeOccurences,
                CountByIdentifiedDataBehavior_ByTestName = countDataBehaviorOccurences,
                CountTestsRun = StatsCountTotalUpis * countSeverityResults.Keys.Count(),
                CountTestsPerUser = countSeverityResults.Keys.Count(),
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
            return summaryReportData;
        }

        #region I/O - Files

        /// <summary>
        /// Creates and Adds the header to the Detailed HTML file.
        /// </summary>
        private static void StartWritingDetailedReports()
        {
            try
            {
                string filePath;

                StopWritingReports();

                detailedReportsWriters = new Dictionary<EnumTestUnitNames, HtmlTextWriter>();
                countFilesGenerated++;

                foreach (EnumTestUnitNames testName in AllTestNames)
                {
                    filePath = testName + "_" + countFilesGenerated + HTM_EXTENSION;
                    detailedReportsWriters.Add(testName, new HtmlTextWriter(new StreamWriter(filePath)));
                }

                TestNameDetailedReport_Header headerTemplate = null;

                foreach (HtmlTextWriter htmlWriter in detailedReportsWriters.Values)
                {
                    // if the header template is created out of the loop, its content duplicates itself...
                    headerTemplate = new TestNameDetailedReport_Header();
                    htmlWriter.WriteLine(headerTemplate.TransformText());
                }

                filePath = SUMMARY_BY_PROFILE_FILENAME;
                profileReportWriter = new HtmlTextWriter(new StreamWriter(filePath));
            }
            catch (IOException ioe)
            {
                System.Diagnostics.Debug.WriteLine(ioe.StackTrace);
            }
        }

        /// <summary>
        /// Creates and Adds the header to the Profile Overview HTML file.
        /// </summary>
        private static void StartWritingProfileReport()
        {
            try
            {
                var sharedHeaderProfileData = new SharedHeaderProfileReportData() { AllTestNames = AllTestNames };

                var headerProfileTemplate = new ProfileReport_Header();
                headerProfileTemplate.Session = new Dictionary<string, object>()
                    {
                        { "ProfileHeaderReportDataObject", sharedHeaderProfileData }
                    };

                headerProfileTemplate.Initialize();
                profileReportWriter.WriteLine(headerProfileTemplate.TransformText());
            }
            catch (IOException ioe)
            {
                System.Diagnostics.Debug.WriteLine(ioe.StackTrace);
            }
        }

        /// <summary>
        /// Closes after Adding the footer to the Profile Overview HTML file.
        /// Closes all other HTML files.
        /// </summary>
        private static void StopWritingReports()
        {
            try
            {
                if (detailedReportsWriters.Count > 0)
                {
                    // finish the HTML syntax and cleanup resource
                    TestNameDetailedReport_Footer jsTemplate = null;

                    foreach (HtmlTextWriter htmlTestWriter in detailedReportsWriters.Values)
                    {
                        // if the template is created out of the loop, its content duplicates itself...
                        jsTemplate = new TestNameDetailedReport_Footer();
                        htmlTestWriter.WriteLine(jsTemplate.TransformText());
                        htmlTestWriter.Close();
                    }
                }

                if (profileReportWriter != null)
                {
                    var footerTemplate = new ProfileReport_Footer();
                    profileReportWriter.WriteLine(footerTemplate.TransformText());
                    profileReportWriter.Close();
                    profileReportWriter = null;
                }
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Prepares Aggregations for Template and populates it. 
        /// </summary>
        /// <param name="countTotalUpis"></param>
        /// <param name="countIgnoredUpis"></param>
        /// <param name="duration"></param>
        /// <param name="errorHappened"></param>
        /// <param name="errorMessage"></param>
        private static void WriteSummaryReport(SharedSummaryReportData templateData)
        {
            var htmlWriterForSummaryReport = new HtmlTextWriter(new StreamWriter(SUMMARY_FILENAME));

            var template = new SummaryReport();
            template.Session = new Dictionary<string, object>()
                    {
                        { "SummaryReportDataObject", templateData }
                    };

            template.Initialize();
            htmlWriterForSummaryReport.WriteLine(template.TransformText());
            htmlWriterForSummaryReport.Close();

            htmlWriterForSummaryReport.Dispose();
            htmlWriterForSummaryReport = null;
        }

        /// <summary>
        /// Writes entry of specific Test to File.
        /// </summary>
        /// <param name="templateData"></param>
        /// <param name="displayFormat"></param>
        private static void WriteDetailedEntry(SharedDetailedReportData templateData, EnumResultDisplayFormat displayFormat)
        {
            lock (lockLogResult)
            {
                switch (displayFormat)
                {
                    case EnumResultDisplayFormat.ListOfValues:
                        var templateListValues = new TestNameDetailedReport_ListValues();
                        templateListValues.Session = new Dictionary<string, object>()
                        {
                            { "DetailedReportDataObject", templateData }
                        };

                        templateListValues.Initialize();
                        if (detailedReportsWriters.ContainsKey(templateData.TestName))
                        {
                            detailedReportsWriters[templateData.TestName].WriteLine(templateListValues.TransformText());
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("no writer for " + templateData.TestName);
                        }
                        break;
                    case EnumResultDisplayFormat.OrganizationTree:
                        var templateOrgTree = new TestNameDetailedReport_OrganizationTree();
                        templateOrgTree.Session = new Dictionary<string, object>()
                        {
                            { "DetailedReportDataObject", templateData }
                        };

                        templateOrgTree.Initialize();
                        if (detailedReportsWriters.ContainsKey(templateData.TestName))
                        {
                            detailedReportsWriters[templateData.TestName].WriteLine(templateOrgTree.TransformText());
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("no writer for " + templateData.TestName);
                        }
                        break;
                    case EnumResultDisplayFormat.StructureOfValues:
                        var templateListStructures = new TestNameDetailedReport_ListStructures();
                        templateListStructures.Session = new Dictionary<string, object>()
                        {
                            { "DetailedReportDataObject", templateData }
                        };

                        templateListStructures.Initialize();
                        if (detailedReportsWriters.ContainsKey(templateData.TestName))
                        {
                            detailedReportsWriters[templateData.TestName].WriteLine(templateListStructures.TransformText());
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("no writer for " + templateData.TestName);
                        }
                        break;
                }
            }
        }

        private static void WriteProfileEntry(SharedProfileReportData summaryProfileData)
        {
            var template = new ProfileReport();
            template.Session = new Dictionary<string, object>()
            {
                { "ProfileReportDataObject", summaryProfileData }
            };

            template.Initialize();
            profileReportWriter.WriteLine(template.TransformText());
        }

        #endregion

        /// <summary>
        /// Releases all the Writers instances used.
        /// </summary>
        public void Dispose()
        {
            if (detailedReportsWriters.Count > 0)
            {
                foreach (HtmlTextWriter htmlTestWriter in detailedReportsWriters.Values)
                {
                    htmlTestWriter.Close();
                }
            }

            detailedReportsWriters = null;

            if (profileReportWriter != null)
            {
                profileReportWriter.Dispose();
                profileReportWriter = null;
            }
        }
    }
}