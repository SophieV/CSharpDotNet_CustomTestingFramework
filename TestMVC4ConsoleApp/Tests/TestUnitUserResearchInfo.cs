using System.Collections.Generic;
using System.Linq;
using System;
using System.Web;
using YSM.PMS.Web.Service.DataTransfer.Models;

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
            this.CompareAndLog_Test(EnumTestUnitNames.UserResearchInfo_Summary, "Comparing Research Summary(ies)", this.OldDataNodes, EnumOldServiceFieldsAsKeys.researchSummary.ToString(), HttpUtility.HtmlEncode(HttpUtility.HtmlDecode((this.newData != null && !string.IsNullOrEmpty(this.newData.BriefSummary)?this.newData.BriefSummary:string.Empty))));
            this.CompareAndLog_Test(EnumTestUnitNames.UserResearchInfo_Overview, "Comparing Research Overview(s)", this.OldDataNodes, EnumOldServiceFieldsAsKeys.researchOverview.ToString(), HttpUtility.HtmlEncode(HttpUtility.HtmlDecode((this.newData != null&& !string.IsNullOrEmpty(this.newData.ExtensiveDescription)?this.newData.ExtensiveDescription:string.Empty))));
            UserResearchInfo_PublicHealthKeywords();
        }

        private void UserResearchInfo_PublicHealthKeywords()
        {
            HashSet<string> newValues = new HashSet<string>();;

            try
            {
                if (this.newData != null && this.newData.PublicHealths != null)
                {
                    newValues = new HashSet<string>(newData.PublicHealths.Where(x => x != null).Select(x => HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(x.Keyword))));
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