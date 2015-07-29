namespace TestMVC4App.Models
{
    /// <summary>
    /// Defines the various Result States possible (SUCCESS, ERROR, WARNING and sub-categories).
    /// </summary>
    public enum EnumResultSeverityType
    {
        /// <summary>
        /// Mismatch identified.
        /// </summary>
        [System.ComponentModel.Description("The data consistency between the services is not maintained.")]
        ERROR,
        /// <summary>
        /// Mismatch identified.
        /// </summary>
        [System.ComponentModel.Description("No data returned by the new service.")]
        ERROR_ONLY_OLD,
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
        /// No data.
        /// </summary>
        /// <remarks>A warning means that NO data was returned by either service.</remarks>
        [System.ComponentModel.Description("None of the service has returned any data.")]
        WARNING_NO_DATA,
        /// <summary>
        /// No data.
        /// </summary>
        /// <remarks>A warning means that NO data was returned by either service.</remarks>
        [System.ComponentModel.Description("Only the new service has returned any data.")]
        WARNING_ONLY_NEW,
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
