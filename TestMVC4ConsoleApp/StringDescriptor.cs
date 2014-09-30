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
        public static HashSet<StringDescriptor> EmbedInDescriptors(HashSet<string> values)
        {
            HashSet<StringDescriptor> embeddedValues = new HashSet<StringDescriptor>();
            foreach(string value in values)
            {
                embeddedValues.Add(new StringDescriptor(value));
            }
            return embeddedValues;
        }
    }
}
