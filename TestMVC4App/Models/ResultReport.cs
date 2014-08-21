using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class ResultReport
    {
        public TimeSpan Duration { get; set; }

        public ResultSeverityType Result { get { return result; } }
        private ResultSeverityType result;

        public List<IdentifiedDataBehavior> IdentifedDataBehaviors { get; set; }

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
            this.IdentifedDataBehaviors = new List<IdentifiedDataBehavior>();
            this.OldValues = new List<string>();
            this.NewValues = new List<string>();
            this.result = ResultSeverityType.SUCCESS;
        }

        public void UpdateResult(ResultSeverityType newSeverityStateReturned)
        {
            if (newSeverityStateReturned == ResultSeverityType.WARNING)
            {
                if (result == ResultSeverityType.SUCCESS)
                {
                    result = newSeverityStateReturned;
                    System.Diagnostics.Debug.WriteLine("Severity was updated to " + newSeverityStateReturned);
                }
            }
            else
            {
                // warnings are the only severity that is weaker than the rest and can happen afterwards
                // for all the others we assume chronological order as it investigates more specific scenarios
                result = newSeverityStateReturned;
                System.Diagnostics.Debug.WriteLine("Severity was updated to " + newSeverityStateReturned);
            }
        }
    }
}