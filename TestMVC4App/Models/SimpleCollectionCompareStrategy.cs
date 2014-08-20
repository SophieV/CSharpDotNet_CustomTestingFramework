using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace TestMVC4App.Models
{
    public class SimpleCollectionCompareStrategy : CompareStrategy
    {
        public SimpleCollectionCompareStrategy(List<string> oldValues, List<string> newValues, ResultReport resultReport) 
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
                this.resultReport.Observations.Add(ObservationLabel.VALUES_NOT_POPULATED);
                this.resultReport.UpdateSeverityState(SeverityState.WARNING);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateSeverityState(SeverityState.SUCCESS);
            }

            return shouldContinueTesting;
        }

        private bool AreBothCollectionsEquivalent()
        {
            bool shouldContinueTesting = true;

            try
            {
                CollectionAssert.AreEquivalent(this.resultReport.OldValues, this.resultReport.NewValues, this.resultReport.TestDescription);
                this.resultReport.UpdateSeverityState(SeverityState.SUCCESS);
                shouldContinueTesting = false;
            }
            catch (AssertFailedException e)
            {
                this.resultReport.UpdateSeverityState(SeverityState.ERROR);
                this.resultReport.ErrorMessage = CompareStrategy.ReplaceProblematicTagsForHtml(e.Message);
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
                    this.resultReport.Observations.Add(ObservationLabel.MORE_DUPLICATED_VALUES_ON_OLD_SERVICE);
                    this.resultReport.UpdateSeverityState(SeverityState.FALSE_POSITIVE);
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
                this.resultReport.Observations.Add(ObservationLabel.DUPLICATED_VALUES_ON_NEW_SERVICE);
                this.resultReport.UpdateSeverityState(SeverityState.WARNING);
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
                this.resultReport.Observations.Add(ObservationLabel.MORE_VALUES_ON_NEW_SERVICE);

                try
                {
                    // TO DO : The evaluation of success may be delegated to a higher level - it could be that no OrgID is returned BUT all the dept names.
                    // This is why this ERROR is ranked as WARNING.
                    CollectionAssert.IsSubsetOf(this.resultReport.OldValues, this.resultReport.NewValues, this.resultReport.TestDescription);

                    this.resultReport.UpdateSeverityState(SeverityState.FALSE_POSITIVE);
                    this.resultReport.Observations.Add(ObservationLabel.ALL_VALUES_OF_OLD_SUBSET_FOUND);
                }
                catch (AssertFailedException)
                {
                    this.resultReport.UpdateSeverityState(SeverityState.WARNING);
                    this.resultReport.Observations.Add(ObservationLabel.MISSING_VALUES_ON_NEW_SERVICE);
                }
            }

            return shouldContinueTesting;
        }

        public bool AreTheMismatchesDueToTrailingSpaces()
        {
            bool shouldContinueTesting = true;

            // check if some of the inconsistencies are due to trailing spaces in the single string values
            var missingOldValues = this.resultReport.OldValues.Except(this.resultReport.NewValues);
            var missingNewValues = this.resultReport.NewValues.Except(this.resultReport.OldValues);

            IEnumerable<string> trimmedMissingOldValues = missingOldValues.Select(s => s.Trim());
            IEnumerable<string> trimmedMissingNewValues = missingNewValues.Select(s => s.Trim());

            var leftovers = trimmedMissingOldValues.Except(trimmedMissingNewValues);
#if DEBUG
            System.Diagnostics.Debug.WriteLine(leftovers.Count());
#endif

            if (leftovers.Count() != missingOldValues.Count())
            {
                this.resultReport.Observations.Add(ObservationLabel.VALUE_CONTAINS_TRAILING_WHITE_SPACES);
                this.resultReport.UpdateSeverityState(SeverityState.WARNING);

                // all of the mismatches are due to trailing spaces
                if (leftovers.Count() == 0)
                {
                    this.resultReport.UpdateSeverityState(SeverityState.FALSE_POSITIVE);
                }
            }

            return shouldContinueTesting;
        }

        #endregion
    }
}