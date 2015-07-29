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
            UserResearchCareInfo_ResearchInterestsDepecatedKeywords();
            UserResearchCareInfo_Meshes();
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

        private void UserResearchCareInfo_ResearchInterestsDepecatedKeywords()
        {
            string newValue = string.Empty;

            if (this.newData != null && this.newData.DeprecatedKeywordList != null)
            {
                newValue = HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(this.newData.DeprecatedKeywordList));
            }

            this.CompareAndLog_Test(
                EnumTestUnitNames.UserResearchInfo_ResearchInterestsDeprecatedKeywords,
                "Comparing Staywells deprecated keywords",
                this.OldDataNodes,
                EnumOldServiceFieldsAsKeys.researchInterests.ToString(),
                newValue);
        }

        private void UserResearchCareInfo_Meshes()
        {
            var oldValuesMerged = HttpUtility.HtmlDecode(ParsingHelper.ParseSingleValue(this.OldDataNodes, EnumOldServiceFieldsAsKeys.mesh.ToString()));
            // data is originally in an HTML list
            oldValuesMerged = oldValuesMerged.Replace("</li><li>", ",");
            oldValuesMerged = oldValuesMerged.Replace("<ul><li>", "");
            oldValuesMerged = oldValuesMerged.Replace("</li></ul>", "");
            var oldValues = ParsingHelper.StringToList(oldValuesMerged, ',');

            var newValues = new HashSet<string>();
            if(newData != null && newData.Meshes != null && newData.Meshes.Count() > 0)
            {
                foreach(var entry in newData.Meshes)
                {
                    newValues.Add(entry.Keyword);
                }
            }
            
            this.CompareAndLog_Test(
                EnumTestUnitNames.UserResearchInfo_Meshes, 
                "Comparing Research Meshes", 
                oldValues, 
                newValues);
        }
    }
}