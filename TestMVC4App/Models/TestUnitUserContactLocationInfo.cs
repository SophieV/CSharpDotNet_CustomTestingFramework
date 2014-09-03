using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class TestUnitUserContactLocationInfo : TestUnit
    {
        private UsersClient newServiceAccessor;
        private XDocument oldServiceData;
        private int upi;
        private int userId;

        public override string newServiceURLExtensionBeginning
        {
            get { return "Users/blablala/"; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return string.Empty; }
        }

        public TestUnitUserContactLocationInfo(TestSuite parent) : base(parent)
        {

        }

        protected override void RunAllSingleTests()
        {
            var newUserContactLocationInfo = newServiceAccessor.GetUserContactLocationById(userId);

            UserContactLocationInfo_Assistants_Test(newUserContactLocationInfo, oldServiceData);

            ComputeOverallSeverity();
        }

        public void ProvideUserData(XDocument oldData, UsersClient newDataAccessor, int upi, int userId)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.userId = userId;
            this.upi = upi;
        }

        private void UserContactLocationInfo_Assistants_Test(UserContactLocationInfo newServiceData, XDocument oldServiceData)
        {
            var watch = new Stopwatch();
            watch.Start();
            HashSet<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/assistant", "fname");

            HashSet<string> newValues = new HashSet<string>();
            if(newServiceData.Assistants.Count() > 0)
            {
                foreach(var title in newServiceData.Assistants)
                {
                    if (!string.IsNullOrEmpty(title.UserMinimalInfo.Name))
                    {
                        newValues.Add(title.UserMinimalInfo.Name);
                    }
                }
            }

            var resultReport = new ResultReport("UserContactLocationInfo_Assistants_Test", "Comparing Assistant Name(s)");
            var compareStrategy = new CompareStrategyStringCollection(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(upi),
                                              resultReport);
        }
    }
}