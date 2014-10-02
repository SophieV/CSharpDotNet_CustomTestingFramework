using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public abstract class CompareStrategy
    {
        protected ResultReport resultReport;

        public CompareStrategy(HashSet<StringDescriptor> oldValues,HashSet<StringDescriptor> newValues, ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues, newValues);
        }

        public CompareStrategy(HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> newValues, ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues,newValues);
        }

        public CompareStrategy(HashSet<OrganizationTreeDescriptor> oldValues, OrganizationTreeDescriptor oldTreeRoot, 
                               HashSet<OrganizationTreeDescriptor> newValues, OrganizationTreeDescriptor newTreeRoot, 
                               ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues, oldTreeRoot, newValues, newTreeRoot);
        }

        public abstract void Investigate();

        public static int CountEntriesNotMatched(IEnumerable<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> list)
        {
            int count = 0;

            if (list != null)
            {
                foreach (var pair in list)
                {
                    count += pair.Values.Where(x => !x.HasBeenMatched).Count();
                }
            }

            return count;
        }
    }
}