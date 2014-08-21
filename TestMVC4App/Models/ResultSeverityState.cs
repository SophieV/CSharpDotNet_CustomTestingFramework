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
    public enum ResultSeverityState
    {
        /// <summary>
        /// Mismatch identified.
        /// </summary>
        [System.ComponentModel.Description("The data consistency between the services is not maintained.")]
        ERROR,
        /// <summary>
        /// Mismatch identified and reason explained.
        /// </summary>
        [System.ComponentModel.Description("The data consistency between the services is not maintained AND some data behaviors were identified.")]
        ERROR_WITH_EXPLANATION,
        /// <summary>
        /// Inconsistency identified, such as no value from both services or unexpected data pattern on the new side.
        /// </summary>
        /// <remarks>A warning means that ALL the data of the old service was found in the new service.</remarks>
        [System.ComponentModel.Description("The data consistency between the services is maintained AND some data behaviors were identified.")]
        WARNING,
        /// <summary>
        /// Mismatch identified and pattern analysis revealed that there is no data inconsistency (e.g. doublons in the old service).
        /// </summary>
        /// <remarks>A false positive means that ALL the data of the old service was found in the new service.</remarks>
        [System.ComponentModel.Description("The data consistency between the services is maintained EVEN THOUGH the test returned an error.")]
        FALSE_POSITIVE,
        /// <summary>
        /// No error. Test completes with success.
        /// </summary>
        [System.ComponentModel.Description("The data consistency between the services is maintained.")]
        SUCCESS
    }
}
