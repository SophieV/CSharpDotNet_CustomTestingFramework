using System;
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    /// <summary>
    /// The string value is embedded in a descriptive container that carries information about the tests result on a micro-level.
    /// </summary>
    public class StringDescriptor
    {
        /// <summary>
        /// Flag that indicates the side of origin.
        /// </summary>
        /// <remarks>Its purpose is to avoid pairing data that comes from the same side.</remarks>
        public bool IsOld { get; private set; }

        /// <summary>
        /// Value being compared.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Flag that indicated that the value was successfully paired on the other side.
        /// </summary>
        public bool HasBeenMatched { get; set; }

        /// <summary>
        /// Flag that indicates that this value appears at least another time in the list returned by a given service (old or new).
        /// </summary>
        public bool IsDuplicate { get; set; }

        /// <summary>
        /// Flag that indicates that the value has been matched with identified pattern.
        /// </summary>
        public bool MatchedOnceTrailingSpacesRemoved { get; set; }

        /// <summary>
        /// Flag that indicates that the value has been matched with identified pattern.
        /// </summary>
        public bool MatchedOnceCaseCorrected { get; set; }

        /// <summary>
        /// Flag that indicates that the value has been matched with identified pattern.
        /// </summary>
        public bool PartialMatchOnly { get; set; }

        /// <summary>
        /// Flag that indicates that the value has been matched with identified pattern.
        /// </summary>
        public bool MatchedOnceShifted { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isOld"></param>
        /// <param name="value"></param>
        public StringDescriptor(bool isOld, string value)
        {
            this.IsOld = isOld;
            this.Value = value;
            this.HasBeenMatched = false;
            this.IsDuplicate = false;
            this.MatchedOnceCaseCorrected = false;
            this.MatchedOnceTrailingSpacesRemoved = false;
            this.PartialMatchOnly = false;
            this.MatchedOnceShifted = false;
        }

        /// <summary>
        /// Helper method that embeds a list of string values to a list of custom decorators <see cref="StringDecorator"/>.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static HashSet<StringDescriptor> EmbedInDescriptors(bool isOld, HashSet<string> values)
        {
            var embeddedValues = new HashSet<StringDescriptor>();
            foreach(string value in values)
            {
                embeddedValues.Add(new StringDescriptor(isOld, value));
            }
            return embeddedValues;
        }

        /// <summary>
        /// Helper method that embeds a list of structures of string values to a list of structures of custom decorators <see cref="StringDecorator"/>.
        /// </summary>
        /// <param name="isOld"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> EmbedInDescriptors(bool isOld, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> values)
        {
            var embeddedValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>();
            Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> dic;

            foreach (var element in values)
            {
                dic = new Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>();
                foreach (var value in element)
                {
                    dic.Add(value.Key, new StringDescriptor(isOld, value.Value));
                }
                embeddedValues.Add(dic);
            }
            return embeddedValues;
        }

        /// <summary>
        /// Helper method that embeds a single value to a custom decorator <see cref="StringDecorator"/>.
        /// </summary>
        /// <param name="isOld"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HashSet<StringDescriptor> EmbedInDescriptors(bool isOld, string value)
        {
            var embeddedValues = new HashSet<StringDescriptor>();
            embeddedValues.Add(new StringDescriptor(isOld, value));
            return embeddedValues;
        }

        /// <summary>
        /// Helper method that embeds a structure of string values to a structure of custom decorators <see cref="StringDecorator"/>.
        /// </summary>
        /// <param name="isOld"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>> EmbedInDescriptors(bool isOld, Dictionary<EnumOldServiceFieldsAsKeys, string> values)
        {
            var embeddedValues = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>();
            var embeddedDic = new Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>();

            foreach (var value in values)
            {
                embeddedDic.Add(value.Key, new StringDescriptor(isOld, value.Value));
            }
            embeddedValues.Add(embeddedDic);
            return embeddedValues;
        }
    }
}
