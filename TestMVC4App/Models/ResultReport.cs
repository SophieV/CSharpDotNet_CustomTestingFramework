using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class ResultReport
    {
        public TimeSpan Duration { get; set; }

        public ResultSeverityType Result { get; private set; }

        public List<IdentifiedDataBehavior> IdentifedDataBehaviors { get; set; }

        public string ErrorMessage { get; set; }

        public string TestName { get; private set; }

        public string TestDescription { get; private set; }

        public List<string> OldValues { get; private set; }

        public List<string> NewValues { get; private set; }

        public ResultReport(string testName, string testDescription)
        {
            this.TestName = testName;
            this.TestDescription = testDescription;
            this.ErrorMessage = string.Empty;
            this.IdentifedDataBehaviors = new List<IdentifiedDataBehavior>();
            this.OldValues = new List<string>();
            this.NewValues = new List<string>();
            this.Result = ResultSeverityType.SUCCESS;
        }

        public void ResetForReTesting()
        {
            this.OldValues.Clear();
            this.NewValues.Clear();
            this.Result = ResultSeverityType.SUCCESS;
            this.ErrorMessage = string.Empty;
            this.IdentifedDataBehaviors.Clear();
        }

        public void AddDetailedValues(List<string> oldValues, List<string> newValues)
        {
            if (oldValues != null)
            {
                this.OldValues = oldValues;
            }

            if (newValues != null)
            {
                this.NewValues = newValues;
            }
        }

        public void UpdateResult(ResultSeverityType newSeverityStateReturned)
        {
            if (newSeverityStateReturned == ResultSeverityType.WARNING)
            {
                if (this.Result == ResultSeverityType.SUCCESS)
                {
                    this.Result = newSeverityStateReturned;
                    System.Diagnostics.Debug.WriteLine("Severity was updated to " + newSeverityStateReturned);
                }
            }
            else
            {
                // warnings are the only severity that is weaker than the rest and can happen afterwards
                // for all the others we assume chronological order as it investigates more specific scenarios
                this.Result = newSeverityStateReturned;
                System.Diagnostics.Debug.WriteLine("Severity was updated to " + newSeverityStateReturned);
            }
        }
    }
}