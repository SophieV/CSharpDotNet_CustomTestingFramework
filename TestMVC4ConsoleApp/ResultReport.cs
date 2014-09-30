using System;
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class ResultReport
    {
        public int Upi { get; private set; }

        public int UserId { get; protected set; }

        public EnumResultDisplayFormat DisplayFormat { get; private set; }
        public TimeSpan Duration { get; set; }

        public EnumResultSeverityType Severity { get; private set; }

        public List<EnumIdentifiedDataBehavior> IdentifedDataBehaviors { get; set; }

        public string ErrorMessage { get; set; }

        public EnumTestUnitNames TestName { get; private set; }

        public string TestDescription { get; private set; }

        public HashSet<StringDescriptor> OldValues { get; private set; }

        public HashSet<OrganizationTreeDescriptor> OldOrganizationValues { private set; get; }

        public HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> OldStructureValues { get; private set; }

        public OrganizationTreeDescriptor OldTreeRoot { private set; get; }

        public HashSet<StringDescriptor> NewValues { get; private set; }

        public HashSet<OrganizationTreeDescriptor> NewOrganizationValues { private set; get; }

        public HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> NewStructureValues { get; private set; }

        public OrganizationTreeDescriptor NewTreeRoot { private set; get; }

        public int TreeComparisonIndexError { get; set; }

        public ResultReport(int userId, int upi, EnumTestUnitNames testName, string testDescription)
        {
            this.TestName = testName;
            this.TestDescription = testDescription;
            this.ErrorMessage = string.Empty;
            this.IdentifedDataBehaviors = new List<EnumIdentifiedDataBehavior>();
            this.OldValues = new HashSet<StringDescriptor>();
            this.NewValues = new HashSet<StringDescriptor>();
            this.OldOrganizationValues = new HashSet<OrganizationTreeDescriptor>();
            this.NewOrganizationValues = new HashSet<OrganizationTreeDescriptor>();
            this.OldStructureValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>();
            this.NewStructureValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>();
            this.OldTreeRoot = null;
            this.NewTreeRoot = null;
            this.TreeComparisonIndexError = -1;
            this.Severity = EnumResultSeverityType.SUCCESS;
            this.Upi = upi;
            this.UserId = userId;
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
            this.Severity = EnumResultSeverityType.SUCCESS;
            this.ErrorMessage = string.Empty;
            this.IdentifedDataBehaviors.Clear();
        }

        public void AddDetailedValues(HashSet<StringDescriptor> oldValues, HashSet<StringDescriptor> newValues)
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

        public void AddDetailedValues(HashSet<Dictionary<EnumOldServiceFieldsAsKeys,StringDescriptor>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys,StringDescriptor>> newValues)
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

        public void UpdateSeverity(EnumResultSeverityType newSeverityStateReturned)
        {
            if (newSeverityStateReturned == EnumResultSeverityType.WARNING)
            {
                if (this.Severity == EnumResultSeverityType.SUCCESS || this.Severity == EnumResultSeverityType.FALSE_POSITIVE)
                {
                    this.Severity = newSeverityStateReturned;
                }
            }
            else
            {
                // warnings are the only severity that is weaker than the rest and can happen afterwards
                // for all the others we assume chronological order as it investigates more specific scenarios
                this.Severity = newSeverityStateReturned;
            }
        }
    }
}