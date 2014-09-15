using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public class CompareStrategyStringCollection : CompareStrategy
    {
        public CompareStrategyStringCollection(HashSet<string> oldValues, HashSet<string> newValues, ResultReport resultReport) 
            : base(oldValues,newValues,resultReport)
        {

        }

        public override void Investigate()
        {
            bool keepGoing = true;

            if (keepGoing)
            {
                keepGoing = AreThereDuplicatesOnTheNewSide();
            }

            if(keepGoing)
            {
                keepGoing = DoesAtLeastOneCollectionContainEntries();
            }

            if (keepGoing)
            {
                keepGoing = DoesOldCollectionContainEntries();
            }

            if (keepGoing)
            {
                keepGoing = DoesNewCollectionContainEntries();
            }

            if (keepGoing)
            {
                keepGoing = AreBothCollectionsEquivalent();
            }

            if (keepGoing)
            {
                keepGoing = AreMissingValuesOnTheNewSideDuplicatesOnTheOldSide();
            }

            if (keepGoing)
            {
                keepGoing = AreAllTheOldValuesFoundOnTheNewSide();
            }

            if (keepGoing)
            {
                keepGoing = AreTheMismatchesDueToTrailingSpaces();
            }
        }

        #region Scenarios

        private bool DoesAtLeastOneCollectionContainEntries()
        {
            bool shouldContinueTesting = true;

            if (this.resultReport.OldValues.Count <= 0 && this.resultReport.NewValues.Count <= 0)
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

        private bool DoesOldCollectionContainEntries()
        {
            bool shouldContinueTesting = true;

            if (this.resultReport.OldValues.Where(x => x != null).Count() <= 0)
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.WARNING_ONLY_NEW);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.SUCCESS);
            }

            return shouldContinueTesting;
        }

        private bool DoesNewCollectionContainEntries()
        {
            bool shouldContinueTesting = true;

            if (this.resultReport.NewValues.Where(x=>x!=null).Count() <= 0)
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.ERROR_ONLY_OLD);
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

            try
            {
                CollectionAssert.AreEquivalent(this.resultReport.OldValues.ToList(), this.resultReport.NewValues.ToList(), this.resultReport.TestDescription);
                this.resultReport.UpdateResult(EnumResultSeverityType.SUCCESS);
                shouldContinueTesting = false;
            }
            catch (AssertFailedException e)
            {
                this.resultReport.UpdateResult(EnumResultSeverityType.ERROR);
                this.resultReport.ErrorMessage = ParsingHelper.ReplaceProblematicTagsForHtml(e.Message);
            }
            return shouldContinueTesting;
        }

        private bool AreMissingValuesOnTheNewSideDuplicatesOnTheOldSide()
        {
            bool shouldContinueTesting = true;

            // if there are more values on the old side
            // check whether the values are really missing on the new side
            // or if they were all doublons - which ends up being a FALSE POSITIVE
            if (this.resultReport.OldValues.Count > this.resultReport.NewValues.Count)
            {
                var differenceQueryToAvoidDoublons = this.resultReport.OldValues.Except(this.resultReport.NewValues);

                if (differenceQueryToAvoidDoublons.Count() == 0)
                {
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MORE_VALUES_ON_OLD_SERVICE_ALL_DUPLICATES);
                    this.resultReport.UpdateResult(EnumResultSeverityType.FALSE_POSITIVE);
                }
            }

            return shouldContinueTesting;
        }

        private bool AreThereDuplicatesOnTheNewSide()
        {
            bool shouldContinueTesting = true;

            // check for doublons on new side anyway
            var differenceQueryCheckDoublonsInNewService = this.resultReport.NewValues.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key);
            if (differenceQueryCheckDoublonsInNewService.Count() > 0)
            {
                this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.DUPLICATED_VALUES_ON_NEW_SERVICE);
                this.resultReport.UpdateResult(EnumResultSeverityType.WARNING);
            }

            return shouldContinueTesting;
        }

        private bool AreAllTheOldValuesFoundOnTheNewSide()
        {
            bool shouldContinueTesting = true;

            // if there are more values on the new side
            // it could be that the system has corrected missing data from the old service
            // a warning will be issued anyway, but if all the old value is found in the new service, it will not be ranked as an error
            // since consistency is maintained
            // e.g. the old service does not always An Identifier for the Organization
            if (this.resultReport.NewValues.Count > this.resultReport.OldValues.Count)
            {
                this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MORE_VALUES_ON_NEW_SERVICE);

                try
                {
                    // TO DO : The evaluation of success may be delegated to a higher level - it could be that no OrgID is returned BUT all the dept names.
                    // This is why this ERROR is ranked as WARNING.
                    CollectionAssert.IsSubsetOf(this.resultReport.OldValues.ToList(), this.resultReport.NewValues.ToList(), this.resultReport.TestDescription);

                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND);
                    this.resultReport.UpdateResult(EnumResultSeverityType.FALSE_POSITIVE);
                }
                catch (AssertFailedException)
                {
                    this.resultReport.UpdateResult(EnumResultSeverityType.WARNING);
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MISSING_VALUES_ON_NEW_SERVICE);
                }
            }

            return shouldContinueTesting;
        }

        public bool AreTheMismatchesDueToTrailingSpaces()
        {
            bool shouldContinueTesting = true;

            // updating to the latest version of the service, it happens that the values retrieved from the new service are null
            if (this.resultReport.OldValues.Where(z => z != null).Count() > 0 && this.resultReport.NewValues.Where(z => z != null).Count() > 0)
            {

            // check if some of the inconsistencies are due to trailing spaces in the single string values
            var missingOldValues = this.resultReport.OldValues.Except(this.resultReport.NewValues);
            var missingNewValues = this.resultReport.NewValues.Except(this.resultReport.OldValues);

                IEnumerable<string> trimmedMissingOldValues;
                try 
                {
                    trimmedMissingOldValues = missingOldValues.Select(s => s.Trim());

                } catch (Exception)
                {
                    trimmedMissingOldValues = new List<string>();
                }

                IEnumerable<string> trimmedMissingNewValues;
                try
                {
                    trimmedMissingNewValues = missingNewValues.Select(s => s.Trim());

                }
                catch (Exception)
                {
                    trimmedMissingNewValues = new List<string>();
                }

                var leftovers = trimmedMissingOldValues.Except(trimmedMissingNewValues);

                if (leftovers.Count() != missingOldValues.Count())
                {
                    this.resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MISMATCH_DUE_TO_TRAILING_WHITE_SPACES);
                    this.resultReport.UpdateResult(EnumResultSeverityType.WARNING);

                    // all of the mismatches are due to trailing spaces
                    if (leftovers.Count() == 0)
                    {
                        this.resultReport.UpdateResult(EnumResultSeverityType.FALSE_POSITIVE);
                    }
                }
            }

            return shouldContinueTesting;
        }

        #endregion
    }
}