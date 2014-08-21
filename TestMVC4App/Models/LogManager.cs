using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Web.UI;
    using TestMVC4App.Templates;

    public sealed class LogManager
    {
        private static volatile LogManager instance;
        private static object syncRoot = new Object();

        private List<string> allTestNames;
        private StreamWriter streamWriter;
        private Dictionary<string, HtmlTextWriter> htmlDetailedWriters;
        private HtmlTextWriter htmlWriter;

        private static Dictionary<string, Dictionary<ResultSeverityState, int>> statsCountFailuresTypesPerTest;
        private static Dictionary<string, Dictionary<IdentifiedDataBehavior, int>> statsCountObservationTypesPerTest;
        private static Dictionary<int, bool> statsMapUpiTraceFailureCalledAtLeastOnce;

        private static Dictionary<string, string> sampleDataByTestName = new Dictionary<string,string>();

        public int StatsCountProfilesProcessed {get;set;}

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
            allTestNames = new List<string>();
            allTestNames.Add("UserBasicInfo_LastName_Test");
            allTestNames.Add("UserBasicInfo_FirstName_Test");
            allTestNames.Add("UserBasicInfo_MiddleName_Test");
            allTestNames.Add("UserBasicInfo_Email_Test");
            allTestNames.Add("UserBasicInfo_NetId_Test");
            allTestNames.Add("UserBasicInfo_PageName_Test");
            allTestNames.Add("UserBasicInfo_Suffix_Test");
            allTestNames.Add("UserBasicInfo_Gender_Test");
            allTestNames.Add("UserBasicInfo_UPI_Test");

            allTestNames.Add("UserGeneralInfo_Bio_Test");
            allTestNames.Add("UserGeneralInfo_Titles_Test");
            allTestNames.Add("UserGeneralInfo_Organization_Id_Test");
            allTestNames.Add("UserGeneralInfo_Organization_Name_Test");

            statsMapUpiTraceFailureCalledAtLeastOnce = new Dictionary<int, bool>();
            statsCountFailuresTypesPerTest = new Dictionary<string, Dictionary<ResultSeverityState, int>>();
            statsCountObservationTypesPerTest = new Dictionary<string, Dictionary<IdentifiedDataBehavior, int>>();

            htmlDetailedWriters = new Dictionary<string, HtmlTextWriter>();

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
            var failReport = new SharedDetailedReportData
            {
                Message = resultReport.ErrorMessage,
                TestName = resultReport.TestName,
                UserId = userId,
                UPI = upi,
                FailureType = resultReport.SeverityResult,
                OldUrl = oldUrl,
                NewUrl = newServiceUrl,
                TaskDescription = resultReport.TestDescription,
                Observations = resultReport.Observations,
                OldValues = resultReport.OldValues,
                NewValues = resultReport.NewValues,
                Duration = resultReport.Duration
            };

            var template = new DetailedReport();
            template.Session = new Dictionary<string, object>()
            {
                { "DetailedReportDataObject", failReport }
            };

            template.Initialize();

            if (htmlDetailedWriters.ContainsKey(resultReport.TestName))
            {
                htmlDetailedWriters[resultReport.TestName].WriteLine(template.TransformText());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(resultReport.TestName);
            }

            // keeping track of profiles without failures by logging any failure happening
            if (!statsMapUpiTraceFailureCalledAtLeastOnce.ContainsKey(upi))
            {
                statsMapUpiTraceFailureCalledAtLeastOnce.Add(upi, false);
            }

            if (resultReport.SeverityResult != ResultSeverityState.SUCCESS && statsMapUpiTraceFailureCalledAtLeastOnce.ContainsKey(upi))
            {
                statsMapUpiTraceFailureCalledAtLeastOnce[upi] = true;
            }

            if (!statsCountFailuresTypesPerTest.ContainsKey(resultReport.TestName))
            {
                // initialize all the possible combinations for the given test name
                statsCountFailuresTypesPerTest.Add(resultReport.TestName, new Dictionary<ResultSeverityState, int>());
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(ResultSeverityState.ERROR, 0);
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(ResultSeverityState.ERROR_WITH_EXPLANATION, 0);
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(ResultSeverityState.FALSE_POSITIVE, 0);
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(ResultSeverityState.WARNING, 0);
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(ResultSeverityState.SUCCESS, 0);
            }

            // increase call counter
            statsCountFailuresTypesPerTest[resultReport.TestName][resultReport.SeverityResult]++;

            if (!statsCountObservationTypesPerTest.ContainsKey(resultReport.TestName))
            {
                // initialize all the possible combinations for the given test name
                statsCountObservationTypesPerTest.Add(resultReport.TestName, new Dictionary<IdentifiedDataBehavior, int>());
                statsCountObservationTypesPerTest[resultReport.TestName].Add(IdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(IdentifiedDataBehavior.DUPLICATED_VALUES_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(IdentifiedDataBehavior.MORE_VALUES_ON_OLD_SERVICE_ALL_DUPLICATES, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(IdentifiedDataBehavior.MORE_VALUES_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(IdentifiedDataBehavior.MISMATCH_DUE_TO_TRAILING_WHITE_SPACES, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(IdentifiedDataBehavior.VALUE_POPULATED_WITH_WHITE_SPACE_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(IdentifiedDataBehavior.VALUES_NOT_POPULATED, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(IdentifiedDataBehavior.MISSING_VALUES_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(IdentifiedDataBehavior.WRONG_VALUE, 0);
            }

            // increase call counter
            foreach (IdentifiedDataBehavior label in resultReport.Observations)
            {
                statsCountObservationTypesPerTest[resultReport.TestName][label]++;
            }
        }

        #region Set Up Output Files

        public void StartWritingDetailedReports()
        {
            try
            {
                string filePath;

                StopWritingDetailedReports();

                htmlDetailedWriters = new Dictionary<string, HtmlTextWriter>();
                countFilesGenerated++;

                foreach (string testName in allTestNames)
                {
                    filePath = System.IO.Path.Combine(@"C:\\QA_LOGS\\", testName + "_" + countFilesGenerated + ".html");
                    System.Diagnostics.Debug.WriteLine(filePath);
                    streamWriter = new StreamWriter(filePath);
                    htmlDetailedWriters.Add(testName, new HtmlTextWriter(streamWriter));
                }

                HeaderJS_DetailedReport headerTemplate = null;

                foreach (HtmlTextWriter htmlWriter in htmlDetailedWriters.Values)
                {
                    // if the header template is created out of the loop, its content duplicates itself...
                    headerTemplate = new HeaderJS_DetailedReport();
                    htmlWriter.WriteLine(headerTemplate.TransformText());
                }
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
                if (htmlDetailedWriters.Count > 0)
                {
                    // finish the HTML syntax and cleanup resource
                    FooterJS_DetailedReport jsTemplate = null;

                    foreach (HtmlTextWriter htmlTestWriter in htmlDetailedWriters.Values)
                    {
                        // if the template is created out of the loop, its content duplicates itself...
                        jsTemplate = new FooterJS_DetailedReport();
                        htmlTestWriter.WriteLine(jsTemplate.TransformText());
                        htmlTestWriter.Close();
                    }
                }
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        public void WriteSummaryReport()
        {
            string filePath = System.IO.Path.Combine(@"C:\\QA_LOGS\\", "QA_Reporting_Summary.html");
            System.Diagnostics.Debug.WriteLine(filePath);
            
            streamWriter = new StreamWriter(filePath);
            htmlWriter = new HtmlTextWriter(streamWriter);

            int countProfilesWithoutWarning = 0;
            foreach (var entry in statsMapUpiTraceFailureCalledAtLeastOnce)
            {
                if (entry.Value == false)
                {
                    countProfilesWithoutWarning++;
                }
            }

            Dictionary<ResultSeverityState, int> countBySeverity = new Dictionary<ResultSeverityState, int>();
            foreach (KeyValuePair<string, Dictionary<ResultSeverityState, int>> entry in statsCountFailuresTypesPerTest)
            {
                foreach (KeyValuePair<ResultSeverityState, int> subEntry in entry.Value)
                {
                    if (!countBySeverity.ContainsKey(subEntry.Key))
                    {
                        countBySeverity.Add(subEntry.Key, 0);
                    }
                    countBySeverity[subEntry.Key] += subEntry.Value;
                }
            }

            Dictionary<IdentifiedDataBehavior, int> countByObservation = new Dictionary<IdentifiedDataBehavior, int>();
            foreach (var entry in statsCountObservationTypesPerTest)
            {
                foreach (var subEntry in entry.Value)
                {
                    if (!countByObservation.ContainsKey(subEntry.Key))
                    {
                        countByObservation.Add(subEntry.Key, 0);
                    }
                    countByObservation[subEntry.Key] += subEntry.Value;

                }
            }

            // report overview
            var overviewReport = new SharedSummaryReportData
            {
                CountProfilesTested = StatsCountProfilesProcessed,
                CountProfilesWithoutWarnings = countProfilesWithoutWarning,
                CountBySeverity = countBySeverity,
                CountByIdentifiedDataBehavior = countByObservation.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value),
                TestNames = statsCountFailuresTypesPerTest.Keys.ToList(),
                CountBySeverity_ByTestName = statsCountFailuresTypesPerTest,
                CountByIdentifiedDataBehavior_ByTestName = statsCountObservationTypesPerTest,
                CountTestsRun = StatsCountProfilesProcessed * statsCountFailuresTypesPerTest.Keys.ToList().Count(),
                CountTestsPerUser = statsCountFailuresTypesPerTest.Keys.ToList().Count(),
                SampleData_ByTestName = sampleDataByTestName
            };

            var template = new SummaryReport();
            template.Session = new Dictionary<string, object>()
                    {
                        { "SummaryReportDataObject", overviewReport }
                    };

            template.Initialize();
            htmlWriter.WriteLine(template.TransformText());
            htmlWriter.Close();
            htmlWriter = null;
        }

        public void CleanUpResources()
        {
            if (htmlDetailedWriters.Count > 0)
            {
                foreach (HtmlTextWriter htmlTestWriter in htmlDetailedWriters.Values)
                {
                    htmlTestWriter.Close();
                }
            }

            htmlDetailedWriters = null;
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