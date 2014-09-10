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
        private IEnumerable<XElement> oldServiceData;
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
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_UPI, "Comparing UPI", this.MappedUserId, this.upi, oldServiceData, "UPI", newServiceInfo.Upi.ToString());
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_LastName, "Comparing LastName", this.MappedUserId, this.upi, oldServiceData, "lastname", newServiceInfo.LastName);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Email, "Comparing Email", this.MappedUserId, this.upi, oldServiceData, "emailAddress", newServiceInfo.YaleEmail);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_MiddleName, "Comparing MiddleName", this.MappedUserId, this.upi, oldServiceData, "middle", newServiceInfo.MiddleName);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_FirstName, "Comparing FirstName", this.MappedUserId, this.upi, oldServiceData, "firstname", newServiceInfo.FirstName);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Gender, "Comparing Gender", this.MappedUserId, this.upi, oldServiceData, "gender", newServiceInfo.Gender);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_NetId, "Comparing NetId", this.MappedUserId, this.upi, oldServiceData, "netID", newServiceInfo.NetId);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_PageName, "Comparing PageName", this.MappedUserId, this.upi, oldServiceData, "pageName", newServiceInfo.PageName);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Suffix, "Comparing Suffix", this.MappedUserId, this.upi, oldServiceData, "Suffix", newServiceInfo.Suffix);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Idx, "Comparing Idx", this.MappedUserId, this.upi, oldServiceData, "Idx", newServiceInfo.Idx);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_LicenseNumber, "Comparing License Number", this.MappedUserId, this.upi, oldServiceData, "LicenseNumber", newServiceInfo.LicenseNumber);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Npi, "Comparing Npi", this.MappedUserId, this.upi, oldServiceData, "Npi", newServiceInfo.Npi);
            UserBasicInfo_UserEditors_Test(newServiceInfo, oldServiceData);

            ComputeOverallSeverity();
        }

        public void ProvideData(IEnumerable<XElement> oldData, UsersClient newDataAccessor, int upi)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.upi = upi;
        }

        #region Field Comparison Tests

        // TODO: test ! The node name is not reliable ! I need an example with data
        public void UserBasicInfo_UserEditors_Test(UserBasicInfo newServiceData, IEnumerable<XElement> oldServiceData)
        {
            var oldValues = ParsingHelper.ParseListSimpleValues(oldServiceData, "UserEditors", "emailAddress");

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

            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_UserEditors_Email, "Comparing User Editors",this.MappedUserId,this.upi,oldValues,newValues);
        }

        #endregion
    }
}