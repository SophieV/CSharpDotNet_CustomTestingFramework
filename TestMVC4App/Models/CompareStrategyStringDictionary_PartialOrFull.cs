using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class CompareStrategyStringDictionary_PartialOrFull : CompareStrategy
    {
        private Dictionary<HashSet<string>, HashSet<string>> containerAndContents;
        private bool stringContainsMatch;

        public CompareStrategyStringDictionary_PartialOrFull(Dictionary<HashSet<string>, HashSet<string>> containerAndContents, ResultReport resultReport, bool stringPartialMatch)
            :base (new HashSet<string>(containerAndContents.SelectMany(x=>x.Key)), new HashSet<string>(containerAndContents.SelectMany(x=>x.Value)), resultReport)
        {
            this.containerAndContents = containerAndContents;
            this.stringContainsMatch = stringPartialMatch;
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
                        if (stringContainsMatch)
                        {
                            wasFoundItem = pair.Value.Any(otherEntry => otherEntry.Trim().Contains(entry.Trim()) || entry.Trim().Contains(otherEntry.Trim()));
                        }
                        else
                        {
                            wasFoundItem = pair.Value.Any(otherEntry => otherEntry.Trim().Equals(entry.Trim()) || entry.Trim().Equals(otherEntry.Trim()));
                        }

                        wasFound &= wasFoundItem;
                    }
                }

                if (wasFound)
                {
                    resultReport.UpdateResult(EnumResultSeverityType.SUCCESS);
                } 
                else 
                {
                    resultReport.UpdateResult(EnumResultSeverityType.ERROR);
                    resultReport.ErrorMessage = "The values do not match";
                }
            } 
            else 
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.WARNING_NO_DATA);
            }
        }
    }
}