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
    public class TestUnitUserLabWebsite : TestUnit
    {
        private IEnumerable<LabWebsite> newServiceWebsites = new HashSet<LabWebsite>();
        private IEnumerable<XElement> oldServiceWebsites;
        private int userId;
        private int upi;

        public void ProvideData(int userId,
                                            int upi,
                                            IEnumerable<XElement> oldServiceWebsites,
                                            IEnumerable<LabWebsite> newServiceWebsites)
        {
            this.userId = userId;
            this.upi = upi;

            if (newServiceWebsites != null)
            {
                this.newServiceWebsites = newServiceWebsites;
            }

            if (oldServiceWebsites != null)
            {
                this.oldServiceWebsites = oldServiceWebsites;
            }
        }

        public override string newServiceURLExtensionBeginning
        {
            get { return Parent.newServiceURLExtensionBeginning; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return Parent.newServiceURLExtensionEnding; }
        }

        public TestUnitUserLabWebsite(TestSuite parent, TestUnit bigBrother) 
        : base (parent,bigBrother)
        {

        }

        protected override void RunAllSingleTests()
        {
            UserContactLocationInfo_LabWebsites_Names_Test(this.oldServiceWebsites, new HashSet<string>(this.newServiceWebsites.Select(x=>x.LabName)));
            UserContactLocationInfo_LabWebsites_Links_Test(this.oldServiceWebsites, new HashSet<string>(this.newServiceWebsites.Select(x => x.LabUrl)));

            ComputeOverallSeverity();
        }

        private void UserContactLocationInfo_LabWebsites_Names_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceNodes, "titleName");

            var resultReport = new ResultReport("UserContactLocationInfo_LabWebsites_Names_Test", "Comparing LabWebsite Name(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(userId),
                                              resultReport);
        }

        private void UserContactLocationInfo_LabWebsites_Links_Test(IEnumerable<XElement> oldServiceNodes, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();

            HashSet<string> oldValues = TestUnit.ParseListSimpleOldValues(oldServiceNodes, "link");

            var resultReport = new ResultReport("UserContactLocationInfo_LabWebsites_Links_Test", "Comparing LabWebsite Link(s)");
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();
            watch.Stop();

            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(userId),
                                              resultReport);
        }
    }
}