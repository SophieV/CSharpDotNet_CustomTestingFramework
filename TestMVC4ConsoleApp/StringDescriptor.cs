using System;
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class StringDescriptor
    {
        public String Value { get; set; }

        public bool HasBeenMatched { get; set; }

        public bool Duplicate { get; set; }

        public bool MismatchDueToTrailingSpaces { get; set; }

        public bool MismatchDueToCase { get; set; }

        public bool MismatchDueToPartialName { get; set; }

        public bool MismatchDueToShiftedName { get; set; }

        public StringDescriptor(string value)
        {
            this.Value = value;
            this.HasBeenMatched = false;
            this.Duplicate = false;
            this.MismatchDueToCase = false;
            this.MismatchDueToTrailingSpaces = false;
            this.MismatchDueToPartialName = false;
            this.MismatchDueToShiftedName = false;
        }

        /// <summary>
        /// Helper method that converts a list of string values to a list of custom StringDecorators.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> EmbedInDescriptors(HashSet<string> values)
        {
            var embeddedValues = new Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>();
            foreach(string value in values)
            {
                embeddedValues.Add(EnumOldServiceFieldsAsKeys.GENERIC_KEY, new StringDescriptor(value));
            }
            return embeddedValues;
        }

        public static Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> EmbedInDescriptors(HashSet<Dictionary<EnumOldServiceFieldsAsKeys,string>> values)
        {
            var embeddedValues = new Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>();
            foreach (var element in values)
            {
                foreach (var value in element)
                {
                    embeddedValues.Add(value.Key, new StringDescriptor(value.Value));
                }
            }
            return embeddedValues;
        }

        public static Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> EmbedInDescriptors(string value)
        {
            var embeddedValues = new Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>();
            embeddedValues.Add(EnumOldServiceFieldsAsKeys.GENERIC_KEY, new StringDescriptor(value));
            return embeddedValues;
        }

        public static Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> EmbedInDescriptors(Dictionary<EnumOldServiceFieldsAsKeys, string> values)
        {
            var embeddedValues = new Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>();
            foreach (var value in values)
            {
                embeddedValues.Add(value.Key, new StringDescriptor(value.Value));
            }
            return embeddedValues;
        }
    }
}
