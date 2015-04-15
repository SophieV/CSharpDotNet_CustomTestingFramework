using System;
using System.Collections.Generic;
using System.Web;
using YSM.PMS.Web.Service.DataTransfer.Models;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Test class evaluating the values populating the <see cref="YSM.PMS.Service.Common.DataTransfer.UserGeneralInfo"/> instance
    /// returned by the call to the <see cref="YSM.PMS.Service.Common.DataTransfer.IUserService.GetUserById()"/> method.
    /// </summary>
    /// <remarks>It should be called first, as it contains the UserId needed for the next tests.</remarks>
    public class TestUnitUserBasicInfo : TestUnit
    {
        private UserBasicInfo newData;
        public override string NewServiceURLExtensionBeginning
        {
            get { return "Users/Upi/"; }
        }

        public override string NewServiceURLExtensionEnding
        {
            get { return string.Empty; }
        }

        public TestUnitUserBasicInfo(TestSuite parent, UserBasicInfo newData) : base(parent)
        {
            this.newData = newData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>Special case where the new service needs the UPI -instead of the UserID - in the URL.</remarks>
        override protected string BuildNewServiceURL(string pageName)
        {
            if (this.Container == null)
            {
                throw new NotImplementedException();
            }

            return Container.newServiceURLBase + NewServiceURLExtensionBeginning + this.Upi + NewServiceURLExtensionEnding;
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
            try
            {
                this.UserId = this.newData.UserId;
            } 
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_UPI, "Comparing UPI", this.OldDataNodes, EnumOldServiceFieldsAsKeys.UPI.ToString(), this.newData.Upi.ToString());
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_LastName, "Comparing LastName", this.OldDataNodes, EnumOldServiceFieldsAsKeys.lastname.ToString(), HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(this.newData.LastName)));
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Email, "Comparing Email", this.OldDataNodes, EnumOldServiceFieldsAsKeys.emailAddress.ToString(), this.newData.Email);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_MiddleName, "Comparing MiddleName", this.OldDataNodes, EnumOldServiceFieldsAsKeys.middle.ToString(), HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(this.newData.MiddleName)));
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_FirstName, "Comparing FirstName", this.OldDataNodes, EnumOldServiceFieldsAsKeys.firstname.ToString(), HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(this.newData.FirstName)));
            UserBasicInfo_Gender();
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_NetId, "Comparing NetId", this.OldDataNodes, EnumOldServiceFieldsAsKeys.netID.ToString(), this.newData.NetId);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_PageName, "Comparing PageName", this.OldDataNodes, EnumOldServiceFieldsAsKeys.pageName.ToString(), HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(this.newData.PageName)));
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Idx, "Comparing Idx", this.OldDataNodes, EnumOldServiceFieldsAsKeys.Idx.ToString(), this.newData.Idx);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_LicenseNumber, "Comparing License Number", this.OldDataNodes, EnumOldServiceFieldsAsKeys.LicenseNumber.ToString(), this.newData.LicenseNumber);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Npi, "Comparing Npi", this.OldDataNodes, EnumOldServiceFieldsAsKeys.Npi.ToString(), this.newData.Npi);

            ComputeOverallSeverity();
        }

        private void UserBasicInfo_Gender()
        {
            var oldValuesMerged = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.gender.ToString()));
            var oldValues = ParsingHelper.StringToList(oldValuesMerged, ',');

            var newValues = new HashSet<string>();

            if (this.newData.Gender != null)
            {
                switch(newData.Gender)
                {
                    case "Male":
                        newValues.Add("M");
                    break;
                    case "Female":
                        newValues.Add("F");
                    break;
                    default:
                        newValues.Add("unknown");
                    break;
                }
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserBasicInfo_Gender,
                "Comparing Gender",
                oldValues,
                newValues);
        }
    }
}