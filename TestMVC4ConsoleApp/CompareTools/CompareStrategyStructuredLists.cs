using System.Collections.Generic;
using System.Linq;
using TestMVC4ConsoleApp.CompareTools;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Compares lists of structured data (with keys) against each other.
    /// Limitations :
    /// String values are matched regardless of the structure they belong to, as long as they are in the same list.
    /// However they are matched with consideration to the key they are mapped to.
    /// 
    /// No check for duplicates.
    /// </summary>
    public class CompareStrategyStructuredLists : CompareStrategy
    {
        private HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> oldValues;
        private HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> newValues;

        public CompareStrategyStructuredLists(HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> newValues, ResultReport resultReport) 
            : base(oldValues,newValues,resultReport)
        {
            this.oldValues = oldValues;
            this.newValues = newValues;
        }

        public override void Investigate()
        {
            bool keepGoing = true;

            if(keepGoing)
            {
                keepGoing = AreBothOrAnyListsEmpty();
            }

            //if (keepGoing)
            //{
            //    keepGoing = AreThereDuplicatesOnTheNewSide();
            //}

            if (keepGoing)
            {
                // plain : case sensitive, not shifted, not trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(true,false, false, false);
            }

            //if (keepGoing)
            //{
            //    keepGoing = AreMissingValuesOnTheNewSideDuplicatesOnTheOldSide();
            //}

            if (keepGoing)
            {
                // case sensitive, not shifted, trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(true, false, true, false);
            }

            if (keepGoing)
            {
                // not case sensitive, not shifted, not trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(false, false, false, false);
            }

            if (keepGoing)
            {
                // case sensitive, shifted, not trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(true, true, false, false);
            }

            if (keepGoing)
            {
                // plain : case sensitive, not shifted, not trimmed, partial
                keepGoing = AreBothCollectionsEquivalent(true, false, false, true);
            }
        }

        #region Scenarios

        private bool AreBothOrAnyListsEmpty()
        {
            bool shouldContinueTesting = true;

            bool populatedOld = false;
            foreach (var dictionary in this.oldValues)
            {
                if (dictionary.Where(x=>!string.IsNullOrEmpty(x.Value.Value)).Count() > 0)
                {
                    populatedOld = true;
                }
            }

            bool populatedNew = false;
            foreach (var dictionary in this.newValues)
            {
                if (dictionary.Where(x => !string.IsNullOrEmpty(x.Value.Value)).Count() > 0)
                {
                    populatedNew = true;
                }
            }

            if (!populatedNew && !populatedOld)
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.WARNING_NO_DATA);
                shouldContinueTesting = false;
            }
            else if (!populatedOld)
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.WARNING_ONLY_NEW);
                shouldContinueTesting = false;
            }
            else if (!populatedNew)
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.ERROR_ONLY_OLD);
                shouldContinueTesting = false;
            } 
            else
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.SUCCESS);
            }

            return shouldContinueTesting;
        }


        /// <summary>
        /// Check if content of lists is equivalent.
        /// Scenarios :
        /// - string values tested like they are
        /// - string values are tested against each other in lower case (if caseSensitive flag is set to false)
        /// - string values are tested against each other with shifted content based on the coma (if shifted is set to true)
        /// - string values are tested against each other once trimmed (if trim flag is set to true)
        /// The scenarios do not cumulate !!
        /// </summary>
        /// <param name="caseSensitive"></param>
        /// <param name="trim"></param>
        /// <param name="shifted"></param>
        /// <returns></returns>
        /// <remarks>The comparisons are only applied to non-matched items.</remarks>
        private bool AreBothCollectionsEquivalent(bool caseSensitive, bool shifted, bool trim, bool partial)
        {
            bool shouldContinueTesting = true;

            IEnumerable<Dictionary<EnumOldServiceFieldsAsKeys,StringDescriptor>> leftOversOld;
            IEnumerable<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> leftOversNew;

            int previousOldCount = CompareStrategy.CountEntriesNotMatched(oldValues);
            int previousNewCount = CompareStrategy.CountEntriesNotMatched(newValues);

            if (partial)
            {
                leftOversOld = oldValues.Except(newValues, new ComparerStringWithKeyPartial());
                leftOversNew = newValues.Except(oldValues, new ComparerStringWithKeyPartial());
            }
            else if (shifted)
            {
                leftOversOld = oldValues.Except(newValues, new ComparerStringWithKeyShifted());
                leftOversNew = newValues.Except(oldValues, new ComparerStringWithKeyShifted());
            }
            else if (trim)
            {
                leftOversOld = oldValues.Except(newValues, new ComparerStringWithKeyTrimmed());
                leftOversNew = newValues.Except(oldValues, new ComparerStringWithKeyTrimmed());
            }
            else if (caseSensitive)
            {
                leftOversOld = oldValues.Except(newValues, new ComparerStringWithKey());
                leftOversNew = newValues.Except(oldValues, new ComparerStringWithKey());
            }
            else
            {
                leftOversOld = oldValues.Except(newValues, new ComparerStringWithKeyNotCaseSensitive());
                leftOversNew = newValues.Except(oldValues, new ComparerStringWithKeyNotCaseSensitive());
            }

            int oldCount = CompareStrategy.CountEntriesNotMatched(leftOversOld);
            int newCount = CompareStrategy.CountEntriesNotMatched(leftOversNew);

            if (oldCount < previousOldCount || newCount < previousNewCount)
            {
                if (partial)
                {
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.PARTIAL_MATCH);
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.ERROR_WITH_EXPLANATION);
                } 
                else if (shifted)
                {
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.PARTIAL_MATCH);
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.ERROR_WITH_EXPLANATION);
                }
                else if (trim)
                {
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MISMATCH_DUE_TO_TRAILING_WHITE_SPACES);
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.WARNING);
                }
                else if (!caseSensitive)
                {
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MISMATCH_DUE_TO_CASE_DIFFERENCES);
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.WARNING);
                }
            }

            if (oldCount == 0 && newCount == 0)
            {
                if (!caseSensitive || trim || shifted || partial)
                {
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.FALSE_POSITIVE);
                    shouldContinueTesting = false;
                } 
                else 
                {
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.SUCCESS);
                    shouldContinueTesting = false;
                }
            } else if (oldCount == 0)
            {
                this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MORE_VALUES_ON_NEW_SERVICE);
                this.resultReport.UpdateSeverity(EnumResultSeverityType.SUCCESS);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.ERROR);
            }

            return shouldContinueTesting;
        }

        #endregion
    }
}