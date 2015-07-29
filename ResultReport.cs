using System;
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class ResultReport
    {
        #region Shared

        public int OldId { get; private set; }

        public int UserId { get; protected set; }

        /// <summary>
        /// Will trigger the appropriate template for : Unstructured/Structured List of Strings, Tree of Organizations
        /// </summary>
        public EnumResultDisplayFormat DisplayFormat { get; private set; }

        /// <summary>
        /// Overall duration of the test.
        /// </summary>
        public TimeSpan Duration { get; set; }

        public EnumResultSeverityType Severity { get; private set; }

        public List<EnumIdentifiedDataBehavior> IdentifedDataBehaviors { get; set; }

        public string ErrorMessage { get; set; }

        public EnumTestUnitNames TestName { get; private set; }

        public string TestDescription { get; private set; }

        #endregion

        #region Unstructured List of Strings

        public HashSet<StringDescriptor> UnstructuredOldValues { get; private set; }

        public HashSet<StringDescriptor> UnstructuredNewValues { get; private set; }

        #endregion

        #region Structured List of Strings

        public HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> StructuredOldValues { get; private set; }

        public HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> StructuredNewValues { get; private set; }

        #endregion

        #region Tree of Organizations

        public OrganizationTreeDescriptor OldTreeRoot { private set; get; }

        public HashSet<OrganizationTreeDescriptor> NewOrganizationValues { private set; get; }

        public HashSet<OrganizationTreeDescriptor> OldOrganizationValues { private set; get; }

        public OrganizationTreeDescriptor NewTreeRoot { private set; get; }

        public int TreeComparisonIndexError { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="OldId"></param>
        /// <param name="testName"></param>
        /// <param name="testDescription"></param>
        public ResultReport(int userId, int OldId, EnumTestUnitNames testName, string testDescription)
        {
            this.TestName = testName;
            this.TestDescription = testDescription;
            this.ErrorMessage = string.Empty;
            this.IdentifedDataBehaviors = new List<EnumIdentifiedDataBehavior>();
            this.UnstructuredOldValues = new HashSet<StringDescriptor>();
            this.UnstructuredNewValues = new HashSet<StringDescriptor>();
            this.OldOrganizationValues = new HashSet<OrganizationTreeDescriptor>();
            this.NewOrganizationValues = new HashSet<OrganizationTreeDescriptor>();
            this.StructuredOldValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>();
            this.StructuredNewValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>();
            this.OldTreeRoot = null;
            this.NewTreeRoot = null;
            this.TreeComparisonIndexError = -1;
            this.Severity = EnumResultSeverityType.SUCCESS;
            this.OldId = OldId;
            this.UserId = userId;
        }

        /// <summary>
        /// Overloaded method that stores the input data for later display, according to the data types supplied.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        public void AddDetailedValues(HashSet<StringDescriptor> oldValues, HashSet<StringDescriptor> newValues)
        {
            if (oldValues != null)
            {
                this.UnstructuredOldValues = oldValues;
            }

            if (newValues != null)
            {
                this.UnstructuredNewValues = newValues;
            }

            this.DisplayFormat = EnumResultDisplayFormat.ListOfValues;
        }

        /// <summary>
        /// Overloaded method that stores the input data for later display, according to the data types supplied.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="oldTreeRoot"></param>
        /// <param name="newValues"></param>
        /// <param name="newTreeRoot"></param>
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

        /// <summary>
        /// Overloaded method that stores the input data for later display, according to the data types supplied.
        /// </summary>
        /// <param name="oldValues"></param>
        /// <param name="newValues"></param>
        public void AddDetailedValues(HashSet<Dictionary<EnumOldServiceFieldsAsKeys,StringDescriptor>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys,StringDescriptor>> newValues)
        {
            if (oldValues != null)
            {
                this.StructuredOldValues = oldValues;
            }

            if (newValues != null)
            {
                this.StructuredNewValues = newValues;
            }

            this.DisplayFormat = EnumResultDisplayFormat.StructureOfValues;
        }

        /// <summary>
        /// Keeps track of the overall severity of the test result and updates accordingly.
        /// Algorithm :
        /// Warnings are the only severity that is weaker than the rest and can happen afterwards.
        /// For all the others we assume chronological order as more specific scenarios are investigated.
        /// </summary>
        /// <param name="newSeverityStateReturned"></param>
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
                this.Severity = newSeverityStateReturned;
            }
        }
    }
}