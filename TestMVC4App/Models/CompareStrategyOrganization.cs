using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public class CompareStrategyOrganization : CompareStrategy
    {
        private List<OrganizationTreeDescriptor> oldList;
        private List<OrganizationTreeDescriptor> newList;
        private int leftOversOldCount = -1;

        public CompareStrategyOrganization(List<OrganizationTreeDescriptor> listOldIdsAndNames, OrganizationTreeDescriptor oldTreeRoot, 
                                           List<OrganizationTreeDescriptor> listNewIdsAndNames, OrganizationTreeDescriptor newTreeRoot, 
                                           ResultReport resultReport)
            : base(listOldIdsAndNames, oldTreeRoot,listNewIdsAndNames, newTreeRoot,resultReport)
        {
            this.oldList = listOldIdsAndNames.Where(d => !string.IsNullOrEmpty(d.ID) || !string.IsNullOrEmpty(d.Name)).ToList();
            this.newList = listNewIdsAndNames.Where(z => !string.IsNullOrEmpty(z.ID) || !string.IsNullOrEmpty(z.Name)).ToList();
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

            var potentialDuplicates = newList.GroupBy(v => new { v.ID, v.Name }).Where(g => g.Count() > 1).Select( g => new { GroupName = g.Key, Members = g});

            if (potentialDuplicates.Count() > 0)
            {

                foreach (var duplicateGroup in potentialDuplicates)
                {
                    foreach(var duplicatedMember in duplicateGroup.Members)
                    {
                        duplicatedMember.IsDuplicate = true;
                    }
                }

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

            var leftOversOld = oldList.Except(newList, new ComparerOrganizationIdAndName());
            var leftOversNew = newList.Except(oldList, new ComparerOrganizationIdAndName());

            this.leftOversOldCount = leftOversOld.Count();
            var leftOversNewCount = leftOversNew.Count();

            if (this.leftOversOldCount == 0 && leftOversNewCount == 0)
            {
                this.resultReport.UpdateResult(ResultSeverityType.SUCCESS);
                shouldContinueTesting = false;
            }
            else
            {
                foreach (OrganizationTreeDescriptor missingEntry in leftOversOld)
                {
                    missingEntry.IsMissing = true;
                }

                foreach (OrganizationTreeDescriptor missingEntry in leftOversNew)
                {
                    missingEntry.IsMissing = true;
                }


                this.resultReport.UpdateResult(ResultSeverityType.ERROR);
                this.resultReport.ErrorMessage = "The list of ids and names compared are not equal";
            }

            return shouldContinueTesting;
        }

        private bool AreTheMismatchesDueToMissingIds()
        {
            bool shouldContinueTesting = true;

            // we care about matching entries from the old service
            var comparer = new ComparerOrganizationIdOrName();
            var leftOvers = oldList.Except(newList, new ComparerOrganizationIdOrName());

            // but we will do other way around to maintain consistency in display
            var leftOversNew = newList.Except(oldList, new ComparerOrganizationIdOrName());

            if (leftOvers.Count() < leftOversOldCount)
            {
                // set missing entries back to not missing...
                var listSetToMissing = oldList.Where(x => x.IsMissing == true);
                foreach(var missingEntry in listSetToMissing)
                {
                    if(!leftOvers.Contains(missingEntry))
                    {
                        missingEntry.IsMissing = false;
                    }
                }

                listSetToMissing = newList.Where(x => x.IsMissing == true);
                foreach (var missingEntry in listSetToMissing)
                {
                    if (!leftOversNew.Contains(missingEntry))
                    {
                        missingEntry.IsMissing = false;
                    }
                }

                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.MISMATCH_DUE_TO_MISSING_IDS);

                if (leftOvers.Count() == 0)
                {
                    this.resultReport.UpdateResult(ResultSeverityType.FALSE_POSITIVE);
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
            var leftOvers = oldList.Except(newList, new ComparerOrganizationIdAndTrimmedName());

            var leftOversNew = newList.Except(oldList, new ComparerOrganizationIdAndTrimmedName());

            if (leftOvers.Count() < leftOversOldCount)
            {
                // set missing entries back to not missing...
                var listSetToMissing = oldList.Where(x => x.IsMissing == true);
                foreach (var missingEntry in listSetToMissing)
                {
                    if (!leftOvers.Contains(missingEntry))
                    {
                        missingEntry.IsMissing = false;
                    }
                }

                listSetToMissing = newList.Where(x => x.IsMissing == true);
                foreach (var missingEntry in listSetToMissing)
                {
                    if (!leftOversNew.Contains(missingEntry))
                    {
                        missingEntry.IsMissing = false;
                    }
                }

                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.MISMATCH_DUE_TO_TRAILING_WHITE_SPACES);

                if (leftOvers.Count() == 0)
                {
                    this.resultReport.UpdateResult(ResultSeverityType.FALSE_POSITIVE);
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