using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserResearchInfo : TestUnit
    {
        private Research newData;

        public TestUnitUserResearchInfo(TestSuite parent, Research newData) : base(parent)
        {
            this.newData = newData;
        }

        protected override void RunAllSingleTests()
        {
            string newValue;

            if (this.newData != null)
            {
                newValue = this.newData.BriefSummary;
            }
            else
            {
                newValue = string.Empty;
            }
            this.CompareAndLog_Test(
                EnumTestUnitNames.UserResearchInfo_Summary, 
                "Comparing Research Summary(ies)", 
                this.OldDataNodes,
                EnumOldServiceFieldsAsKeys.researchSummary.ToString(),
                newValue);

            if (this.newData != null)
            {
                newValue = this.newData.ExtensiveDescription;
            }
            else
            {
                newValue = string.Empty;
            }
            this.CompareAndLog_Test(
                EnumTestUnitNames.UserResearchInfo_Overview, 
                "Comparing Research Overview(s)",  
                this.OldDataNodes, 
                EnumOldServiceFieldsAsKeys.researchOverview.ToString(), 
                newValue);
        }
    }
}