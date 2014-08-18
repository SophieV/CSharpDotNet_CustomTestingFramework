using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using TestMVC4App.Templates;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Test class evaluating the values populating the <see cref="YSM.PMS.Service.Common.DataTransfer.UserGeneralInfo"/> instance
    /// returned by the call to the <see cref="YSM.PMS.Service.Common.DataTransfer.IUserService.GetUserById()"/> method.
    /// </summary>
    /// <remarks>It should be called first, as it contains the UserId needed for the next tests.</remarks>
    public class UserBasicInfoTest : ITestStructure
    {
        private const String NEW_SERVICE_SUB_URL_BASE = "Users/Upi/";
        private String NEW_SERVICE_URL_BASE = WebConfigurationManager.AppSettings["ProfileServiceBaseAddress"] + NEW_SERVICE_SUB_URL_BASE;

        /// <summary>
        /// Exposes the UserId used as main reference for querying the new service's interface.
        /// </summary>
        public int MappedUserId { get; set; }

        /// <summary>
        /// Main entry point that manages all the individuals tests for the fields 
        /// of the <see cref="YSM.PMS.Service.Common.DataTransfer.UserGeneralInfo"/> class.
        /// </summary>
        /// <param name="newServiceAccessor">Instance on which the call to the specific
        /// <see cref="YSM.PMS.Service.Common.DataTransfer.IUserService.GetUserById()"/> method tested will be made.</param>
        /// <param name="upi">Old Identifier of the User.</param>
        /// <param name="oldServiceXMLContent">Result returned by the old service - to be parsed.</param>
        public void RunAllTests(IUsersClient newServiceAccessor, int upi, XDocument oldServiceXMLContent, int userId = 0)
        {
            var newUserBasicInfo = newServiceAccessor.GetUserByUpi(upi);

            // if the UPIs do not match, we are not dealing with the same entry !
            if (UserBasicInfo_UPI_Test(newUserBasicInfo, oldServiceXMLContent))
            {
                MappedUserId = newUserBasicInfo.UserId;

                //ProfileEditor userEditor = null;

                //if (newUserBasicInfo.UserEditors.Count() > 0)
                //{
                //    newUserBasicInfo.UserEditors.First();
                //}

                UserBasicInfo_LastName_Test(newUserBasicInfo, oldServiceXMLContent);
                UserBasicInfo_Email_Test(newUserBasicInfo, oldServiceXMLContent);
                UserBasicInfo_MiddleName_Test(newUserBasicInfo, oldServiceXMLContent);
                UserBasicInfo_FirstName_Test(newUserBasicInfo, oldServiceXMLContent);
                UserBasicInfo_Gender_Test(newUserBasicInfo, oldServiceXMLContent);
                UserBasicInfo_NetId_Test(newUserBasicInfo, oldServiceXMLContent);
                UserBasicInfo_PageName_Test(newUserBasicInfo, oldServiceXMLContent);
                UserBasicInfo_Suffix_Test(newUserBasicInfo, oldServiceXMLContent);
            }
        }

        #region Field Comparison Tests

        private bool UserBasicInfo_UPI_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            return UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                          "/Faculty/facultyMember/UPI", 
                                                                          newServiceData.Upi.ToString(), 
                                                                          newServiceData.UserId, 
                                                                          newServiceData.Upi, 
                                                                          "ROOT Comparing UPI", 
                                                                          NEW_SERVICE_URL_BASE + newServiceData.Upi);
        }

        private void UserBasicInfo_Gender_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                   "/Faculty/facultyMember/gender", 
                                                                   newServiceData.Gender, 
                                                                   newServiceData.UserId, 
                                                                   newServiceData.Upi, 
                                                                   "Comparing Gender", 
                                                                   NEW_SERVICE_URL_BASE + newServiceData.Upi);
        }

        private void UserBasicInfo_Idx_Test()
        {
            // TODO : not implemented yet
            throw new AssertInconclusiveException();
        }

        private void UserBasicInfo_LicenseNumber_Test()
        {
            // TODO : not implemented yet
            throw new AssertInconclusiveException();
        }

        private void UserBasicInfo_LastName_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                    "/Faculty/facultyMember/lastname", 
                                                                    newServiceData.LastName, 
                                                                    newServiceData.UserId, 
                                                                    newServiceData.Upi, 
                                                                    "Comparing LastName", 
                                                                    NEW_SERVICE_URL_BASE + newServiceData.Upi);
        }

        private void UserBasicInfo_FirstName_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                   "/Faculty/facultyMember/firstname", 
                                                                   newServiceData.FirstName, 
                                                                   newServiceData.UserId, 
                                                                   newServiceData.Upi, 
                                                                   "Comparing FirstName",
                                                                   NEW_SERVICE_URL_BASE + newServiceData.Upi);
        }

        private void UserBasicInfo_MiddleName_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                   "/Faculty/facultyMember/middle", 
                                                                   newServiceData.MiddleName, 
                                                                   newServiceData.UserId, 
                                                                   newServiceData.Upi, 
                                                                   "Comparing MiddleName", 
                                                                   NEW_SERVICE_URL_BASE + newServiceData.Upi);
        }

        private void UserBasicInfo_Email_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                   "/Faculty/facultyMember/emailAddress", 
                                                                   newServiceData.YaleEmail, 
                                                                   newServiceData.UserId, 
                                                                   newServiceData.Upi, 
                                                                   "Comparing Email", 
                                                                   NEW_SERVICE_URL_BASE + newServiceData.Upi);
        }

        private void UserBasicInfo_NetId_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                   "/Faculty/facultyMember/netID", 
                                                                   newServiceData.NetId.ToString(), 
                                                                   newServiceData.UserId, 
                                                                   newServiceData.Upi, 
                                                                   "Comparing NetId", 
                                                                   NEW_SERVICE_URL_BASE + newServiceData.Upi);
        }

        private void UserBasicInfo_PageName_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                   "/Faculty/facultyMember/pageName", 
                                                                   newServiceData.PageName, 
                                                                   newServiceData.UserId, 
                                                                   newServiceData.Upi, "Comparing PageName", 
                                                                   NEW_SERVICE_URL_BASE + newServiceData.Upi);
        }

        private void UserBasicInfo_Suffix_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                   "/Faculty/facultyMember/Suffix", 
                                                                   newServiceData.Suffix, 
                                                                   newServiceData.UserId, 
                                                                   newServiceData.Upi, 
                                                                   "Comparing Suffix", 
                                                                   NEW_SERVICE_URL_BASE + newServiceData.Upi);
        }

        private void UserBasicInfo_Npi_Test()
        {
            // TODO : not implemented yet
            throw new AssertInconclusiveException();
        }

        #endregion
    }
}