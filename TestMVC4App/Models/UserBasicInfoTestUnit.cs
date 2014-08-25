using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class UserBasicInfoTestUnit : TestUnit
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

        public UserBasicInfoTestUnit(TestSuite parent) : base(parent)
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
            var newUserBasicInfo = newServiceAccessor.GetUserByUpi(upi);
            MappedUserId = newUserBasicInfo.UserId;

            UserBasicInfo_UPI_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_LastName_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_Email_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_MiddleName_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_FirstName_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_Gender_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_NetId_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_PageName_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_Suffix_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_Idx_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_LicenseNumber_Test(newUserBasicInfo, oldServiceData);
            UserBasicInfo_Npi_Test(newUserBasicInfo, oldServiceData);

            // TODO : Implement User Editors
            // UserBasicInfo_UserEditors_Test

            ComputeOverallSeverity();
        }

        public void ProvideUserData(XDocument oldData, UsersClient newDataAccessor, int upi)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.upi = upi;
        }

        #region Field Comparison Tests

        private void UserBasicInfo_UPI_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_UPI_Test", "Comparing UPI");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/UPI");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.Upi.ToString(), resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            MappedUserId = newServiceData.UserId;


            LogManager.Instance.LogTestResult(MappedUserId, 
                                              upi, 
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_Gender_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_Gender_Test", "Comparing Gender");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/gender");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.Gender, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_Idx_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_Idx_Test", "Comparing Idx");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/Idx");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.Idx, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_LicenseNumber_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_LicenseNumber_Test", "Comparing License Number");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/LicenseNumber");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.LicenseNumber, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_LastName_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_LastName_Test", "Comparing LastName");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/lastname");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.LastName, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_FirstName_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_FirstName_Test", "Comparing FirstName");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/firstname");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.FirstName, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_MiddleName_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_MiddleName_Test", "Comparing MiddleName");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/middle");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.MiddleName, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_Email_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_Email_Test", "Comparing Email");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/emailAddress");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.YaleEmail, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
                    }

        private void UserBasicInfo_NetId_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_NetId_Test", "Comparing NetId");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/netID");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.NetId, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_PageName_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_PageName_Test", "Comparing PageName");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/pageName");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.PageName, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_Suffix_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_Suffix_Test", "Comparing Suffix");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/Suffix");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.Suffix, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        private void UserBasicInfo_Npi_Test(UserBasicInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserBasicInfo_Npi_Test", "Comparing Npi");

            string oldValue = TestUnit.ParseSingleOldValue(oldServiceData, "/Faculty/facultyMember/Npi");
            var compareStrategy = new SimpleStringCompareStrategy(oldValue, newServiceData.Npi, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(MappedUserId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }

        public void UserBasicInfo_UserEditors_Test()
        {
            throw new AssertInconclusiveException();
        }

        #endregion
    }
}