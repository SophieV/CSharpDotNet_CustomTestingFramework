using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class CompareStrategyStringContains : CompareStrategy
    {
        private Dictionary<HashSet<string>, HashSet<string>> containerAndContents;

        public CompareStrategyStringContains(Dictionary<HashSet<string>, HashSet<string>> containerAndContents, ResultReport resultReport)
            :base (new HashSet<string>(containerAndContents.SelectMany(x=>x.Key)), new HashSet<string>(containerAndContents.SelectMany(x=>x.Value)), resultReport)
        {
            this.containerAndContents = containerAndContents;
        }

        public override void Investigate()
        {
            if (this.containerAndContents != null && this.containerAndContents.Count() > 0)
            {
                bool wasFound = true;
                bool wasFoundItem;

                foreach (var pair in containerAndContents)
                {
                    foreach (var entry in pair.Key)
                    {
                        wasFoundItem = pair.Value.Any(otherEntry => otherEntry.Trim().Contains(entry.Trim()) || entry.Trim().Contains(otherEntry.Trim()));

                        wasFound &= wasFoundItem;
                    }
                }

                if (wasFound)
                {
                    resultReport.UpdateResult(ResultSeverityType.SUCCESS);
                } 
                else 
                {
                    resultReport.UpdateResult(ResultSeverityType.ERROR);
                    resultReport.ErrorMessage = CompareStrategy.ReplaceProblematicTagsForHtml("The values do not match");
                }
            } 
            else 
            {
                this.resultReport.UpdateResult(ResultSeverityType.WARNING_NO_DATA);
            }
        }
    }
}