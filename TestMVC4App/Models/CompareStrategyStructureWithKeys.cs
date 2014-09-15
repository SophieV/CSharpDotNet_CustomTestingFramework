using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class CompareStrategyStructureWithKeys : CompareStrategy
    {
        private List<Dictionary<OldServiceFieldsAsKeys, string>> oldList;
        private List<Dictionary<OldServiceFieldsAsKeys, string>> newList;
        private int leftOversOldCount = -1;

        public CompareStrategyStructureWithKeys(HashSet<Dictionary<OldServiceFieldsAsKeys,string>> oldValues, HashSet<Dictionary<OldServiceFieldsAsKeys,string>> newValues, ResultReport resultReport)
            : base(null,null,resultReport)
        {
            this.oldList = oldValues.ToList();
            this.newList = newValues.ToList();
        }
        public override void Investigate()
        {
            bool keepGoing = true;

            if (keepGoing)
            {
                keepGoing = AreThereDuplicatesOnTheNewSide();
            }

            if (keepGoing)
            {
                keepGoing = DoesAtLeastOneCollectionContainEntries();
            }

            if (keepGoing)
            {
                keepGoing = AreBothCollectionsEquivalent();
            }

            if (keepGoing)
            {
                keepGoing = AreTheMismatchesDueToTrailingSpaces();
            }

            if (keepGoing)
            {
                keepGoing = AreAllTheOldValuesFoundOnTheNewSide();
            }
        }

        private bool AreThereDuplicatesOnTheNewSide()
        {
            bool shouldContinueTesting = true;

            var potentialDuplicates = newList.Except(newList.Distinct(new ComparerStructureWithKeys()));

            if (potentialDuplicates.Count() > 0)
            {
                this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.DUPLICATED_VALUES_ON_NEW_SERVICE);
                this.resultReport.UpdateResult(EnumResultSeverityType.WARNING);
            }

            return shouldContinueTesting;
        }

        private bool DoesAtLeastOneCollectionContainEntries()
        {
            bool shouldContinueTesting = true;

            if (this.oldList.Count() <= 0 && this.newList.Count <= 0)
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.WARNING_NO_DATA);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.SUCCESS);
            }

            return shouldContinueTesting;
        }

        private bool AreBothCollectionsEquivalent()
        {
            bool shouldContinueTesting = true;

            var leftOversOld = oldList.Except(newList, new ComparerStructureWithKeys());
            var leftOversNew = newList.Except(oldList, new ComparerStructureWithKeys());

            this.leftOversOldCount = leftOversOld.Count();
            var leftOversNewCount = leftOversNew.Count();

            if (this.leftOversOldCount == 0 && leftOversNewCount == 0)
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.SUCCESS);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.ERROR);
                this.resultReport.ErrorMessage = "The lists compared are not equal";
            }

            return shouldContinueTesting;
        }

        private bool AreTheMismatchesDueToTrailingSpaces()
        {
            bool shouldContinueTesting = true;

            // we care about matching entries from the old service
            var leftOvers = oldList.Except(newList, new ComparerStructureWithKeysTrimmed());

            var leftOversNew = newList.Except(oldList, new ComparerStructureWithKeysTrimmed());

            if (leftOvers.Count() < leftOversOldCount)
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.WARNING);
                this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MISMATCH_DUE_TO_TRAILING_WHITE_SPACES);

                if (leftOvers.Count() == 0)
                {
                    this.resultReport.UpdateResult(EnumResultSeverityType.FALSE_POSITIVE);
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND);
                    shouldContinueTesting = false;
                }
            }

            return shouldContinueTesting;
        }

        private bool AreAllTheOldValuesFoundOnTheNewSide()
        {
            bool shouldContinueTesting = true;

            if (this.leftOversOldCount > -1 && this.leftOversOldCount < 1)
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.FALSE_POSITIVE);
                this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND);
            }

            return shouldContinueTesting;
        }
    }
}