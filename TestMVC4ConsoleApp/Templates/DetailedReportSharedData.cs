using System;
using System.Collections.Generic;
using TestMVC4App.Models;
using System.Linq;

namespace TestMVC4App.Templates
{
    public class DetailedReportSharedData
    {
        /// <summary>
        /// Error message describing the failure of the test.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Human-readable version of the test task description.
        /// </summary>
        public string TestDescription { get; private set; }

        /// <summary>
        /// Identifier of the User in the new system.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Identifier of the User in the old system.
        /// </summary>
        public int UPI { get; set; }

        /// <summary>
        /// Url to the XML Profile Data using the old web service.
        /// </summary>
        /// <remarks>Is convenient for QA Review purposes.</remarks>
        public String OldUrl { get; set; }

        /// <summary>
        /// Url to the XML User Data using the new web service.
        /// </summary>
        /// <remarks>Is convenient for QA Review purposes.</remarks>
        public String NewUrl { get; set; }

        /// <summary>
        /// Caller method name indicating where the FAIL happened.
        /// </summary>
        public EnumTestUnitNames TestName { get; private set; }

        /// <summary>
        /// Severity of the test result.
        /// </summary>
        public EnumResultSeverityType Result { get; private set; }

        /// <summary>
        /// Hints provided by additional analysis.
        /// </summary>
        public HashSet<string> IdentifiedDataBehaviors { get; private set; }

        public HashSet<StringDescriptor> OldValues { get; private set; }

        public HashSet<OrganizationTreeDescriptor> OldOrganizationValues { private set; get; }

        public HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> OldStructureValues { get; private set; }

        public OrganizationTreeDescriptor OldTreeRoot { private set; get; }

        public HashSet<StringDescriptor> NewValues { get; private set; }

        public HashSet<OrganizationTreeDescriptor> NewOrganizationValues { private set; get; }

        public HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> NewStructureValues { get; private set; }

        public OrganizationTreeDescriptor NewTreeRoot { private set; get; }

        public int TreeComparisonIndexError { get; private set; }

        public TimeSpan Duration { get; private set; }

        public DetailedReportSharedData(ResultReport resultReport)
        {
            this.ErrorMessage = resultReport.ErrorMessage;
            this.TestName = resultReport.TestName;
            this.Result = resultReport.Severity;
            this.TestDescription = resultReport.TestDescription;
            this.IdentifiedDataBehaviors = new HashSet<string>(LogManager.IdentifiedBehaviorsDescriptions.Where(x=>resultReport.IdentifedDataBehaviors.Contains(x.Key)).Select(x=>x.Value));
            this.OldValues = resultReport.OldValues;
            this.NewValues = resultReport.NewValues;
            this.OldOrganizationValues = resultReport.OldOrganizationValues;
            this.NewOrganizationValues = resultReport.NewOrganizationValues;
            this.OldStructureValues = resultReport.OldStructureValues;
            this.NewStructureValues = resultReport.NewStructureValues;
            this.OldTreeRoot = resultReport.OldTreeRoot;
            this.NewTreeRoot = resultReport.NewTreeRoot;
            this.Duration = resultReport.Duration;
            this.TreeComparisonIndexError = resultReport.TreeComparisonIndexError;
            this.UPI = resultReport.Upi;
            this.UserId = resultReport.UserId;
        }
    }
}