using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public abstract class CompareStrategy
    {
        protected ResultReport resultReport;

        public CompareStrategy(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> oldValues, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> newValues, ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AppendDetailedValues(oldValues,newValues);
        }

        public CompareStrategy(HashSet<OrganizationTreeDescriptor> oldValues, OrganizationTreeDescriptor oldTreeRoot, 
                               HashSet<OrganizationTreeDescriptor> newValues, OrganizationTreeDescriptor newTreeRoot, 
                               ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues, oldTreeRoot, newValues, newTreeRoot);
        }

        public abstract void Investigate();
    }
}