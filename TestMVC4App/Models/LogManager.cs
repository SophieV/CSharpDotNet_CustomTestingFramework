using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    using System;
    using System.IO;
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

        private static Dictionary<string, Dictionary<SeverityState, int>> statsCountFailuresTypesPerTest;
        private static Dictionary<string, Dictionary<ObservationLabel, int>> statsCountObservationTypesPerTest;
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
            statsCountFailuresTypesPerTest = new Dictionary<string, Dictionary<SeverityState, int>>();
            statsCountObservationTypesPerTest = new Dictionary<string, Dictionary<ObservationLabel, int>>();

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
            var failReport = new AssertFailedReport
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
                NewValues = resultReport.NewValues
            };

            var template = new TestFailedReportTemplate();
            template.Session = new Dictionary<string, object>()
            {
                { "FailedReport", failReport }
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

            if (resultReport.SeverityResult != SeverityState.SUCCESS && statsMapUpiTraceFailureCalledAtLeastOnce.ContainsKey(upi))
            {
                statsMapUpiTraceFailureCalledAtLeastOnce[upi] = true;
            }

            if (!statsCountFailuresTypesPerTest.ContainsKey(resultReport.TestName))
            {
                // initialize all the possible combinations for the given test name
                statsCountFailuresTypesPerTest.Add(resultReport.TestName, new Dictionary<SeverityState, int>());
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(SeverityState.ERROR, 0);
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(SeverityState.ERROR_WITH_EXPLANATION, 0);
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(SeverityState.FALSE_POSITIVE, 0);
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(SeverityState.WARNING, 0);
                statsCountFailuresTypesPerTest[resultReport.TestName].Add(SeverityState.SUCCESS, 0);
            }

            // increase call counter
            statsCountFailuresTypesPerTest[resultReport.TestName][resultReport.SeverityResult]++;

            if (!statsCountObservationTypesPerTest.ContainsKey(resultReport.TestName))
            {
                // initialize all the possible combinations for the given test name
                statsCountObservationTypesPerTest.Add(resultReport.TestName, new Dictionary<ObservationLabel, int>());
                statsCountObservationTypesPerTest[resultReport.TestName].Add(ObservationLabel.ALL_VALUES_OF_OLD_SUBSET_FOUND, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(ObservationLabel.DUPLICATED_VALUES_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(ObservationLabel.MORE_DUPLICATED_VALUES_ON_OLD_SERVICE, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(ObservationLabel.MORE_VALUES_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(ObservationLabel.VALUE_CONTAINS_TRAILING_WHITE_SPACES, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(ObservationLabel.VALUE_POPULATED_WITH_EMPTY_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(ObservationLabel.VALUES_NOT_POPULATED, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(ObservationLabel.MISSING_VALUES_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[resultReport.TestName].Add(ObservationLabel.WRONG_VALUE, 0);
            }

            // increase call counter
            foreach (ObservationLabel label in resultReport.Observations)
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

                try
                {
                    if (htmlDetailedWriters.Count > 0)
                    {
                        // finish the HTML syntax and cleanup resource
                        var jsTemplate = new JavascriptForTable();

                        foreach (HtmlTextWriter htmlTestWriter in htmlDetailedWriters.Values)
                        {
                            htmlTestWriter.WriteLine(jsTemplate.TransformText());

                            htmlTestWriter.WriteEndTag("table");
                            htmlTestWriter.WriteEndTag("body");
                            htmlTestWriter.WriteEndTag("html");
                            htmlTestWriter.Close();
                        }
                    }
                }
                catch (IOException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }

                htmlDetailedWriters = new Dictionary<string, HtmlTextWriter>();
                countFilesGenerated++;

                foreach (string testName in allTestNames)
                {
                    filePath = System.IO.Path.Combine(@"C:\\QA_LOGS\\", testName + "_" + countFilesGenerated + ".html");
                    System.Diagnostics.Debug.WriteLine(filePath);
                    streamWriter = new StreamWriter(filePath);
                    htmlDetailedWriters.Add(testName, new HtmlTextWriter(streamWriter));
                }

                var headerTemplate = new AssertFailedReportFilterInHeader();

                foreach (HtmlTextWriter htmlWriter in htmlDetailedWriters.Values)
                {
                    htmlWriter.WriteLine(headerTemplate.TransformText());

                    // styling applies to the table !
                    htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "2px");
                    htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "lightgrey");
                    htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
                    htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse");
                    htmlWriter.AddAttribute("id", "individual_test_results");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                    htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    htmlWriter.Write("Test Name");
                    htmlWriter.RenderEndTag();
                    htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    htmlWriter.Write("Result");
                    htmlWriter.RenderEndTag();
                    htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    htmlWriter.Write("User Under Test");
                    htmlWriter.RenderEndTag();
                    htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    htmlWriter.Write("Details");
                    htmlWriter.RenderEndTag();
                    htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    htmlWriter.Write("Explanations/Observations");
                    htmlWriter.RenderEndTag();
                    htmlWriter.RenderEndTag();

                    htmlWriter.RenderEndTag();
                }
            }
            catch (IOException ioe)
            {
                System.Diagnostics.Debug.WriteLine(ioe.StackTrace);
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

            Dictionary<SeverityState, int> countBySeverity = new Dictionary<SeverityState, int>();
            foreach (KeyValuePair<string, Dictionary<SeverityState, int>> entry in statsCountFailuresTypesPerTest)
            {
                foreach (KeyValuePair<SeverityState, int> subEntry in entry.Value)
                {
                    if (!countBySeverity.ContainsKey(subEntry.Key))
                    {
                        countBySeverity.Add(subEntry.Key, 0);
                    }
                    countBySeverity[subEntry.Key] += subEntry.Value;
                }
            }

            Dictionary<ObservationLabel, int> countByObservation = new Dictionary<ObservationLabel, int>();
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
            var overviewReport = new OverviewStatsReport
            {
                CountProfilesTested = StatsCountProfilesProcessed,
                CountProfilesWithoutWarnings = countProfilesWithoutWarning,
                OverviewCountBySeverityState = countBySeverity,
                OverviewCountByObservationType = countByObservation.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value),
                TestNames = statsCountFailuresTypesPerTest.Keys.ToList(),
                ByTestNameCountBySeverityState = statsCountFailuresTypesPerTest,
                ByTestNameCountByObservationType = statsCountObservationTypesPerTest,
                CountTestsRun = StatsCountProfilesProcessed * statsCountFailuresTypesPerTest.Keys.ToList().Count(),
                CountTestsPerUser = statsCountFailuresTypesPerTest.Keys.ToList().Count(),
                SampleDataByTestName = sampleDataByTestName
            };

            var template = new OverviewTestsFailedReportTemplate();
            template.Session = new Dictionary<string, object>()
                    {
                        { "StatsReport", overviewReport }
                    };

            template.Initialize();

            htmlWriter.WriteLine(template.TransformText());

            htmlWriter.WriteEndTag("table");
            htmlWriter.WriteEndTag("body");
            htmlWriter.WriteEndTag("html");
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

    }
}