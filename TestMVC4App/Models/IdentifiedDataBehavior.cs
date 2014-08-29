
namespace TestMVC4App.Models
{
    /// <summary>
    /// Descriptions of data patterns observed.
    /// </summary>
    public enum IdentifiedDataBehavior
    {
        [System.ComponentModel.Description("No entry returned by the new nor the old service.")]
        VALUES_NOT_POPULATED,
        [System.ComponentModel.Description("All the entries of the old service are found in the new service.")]
        ALL_VALUES_OF_OLD_SUBSET_FOUND,
        [System.ComponentModel.Description("There are more entries in the new service.")]
        MORE_VALUES_ON_NEW_SERVICE,
        [System.ComponentModel.Description("There are more entries in the old service BUT they are all duplicates.")]
        MORE_VALUES_ON_OLD_SERVICE_ALL_DUPLICATES,
        [System.ComponentModel.Description("The new service contains duplicates.")]
        DUPLICATED_VALUES_ON_NEW_SERVICE,
        [System.ComponentModel.Description("The new service returns one or more entries that contain only a white space.")]
        VALUE_POPULATED_WITH_WHITE_SPACE_ON_NEW_SERVICE,
        [System.ComponentModel.Description("The mismatch between some entries is due to trailing white spaces.")]
        MISMATCH_DUE_TO_TRAILING_WHITE_SPACES,
        [System.ComponentModel.Description("Some entries of the old service are not found in the new service.")]
        MISSING_VALUES_ON_NEW_SERVICE,
        [System.ComponentModel.Description("The content of the entries does not match.")]
        WRONG_VALUE,
        [System.ComponentModel.Description("The old tree has more elements than the new one.")]
        OLD_TREE_HAS_MORE_CHILDREN,
        [System.ComponentModel.Description("The old tree does not include all the organizations returned by the old service.")]
        OLD_TREE_NON_EXHAUSTIVE,
        [System.ComponentModel.Description("The new tree contains at least as many elements as the old one.")]
        NEW_TREE_COUNT_CONSISTENT,
        [System.ComponentModel.Description("The mismatch between some entries is due to missing Id values.")]
        MISMATCH_DUE_TO_MISSING_IDS
    }
}