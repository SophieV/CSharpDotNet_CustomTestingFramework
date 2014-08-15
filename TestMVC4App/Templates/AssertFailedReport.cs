using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    public enum SeverityLevel
    {
        /// <summary>
        /// Mismatch identified.
        /// </summary>
        [System.ComponentModel.Description("The consistency of the data between the services is not maintained.")]
        ERROR,
        /// <summary>
        /// Mismatch identified and reason explained.
        /// </summary>
        [System.ComponentModel.Description("The consistency of the data between the services is not maintained.<br/>An explanation of the scenario is provided.")]
        ERROR_WITH_EXPLANATION,
        /// <summary>
        /// Inconsistency identified, such as no value from both services or unexpected data pattern on the new side.
        /// </summary>
        /// <remarks>A warning means that ALL the data of the old service was found in the new service.</remarks>
        [System.ComponentModel.Description("The consistency of the data is maintained.<br/>Possible problems were detected.")]
        WARNING,
        /// <summary>
        /// Mismatch identified and pattern analysis revealed that there is no data inconsistency (e.g. doublons in the old service).
        /// </summary>
        /// <remarks>A false positive means that ALL the data of the old service was found in the new service.</remarks>
        [System.ComponentModel.Description("Even though the tests failed, the data is consistent.<br/>An explanation of the scenario is provided.")]
        FALSE_POSITIVE,
        /// <summary>
        /// No error. Test completes with success.
        /// </summary>
        [System.ComponentModel.Description("No error.")]
        SUCCESS
    }

    public class AssertFailedReport
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
        public SeverityLevel FailureType { get; set; }

        /// <summary>
        /// Hints provided by additional analysis.
        /// </summary>
        public List<ObservationLabel> Observations { get; set; }

        /// <summary>
        /// Allows to display the string text associated with an enum entry.
        /// </summary>
        /// <param name="value">Enum type from which we want the description.</param>
        /// <returns>Description text.</returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

    }
}