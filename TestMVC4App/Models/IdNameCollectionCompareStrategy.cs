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
        private int countPerfectMatchIdNameLeftoversOld = 0;

        public IdNameCollectionCompareStrategy(List<Tuple<string,string>> listOldIdsAndNames,
                                               List<Tuple<string, string>> listNewIdsAndNames,
                                               ResultReport resultReport) 
                                               : base(listOldIdsAndNames.Select(x=> x.Item1 + " - " + x.Item2).ToList(),
                                                      listNewIdsAndNames.Select(x => x.Item1 + " - " + x.Item2).ToList(),
                                                      resultReport)
        {
            this.oldList = listOldIdsAndNames.Where(z=> z.Item1 != null && z.Item2 != null).ToList();
            this.newList = listNewIdsAndNames.Where(z => z.Item1 != null && z.Item2 != null).ToList();
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

            if (leftOversOld.Count() == 0 && leftOversNew.Count() == 0)
            {
                this.resultReport.UpdateResult(ResultSeverityType.SUCCESS);
                shouldContinueTesting = false;
            }
            else
            {
                countPerfectMatchIdNameLeftoversOld = leftOversOld.Count();
                this.resultReport.UpdateResult(ResultSeverityType.ERROR);
                this.resultReport.ErrorMessage = CompareStrategy.ReplaceProblematicTagsForHtml("The list of ids and names compared are not equal");
            }

            return shouldContinueTesting;
        }

        private bool AreTheMismatchesDueToMissingIds()
        {
            bool shouldContinueTesting = true;

            // we care about matching entries from the old service
            var leftOvers = oldList.Concat(newList).Distinct(new IdOrNameTupleComparer());

            if (leftOvers.Count() < countPerfectMatchIdNameLeftoversOld)
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

            if (leftOvers.Count() < countPerfectMatchIdNameLeftoversOld)
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
    }
}