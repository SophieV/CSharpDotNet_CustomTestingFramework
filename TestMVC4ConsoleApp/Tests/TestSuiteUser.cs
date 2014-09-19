using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Configuration;
using System.Xml.Linq;
using YSM.PMS.Web.Service.Clients;
using System.Diagnostics;
using System.Xml.XPath;
using System.Threading;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public sealed class TestSuiteUser : TestSuite
    {
        public override string newServiceURLBase
        {
            get { return WebConfigurationManager.AppSettings["ProfileServiceBaseAddress"]; }
        }

        public override string oldServiceURLBase
        {
            get { return "http://yale-faculty.photobooks.com/directory/XMLProfile.asp?UPI="; }
        }

        public static bool IsDebugMode { get { return true;  } }

        private const int MaxProfilesForOneFile = 50000;

        private HashSet<int> upiList = new HashSet<int>();

        public override void RunAllTests()
        {
            //init all loop variables
            string oldServiceURL;
            string oldServiceXMLOutput;

            string errorType = string.Empty;
            string errorMessage = string.Empty;

            upiList = new DatabaseFacade().ConnectToDataSourceAndRetrieveUPIs();

            Stopwatch profileWatch = null;
            var watch = new Stopwatch();
            watch.Start();

            LogManager.Instance.StartWritingDetailedReports();

            if (TestSuiteUser.IsDebugMode)
            {
                upiList = new HashSet<int>() { 10410346, 10071485, 10934133, 12149599, 12641341, 10151776, 10290564, 11091604, 11472557, 12149599, 13132301, 10146455, 13157019, 10646102, 12192949, 10106216, 12268225, 11161032, 11832447, 11436806, 10736848 };
            }

            bool keepGoing = true;
            //loop on the list of all UPIs retrieved from the old database
            foreach (int upi in upiList)
            //Parallel.ForEach(upiList, upi =>
            {
                if (keepGoing)
                {
                    oldServiceXMLOutput = string.Empty;

                    System.Diagnostics.Debug.WriteLine(upi);
                    System.Console.Out.WriteLine(upi);

                    LogManager.Instance.StatsCountTotalUpis++;

                    if (false && LogManager.Instance.StatsCountTotalUpis > 1000)
                    {
                        keepGoing = false;
                    }

                    profileWatch = new Stopwatch();
                    profileWatch.Start();

                    if (LogManager.Instance.StatsCountTotalUpis % MaxProfilesForOneFile == 0)
                    {
                        // change to next output files
                        LogManager.Instance.StartWritingDetailedReports();
                    }

                    //go to the old service and retrieve the data
                    oldServiceURL = this.BuildOldServiceFullURL(upi);

                    //Find a way to set the 'Timeout' property in Milliseconds. The old service can be slow.
                    //we also need exception handling!
                    try
                    {
                        using (var webClient = new TimeoutExtendedWebClient())
                        {
                            try
                            {
                                oldServiceXMLOutput = webClient.DownloadString(oldServiceURL);
                            }
                            catch (WebException we)
                            {
                                System.Diagnostics.Debug.WriteLine(we.StackTrace);
                                System.Diagnostics.Debug.WriteLine("Trying to get data from old service for upi " + upi + " again");

                                try
                                {
                                    oldServiceXMLOutput = webClient.DownloadString(oldServiceURL);
                                }
                                catch (WebException we2)
                                {
                                    System.Diagnostics.Debug.WriteLine(we2.StackTrace);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    }

                    var allTheTests = new HashSet<TestUnit>();
                    var allTheResults = new HashSet<ResultReport>();

                    XDocument oldServiceXMLOutputDocument = null;
                    IEnumerable<XElement> oldData = null;
                    UserBasicInfo newDataBasic = null;
                    UserCompleteInfo newData = null;
                    string pageName = string.Empty;
                    int userId;

                    if (!string.IsNullOrEmpty(oldServiceXMLOutput))
                    {
                        try
                        {
                            oldServiceXMLOutputDocument = XDocument.Parse(oldServiceXMLOutput);
                            oldData = oldServiceXMLOutputDocument.XPathSelectElements("/Faculty/facultyMember/*");

                            var rootDepthOnly = ParsingHelper.ParseListNodesOnlySameDepth(oldData, null);
                            //} 
                            //catch (Exception e)
                            //{
                            //    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                            //}

                            //try
                            //{

                            bool isInactive = false;

                            // there is a third state : "Read-Only", not used for the moment
                            isInactive = (ParsingHelper.ParseSingleValue(oldData,"Inactive") == "Yes");

                            if (!isInactive)
                            {
                                TestUnit testUnit;
                                IEnumerable<XElement> oldDataSubset = null;
                                var usersClient = new UsersClient();

                                newDataBasic = usersClient.GetUserByUpi(upi);

                                // This service has to be called first because it will provided the User ID mapped to the UPI for the next calls.
                                oldDataSubset = rootDepthOnly;
                                testUnit = new TestUnitUserBasicInfo(this, newDataBasic);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    upi, 
                                    oldDataSubset,
                                    -1,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                userId = testUnit.UserId;
                                pageName = (testUnit as TestUnitUserBasicInfo).PageName;
                                newData = usersClient.GetUserCompleteByPageName(pageName);

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.title.ToString(), new List<XElement>(rootDepthOnly));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.cv.ToString(), new List<XElement>(oldDataSubset));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.language.ToString(), new List<XElement>(oldDataSubset));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.department.ToString(), new List<XElement>(oldDataSubset));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.treeDepartments.ToString(), new List<XElement>(oldDataSubset));
                                testUnit = new TestUnitUserGeneralInfo(this, newData);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    upi,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.assistant.ToString());
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.labWebsite.ToString(), new List<XElement>(oldDataSubset));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.location.ToString(), new List<XElement>(oldDataSubset));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.mailing.ToString(), new List<XElement>(oldDataSubset),true);
                                testUnit = new TestUnitUserContactLocationInfo(this, newData.LabWebsites, newData.UserAddresses);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    upi,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.featuredPublication.ToString());
                                testUnit = new TestUnitUserPublicationInfo(this, newData.Publications);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    upi,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.researchSummary.ToString());
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.researchOverview.ToString(), new List<XElement>(oldDataSubset));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.publicHealthKeywords.ToString(), new List<XElement>(oldDataSubset));
                                testUnit = new TestUnitUserResearchInfo(this, newData.Research);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    upi,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.education.ToString());
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.training.ToString(), new List<XElement>(oldDataSubset));
                                testUnit = new TestUnitUserEducationTrainingInfo(this, newData.Educations, newData.Trainings);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    upi,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.professionalHonor.ToString());
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.professionalService.ToString(), new List<XElement>(oldDataSubset));
                                testUnit = new TestUnitUserHonorServiceInfo(this, newData.Honors, newData.Services);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    upi,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.boardCertification.ToString(), new List<XElement>(rootDepthOnly));
                                testUnit = new TestUnitUserPatientCareInfo(this, newData.BoardCertifications, newData.PatientCare);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    upi,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.treeDepartments.ToString());
                                testUnit = new TestUnitUserOrganization(this, newData.OrganizationsTree);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    upi,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                foreach (var test in allTheTests)
                                {
                                    test.ComputerOverallResults();
                                    allTheResults.UnionWith(test.DetailedResults);

                                    // log only first occurence of error - enough to generate the warning
                                    if ((test.HttpErrorHappened || test.UnknownErrorHappened) && string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorMessage = test.ErrorMessage;

                                        if (test.HttpErrorHappened)
                                        {
                                            errorType = "HTTP";
                                        }
                                        else
                                        {
                                            errorType = "UNKNOWN";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                LogManager.Instance.StatsCountProfilesIgnored++;
                            }

                            Thread.Sleep(5000);
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine(e.StackTrace);
                            System.Diagnostics.Debug.WriteLine(e.StackTrace);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No data returned by old service for UPI " + upi);
                    }

                    profileWatch.Stop();

                    LogManager.Instance.LogProfileResult(upi, allTheResults, profileWatch.Elapsed);
                }
            }
                //);

                LogManager.Instance.StopWritingDetailedReports();

                watch.Stop();
                LogManager.Instance.WriteSummaryReport(watch.Elapsed, errorType, errorMessage);

                LogManager.Instance.CleanUpResources();
        }
    }
}