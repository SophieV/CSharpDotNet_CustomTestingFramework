using System.Collections.Generic;
using System.Linq;
using TestMVC4ConsoleApp.CompareTools;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Will create a <see cref="CompareStrategy"/> based on the context - data types supplied.
    /// </summary>
    public class CompareStrategyFactory
    {
        // compare a bunch of data that belongs together - an entity
        private HashSet<CompareStrategy> compareStrategies = new HashSet<CompareStrategy>();

        /// <summary>
        /// Compare values.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="resultReport"></param>
        public CompareStrategyFactory(string oldValue, string newValue, ResultReport resultReport)
        {
            compareStrategies.Add(new CompareStrategyUnstructuredLists(StringDescriptor.EmbedInDescriptors(true, oldValue), StringDescriptor.EmbedInDescriptors(false, newValue), resultReport));
        }

        /// <summary>
        /// Compare unstructured lists of values.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        /// <param name="resultReport"></param>
        public CompareStrategyFactory(HashSet<string> oldValues, HashSet<string> newValues, ResultReport resultReport)
        {
            compareStrategies.Add(new CompareStrategyUnstructuredLists(StringDescriptor.EmbedInDescriptors(true, oldValues),StringDescriptor.EmbedInDescriptors(false, newValues),resultReport));
        }

        /// <summary>
        /// Compare structured lists of values. Each property value is mapped to a specific key.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        /// <param name="resultReport"></param>
        public CompareStrategyFactory(HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> newValues, ResultReport resultReport)
        {
            compareStrategies.Add(new CompareStrategyStructuredLists(StringDescriptor.EmbedInDescriptors(true, oldValues), StringDescriptor.EmbedInDescriptors(false, newValues), resultReport));
        }

        /// <summary>
        /// Compare unstructured lists of values. They are organized in slices.
        /// </summary>
        /// <param name="oldAndNewValues"></param>
        /// <param name="resultReport"></param>
        /// <param name="stringPartialMatch"></param>
        public CompareStrategyFactory(Dictionary<HashSet<string>, HashSet<string>> oldAndNewValues, ResultReport resultReport)
        {
            foreach(var slice in oldAndNewValues)
            {
                compareStrategies.Add(new CompareStrategyUnstructuredLists(StringDescriptor.EmbedInDescriptors(true, slice.Key), StringDescriptor.EmbedInDescriptors(false, slice.Value), resultReport));
            }
        }

        /// <summary>
        /// Compare structured lists of values. They are organized in slices. And each value is mapped to a specific key.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        /// <param name="resultReport"></param>
        public CompareStrategyFactory(Dictionary<Dictionary<EnumOldServiceFieldsAsKeys, string>,Dictionary<EnumOldServiceFieldsAsKeys, string>> oldAndNewValues, ResultReport resultReport)
        {
            foreach (var slice in oldAndNewValues)
            {
                compareStrategies.Add(new CompareStrategyStructuredLists(StringDescriptor.EmbedInDescriptors(true, slice.Key), StringDescriptor.EmbedInDescriptors(false, slice.Value),resultReport));
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
        public CompareStrategyFactory(HashSet<OrganizationTreeDescriptor> oldValues, OrganizationTreeDescriptor oldTreeRoot, 
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