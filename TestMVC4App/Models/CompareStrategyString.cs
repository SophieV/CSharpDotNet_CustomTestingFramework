using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public class CompareStrategyString : CompareStrategy
    {
        private string oldValue = string.Empty;
        private string newValue = string.Empty;

        public CompareStrategyString(HashSet<string> oldValues, HashSet<string> newValues, ResultReport resultReport)
        : base(oldValues, newValues, resultReport)
        {
            try
            {
                this.oldValue = oldValues.First();
                if (string.IsNullOrEmpty(this.oldValue))
                {
                    this.oldValue = string.Empty;
                }
            }
            catch(Exception)
            {
                this.oldValue = string.Empty;
            }


            try
            {
                this.newValue = newValues.First();
                if (string.IsNullOrEmpty(this.newValue))
                {
                    this.newValue = string.Empty;
                }
            }
            catch (Exception)
            {
                this.newValue = string.Empty;
            }

            System.Diagnostics.Debug.WriteLine("Current Test is " + resultReport.TestDescription);
        }

        public CompareStrategyString(string oldValue, string newValue, ResultReport resultReport) 
            : base (new HashSet<string>() {oldValue}, new HashSet<string>() {newValue}, resultReport)
        {
            if (!string.IsNullOrEmpty(oldValue))
            {
                this.oldValue = oldValue;
            }

            if (!string.IsNullOrEmpty(newValue))
            {
                this.newValue = newValue;
            }

            System.Diagnostics.Debug.WriteLine("Current Test is " + resultReport.TestDescription);
        }

        public override void Investigate()
        {
            // in some cases there is no need to run warning checks
            bool keepGoing = true;

            if (keepGoing)
            {
                keepGoing = AreBothValuesEmpty();
            }

            if (keepGoing)
            {
                keepGoing = AreBothValuesEqual();
            }

            if (keepGoing)
            {
                keepGoing = AreBothValuesEqualOnceTrimmed();
            }

            if (keepGoing)
            {
                keepGoing = DoesNewValueContainWhiteSpaceOnly();
            }

            if (keepGoing)
            {
                keepGoing = DoBothValuesHaveDifferentContent();
            }

            if (keepGoing)
            {
                keepGoing = MissingValueOnTheOldSide();
            }

            if (keepGoing)
            {
                keepGoing = MissingValueOnTheNewSide();
            }
        }

        #region Scenarios

        private bool AreBothValuesEmpty()
        {
            bool shouldContinueTesting = true;

            if(string.IsNullOrEmpty(oldValue) && string.IsNullOrEmpty(newValue))
            {
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.VALUES_NOT_POPULATED);
                this.resultReport.UpdateResult(ResultSeverityType.WARNING_NO_DATA);
                shouldContinueTesting = false;
            }
            else
            {
                this.resultReport.UpdateResult(ResultSeverityType.SUCCESS);
            }

            return shouldContinueTesting;
        }

        private bool AreBothValuesEqual()
        {
            bool shouldContinueTesting = true;

            try
            {
                Assert.AreEqual(oldValue, newValue, this.resultReport.TestDescription);
                this.resultReport.UpdateResult(ResultSeverityType.SUCCESS);
                shouldContinueTesting = false;

            }
            catch (AssertFailedException e)
            {
                this.resultReport.UpdateResult(ResultSeverityType.ERROR);
                this.resultReport.ErrorMessage = CompareStrategy.ReplaceProblematicTagsForHtml(e.Message);
            }

            return shouldContinueTesting;
        }

        private bool AreBothValuesEqualOnceTrimmed()
        {
            bool shouldContinueTesting = true;

            if (oldValue.Trim() == newValue.Trim())
            {
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.MISMATCH_DUE_TO_TRAILING_WHITE_SPACES);
                this.resultReport.UpdateResult(ResultSeverityType.ERROR_WITH_EXPLANATION);
                shouldContinueTesting = false;
            }

            return shouldContinueTesting;
        }

        private bool DoesNewValueContainWhiteSpaceOnly()
        {
            bool shouldContinueTesting = true;

            if (newValue == " ")
            {
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.VALUE_POPULATED_WITH_WHITE_SPACE_ON_NEW_SERVICE);
                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
            }

            return shouldContinueTesting;
        }

        private bool DoBothValuesHaveDifferentContent()
        {
            bool shouldContinueTesting = true;

            if (!string.IsNullOrEmpty(oldValue) && !string.IsNullOrEmpty(newValue))
            {
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.WRONG_VALUE);
                // it is set as warning only because it provides more explicit info on the error
                // the error has been logged already
                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
            }

            return shouldContinueTesting;
        }

        private bool MissingValueOnTheOldSide()
        {
            bool shouldContinueTesting = true;

            if (string.IsNullOrEmpty(oldValue) && !string.IsNullOrEmpty(newValue) && newValue != " ")
            {
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.MORE_VALUES_ON_NEW_SERVICE);
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND);
                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
            }

            return shouldContinueTesting;
        }

        private bool MissingValueOnTheNewSide()
        {
            bool shouldContinueTesting = true;

            if (!string.IsNullOrEmpty(oldValue) && string.IsNullOrEmpty(newValue) && newValue != " ")
            {
                this.resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.MISSING_VALUES_ON_NEW_SERVICE);
                this.resultReport.UpdateResult(ResultSeverityType.WARNING);
            }

            return shouldContinueTesting;
        }

        #endregion
    }
}