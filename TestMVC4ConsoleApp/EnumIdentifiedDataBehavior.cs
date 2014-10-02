
namespace TestMVC4App.Models
{
    /// <summary>
    /// Descriptions of data patterns observed.
    /// </summary>
    public enum EnumIdentifiedDataBehavior
    {
        [System.ComponentModel.Description("More data from the new service.")]
        MORE_VALUES_ON_NEW_SERVICE,
        [System.ComponentModel.Description("More data from the old service, ALL DUPLICATES.")]
        MORE_VALUES_ON_OLD_SERVICE_ALL_DUPLICATES,
        [System.ComponentModel.Description("DUPLICATES returned by the new service.")]
        DUPLICATED_VALUES_ON_NEW_SERVICE,
        [System.ComponentModel.Description("WHITE SPACES returned by the new service.")]
        VALUE_POPULATED_WITH_WHITE_SPACE_ON_NEW_SERVICE,
        [System.ComponentModel.Description("Some mismatches are caused by trailing WHITE SPACES.")]
        MISMATCH_DUE_TO_TRAILING_WHITE_SPACES,
        [System.ComponentModel.Description("Some mismatches are caused by differences in the CASE.")]
        MISMATCH_DUE_TO_CASE_DIFFERENCES,
        [System.ComponentModel.Description("Tree contains LESS elements at a given depth.")]
        OLD_TREE_HAS_MORE_CHILDREN_GIVEN_DEPTH,
        [System.ComponentModel.Description("Tree contains AT LEAST AS MANY ELEMENTS at a given depth.")]
        NEW_TREE_COUNT_CONSISTENT,
        [System.ComponentModel.Description("Some mismatches are due to MISSING IDs.")]
        MISMATCH_DUE_TO_MISSING_IDS,
        [System.ComponentModel.Description("Merging the trees required using an element at a given depth that MAY have failed the conditions.")]
        MATCHING_SINGLE_ELEMENT_GIVEN_DEPTH_MISMATCH,
        [System.ComponentModel.Description("Merging the trees required using an element SEVERAL TIMES.")]
        REUSED_ELEMENT_TO_FILL_GAP,
        [System.ComponentModel.Description("The old value is a PARTIAL MATCH and/or contained by the new value.")]
        PARTIAL_MATCH
    }
}