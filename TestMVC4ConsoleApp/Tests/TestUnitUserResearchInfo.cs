using System.Collections.Generic;
using YSM.PMS.Service.Common.DataTransfer;
using System.Linq;
using System;

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
            this.CompareAndLog_Test(EnumTestUnitNames.UserResearchInfo_Summary, "Comparing Research Summary(ies)", this.OldDataNodes, EnumOldServiceFieldsAsKeys.researchSummary.ToString(), this.newData.BriefSummary);
            this.CompareAndLog_Test(EnumTestUnitNames.UserResearchInfo_Overview, "Comparing Research Overview(s)", this.OldDataNodes, EnumOldServiceFieldsAsKeys.researchOverview.ToString(), this.newData.ExtensiveDescription);
            UserResearchInfo_PublicHealthKeywords();
        }

        private void UserResearchInfo_PublicHealthKeywords()
        {
            HashSet<string> newValues = new HashSet<string>();;

            try
            {
                if (this.newData != null && this.newData.PublicHealths != null)
                {
                    newValues = new HashSet<string>(newData.PublicHealths.Where(x => x != null).Select(x => x.Keyword));
                }
            }
            catch (Exception) { }

            this.CompareAndLog_Test(
                        EnumTestUnitNames.UserResearchInfo_PublicHealthKeywords,
                        "Comparing Public Health Keyword(s)",
                        ParsingHelper.ParseUnstructuredListOfValues(ParsingHelper.ParseListNodes(this.OldDataNodes, EnumOldServiceFieldsAsKeys.publicHealthKeywords.ToString()), EnumOldServiceFieldsAsKeys.keyword.ToString(), EnumOldServiceFieldsAsKeys.name.ToString()),
                        newValues);
        }
    }
}