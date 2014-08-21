using System;
using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    public class SharedDetailedReportData
    {
        /// <summary>
        /// Error message describing the failure of the test.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Human-readable version of the test task description.
        /// </summary>
        public string TaskDescription { get; set; }

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
        public string TestName { get; set; }

        /// <summary>
        /// Severity of the test result.
        /// </summary>
        public ResultSeverityState FailureType { get; set; }

        /// <summary>
        /// Hints provided by additional analysis.
        /// </summary>
        public List<IdentifiedDataBehavior> Observations { get; set; }

        public List<string> OldValues { get; set; }

        public List<string> NewValues { get; set; }

        public TimeSpan Duration { get; set; }
    }
}