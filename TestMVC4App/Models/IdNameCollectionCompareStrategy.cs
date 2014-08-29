using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class IdNameCollectionCompareStrategy : CompareStrategy
    {
        private List<Tuple<string, string>> oldList;
        private List<Tuple<string, string>> newList;
        private int leftOversOldCount = -1;

        public IdNameCollectionCompareStrategy(List<Tuple<string,string>> listOldIdsAndNames,
                                               List<Tuple<string, string>> listNewIdsAndNames,
                                               ResultReport resultReport)
            : base(listOldIdsAndNames.Where(d => !string.IsNullOrEmpty(d.Item1) || !string.IsNullOrEmpty(d.Item2)).Select(d => d.Item1 + " - " + d.Item2).ToList(),
                                                      listNewIdsAndNames.Where(x => x.Item1 != null || x.Item2 != null).Select(x => x.Item1 + " - " + x.Item2).ToList(),
                                                      resultReport)
        {
            this.oldList = listOldIdsAndNames.Where(d => !string.IsNullOrEmpty(d.Item1) || !string.IsNullOrEmpty(d.Item2)).ToList();
            this.newList = listNewIdsAndNames.Where(z => !string.IsNullOrEmpty(z.Item1) || !string.IsNullOrEmpty(z.Item2)).ToList();
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
                keepGoing = AreTheMismatchesDueToMissingIds();
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

            var potentialDuplicates = newList.GroupBy(v => new { v.Item1, v.Item2 }).Where(g => g.Count() > 1);

            if (potentialDuplicates.Count() > 0)
            {
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.DUPLICATED_VALUES_ON_NEW_SERVICE);
                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
            }

            return shouldContinueTesting;
        }

        private bool DoesAtLeastOneCollectionContainEntries()
        {
            bool shouldContinueTesting = true;

            if (this.oldList.Count() <= 0 && this.newList.Count <= 0)
            {
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.VALUES_NOT_POPULATED);
                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateResult(ResultSeverityType.SUCCESS);
            }

            return shouldContinueTesting;
        }

        private bool AreBothCollectionsEquivalent()
        {
            bool shouldContinueTesting = true;

            var leftOversOld = oldList.Except(newList, new IdAndNameTupleComparer());
            var leftOversNew = newList.Except(oldList, new IdAndNameTupleComparer());

            this.leftOversOldCount = leftOversOld.Count();
            var leftOversNewCount = leftOversNew.Count();

            if (this.leftOversOldCount == 0 && leftOversNewCount == 0)
            {
                this.resultReport.UpdateResult(ResultSeverityType.SUCCESS);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateResult(ResultSeverityType.ERROR);
                this.resultReport.ErrorMessage = "The list of ids and names compared are not equal";
            }

            return shouldContinueTesting;
        }

        private bool AreTheMismatchesDueToMissingIds()
        {
            bool shouldContinueTesting = true;

            // we care about matching entries from the old service
            var comparer = new IdOrNameTupleComparer();
            var leftOvers = oldList.Except(newList, new IdOrNameTupleComparer());

            if (leftOvers.Count() < leftOversOldCount)
            {
                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.MISMATCH_DUE_TO_MISSING_IDS);

                if (leftOvers.Count() == 0)
                {
                    this.resultReport.UpdateResult(ResultSeverityType.ERROR_WITH_EXPLANATION);
                    this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND);
                    shouldContinueTesting = false;
                }
            }

            return shouldContinueTesting;
        }

        private bool AreTheMismatchesDueToTrailingSpaces()
        {
            bool shouldContinueTesting = true;

            // we care about matching entries from the old service
            var leftOvers = oldList.Except(newList, new IdOrTrimmedNameTupleComparer());

            if (leftOvers.Count() < leftOversOldCount)
            {
                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.MISMATCH_DUE_TO_TRAILING_WHITE_SPACES);

                if (leftOvers.Count() == 0)
                {
                    this.resultReport.UpdateResult(ResultSeverityType.ERROR_WITH_EXPLANATION);
                    this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND);
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
                this.resultReport.UpdateResult(ResultSeverityType.FALSE_POSITIVE);
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND);
            }

            return shouldContinueTesting;
        }
    }
}