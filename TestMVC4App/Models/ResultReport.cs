using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class ResultReport
    {
        public TimeSpan Duration { get; set; }

        public ResultSeverityState SeverityResult { get { return severityResult; } }
        private ResultSeverityState severityResult;

        public List<IdentifiedDataBehavior> Observations { get; set; }

        public string ErrorMessage { get; set; }

        public string TestName { get { return testName; } }
        private string testName;

        public string TestDescription { get { return testDescription; } }
        private string testDescription;

        public List<string> OldValues { get; set; }

        public List<string> NewValues { get; set; }

        public ResultReport(string testName, string testDescription)
        {
            this.testName = testName;
            this.testDescription = testDescription;
            this.ErrorMessage = string.Empty;
            this.Observations = new List<IdentifiedDataBehavior>();
            this.OldValues = new List<string>();
            this.NewValues = new List<string>();
            this.severityResult = ResultSeverityState.SUCCESS;
        }

        public void UpdateSeverityState(ResultSeverityState newSeverityStateReturned)
        {
            if (newSeverityStateReturned == ResultSeverityState.WARNING)
            {
                if (severityResult == ResultSeverityState.SUCCESS)
                {
                    severityResult = newSeverityStateReturned;
                    System.Diagnostics.Debug.WriteLine("Severity was updated to " + newSeverityStateReturned);
                }
            }
            else
            {
                // warnings are the only severity that is weaker than the rest and can happen afterwards
                // for all the others we assume chronological order as it investigates more specific scenarios
                severityResult = newSeverityStateReturned;
                System.Diagnostics.Debug.WriteLine("Severity was updated to " + newSeverityStateReturned);
            }
        }
    }
}