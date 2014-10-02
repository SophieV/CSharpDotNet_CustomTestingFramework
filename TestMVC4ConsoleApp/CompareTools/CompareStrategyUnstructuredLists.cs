using System.Collections.Generic;
using System.Linq;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    /// <summary>
    /// Compares lists of unstructured data against each other.
    /// </summary>
    public class CompareStrategyUnstructuredLists : CompareStrategy
    {
        private HashSet<StringDescriptor> oldValues;
        private HashSet<StringDescriptor> newValues;

        public CompareStrategyUnstructuredLists(HashSet<StringDescriptor> oldValues, HashSet<StringDescriptor> newValues, ResultReport resultReport)
            : base(oldValues, newValues, resultReport)
        {
            this.oldValues = oldValues;
            this.newValues = newValues;
        }

        public override void Investigate()
        {
            bool keepGoing = true;

            if (keepGoing)
            {
                keepGoing = AreBothListsEmpty();
            }

            if (keepGoing)
            {
                keepGoing = IsOldListEmpty();
            }

            if (keepGoing)
            {
                keepGoing = IsNewListEmpty();
            }

            if (keepGoing)
            {
                keepGoing = AreThereDuplicatesOnTheNewSide();
            }

            if (keepGoing)
            {
                // plain : case sensitive, not shifted, not trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(true, false, false, false);
            }

            if (keepGoing)
            {
                keepGoing = AreMissingValuesOnTheNewSideDuplicatesOnTheOldSide();
            }

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

        private bool AreBothListsEmpty()
        {
            bool shouldContinueTesting = true;

            if (this.oldValues.Where(x => !string.IsNullOrEmpty(x.Value)).Count() <= 0
                && this.newValues.Where(x => !string.IsNullOrEmpty(x.Value)).Count() <= 0)
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.WARNING_NO_DATA);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.SUCCESS);
            }

            return shouldContinueTesting;
        }

        private bool IsOldListEmpty()
        {
            bool shouldContinueTesting = true;

            if (this.oldValues.Where(x => !string.IsNullOrEmpty(x.Value)).Count() <= 0)
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.WARNING_ONLY_NEW);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.SUCCESS);
            }

            return shouldContinueTesting;
        }

        private bool IsNewListEmpty()
        {
            bool shouldContinueTesting = true;

            if (this.newValues.Where(x => !string.IsNullOrEmpty(x.Value)).Count() <= 0)
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

            IEnumerable<StringDescriptor> leftOversOld;
            IEnumerable<StringDescriptor> leftOversNew;

            int previousOldCount = this.oldValues.Where(x => !x.HasBeenMatched).Count();
            int previousNewCount = this.newValues.Where(x => !x.HasBeenMatched).Count();

            if (partial)
            {
                leftOversOld = oldValues.Except(newValues, new ComparerStringPartial());
                leftOversNew = newValues.Except(oldValues, new ComparerStringPartial());
            }
            else if (shifted)
            {
                leftOversOld = oldValues.Except(newValues, new ComparerStringShifted());
                leftOversNew = newValues.Except(oldValues, new ComparerStringShifted());
            }
            else if (trim)
            {
                leftOversOld = oldValues.Except(newValues, new ComparerStringTrimmed());
                leftOversNew = newValues.Except(oldValues, new ComparerStringTrimmed());
            }
            else if (caseSensitive)
            {
                leftOversOld = oldValues.Except(newValues, new ComparerString());
                leftOversNew = newValues.Except(oldValues, new ComparerString());
            }
            else
            {
                leftOversOld = oldValues.Except(newValues, new ComparerStringNotCaseSensitive());
                leftOversNew = newValues.Except(oldValues, new ComparerStringNotCaseSensitive());
            }

            int oldCount = leftOversOld.Where(x => !x.HasBeenMatched).Count();
            int newCount = leftOversNew.Where(x => !x.HasBeenMatched).Count();

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
            }
            else if (oldCount == 0)
            {
                this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MORE_VALUES_ON_NEW_SERVICE);
                this.resultReport.UpdateSeverity(EnumResultSeverityType.FALSE_POSITIVE);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.ERROR);
            }

            return shouldContinueTesting;
        }

        private bool AreMissingValuesOnTheNewSideDuplicatesOnTheOldSide()
        {
            bool shouldContinueTesting = true;

            // if there are more values on the old side
            // check whether the values are really missing on the new side
            // or if they were all doublons - which ends up being a FALSE POSITIVE
            if (this.oldValues.Count > this.newValues.Count)
            {
                var potentialDuplicates = this.oldValues.GroupBy(v => new { v.Value }).Where(g => g.Count() > 1).Select(g => new { GroupName = g.Key, Members = g });

                if (potentialDuplicates.Count() > 0)
                {
                    foreach (var duplicateGroup in potentialDuplicates)
                    {
                        foreach (var duplicatedMember in duplicateGroup.Members)
                        {
                            duplicatedMember.IsDuplicate = true;
                        }
                    }
                }

                if (this.oldValues.Where(x => !x.HasBeenMatched && !x.IsDuplicate).Count() == 0)
                {
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MORE_VALUES_ON_OLD_SERVICE_ALL_DUPLICATES);
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.FALSE_POSITIVE);
                }
            }

            return shouldContinueTesting;
        }

        private bool AreThereDuplicatesOnTheNewSide()
        {
            bool shouldContinueTesting = true;

            var potentialDuplicates = this.newValues.GroupBy(v => new { v.Value }).Where(g => g.Count() > 1).Select(g => new { GroupName = g.Key, Members = g });
            if (potentialDuplicates.Count() > 0)
            {
                foreach (var duplicateGroup in potentialDuplicates)
                {
                    foreach (var duplicatedMember in duplicateGroup.Members)
                    {
                        duplicatedMember.IsDuplicate = true;
                    }
                }

                this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.DUPLICATED_VALUES_ON_NEW_SERVICE);
                this.resultReport.UpdateSeverity(EnumResultSeverityType.WARNING);
            }

            return shouldContinueTesting;
        }

        #endregion
    }
}
