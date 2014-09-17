using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserResearchInfo : TestUnit
    {
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

        protected override void RunAllSingleTests()
        {
            UserResearchInfo newServiceInfo = this.NewDataAccessor.GetUserResearchById(this.UserId);
            string newValue;

            if (newServiceInfo.Research != null)
            {
                newValue = newServiceInfo.Research.BriefSummary;
            }
            else
            {
                newValue = string.Empty;
            }
            this.CompareAndLog_Test(
                EnumTestUnitNames.UserResearchInfo_Summary, 
                "Comparing Research Summary(ies)", 
                this.UserId, 
                this.Upi, 
                this.OldDataNodes,
                EnumOldServiceFieldsAsKeys.researchSummary.ToString(),
                newValue);

            if (newServiceInfo.Research != null)
            {
                newValue = newServiceInfo.Research.ExtensiveDescription;
            }
            else
            {
                newValue = string.Empty;
            }
            this.CompareAndLog_Test(
                EnumTestUnitNames.UserResearchInfo_Overview, 
                "Comparing Research Overview(s)", 
                this.UserId, 
                this.Upi, 
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.researchOverview.ToString(), 
                newValue);
        }
    }
}