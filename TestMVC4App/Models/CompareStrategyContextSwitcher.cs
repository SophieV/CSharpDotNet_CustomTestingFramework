using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class CompareStrategyContextSwitcher
    {
        private CompareStrategy compareStrategy;

        public CompareStrategyContextSwitcher(HashSet<string> oldValues, HashSet<string> newValues, ResultReport resultReport)
        {
            if (oldValues.Count() <= 1 && newValues.Count() <= 1)
            {
                compareStrategy = new CompareStrategyString(oldValues, newValues,resultReport);
            } 
            else
            {
                compareStrategy = new CompareStrategyStringCollection(oldValues,newValues,resultReport);
            }
        }

        public CompareStrategyContextSwitcher(string oldValue, string newValue, ResultReport resultReport)
        {
            compareStrategy = new CompareStrategyString(oldValue, newValue, resultReport);
        }

        public CompareStrategyContextSwitcher(HashSet<OrganizationTreeDescriptor> listOldIdsAndNames, OrganizationTreeDescriptor oldTreeRoot, 
                                           HashSet<OrganizationTreeDescriptor> listNewIdsAndNames, OrganizationTreeDescriptor newTreeRoot, 
                                           ResultReport resultReport)
        {
            compareStrategy = new CompareStrategyOrganization(listOldIdsAndNames,oldTreeRoot,listNewIdsAndNames,newTreeRoot,resultReport);
        }

        public void Investigate()
        {
            if (compareStrategy != null)
            {
                this.compareStrategy.Investigate();
            }
        }
    }
}