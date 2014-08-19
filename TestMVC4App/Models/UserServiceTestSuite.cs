using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Xml.Linq;
using System.Xml.XPath;
using System.ComponentModel.DataAnnotations;
using TestMVC4App.Templates;
using YSM.PMS.Web.Service.Clients;
using System.Reflection;

namespace TestMVC4App.Models
{
    public class UserServiceTestSuite
    {
        public const String OLD_SERVICE_URL_BASE = "http://yale-faculty.photobooks.com/directory/XMLProfile.asp?UPI=";

        private const string SPAN_DOUBLON = " <span style=\"color:pink;\">[DOUBLON]</span>";
        private const string SPAN_MISSING = " <span style=\"color:purple;\">[MISSING]</span>";

        // total UPIs in the list : 38,957
#if DEBUG
        private const int MaxProfilesForOneFile = 5000;
#else
        private const int MaxProfilesForOneFile = 50000;
#endif

        private List<int> upiList = new List<int>();
        private static StreamWriter streamWriter;
        private static Dictionary<string, HtmlTextWriter> htmlWriters;
        private static HtmlTextWriter htmlWriter; 
        private static List<string> allTestNames;
        private static Dictionary<string, string> sampleDataByTestName;

        private static Dictionary<string, Dictionary<SeverityState,int>> statsCountFailuresTypesPerTest;
        private static Dictionary<string, Dictionary<ObservationLabel, int>> statsCountObservationTypesPerTest;
        private static Dictionary<int, bool> statsMapUpiTraceFailureCalledAtLeastOnce;
        private static int statsCountProfilesProcessed = 0;

        #region Database Connection Details

        //retrieve all of the Users by UPI in the database
        //TODO: move this to a connection setting!
        static string connectionString = @"Server=tcp:le9rmjfn5q.database.windows.net,1433;Database=yfmps-entities;User ID=slamTHEdbNOince@le9rmjfn5q;Password=allQUIETallsWELL104;Encrypt=True;Connection Timeout=30;";
        static string selectStatement = "SELECT Upi FROM [User]";
        static SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand queryCommand = new SqlCommand(selectStatement, conn);

        private List<int> ConnectToDataSourceAndRetriveUPIs()
        {
            conn.Open();
            Console.Out.WriteLine("Connection state is: " + conn.State.ToString());

            SqlDataReader sdr = queryCommand.ExecuteReader();

            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    upiList.Add(sdr.GetInt32(0));
                    //Console.WriteLine("{0}", sdr.GetInt32(0));
                }
            }
            else
            {
                Console.Out.WriteLine("No rows found.");
            }

            foreach (int upi in upiList)
            {
                //Console.WriteLine(upi);
            }
            sdr.Close();

            conn.Close();

