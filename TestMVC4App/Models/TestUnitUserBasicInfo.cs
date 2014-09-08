using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Test class evaluating the values populating the <see cref="YSM.PMS.Service.Common.DataTransfer.UserGeneralInfo"/> instance
    /// returned by the call to the <see cref="YSM.PMS.Service.Common.DataTransfer.IUserService.GetUserById()"/> method.
    /// </summary>
    /// <remarks>It should be called first, as it contains the UserId needed for the next tests.</remarks>
    public class TestUnitUserBasicInfo : TestUnit
    {
        private UsersClient newServiceAccessor;
        private XDocument oldServiceData;
        private int upi;

        public override string newServiceURLExtensionBeginning
        {
            get { return "Users/Upi/"; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Exposes the UserId used as main reference for querying the new service's interface.
        /// </summary>
        public int MappedUserId { get; set; }

        public TestUnitUserBasicInfo(TestSuite parent) : base(parent)
        {

        }

        /// <summary>
        /// Main entry point that manages all the individuals tests for the fields 
        /// of the <see cref="YSM.PMS.Service.Common.DataTransfer.UserGeneralInfo"/> class.
        /// </summary>
        /// <param name="newServiceAccessor">Instance on which the call to the specific
        /// <see cref="YSM.PMS.Service.Common.DataTransfer.IUserService.GetUserById()"/> method tested will be made.</param>
        /// <param name="upi">Old Identifier of the User.</param>
        /// <param name="oldServiceXMLContent">Result returned by the old service - to be parsed.</param>
        protected override void RunAllSingleTests()
        {
            var newServiceInfo = newServiceAccessor.GetUserByUpi(upi);

            try
            {
                MappedUserId = newServiceInfo.UserId;
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }

            MappedUserId = newServiceInfo.UserId;
            this.CompareAndLog_Test("UserBasicInfo_UPI_Test", "Comparing UPI", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/UPI", newServiceInfo.Upi.ToString());
            this.CompareAndLog_Test("UserBasicInfo_LastName_Test", "Comparing LastName", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/lastname", newServiceInfo.LastName);
            this.CompareAndLog_Test("UserBasicInfo_Email_Test", "Comparing Email", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/emailAddress", newServiceInfo.YaleEmail);
            this.CompareAndLog_Test("UserBasicInfo_MiddleName_Test", "Comparing MiddleName", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/middle", newServiceInfo.MiddleName);
            this.CompareAndLog_Test("UserBasicInfo_FirstName_Test", "Comparing FirstName", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/firstname", newServiceInfo.FirstName);
            this.CompareAndLog_Test("UserBasicInfo_Gender_Test", "Comparing Gender", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/gender", newServiceInfo.Gender);
            this.CompareAndLog_Test("UserBasicInfo_NetId_Test", "Comparing NetId", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/netID", newServiceInfo.NetId);
            this.CompareAndLog_Test("UserBasicInfo_PageName_Test", "Comparing PageName", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/netID", newServiceInfo.NetId);
            this.CompareAndLog_Test("UserBasicInfo_Suffix_Test", "Comparing Suffix", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/Suffix", newServiceInfo.Suffix);
            this.CompareAndLog_Test("UserBasicInfo_Idx_Test", "Comparing Idx", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/pageName", newServiceInfo.PageName);
            this.CompareAndLog_Test("UserBasicInfo_LicenseNumber_Test", "Comparing License Number", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/LicenseNumber", newServiceInfo.LicenseNumber);
            this.CompareAndLog_Test("UserBasicInfo_Npi_Test", "Comparing Npi", this.MappedUserId, this.upi, oldServiceData, "/Faculty/facultyMember/Npi", newServiceInfo.Npi);
            UserBasicInfo_UserEditors_Test(newServiceInfo, oldServiceData);

            ComputeOverallSeverity();
        }

        public void ProvideData(XDocument oldData, UsersClient newDataAccessor, int upi)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.upi = upi;
        }

        #region Field Comparison Tests

        // TODO: test ! The node name is not reliable ! I need an example with data
        public void UserBasicInfo_UserEditors_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var oldValues = TestUnit.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/UserEditors", "emailAddress");

            var newValues = new HashSet<string>();
            if (newServiceData.UserEditors != null)
            {
                foreach (ProfileEditor profile in newServiceData.UserEditors)
                {
                    if (!string.IsNullOrEmpty(profile.YaleEmail))
                    {
                        newValues.Add(profile.YaleEmail);
                    }
                }
            }

            this.CompareAndLog_Test("UserBasicInfo_UserEditors_Test", "Comparing User Editors",this.MappedUserId,this.upi,oldValues,newValues);
        }

        #endregion
    }
}