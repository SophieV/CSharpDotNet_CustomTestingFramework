using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TestMVC4ConsoleApp.CompareTools;

namespace TestMVC4App.Models
{
    public class CompareStrategyStringDescriptorsDictionary : CompareStrategy
    {
        private int leftOversOldCount = -1;
        private HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> oldValues;
        private HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> newValues;

        public CompareStrategyStringDescriptorsDictionary(HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> newValues, ResultReport resultReport) 
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
                // case sensitive, shifted, not trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(true, true, false, false);
            }

            if (keepGoing)
            {
                // plain : case sensitive, not shifted, not trimmed, partial
                keepGoing = AreBothCollectionsEquivalent(true, false, false, true);
            }

            if (keepGoing)
            {
                // not case sensitive, not shifted, not trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(false,false, false, false);
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

            int previousCount = CountEntriesNotMatched(oldValues);

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
             
            this.leftOversOldCount = CountEntriesNotMatched(leftOversOld);
            var leftOversNewCount = CountEntriesNotMatched(leftOversNew);

            if (this.leftOversOldCount < previousCount)
            {
                if (partial)
                {
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.NEW_CONTAINED_IN_OLD);
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.WARNING);
                } 
                else if (shifted)
                {
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.NEW_CONTAINED_IN_OLD);
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.WARNING);
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

            if (this.leftOversOldCount == 0 && leftOversNewCount == 0)
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
            } else if (this.leftOversOldCount == 0)
            {
                this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MORE_VALUES_ON_NEW_SERVICE);
                this.resultReport.UpdateSeverity(EnumResultSeverityType.FALSE_POSITIVE);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.ERROR);
                this.resultReport.ErrorMessage = "The lists of " + (trim ? "trimmed " : string.Empty) + (shifted ? "shifted " : string.Empty) + " strings compared " + (!caseSensitive ? "without case " : string.Empty) + "are not equal";
            }

            return shouldContinueTesting;
        }

        private int CountEntriesNotMatched(IEnumerable<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> list)
        {
            int count = 0;

            foreach(var pair in list)
            {
                count += pair.Values.Where(x => !x.HasBeenMatched).Count();
            }

            return count;
        }

        #endregion
    }
}