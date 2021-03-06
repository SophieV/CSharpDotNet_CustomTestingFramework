﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Configuration;
using System.Xml.Linq;
using YSM.PMS.Web.Service.Clients;
using System.Diagnostics;
using System.Xml.XPath;
using YSM.PMS.Web.Service.DataTransfer.Models;

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
            get { return "http://yale-faculty.photobooks.com/directory/XMLProfile.asp?OldId="; }
        }

        public static bool IsDebugMode { get { return  true;  } }

        private const int MaxProfilesForOneFile = 50000;

        private HashSet<int> OldIdList = new HashSet<int>();

        public override void RunAllTests()
        {
            //init all loop variables
            string oldServiceURL;
            string oldServiceXMLOutput;

            string errorType = string.Empty;
            string errorMessage = string.Empty;

            OldIdList = new DatabaseFacade().ConnectToDataSourceAndRetrieveOldIds("a30");

            var singleProfileWatch = new Stopwatch();
            var testingDurationWatch = new Stopwatch();
            var oldServiceDataWatch = new Stopwatch();
            var newServiceDataWatch = new Stopwatch();

            testingDurationWatch.Start();

            LogManager.Instance.StartWritingReports();

            if (TestSuiteUser.IsDebugMode)
            {
                OldIdList = new HashSet<int>() { 12386392, 12789020, 10254830, 10345899, 10643722, 10356439, 13087115, 10098855, 10445859, 12445467, 10044421, 11842460, 10273683, 11228709, 12528002, 10410346, 10071485, 10934133, 12149599, 12641341, 10151776, 10290564, 11091604, 11472557, 12149599, 13132301, 10146455, 13157019, 10646102, 12192949, 10106216, 12268225, 11161032, 11832447, 11436806, 10736848 };
            }

            bool keepGoing = true;
            //loop on the list of all OldIds retrieved from the old database
            foreach (int OldId in OldIdList)
            //Parallel.ForEach(OldIdList, OldId =>
            {
                if (keepGoing)
                {
                    oldServiceXMLOutput = string.Empty;

                    System.Diagnostics.Debug.WriteLine(OldId);
                    System.Console.Out.WriteLine(OldId);

                    LogManager.Instance.StatsCountTotalOldIds++;

                    if (false && LogManager.Instance.StatsCountTotalOldIds > 1000)
                    {
                        keepGoing = false;
                    }

                    
                    singleProfileWatch.Start();

                    if (LogManager.Instance.StatsCountTotalOldIds % MaxProfilesForOneFile == 0)
                    {
                        // change to next output files
                        LogManager.Instance.StartWritingReports();
                    }

                    //go to the old service and retrieve the data
                    oldServiceURL = this.BuildOldServiceFullURL(OldId);

                    //Find a way to set the 'Timeout' property in Milliseconds. The old service can be slow.
                    //we also need exception handling!
                    oldServiceDataWatch.Start();
                    try
                    {
                        using (var webClient = new TimeoutExtendedWebClient())
                        {
                            try
                            {
                                webClient.Encoding = System.Text.Encoding.UTF8;
                                oldServiceXMLOutput = webClient.DownloadString(oldServiceURL);
                            }
                            catch (WebException we)
                            {
                                System.Diagnostics.Debug.WriteLine(we.StackTrace);
                                System.Diagnostics.Debug.WriteLine("Trying to get data from old service for OldId " + OldId + " again");

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
                        System.Diagnostics.Debug.WriteLine("Some inactive profiles do not have the flag set in the old service. Likely to be the case here.");
                    }
                    oldServiceDataWatch.Stop();

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

                                newServiceDataWatch.Start();

                                newDataBasic = usersClient.GetUserByOldId(OldId);

                                newServiceDataWatch.Stop();

                                userId = newDataBasic.UserId;
                                pageName = newDataBasic.PageName;

                                // This service has to be called first because it will provided the User ID mapped to the OldId for the next calls.
                                oldDataSubset = rootDepthOnly;
                                testUnit = new TestUnitUserBasicInfo(this, newDataBasic);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    OldId, 
                                    oldDataSubset,
                                    -1,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                newServiceDataWatch.Start();

                                newData = usersClient.GetUserCompleteByPageName(pageName);

                                newServiceDataWatch.Stop();

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.title.ToString(), new List<XElement>(rootDepthOnly));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.cv.ToString(), new List<XElement>(oldDataSubset));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.language.ToString(), new List<XElement>(oldDataSubset));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.department.ToString(), new List<XElement>(oldDataSubset));
                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.treeDepartments.ToString(), new List<XElement>(oldDataSubset));
                                testUnit = new TestUnitUserGeneralInfo(this, newData);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    OldId,
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
                                    OldId,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.featuredPublication.ToString());
                                testUnit = new TestUnitUserPublicationInfo(this, newData.UserPublications);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    OldId,
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
                                    OldId,
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
                                    OldId,
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
                                    OldId,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.boardCertification.ToString(), new List<XElement>(rootDepthOnly));
                                testUnit = new TestUnitUserPatientCareInfo(this, newData.BoardCertifications, newData.PatientCare);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    OldId,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = ParsingHelper.ParseListNodes(oldData, EnumOldServiceFieldsAsKeys.treeDepartments.ToString());
                                testUnit = new TestUnitUserOrganization(this, newData.OrganizationsTree);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    OldId,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                oldDataSubset = rootDepthOnly;
                                testUnit = new TestUnitUserGeneralContact(this, newData.GeneralContact);
                                allTheTests.Add(testUnit);
                                testUnit.ProvideData(
                                    OldId,
                                    oldDataSubset,
                                    userId,
                                    pageName);
                                testUnit.RunAllTests();
                                oldDataSubset = null;

                                foreach (var test in allTheTests)
                                {
                                    test.ComputerOverallResults();
                                    allTheResults.UnionWith(test.DetailedResults.Values);

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
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine(e.StackTrace);
                            System.Diagnostics.Debug.WriteLine(e.StackTrace);
                            System.Console.WriteLine(e.InnerException);
                            System.Diagnostics.Debug.WriteLine(e.InnerException);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No data returned by old service for OldId " + OldId);
                        System.Console.Out.WriteLine("No data returned by old service for OldId " + OldId);
                    }

                    singleProfileWatch.Stop();

                    LogManager.Instance.LogProfileResult(OldId, allTheResults, singleProfileWatch.Elapsed, oldServiceDataWatch.Elapsed, newServiceDataWatch.Elapsed);

                    singleProfileWatch.Reset();
                    oldServiceDataWatch.Reset();
                    newServiceDataWatch.Reset();
                }
            }
                testingDurationWatch.Stop();

                LogManager.Instance.LogSummary(testingDurationWatch.Elapsed, errorType, errorMessage);

                LogManager.Instance.Dispose();
        }
    }
}