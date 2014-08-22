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
using System.Diagnostics;

namespace TestMVC4App.Models
{
    public class UserTestSuite : TestSuite
    {
        public override string newServiceURLBase
        {
            get { return WebConfigurationManager.AppSettings["ProfileServiceBaseAddress"]; }
        }

        public override string oldServiceURLBase
        {
            get { return "http://yale-faculty.photobooks.com/directory/XMLProfile.asp?UPI="; }
        }

        private const int MaxProfilesForOneFile = 50000;

        private List<int> upiList = new List<int>();

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
            System.Diagnostics.Debug.WriteLine("Connection state is: " + conn.State.ToString());

            SqlDataReader sdr = queryCommand.ExecuteReader();

            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    upiList.Add(sdr.GetInt32(0));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No rows found.");
            }

            //foreach (int upi in upiList)
            //{
            //    System.Diagnostics.Debug.WriteLine(upi);
            //}
            sdr.Close();

            conn.Close();

            return upiList;
        }

        #endregion

        public override void RunAllTests()
        {
            //init all loop variables
            string oldServiceURL;
            string oldServiceXMLOutput;

            upiList = ConnectToDataSourceAndRetriveUPIs();

            var watch = new Stopwatch();
            watch.Start();

            LogManager.Instance.StartWritingDetailedReports();

#if DEBUG
            // upiList = new List<int>() { 10151776, 10290564, 11091604 };
#endif
            //loop on the list of all UPIs retrieved from the old database
            foreach (int upi in upiList)
            {
//#if DEBUG
                if (LogManager.Instance.StatsCountProfilesProcessed > 100)
                {
                    break;
                }
//#endif
                oldServiceXMLOutput = string.Empty;

                System.Diagnostics.Debug.WriteLine(upi);

                LogManager.Instance.StatsCountProfilesProcessed++;
                System.Diagnostics.Debug.WriteLine(LogManager.Instance.StatsCountProfilesProcessed);

                if (LogManager.Instance.StatsCountProfilesProcessed % MaxProfilesForOneFile == 0)
                {
                    // change to next output files
                    LogManager.Instance.StartWritingDetailedReports();
                }

                //go to the old service and retrieve the data
                oldServiceURL = this.BuildOldServiceFullURL(upi);

                //Find a way to set the 'Timeout' property in Milliseconds. The old service can be slow.
                //we also need exception handling!
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

                List<ResultReport> allTheResults = new List<ResultReport>();

                if (!string.IsNullOrEmpty(oldServiceXMLOutput))
                {
                    try
                    {
                        XDocument oldServiceXMLOutputDocument = XDocument.Parse(oldServiceXMLOutput);

                        var usersClient = new UsersClient();

                        // This service has to be called first because it will provided the User ID mapped to the UPI for the next calls.
                        UserBasicInfoTestUnit userBasicInfoTest = new UserBasicInfoTestUnit(this);
                        userBasicInfoTest.ProvideUserData(oldServiceXMLOutputDocument, usersClient, upi);
                        userBasicInfoTest.RunAllTests();

                        int userId = userBasicInfoTest.MappedUserId;

                        UserGeneralInfoTestUnit userGeneralInfoTest = new UserGeneralInfoTestUnit(this);
                        userGeneralInfoTest.ProvideUserData(oldServiceXMLOutputDocument, upi, usersClient, userId);
                        userGeneralInfoTest.RunAllTests();

                        userBasicInfoTest.ComputerOverallResults();
                        userGeneralInfoTest.ComputerOverallResults();
                        allTheResults.AddRange(userBasicInfoTest.DetailedResults);
                        allTheResults.AddRange(userGeneralInfoTest.DetailedResults);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No data returned by old service for UPI " + upi);
                }

                LogManager.Instance.LogProfileResult(upi, allTheResults);
            }

            LogManager.Instance.StopWritingDetailedReports();

            watch.Stop();
            LogManager.Instance.WriteSummaryReport(watch.Elapsed);

            LogManager.Instance.CleanUpResources();
        }
    }
}