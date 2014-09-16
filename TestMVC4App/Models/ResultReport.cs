using System;
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class ResultReport
    {
        public EnumResultDisplayFormat DisplayFormat { get; private set; }
        public TimeSpan Duration { get; set; }

        public EnumResultSeverityType Result { get; private set; }

        public List<EnumIdentifiedDataBehavior> IdentifedDataBehaviors { get; set; }

        public string ErrorMessage { get; set; }

        public EnumTestUnitNames TestName { get; private set; }

        public string TestDescription { get; private set; }

        public HashSet<string> OldValues { get; private set; }

        public HashSet<OrganizationTreeDescriptor> OldOrganizationValues { private set; get; }

        public HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> OldStructureValues { get; private set; }

        public OrganizationTreeDescriptor OldTreeRoot { private set; get; }

        public HashSet<string> NewValues { get; private set; }

        public HashSet<OrganizationTreeDescriptor> NewOrganizationValues { private set; get; }

        public HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> NewStructureValues { get; private set; }

        public OrganizationTreeDescriptor NewTreeRoot { private set; get; }

        public int TreeComparisonIndexError { get; set; }

        public ResultReport(EnumTestUnitNames testName, string testDescription)
        {
            this.TestName = testName;
            this.TestDescription = testDescription;
            this.ErrorMessage = string.Empty;
            this.IdentifedDataBehaviors = new List<EnumIdentifiedDataBehavior>();
            this.OldValues = new HashSet<string>();
            this.NewValues = new HashSet<string>();
            this.OldOrganizationValues = new HashSet<OrganizationTreeDescriptor>();
            this.NewOrganizationValues = new HashSet<OrganizationTreeDescriptor>();
            this.OldStructureValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();
            this.NewStructureValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();
            this.OldTreeRoot = null;
            this.NewTreeRoot = null;
            this.TreeComparisonIndexError = -1;
            this.Result = EnumResultSeverityType.SUCCESS;
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
            this.Result = EnumResultSeverityType.SUCCESS;
            this.ErrorMessage = string.Empty;
            this.IdentifedDataBehaviors.Clear();
        }

        public void AddDetailedValues(HashSet<string> oldValues, HashSet<string> newValues)
        {
            if (oldValues != null)
            {
                this.OldValues = oldValues;
            }

            if (newValues != null)
            {
                this.NewValues = newValues;
            }

            this.DisplayFormat = EnumResultDisplayFormat.ListOfValues;
        }

        public void AddDetailedValues(HashSet<OrganizationTreeDescriptor> oldValues, OrganizationTreeDescriptor oldTreeRoot, HashSet<OrganizationTreeDescriptor> newValues, OrganizationTreeDescriptor newTreeRoot)
        {
            if (oldValues != null)
            {
                this.OldOrganizationValues = oldValues;
            }

            this.OldTreeRoot = oldTreeRoot;

            if (newValues != null)
            {
                this.NewOrganizationValues = newValues;
            }

            this.NewTreeRoot = newTreeRoot;

            this.DisplayFormat = EnumResultDisplayFormat.OrganizationTree;
        }

        public void AddDetailedValues(HashSet<Dictionary<EnumOldServiceFieldsAsKeys,string>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys,string>> newValues)
        {
            if (oldValues != null)
            {
                this.OldStructureValues = oldValues;
            }

            if (newValues != null)
            {
                this.NewStructureValues = newValues;
            }

            this.DisplayFormat = EnumResultDisplayFormat.StructureOfValues;
        }

        public void UpdateResult(EnumResultSeverityType newSeverityStateReturned)
        {
            if (newSeverityStateReturned == EnumResultSeverityType.WARNING)
            {
                if (this.Result == EnumResultSeverityType.SUCCESS 
                    || (this.IdentifedDataBehaviors.Contains(EnumIdentifiedDataBehavior.MORE_VALUES_ON_NEW_SERVICE) && this.IdentifedDataBehaviors.Contains(EnumIdentifiedDataBehavior.ALL_VALUES_OF_OLD_SUBSET_FOUND)))
                {
                    this.Result = newSeverityStateReturned;
                }
            }
            else
            {
                // warnings are the only severity that is weaker than the rest and can happen afterwards
                // for all the others we assume chronological order as it investigates more specific scenarios
                this.Result = newSeverityStateReturned;
            }
        }
    }
}