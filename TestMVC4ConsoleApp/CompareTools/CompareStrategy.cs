using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Specificies the structure of a Comparison Test.
    /// Includes helper methods.
    /// </summary>
    public abstract class CompareStrategy
    {
        protected ResultReport resultReport;

        /// <summary>
        /// Constructor for comparing Unstructured Lists.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        /// <param name="resultReport"></param>
        public CompareStrategy(HashSet<StringDescriptor> oldValues,HashSet<StringDescriptor> newValues, ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues, newValues);
        }

        /// <summary>
        /// Constructor for comparing Structured Lists.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        /// <param name="resultReport"></param>
        public CompareStrategy(HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> newValues, ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues,newValues);
        }

        /// <summary>
        /// Constructor for comparing Trees of Organizations.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="oldTreeRoot"></param>
        /// <param name="newValues"></param>
        /// <param name="newTreeRoot"></param>
        /// <param name="resultReport"></param>
        public CompareStrategy(HashSet<OrganizationTreeDescriptor> oldValues, OrganizationTreeDescriptor oldTreeRoot, 
                               HashSet<OrganizationTreeDescriptor> newValues, OrganizationTreeDescriptor newTreeRoot, 
                               ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues, oldTreeRoot, newValues, newTreeRoot);
        }

        public abstract void Investigate();

        /// <summary>
        /// Counts the <see cref="StringDescriptor"/> descriptors where the flag HasBeenMatched is set to false.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
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