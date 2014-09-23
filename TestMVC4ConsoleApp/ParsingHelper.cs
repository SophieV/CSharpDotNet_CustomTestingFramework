﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TestMVC4App.Models
{
    public class ParsingHelper
    {
        /// <summary>
        /// Allows to display the string text associated with an enum entry.
        /// </summary>
        /// <param name="value">Enum type from which we want the description.</param>
        /// <returns>Description text.</returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static string ParseSingleValue(IEnumerable<XElement> elements, string nodeName)
        {
            string value = string.Empty;

            try
            {
                value = elements.Where(x => x.Name == nodeName).Select(x => x.Value).First();
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            return value;
        }

        public static HashSet<Dictionary<EnumOldServiceFieldsAsKeys,string>> ParseListSimpleValuesStructure(IEnumerable<XElement> elements, string nodeName, EnumOldServiceFieldsAsKeys[] childNodeNames)
        {
            var values = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();
            Dictionary<EnumOldServiceFieldsAsKeys,string> properties;

            var parentItems = elements.Where(x => x.Name == nodeName);

            if (parentItems.Count() > 0)
            {
                foreach (var parentItem in parentItems)
                {
                    properties = new Dictionary<EnumOldServiceFieldsAsKeys, string>();
                    foreach (var childNodeName in childNodeNames)
                    {
                        try
                        {
                            properties.Add(childNodeName,parentItem.Descendants(childNodeName.ToString()).Select(x => x.Value).First());
                        }
                        catch (Exception)
                        {
                            // there is no existing attribute to parse
                            // we need to make sure that there are just as many values as expected
                            properties.Add(childNodeName,string.Empty);
                        }
                    }

                    values.Add(properties);
                }
            }

            return values;
        }

        public static HashSet<string> ParseListSimpleValues(IEnumerable<XElement> elements, string nodeName, string childNodeName)
        {
            HashSet<string> values;

            try
            {
                values = new HashSet<string>(elements.Where(x => x.Name == nodeName).Descendants(childNodeName).Select(x => x.Value));

                if (values.Count() == 0)
                {
                    values = new HashSet<string>(elements.Descendants().Where(x => x.Name == nodeName).Descendants(childNodeName).Select(x => x.Value));
                }
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
                values = new HashSet<string>();
            }

            return values;
        }

        public static HashSet<string> ParseListSimpleValues(IEnumerable<XElement> elements, string nodeName)
        {
            var values = new HashSet<string>();

            try
            {
                values = new HashSet<string>(elements.Where(x => x.Name == nodeName).Select(x=>x.Value));

                if (values.Count() == 0)
                {
                    values = new HashSet<string>(elements.Descendants().ToList().Where(x => x.Name == nodeName).Select(x => x.Value));
                }
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            return values;
        }

        public static IEnumerable<XElement> ParseListNodes(IEnumerable<XElement> elements ,string nodeName, bool isBeginningPattern = false)
        {
            return ParseListNodes(elements, nodeName, new List<XElement>(), isBeginningPattern);
        }

        public static IEnumerable<XElement> ParseListNodes(IEnumerable<XElement> elements, string nodeName, List<XElement> appendTo, bool isBeginningPattern = false)
        {
            if (appendTo == null)
            {
                appendTo = new List<XElement>();
            }

            try
            {
                if (isBeginningPattern)
                {
                    appendTo.AddRange(elements.Where(x => x.Name.ToString().StartsWith(nodeName)));
                }
                else
                {
                    appendTo.AddRange(elements.Where(x => x.Name == nodeName));
                }
            }
            catch (Exception)
            {
            }

            return appendTo;
        }

        public static IEnumerable<XElement> ParseListNodesOnlySameDepth(IEnumerable<XElement> elements, List<XElement> appendTo, bool isBeginningPattern = false)
        {
            if (appendTo == null)
            {
                appendTo = new List<XElement>();
            }

            try
            {
                appendTo.AddRange(elements.Where(x => x.Descendants().Count() == 0));

            }
            catch (Exception)
            {
            }

            return appendTo;
        }

        public static HashSet<string> StringToList(string valueToSplit, char separator, HashSet<string> appendTo = null)
        {
            HashSet<string> values;
            string entry;

            if (appendTo != null)
            {
                values = appendTo;
            } else {
                values = new HashSet<string>();
            }
            
            var valuesPart = valueToSplit.Split(separator);
            foreach (string value in valuesPart)
            {
                entry = value.Trim();
                if (!string.IsNullOrEmpty(entry))
                {
                    values.Add(entry);
                };
            }

            return values;
        }

        /// <summary>
        /// Replaces the default characters used for describing the mismatched values of the Assert so that their content
        /// is not (mis)interpreted as HTML content.
        /// </summary>
        /// <param name="message">The string to clean.</param>
        /// <returns>The cleansed string.</returns>
        /// <remarks>This process has to take place before HTML content is generated for visualization on the report/includes exceptions to be rendered as HTML.</remarks>
        public static String ReplaceProblematicTagsForHtml(string message)
        {
            message = message.Replace("<", "<span style='color:red;'>[");
            message = message.Replace(">", "]</span>");
            return message;
        }

        /// <summary>
        /// Transforms an unformatted Phone Number from the new Service into the format returned by the Old Service.
        /// </summary>
        /// <param name="unformattedPhoneNumber"></param>
        /// <param name="optionalPhoneExtension"></param>
        /// <returns></returns>
        public static string FormatPhoneNumber(string unformattedPhoneNumber, string optionalPhoneExtension = "")
        {
            StringBuilder value = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(unformattedPhoneNumber))
            {
                unformattedPhoneNumber = new System.Text.RegularExpressions.Regex(@"\D").Replace(unformattedPhoneNumber, string.Empty);
                unformattedPhoneNumber = unformattedPhoneNumber.TrimStart('1');
                
                if (unformattedPhoneNumber.Length == 7)
                {
                    value.Append(Convert.ToInt64(unformattedPhoneNumber).ToString("###-####"));
                }
                else if (unformattedPhoneNumber.Length == 10)
                {
                    value.Append(Convert.ToInt64(unformattedPhoneNumber).ToString("(###) ###-####"));
                }
                else if (unformattedPhoneNumber.Length > 10)
                {
                    value.Append(Convert.ToInt64(unformattedPhoneNumber).ToString("###-###-#### " + new String('#', (unformattedPhoneNumber.Length - 10))));
                }
            }

            if (!string.IsNullOrEmpty(optionalPhoneExtension))
            {
                value.Append(" x" + optionalPhoneExtension);
            }
            return value.ToString();
        }
    }
}