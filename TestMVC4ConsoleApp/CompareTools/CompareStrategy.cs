using System;
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public abstract class CompareStrategy
    {
        protected ResultReport resultReport;

        public CompareStrategy(HashSet<string> oldValues, HashSet<string> newValues, ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues, newValues);
        }

        public CompareStrategy(HashSet<OrganizationTreeDescriptor> oldValues, OrganizationTreeDescriptor oldTreeRoot, 
                               HashSet<OrganizationTreeDescriptor> newValues, OrganizationTreeDescriptor newTreeRoot, 
                               ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues, oldTreeRoot, newValues, newTreeRoot);
        }

        public CompareStrategy(HashSet<Dictionary<EnumOldServiceFieldsAsKeys,string>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys,string>> newValues, ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues, newValues);
        }

        public abstract void Investigate();
    }
}