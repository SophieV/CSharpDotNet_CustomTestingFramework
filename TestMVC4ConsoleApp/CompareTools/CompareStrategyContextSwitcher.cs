using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestMVC4ConsoleApp.CompareTools;

namespace TestMVC4App.Models
{
    public class CompareStrategyContextSwitcher
    {
        // compare a bunch of data that belongs together - an entity
        private HashSet<CompareStrategy> compareStrategies = new HashSet<CompareStrategy>();

        /// <summary>
        /// Compare values.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="resultReport"></param>
        public CompareStrategyContextSwitcher(string oldValue, string newValue, ResultReport resultReport)
        {
            compareStrategies.Add(new CompareStrategyStringDescriptors(StringDescriptor.EmbedInDescriptors(oldValue), StringDescriptor.EmbedInDescriptors(newValue), resultReport));
        }

        /// <summary>
        /// Compare list of values.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        /// <param name="resultReport"></param>
        public CompareStrategyContextSwitcher(HashSet<string> oldValues, HashSet<string> newValues, ResultReport resultReport)
        {
            compareStrategies.Add(new CompareStrategyStringDescriptors(StringDescriptor.EmbedInDescriptors(oldValues),StringDescriptor.EmbedInDescriptors(newValues),resultReport));
        }

        /// <summary>
        /// Compare list of structured values. Each property value is mapped to a specific key.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        /// <param name="resultReport"></param>
        public CompareStrategyContextSwitcher(HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> newValues, ResultReport resultReport)
        {
            compareStrategies.Add(new CompareStrategyStringDescriptorsDictionary(StringDescriptor.EmbedInDescriptors(oldValues), StringDescriptor.EmbedInDescriptors(newValues), resultReport));
        }

        /// <summary>
        /// Compare lists of values. They are organized in slices.
        /// </summary>
        /// <param name="oldAndNewValues"></param>
        /// <param name="resultReport"></param>
        /// <param name="stringPartialMatch"></param>
        public CompareStrategyContextSwitcher(Dictionary<HashSet<string>, HashSet<string>> oldAndNewValues, ResultReport resultReport)
        {
            foreach(var slice in oldAndNewValues)
            {
                compareStrategies.Add(new CompareStrategyStringDescriptors(StringDescriptor.EmbedInDescriptors(slice.Key), StringDescriptor.EmbedInDescriptors(slice.Value), resultReport));
            }
        }

        /// <summary>
        /// Compare lists of values. They are organized in slices. And each value is mapped to a specific key.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        /// <param name="resultReport"></param>
        public CompareStrategyContextSwitcher(Dictionary<Dictionary<EnumOldServiceFieldsAsKeys, string>,Dictionary<EnumOldServiceFieldsAsKeys, string>> oldAndNewValues, ResultReport resultReport)
        {
            foreach (var slice in oldAndNewValues)
            {
                compareStrategies.Add(new CompareStrategyStringDescriptorsDictionary(StringDescriptor.EmbedInDescriptors(slice.Key), StringDescriptor.EmbedInDescriptors(slice.Value),resultReport));
            }
        }

        /// <summary>
        /// Compare Trees of Organizations.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="oldTreeRoot"></param>
        /// <param name="newValues"></param>
        /// <param name="newTreeRoot"></param>
        /// <param name="resultReport"></param>
        public CompareStrategyContextSwitcher(HashSet<OrganizationTreeDescriptor> oldValues, OrganizationTreeDescriptor oldTreeRoot, 
                                           HashSet<OrganizationTreeDescriptor> newValues, OrganizationTreeDescriptor newTreeRoot, 
                                           ResultReport resultReport)
        {
            compareStrategies.Add(new CompareStrategyOrganizationList(oldValues,oldTreeRoot,newValues,newTreeRoot,resultReport));
        }

        public void Investigate()
        {
            if (compareStrategies != null && compareStrategies.Count() > 0)
            {
                foreach (var compareStrategy in compareStrategies)
                {
                    compareStrategy.Investigate();
                }
            }
        }
    }
}