using System.Collections.Generic;
using YSM.PMS.Service.Common.DataTransfer;
using System.Linq;

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
            UserResearchInfo_Summary();
            UserResearchInfo_Overview();
            UserResearchInfo_PublicHealthKeywords();
        }

        private void UserResearchInfo_PublicHealthKeywords()
        {
            HashSet<string> newValues;

            if (this.newData != null && this.newData.PublicHealths != null)
            {
                newValues = new HashSet<string>(newData.PublicHealths.Where(x => x != null).Select(x => x.Keyword));
            }
            else
            {
                newValues = new HashSet<string>();
            }
            this.CompareAndLog_Test(
                        EnumTestUnitNames.UserResearchInfo_PublicHealthKeywords,
                        "Comparing Public Health Keyword(s)",
                        ParsingHelper.ParseUnstructuredListOfValues(ParsingHelper.ParseListNodes(this.OldDataNodes, EnumOldServiceFieldsAsKeys.publicHealthKeywords.ToString()), EnumOldServiceFieldsAsKeys.keyword.ToString(), EnumOldServiceFieldsAsKeys.name.ToString()),
                        newValues);
        }

        private void UserResearchInfo_Overview()
        {
            string newValue;

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

        private void UserResearchInfo_Summary()
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
        }
    }
}