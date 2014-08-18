using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Result state.
    /// </summary>
    public enum SeverityState
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
}
