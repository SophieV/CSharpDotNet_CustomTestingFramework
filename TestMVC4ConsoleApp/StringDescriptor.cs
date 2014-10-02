using System;
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class StringDescriptor
    {
        /// <summary>
        /// Used to make sure comparison of values is not done on the "same side".
        /// </summary>
        public bool IsOld { get; private set; }
        public string Value { get; set; }

        public bool SingleValueHasBeenMatched { get; set; }

        public bool Duplicate { get; set; }

        public bool MismatchDueToTrailingSpaces { get; set; }

        public bool MismatchDueToCase { get; set; }

        public bool MismatchDueToPartialName { get; set; }

        public bool MismatchDueToShiftedName { get; set; }

        public StringDescriptor(bool isOld, string value)
        {
            this.IsOld = isOld;
            this.Value = value;
            this.SingleValueHasBeenMatched = false;
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
        public static HashSet<StringDescriptor> EmbedInDescriptors(bool isOld, HashSet<string> values)
        {
            var embeddedValues = new HashSet<StringDescriptor>();
            foreach(string value in values)
            {
                embeddedValues.Add(new StringDescriptor(isOld, value));
            }
            return embeddedValues;
        }

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

        public static HashSet<StringDescriptor> EmbedInDescriptors(bool isOld, string value)
        {
            var embeddedValues = new HashSet<StringDescriptor>();
            embeddedValues.Add(new StringDescriptor(isOld, value));
            return embeddedValues;
        }

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
