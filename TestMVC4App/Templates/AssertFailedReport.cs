using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
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
        public SeverityState FailureType { get; set; }

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