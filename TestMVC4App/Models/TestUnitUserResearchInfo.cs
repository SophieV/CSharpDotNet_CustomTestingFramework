using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class TestUnitUserResearchInfo : TestUnit
    {
        private UsersClient newServiceAccessor;
        private IEnumerable<XElement> oldServiceData;
        private int upi;
        private int userId;

        public override string newServiceURLExtensionBeginning
        {
            get { return "Users/"; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return "/Research"; }
        }

        public TestUnitUserResearchInfo(TestSuite parent) : base(parent)
        {
        }

        public void ProvideData(IEnumerable<XElement> oldData, UsersClient newDataAccessor, int upi, int userId)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.upi = upi;
            this.userId = userId;
        }

        protected override void RunAllSingleTests()
        {
            UserResearchInfo newServiceInfo = newServiceAccessor.GetUserResearchById(userId);
            this.CompareAndLog_Test(EnumTestUnitNames.UserResearchInfo_Summary, "Comparing Research Summary(ies)", userId,upi, oldServiceData,"researchSummary",newServiceInfo.Research.BriefSummary);
            this.CompareAndLog_Test(EnumTestUnitNames.UserResearchInfo_Overview, "Comparing Research Overview(s)", userId, upi, oldServiceData, "researchOverview", newServiceInfo.Research.ExtensiveDescription);
        }
    }
}