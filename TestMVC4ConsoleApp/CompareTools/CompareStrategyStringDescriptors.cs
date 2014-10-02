using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class CompareStrategyStringDescriptors : CompareStrategy
    {
        private int leftOversOldCount = -1;
        private HashSet<StringDescriptor> oldValues;
        private HashSet<StringDescriptor> newValues;

        public CompareStrategyStringDescriptors(HashSet<StringDescriptor> oldValues, HashSet<StringDescriptor> newValues, ResultReport resultReport)
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
                // case sensitive, shifted, not trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(true, true, false, false);
            }

            if (keepGoing)
            {
                // case sensitive, not shifted, trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(true, false, true, false);
            }

            if (keepGoing)
            {
                // plain : case sensitive, not shifted, not trimmed, partial
                keepGoing = AreBothCollectionsEquivalent(true, false, false, true);
            }

            if (keepGoing)
            {
                // not case sensitive, not shifted, not trimmed, not partial
                keepGoing = AreBothCollectionsEquivalent(false, false, false, false);
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

            IEnumerable<HashSet<StringDescriptor>> leftOversOld;
            IEnumerable<HashSet<StringDescriptor>> leftOversNew;

            int previousCount = this.oldValues.Where(x => !x.SingleValueHasBeenMatched).Count();

            List<HashSet<StringDescriptor>> oldValuesList = new List<HashSet<StringDescriptor>>();
            oldValuesList.Add(new HashSet<StringDescriptor>(this.oldValues.Where(x => !x.SingleValueHasBeenMatched)));

            List<HashSet<StringDescriptor>> newValuesList = new List<HashSet<StringDescriptor>>();
            newValuesList.Add(new HashSet<StringDescriptor>(this.newValues.Where(x => !x.SingleValueHasBeenMatched)));

            if (partial)
            {
                leftOversOld = oldValuesList.Except(newValuesList, new ComparerStringPartial());
                leftOversNew = newValuesList.Except(oldValuesList, new ComparerStringPartial());
            }
            else if (shifted)
            {
                leftOversOld = oldValuesList.Except(newValuesList, new ComparerStringShifted());
                leftOversNew = newValuesList.Except(oldValuesList, new ComparerStringShifted());
            }
            else if (trim)
            {
                leftOversOld = oldValuesList.Except(newValuesList, new ComparerStringTrimmed());
                leftOversNew = newValuesList.Except(oldValuesList, new ComparerStringTrimmed());
            }
            else if (caseSensitive)
            {
                leftOversOld = oldValuesList.Except(newValuesList, new ComparerString());
                leftOversNew = newValuesList.Except(oldValuesList, new ComparerString());
            }
            else
            {
                leftOversOld = oldValuesList.Except(newValuesList, new ComparerStringNotCaseSensitive());
                leftOversNew = newValuesList.Except(oldValuesList, new ComparerStringNotCaseSensitive());
            }

            this.leftOversOldCount = leftOversOld.Count();
            var leftOversNewCount = leftOversNew.Count();

            if (this.leftOversOldCount < previousCount)
            {
                if (shifted)
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
                if (!caseSensitive || trim || shifted)
                {
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.FALSE_POSITIVE);
                }
                else
                {
                    this.resultReport.UpdateSeverity(EnumResultSeverityType.SUCCESS);
                    shouldContinueTesting = false;
                }
            }
            else
            {
                this.resultReport.UpdateSeverity(EnumResultSeverityType.ERROR);
                this.resultReport.ErrorMessage = "The lists of " + (trim ? "trimmed " : string.Empty) + (shifted ? "shifted " : string.Empty) + " strings compared " + (!caseSensitive ? "without case " : string.Empty) + "are not equal";
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
                            duplicatedMember.Duplicate = true;
                        }
                    }
                }

                if (this.oldValues.Where(x => !x.SingleValueHasBeenMatched).Count() == 0)
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
                        duplicatedMember.Duplicate = true;
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