            return upiList;
        }

        #endregion

        public void compareServices()
        {
            //init all loop variables
            string oldServiceURL;
            string oldServiceXMLOutput;

            statsMapUpiTraceFailureCalledAtLeastOnce = new Dictionary<int, bool>();
            statsCountFailuresTypesPerTest = new Dictionary<string,Dictionary<SeverityState,int>>();
            statsCountObservationTypesPerTest = new Dictionary<string, Dictionary<ObservationLabel, int>>();

            sampleDataByTestName = new Dictionary<string, string>();

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
            allTestNames.Add("UserGeneralInfo_Organizations_Test");

            upiList = ConnectToDataSourceAndRetriveUPIs();

            // initial setup
            SetupConsoleRedirectToFile(statsCountProfilesProcessed + 1);

#if DEBUG
            //upiList = new List<int>() { 10151776, 10290564, 11091604 };
#endif
            //loop on the list of all UPIs retrieved from the old database
            foreach (int upi in upiList)
            {
#if DEBUG
                            if (statsCountProfilesProcessed > 30)
            {
                break;
            }
#endif
                oldServiceXMLOutput = string.Empty;

                if (!statsMapUpiTraceFailureCalledAtLeastOnce.ContainsKey(upi))
                {
                    statsMapUpiTraceFailureCalledAtLeastOnce.Add(upi, false);
                }

                // modulo to avoid resetting the counter
                if (statsCountProfilesProcessed % MaxProfilesForOneFile == 0)
                {
                    if (statsCountProfilesProcessed > 0)
                    {
                        // change to next output file
                        CleanUpConsoleRedirectToFile();
                        SetupConsoleRedirectToFile((statsCountProfilesProcessed / MaxProfilesForOneFile) + 1);
                    }
                }

                //go to the old service and retrieve the data
                oldServiceURL = OLD_SERVICE_URL_BASE + upi.ToString();

                //Find a way to set the 'Timeout' property in Milliseconds. The old service can be slow.
                //we also need exception handling!
                using (var webClient = new TimeoutExtendedWebClient())
                {
                    try
                    {
                        oldServiceXMLOutput = webClient.DownloadString(oldServiceURL);
                    } catch (WebException we)
                    {
                        System.Console.Out.WriteLine(we.StackTrace);
                        System.Diagnostics.Debug.WriteLine(we.StackTrace);

                        System.Console.Out.WriteLine("second chance");
                        System.Diagnostics.Debug.WriteLine("second chance");
                        try
                        {
                            oldServiceXMLOutput = webClient.DownloadString(oldServiceURL);
                        }
                        catch (WebException we2)
                        {
                            System.Console.Out.WriteLine(we2.StackTrace);
                            System.Diagnostics.Debug.WriteLine(we2.StackTrace);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(oldServiceXMLOutput))
                {
                    try
                    {
                        XDocument oldServiceXMLOutputDocument = XDocument.Parse(oldServiceXMLOutput);

                        //new service value is stored in Web.Config, Key : "ProfileServiceBaseAddress"
                        var usersClient = new UsersClient();

                        // This service has to be called first because it will provided the User ID mapped to the UPI for the next calls.
                        UserBasicInfoTest userBasicInfoTest = new UserBasicInfoTest();
                        userBasicInfoTest.RunAllTests(usersClient, upi, oldServiceXMLOutputDocument);
                        int userId = userBasicInfoTest.MappedUserId;

                        ITestStructure classTest = new UserGeneralInfoTest();
                        classTest.RunAllTests(usersClient, upi, oldServiceXMLOutputDocument, userId);

                        statsCountProfilesProcessed++;
                        System.Console.Out.WriteLine(statsCountProfilesProcessed);
                    }
                    catch (Exception e)
                    {
                        System.Console.Out.WriteLine(e.StackTrace);
                        System.Diagnostics.Debug.WriteLine(e.StackTrace);
                        //CleanUpConsoleRedirectToFile();
                        // create report overview
                        SetupConsoleRedirectToFile(0);
                        CleanUpConsoleRedirectToFile();
                        // reconnect writers
                        SetupConsoleRedirectToFile(statsCountProfilesProcessed);
                    }
                }
            }

            CleanUpConsoleRedirectToFile();
            // create report overview
            SetupConsoleRedirectToFile(0);
            CleanUpConsoleRedirectToFile();
        }

        #region Set Up Output Files

        private void SetupConsoleRedirectToFile(int countForFileName)
        {
            try
            {
                string filePath;

                if (countForFileName > 0)
                {
                    htmlWriters = new Dictionary<string, HtmlTextWriter>();

                    foreach (string testName in allTestNames)
                    {
                        filePath = System.IO.Path.Combine(@"C:\\QA_LOGS\\", testName + "_" + countForFileName + ".html");
                        System.Diagnostics.Debug.WriteLine(filePath);
                        streamWriter = new StreamWriter(filePath);
                        htmlWriters.Add(testName, new HtmlTextWriter(streamWriter));

                    }

                    var template = new AssertFailedReportFilterInHeader();

                    // System.Diagnostics.Debug - however we use the redirection from the Console output
                    foreach (HtmlTextWriter htmlTestWriter in htmlWriters.Values)
                    {
                        htmlTestWriter.WriteLine(template.TransformText());

                        // styling applies to the table !
                        htmlTestWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "2px");
                        htmlTestWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "lightgrey");
                        htmlTestWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
                        htmlTestWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse");
                        htmlTestWriter.AddAttribute("id", "individual_test_results");
                        htmlTestWriter.RenderBeginTag(HtmlTextWriterTag.Table);
                        htmlTestWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);
                        htmlTestWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                        htmlTestWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                        htmlTestWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                        htmlTestWriter.Write("Test Name");
                        htmlTestWriter.RenderEndTag();
                        htmlTestWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                        htmlTestWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                        htmlTestWriter.Write("Result");
                        htmlTestWriter.RenderEndTag();
                        htmlTestWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                        htmlTestWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                        htmlTestWriter.Write("User Under Test");
                        htmlTestWriter.RenderEndTag();
                        htmlTestWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                        htmlTestWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                        htmlTestWriter.Write("Details");
                        htmlTestWriter.RenderEndTag();
                        htmlTestWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "lightgrey");
                        htmlTestWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                        htmlTestWriter.Write("Explanations/Observations");
                        htmlTestWriter.RenderEndTag();
                        htmlTestWriter.RenderEndTag();

                        htmlTestWriter.RenderEndTag();
                    }
                }
                else
                {
                    filePath = System.IO.Path.Combine(@"C:\\QA_LOGS\\", "QA_Reporting_" + countForFileName + ".html");
                    System.Diagnostics.Debug.WriteLine(filePath);
                    streamWriter = new StreamWriter(filePath);
                    // Console.SetOut(streamWriter);
                    htmlWriter = new HtmlTextWriter(streamWriter);

                    // TODO : Linq query replace ?
                    int countProfilesWithoutWarning = 0;
                    foreach(var entry in statsMapUpiTraceFailureCalledAtLeastOnce)
                    {
                        if(entry.Value == false)
                        {
                            countProfilesWithoutWarning++;
                        }
                    }

                    int totalCountErrors = 0;
                    Dictionary<SeverityState, int> countBySeverity = new Dictionary<SeverityState,int>();
                    foreach(KeyValuePair<string,Dictionary<SeverityState,int>> entry in statsCountFailuresTypesPerTest)
                    {
                        // deduce the warnings from the count of successful tests because it is easier to read on the report
                        if (entry.Value.ContainsKey(SeverityState.SUCCESS) && entry.Value.ContainsKey(SeverityState.WARNING))
                        {
                            entry.Value[SeverityState.SUCCESS] -= entry.Value[SeverityState.WARNING];
                        }
                        else
                        {

                        }

                       foreach(KeyValuePair<SeverityState,int> subEntry in entry.Value)
                       {
                           if(!countBySeverity.ContainsKey(subEntry.Key))
                           {
                               countBySeverity.Add(subEntry.Key, 0);
                           }
                           countBySeverity[subEntry.Key] += subEntry.Value;

                           if (subEntry.Key != SeverityState.FALSE_POSITIVE)
                           {
                               totalCountErrors += subEntry.Value;
                           }
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
                        CountProfilesTested = statsCountProfilesProcessed,
                        CountProfilesWithoutWarnings = countProfilesWithoutWarning,
                        CountErrors = totalCountErrors,
                        OverviewCountBySeverityState = countBySeverity,
                        OverviewCountByObservationType = countByObservation.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value),
                        TestNames = statsCountFailuresTypesPerTest.Keys.ToList(),
                        ByTestNameCountBySeverityState = statsCountFailuresTypesPerTest,
                        ByTestNameCountByObservationType = statsCountObservationTypesPerTest,
                        CountTestsRun = statsCountProfilesProcessed * statsCountFailuresTypesPerTest.Keys.ToList().Count(),
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
                }
            }
            catch (IOException ioe)
            {
                System.Console.WriteLine(ioe.StackTrace);
                System.Diagnostics.Debug.WriteLine(ioe.StackTrace);
            }
        }

        private void CleanUpConsoleRedirectToFile()
        {
            try
            {
                if (htmlWriter == null)
                {
                    var template = new JavascriptForTable();

                    foreach(HtmlTextWriter htmlTestWriter in htmlWriters.Values)
                    {
                        htmlTestWriter.WriteLine(template.TransformText());

                        htmlTestWriter.WriteEndTag("table");
                        htmlTestWriter.WriteEndTag("body");
                        htmlTestWriter.WriteEndTag("html");
                        htmlTestWriter.Close();
                    }
                }
                else
                {
                    htmlWriter.WriteEndTag("table");
                    htmlWriter.WriteEndTag("body");
                    htmlWriter.WriteEndTag("html");
                    htmlWriter.Close();
                    htmlWriter = null;
                }
            }
            catch (IOException ioe)
            {
                System.Console.WriteLine(ioe.StackTrace);
                System.Diagnostics.Debug.WriteLine(ioe.StackTrace);
            }

            // Recover the standard output stream so that a  
            // completion message can be displayed.
            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// This method will :
        /// - Try to extract the simple value from the XML provided.
        /// - Perform the comparison between the two values.
        /// - Try to interpret possible explanations if the test fails.
        /// - Transmit test results for logging.
        /// </summary>
        /// <param name="oldServiceData">XML received from the old service.</param>
        /// <param name="oldValueXMLPath">XML Path where to find the value to compare.</param>
        /// <param name="newValue">Value to compare returned by the new service.</param>
        /// <param name="newUserId">User Unique Identifier in the new service.</param>
        /// <param name="oldUPI">User Unique Identifier in the old service.</param>
        /// <param name="descriptionTask">Human-readable description of the test.</param>
        /// <param name="newServiceUrl">URL of the new service being called (for the specific User Identifier).</param>
        /// <param name="memberName">Automatically populated - name of calling method.</param>
        /// <returns>Flag indicating success of the test performed.</returns>
        public static bool HandleSimpleStringCompare(XDocument oldServiceData, string oldValueXMLPath, string newValue, int newUserId, int oldUPI, string descriptionTask, string newServiceUrl, [CallerMemberName] string memberName = "")
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Processing user " + newUserId);
#endif

            bool success = true;
            string oldValue = string.Empty;
            List<ObservationLabel> additionalObservations = new List<ObservationLabel>();
            string message = string.Empty;

            try
            {
                oldValue = oldServiceData.XPathSelectElement(oldValueXMLPath).Value;
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            if (!string.IsNullOrEmpty(oldValue) && !string.IsNullOrEmpty(newValue))
            {
                if (!sampleDataByTestName.ContainsKey(memberName))
                {
                    sampleDataByTestName.Add(memberName, newValue);
                }

                try 
                { 
                    Assert.AreEqual(oldValue, newValue, descriptionTask);
                }
                catch (AssertFailedException afe)
                {
                    success = false;
                    bool specificScenarioIdentified = false;

                    message = ReplaceProblematicTagsForHtml(afe.Message);
                    
                    // Analysis of results revealed a pattern of using spaces in populating values in the new service.
                    if(newValue == " ")
                    {
                        if (string.IsNullOrEmpty(oldValue))
                        {
                            specificScenarioIdentified = true;
                            additionalObservations.Add(ObservationLabel.VALUE_POPULATED_WITH_EMPTY_ON_NEW_SERVICE);

                            TraceFailedTest(message,
                                            newUserId,
                                            oldUPI,
                                            SeverityState.ERROR_WITH_EXPLANATION,
                                            newServiceUrl,
                                            memberName,
                                            descriptionTask,
                                            additionalObservations);
                        }
                        else
                        {
                            // still inform that the new value is an empty space
                            additionalObservations.Add(ObservationLabel.VALUE_POPULATED_WITH_EMPTY_ON_NEW_SERVICE);
                            additionalObservations.Add(ObservationLabel.MISSING_VALUES_ON_NEW_SERVICE);
                        }
                    }
                    else if(!string.IsNullOrEmpty(oldValue) && !string.IsNullOrEmpty(newValue))
                    {
                        additionalObservations.Add(ObservationLabel.WRONG_VALUE);
                    }

                    if (!specificScenarioIdentified && oldValue.Trim() == newValue.Trim())
                    {
                        specificScenarioIdentified = true;
                        additionalObservations.Add(ObservationLabel.VALUE_CONTAINS_TRAILING_WHITE_SPACES);

                        TraceFailedTest(message,
                                        newUserId,
                                        oldUPI,
                                        SeverityState.ERROR_WITH_EXPLANATION,
                                        newServiceUrl,
                                        memberName,
                                        descriptionTask,
                                        additionalObservations);
                    }

                    // Default behavior, when no explanation can be provided
                    if (!specificScenarioIdentified)
                    {
                        TraceFailedTest(ReplaceProblematicTagsForHtml(afe.Message),
                                        newUserId,
                                        oldUPI,
                                        SeverityState.ERROR,
                                        newServiceUrl,
                                        memberName,
                                        descriptionTask,
                                        additionalObservations);
                    }
                }
            }
            else
            {
                additionalObservations.Add(ObservationLabel.VALUES_NOT_POPULATED);

                TraceFailedTest(message, 
                                newUserId, 
                                oldUPI, 
                                SeverityState.WARNING, 
                                newServiceUrl,
                                memberName,
                                descriptionTask,
                                additionalObservations);
            }

            if (success)
            {
                TraceSuccessTest(memberName);
            }

            return success;
        }

        /// <summary>
        /// Similar to <see cref="HandleSimpleStringCompare"/> but the extraction of the values from the old service remains in the specific
        /// test class because the parsing is specific to the specific subset of data being extracted.
        /// This method will :
        /// - Perform the comparison between the two lists of values.
        /// - Try to interpret possible explanations if the test fails.
        /// - Transmit test results for logging.
        /// </summary>
        /// <param name="oldValues">Values to compare returned by the old service.</param>
        /// <param name="newValues">Values to compare returned by the new service.</param>
        /// <param name="newUserId">User Unique Identifier in the new service.</param>
        /// <param name="oldUPI">User Unique Identifier in the old service</param>
        /// <param name="descriptionTask">Human-readable description of the test.</param>
        /// <param name="newServiceUrl">URL of the new service being called (for the specific User Identifier).</param>
        /// <param name="memberName">Automatically populated - name of calling method.</param>
        /// <returns>Flag indicating success of the test performed.</returns>
        public static bool HandleComparingSimpleCollectionString(List<string> oldValues, List<string> newValues, int newUserId, int oldUPI, string descriptionTask, string newServiceUrl, [CallerMemberName] string memberName = "")
        {
            bool success = true;
            int oldValuesCount = oldValues.Count();
            int newValuesCount = newValues.Count();
            string message = string.Empty;
            List<ObservationLabel> additionalObservations = new List<ObservationLabel>();

            // if values are populated on one side or the other
            if (oldValuesCount > 0 || newValuesCount > 0)
            {
                if (!sampleDataByTestName.ContainsKey(memberName))
                {
                    sampleDataByTestName.Add(memberName, newValues.First());
                }

                try 
                { 
                    CollectionAssert.AreEquivalent(oldValues, newValues, descriptionTask);
                }
                catch (AssertFailedException afe)
                {
                    success = false;
                    bool specificScenarioIdentified = false;

                    message = ProvideFormattedDetailedContentOfComparedLists(oldValues, newValues, ReplaceProblematicTagsForHtml(afe.Message));

                    // if there are more values on the old side
                    // check whether the values are really missing on the new side
                    // or if they were all doublons - which ends up being a FALSE POSITIVE
                    if(oldValuesCount > newValuesCount)
                    {
                        var differenceQueryToAvoidDoublons = oldValues.Except(newValues);

                        if (differenceQueryToAvoidDoublons.Count() == 0)
                        {
                            specificScenarioIdentified = true;
                            additionalObservations.Add(ObservationLabel.MORE_DUPLICATED_VALUES_ON_OLD_SERVICE);

                            TraceFailedTest(message,
                                            newUserId,
                                            oldUPI,
                                            SeverityState.FALSE_POSITIVE,
                                            newServiceUrl,
                                            memberName,
                                            descriptionTask,
                                            additionalObservations);
                        }
                        else
                        {
                            // TODO: check !
                            // additionalObservations.Add(ObservationLabel.MISSING_VALUES_ON_NEW_SERVICE);
                        }
                    }

                    // check for doublons on new side anyway
                    var differenceQueryCheckDoublonsInNewService = newValues.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key);
                    if (differenceQueryCheckDoublonsInNewService.Count() > 0)
                    {
                        additionalObservations.Add(ObservationLabel.DUPLICATED_VALUES_ON_NEW_SERVICE);
                    }

                    // if there are more values on the new side
                    // it could be that the system has corrected missing data from the old service
                    // a warning will be issued anyway, but if all the old value is found in the new service, it will not be ranked as an error
                    // since consistency is maintained
                    // e.g. the old service does not always An Identifier for the Organization
                    if (newValuesCount > oldValuesCount)
                    {
                        try
                        {
                            // TO DO : The evaluation of success may be delegated to a higher level - it could be that no OrgID is returned BUT all the dept names.
                            // This is why this ERROR is ranked as WARNING.
                            CollectionAssert.IsSubsetOf(oldValues, newValues);

                            // test succeeded - the old values are all found in the new service
                            specificScenarioIdentified = true;

                            additionalObservations.Add(ObservationLabel.MORE_VALUES_ON_NEW_SERVICE);
                            additionalObservations.Add(ObservationLabel.ALL_VALUES_OF_OLD_SUBSET_FOUND);

                            // even if subset is a match, still need to inform about the lack of values on the old side - make sure the values on the new side make sense !
                            TraceFailedTest(message,
                                            newUserId,
                                            oldUPI,
                                            SeverityState.WARNING,
                                            newServiceUrl,
                                            memberName,
                                            descriptionTask,
                                            additionalObservations);

                        } catch (AssertFailedException)
                        {
                            specificScenarioIdentified = false;
                            additionalObservations.Add(ObservationLabel.MISSING_VALUES_ON_NEW_SERVICE);
                        }
                    }

                    // check if some of the inconsistencies are due to trailing spaces in the single string values
                    var missingOldValues = oldValues.Except(newValues);
                    var missingNewValues = newValues.Except(oldValues);

                    IEnumerable<string> trimmedMissingOldValues = missingOldValues.Select(s => s.Trim());
                    IEnumerable<string> trimmedMissingNewValues = missingNewValues.Select(s => s.Trim());

                    var leftovers = trimmedMissingOldValues.Except(trimmedMissingNewValues);
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(leftovers.Count());
#endif

                    if(leftovers.Count() != missingOldValues.Count())
                    {
                        additionalObservations.Add(ObservationLabel.VALUE_CONTAINS_TRAILING_WHITE_SPACES);

                        if (leftovers.Count() == 0)
                        {
                            specificScenarioIdentified = true;
                            TraceFailedTest(message,
                                            newUserId,
                                            oldUPI,
                                            SeverityState.WARNING,
                                            newServiceUrl,
                                            memberName,
                                            descriptionTask,
                                            additionalObservations);
                        }
                    }

                    // default behavior
                    if (!specificScenarioIdentified)
                    {
                        TraceFailedTest(message,
                                        newUserId,
                                        oldUPI,
                                        SeverityState.ERROR,
                                        newServiceUrl,
                                        memberName,
                                        descriptionTask,
                                        additionalObservations);
                    }
                }
            }
            else
            {
                additionalObservations.Add(ObservationLabel.VALUES_NOT_POPULATED);
                // The field being compared is not populated on both sides.
                TraceFailedTest(message, 
                                newUserId,
                                oldUPI, 
                                SeverityState.WARNING, 
                                newServiceUrl,
                                memberName,
                                descriptionTask,
                                additionalObservations);
            }

            if (success)
            {
                TraceSuccessTest(memberName);
            }

            return success;
        }

        /// <summary>
        /// Appends two HTML lists indicating duplicates and missing/mismatched values.
        /// </summary>
        /// <param name="oldValues">Values to compare returned by the old service.</param>
        /// <param name="newValues">Values to compare returned by the new service.</param>
        /// <param name="messageToAppendTo">Optional input text to which the content generated here would be appended to.</param>
        /// <returns>The enhanced message.</returns>
        private static string ProvideFormattedDetailedContentOfComparedLists(List<string> oldValues, List<string> newValues, string messageToAppendTo = "")
        {
            bool includeDoublon = false;
            bool includeMissing = false;

            messageToAppendTo += "<br/><br/>Values returned by the old service <span style=\"color:red;\">[" + oldValues.Count + "]</span> :<ul>";

            var potentialDuplicates = oldValues.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key);

            foreach (string oldValue in oldValues)
            {
                includeDoublon = false;
                includeMissing = false;

                if (potentialDuplicates.Contains(oldValue))
                {
                    includeDoublon = true;
                }

                if (!newValues.Contains(oldValue))
                {
                    includeMissing = true;
                }

                messageToAppendTo += "<li>" + oldValue + (includeMissing ? SPAN_MISSING : string.Empty) + (includeDoublon ? SPAN_DOUBLON : string.Empty) + "</li>";
            }
            messageToAppendTo += "</ul>";

            potentialDuplicates = newValues.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key);

            messageToAppendTo += "<br/>Values returned by the new service <span style=\"color:red;\">[" + newValues.Count + "]</span> :<ul>";
            foreach (string newValue in newValues)
            {
                includeDoublon = false;
                includeMissing = false;

                if (potentialDuplicates.Contains(newValue))
                {
                    includeDoublon = true;
                }

                if (!oldValues.Contains(newValue))
                {
                    includeMissing = true;
                }

                messageToAppendTo += "<li>" + newValue + (includeMissing ? SPAN_MISSING : string.Empty) + (includeDoublon ? SPAN_DOUBLON : string.Empty) + "</li>";
            }
            messageToAppendTo += "</ul>";
            return messageToAppendTo;
        }

        /// <summary>
        /// Replaces the default characters used for describing the mismatched values of the Assert so that their content
        /// is not (mis)interpreted as HTML content.
        /// </summary>
        /// <param name="message">The string to clean.</param>
        /// <returns>The cleansed string.</returns>
        /// <remarks>This process has to take place before HTML content is generated for visualization on the report/includes exceptions to be rendered as HTML.</remarks>
        private static String ReplaceProblematicTagsForHtml(string message)
        {
            message = message.Replace("<", "<span style='color:red;'>[");
            message = message.Replace(">", "]</span>");
            // TODO : change quick fix here - because of call sequence - regex ?
            message = message.Replace("<span style='color:red;'>[br/]</span>", "<br/>");
            message = message.Replace("<span style='color:red;'>[/b]</span>", "</b>");
            message = message.Replace("<span style='color:red;'>[b]</span>", "<b>");
            message = message.Replace("[br/]", "<br/>");
            message = message.Replace("[/b]", "</b>");
            message = message.Replace("[b]", "<b>");
            return message;
        }

        /// <summary>
        /// Keep track of test runs that did not fail for statistics purposes.
        /// </summary>
        /// <param name="memberName">Test method generated name.</param>
        /// <remarks>Should always be called last.</remarks>
        public static void TraceSuccessTest(string memberName)
        {
            if (!statsCountFailuresTypesPerTest.ContainsKey(memberName))
            {
                // initialize all the possible combinations for the given test name
                statsCountFailuresTypesPerTest.Add(memberName, new Dictionary<SeverityState, int>());
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.ERROR, 0);
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.ERROR_WITH_EXPLANATION, 0);
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.FALSE_POSITIVE, 0);
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.WARNING, 0);
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.SUCCESS, 0);
            }

            // increase call counter
            statsCountFailuresTypesPerTest[memberName][SeverityState.SUCCESS]++;
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
        public static void TraceFailedTest(string errorMessage, int userId, int upi, SeverityState severity, string newServiceUrl, string memberName, string taskDescription, List<ObservationLabel> observations)
        {
            var failReport = new AssertFailedReport
            {
                Message = errorMessage,
                TestName = memberName,
                UserId = userId,
                UPI = upi,
                FailureType = severity,
                OldUrl = OLD_SERVICE_URL_BASE + upi,
                NewUrl = newServiceUrl,
                TaskDescription = taskDescription,
                Observations = observations
            };

            var template = new TestFailedReportTemplate();
            template.Session = new Dictionary<string, object>()
            {
                { "FailedReport", failReport }
            };

            template.Initialize();

            if (htmlWriters.ContainsKey(memberName))
            {
                htmlWriters[memberName].WriteLine(template.TransformText());
            }
            else
            {
                System.Console.WriteLine(memberName);
            }

            // keeping track of profiles without failures by logging any failure happening
            if (statsMapUpiTraceFailureCalledAtLeastOnce.ContainsKey(upi))
            {
                statsMapUpiTraceFailureCalledAtLeastOnce[upi] = true;
            }
            else
            {
                // no reason to happen !
                // System.Console.WriteLine("Missing UPI to track stats : " + upi);
            }
            
            if(!statsCountFailuresTypesPerTest.ContainsKey(memberName))
            {
                // initialize all the possible combinations for the given test name
                statsCountFailuresTypesPerTest.Add(memberName, new Dictionary<SeverityState,int>());
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.ERROR,0);
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.ERROR_WITH_EXPLANATION,0);
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.FALSE_POSITIVE,0);
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.WARNING,0);
                statsCountFailuresTypesPerTest[memberName].Add(SeverityState.SUCCESS, 0);
            }

            // increase call counter
            statsCountFailuresTypesPerTest[memberName][severity]++;

            if (!statsCountObservationTypesPerTest.ContainsKey(memberName))
            {
                // initialize all the possible combinations for the given test name
                statsCountObservationTypesPerTest.Add(memberName, new Dictionary<ObservationLabel, int>());
                statsCountObservationTypesPerTest[memberName].Add(ObservationLabel.ALL_VALUES_OF_OLD_SUBSET_FOUND, 0);
                statsCountObservationTypesPerTest[memberName].Add(ObservationLabel.DUPLICATED_VALUES_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[memberName].Add(ObservationLabel.MORE_DUPLICATED_VALUES_ON_OLD_SERVICE, 0);
                statsCountObservationTypesPerTest[memberName].Add(ObservationLabel.MORE_VALUES_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[memberName].Add(ObservationLabel.VALUE_CONTAINS_TRAILING_WHITE_SPACES, 0);
                statsCountObservationTypesPerTest[memberName].Add(ObservationLabel.VALUE_POPULATED_WITH_EMPTY_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[memberName].Add(ObservationLabel.VALUES_NOT_POPULATED, 0);
                statsCountObservationTypesPerTest[memberName].Add(ObservationLabel.MISSING_VALUES_ON_NEW_SERVICE, 0);
                statsCountObservationTypesPerTest[memberName].Add(ObservationLabel.WRONG_VALUE, 0);
            }

            // increase call counter
            foreach (ObservationLabel label in observations)
            {
                statsCountObservationTypesPerTest[memberName][label]++;
            }
        }

        #endregion
    }
}