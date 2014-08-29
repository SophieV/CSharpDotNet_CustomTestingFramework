using System;
using System.Collections.Generic;

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

        public List<OrganizationTreeDescriptor> OldOrganizationValues { private set; get; }

        public OrganizationTreeDescriptor OldTreeRoot { private set; get; }

        public List<string> NewValues { get; private set; }

        public List<OrganizationTreeDescriptor> NewOrganizationValues { private set; get; }

        public OrganizationTreeDescriptor NewTreeRoot { private set; get; }

        public int TreeComparisonIndexError { get; set; }

        public ResultReport(string testName, string testDescription)
        {
            this.TestName = testName;
            this.TestDescription = testDescription;
            this.ErrorMessage = string.Empty;
            this.IdentifedDataBehaviors = new List<IdentifiedDataBehavior>();
            this.OldValues = new List<string>();
            this.NewValues = new List<string>();
            this.OldOrganizationValues = new List<OrganizationTreeDescriptor>();
            this.NewOrganizationValues = new List<OrganizationTreeDescriptor>();
            this.OldTreeRoot = null;
            this.NewTreeRoot = null;
            this.TreeComparisonIndexError = -1;
            this.Result = ResultSeverityType.SUCCESS;
        }

        public void ResetForReTesting()
        {
            this.OldValues.Clear();
            this.NewValues.Clear();
            this.OldOrganizationValues.Clear();
            this.NewOrganizationValues.Clear();
            this.TreeComparisonIndexError = -1;
            this.OldTreeRoot = null;
            this.NewTreeRoot = null;
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

        public void AddDetailedValues(List<OrganizationTreeDescriptor> oldValues, OrganizationTreeDescriptor oldTreeRoot, List<OrganizationTreeDescriptor> newValues, OrganizationTreeDescriptor newTreeRoot)
        {
            if (oldValues != null)
            {
                this.OldOrganizationValues = oldValues;
                this.OldTreeRoot = oldTreeRoot;
            }

            if (newValues != null)
            {
                this.NewOrganizationValues = newValues;
                this.NewTreeRoot = newTreeRoot;
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