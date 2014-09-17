using System;
using YSM.PMS.Service.Common.DataTransfer;

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
        public override string newServiceURLExtensionBeginning
        {
            get { return "Users/Upi/"; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return string.Empty; }
        }

        public string PageName { get; private set; }

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
        override protected string BuildNewServiceURL(int userId)
        {
            if (this.Container == null)
            {
                throw new NotImplementedException();
            }

            return Container.newServiceURLBase + newServiceURLExtensionBeginning + this.Upi + newServiceURLExtensionEnding;
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
                this.PageName = this.newData.PageName;
            } 
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_UPI, "Comparing UPI", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.UPI.ToString(), this.newData.Upi.ToString());
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_LastName, "Comparing LastName", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.lastname.ToString(), this.newData.LastName);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Email, "Comparing Email", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.emailAddress.ToString(), this.newData.YaleEmail);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_MiddleName, "Comparing MiddleName", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.middle.ToString(), this.newData.MiddleName);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_FirstName, "Comparing FirstName", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.firstname.ToString(), this.newData.FirstName);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Gender, "Comparing Gender", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.gender.ToString(), this.newData.Gender);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_NetId, "Comparing NetId", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.netID.ToString(), this.newData.NetId);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_PageName, "Comparing PageName", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.pageName.ToString(), this.newData.PageName);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Suffix, "Comparing Suffix", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.Suffix.ToString(), this.newData.Suffix);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Idx, "Comparing Idx", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.Idx.ToString(), this.newData.Idx);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_LicenseNumber, "Comparing License Number", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.LicenseNumber.ToString(), this.newData.LicenseNumber);
            this.CompareAndLog_Test(EnumTestUnitNames.UserBasicInfo_Npi, "Comparing Npi", this.UserId, this.Upi, this.OldDataNodes, EnumOldServiceFieldsAsKeys.Npi.ToString(), this.newData.Npi);

            ComputeOverallSeverity();
        }
    }
}