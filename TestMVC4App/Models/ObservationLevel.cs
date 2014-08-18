using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Descriptions of data patterns observed.
    /// </summary>
    public enum ObservationLabel
    {
        [System.ComponentModel.Description("[WARNING ONLY] Values are not populated - on neither side.")]
        VALUES_NOT_POPULATED,
        [System.ComponentModel.Description("The values provided by the old service were all found in the new service.")]
        ALL_VALUES_OF_OLD_SUBSET_FOUND,
        [System.ComponentModel.Description("There are more values on the side of the new service.")]
        MORE_VALUES_ON_NEW_SERVICE,
        [System.ComponentModel.Description("[FALSE POSITIVE]There are more values on the side of the old service. They are all duplicates.")]
        MORE_DUPLICATED_VALUES_ON_OLD_SERVICE,
        [System.ComponentModel.Description("There are duplicates in the new service.")]
        DUPLICATED_VALUES_ON_NEW_SERVICE,
        [System.ComponentModel.Description("The empty value provided by the new service contains a white space.")]
        VALUE_POPULATED_WITH_EMPTY_ON_NEW_SERVICE,
        [System.ComponentModel.Description("Mismatch between values due to trailing white spaces detected.")]
        VALUE_CONTAINS_TRAILING_WHITE_SPACES,
        [System.ComponentModel.Description("Some value(s) are missing on the side of the new service.")]
        MISSING_VALUES_ON_NEW_SERVICE,
        [System.ComponentModel.Description("[ERROR ONLY] Data populated with unexpected value(s).")]
        WRONG_VALUE
    }
}