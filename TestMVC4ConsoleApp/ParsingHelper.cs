using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace TestMVC4App.Models
{
    /// <summary>
    /// This helper groups functions used to extract data from the services, as well as tools to adapt the format(s) to a common ground for testing.
    /// </summary>
    public class ParsingHelper
    {
        /// <summary>
        /// Allows to display the string text associated with an Enum entry.
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

        /// <summary>
        /// Extracts a single value from the XML input data.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Extracts a list of values from the XML input data and stores them with the matching key.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="nodeName"></param>
        /// <param name="childNodeNames"></param>
        /// <returns></returns>
        public static HashSet<Dictionary<EnumOldServiceFieldsAsKeys,string>> ParseStructuredListOfValues(IEnumerable<XElement> elements, string nodeName, EnumOldServiceFieldsAsKeys[] childNodeNames)
        {
            var values = new HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>>();
            Dictionary<EnumOldServiceFieldsAsKeys,string> properties;

            try
            {
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
                                properties.Add(childNodeName, parentItem.Descendants(childNodeName.ToString()).Select(x => x.Value).First());
                            }
                            catch (Exception)
                            {
                                // there is no existing attribute to parse
                                // we need to make sure that there are just as many values as expected
                                properties.Add(childNodeName, string.Empty);
                            }
                        }

                        values.Add(properties);
                    }
                }
            }
            catch (Exception) { }

            return values;
        }

        /// <summary>
        /// Extracts a list of values from the XML input data.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="nodeName"></param>
        /// <param name="childNodeName"></param>
        /// <returns></returns>
        public static HashSet<string> ParseUnstructuredListOfValues(IEnumerable<XElement> elements, string nodeName, string childNodeName)
        {
            var values = new HashSet<string>();

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
            }

            return values;
        }

        /// <summary>
        /// Extracts a list of values from the XML input data.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static HashSet<string> ParseUnstructuredListOfValues(IEnumerable<XElement> elements, string nodeName)
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

        /// <summary>
        /// Extracts a list of children nodes from the XML input data.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="nodeName"></param>
        /// <param name="isBeginningPattern"></param>
        /// <returns></returns>
        /// <remarks>Is used to dispatch more efficiently the old service data to the various tests.</remarks>
        public static IEnumerable<XElement> ParseListNodes(IEnumerable<XElement> elements ,string nodeName, bool isBeginningPattern = false)
        {
            return ParseListNodes(elements, nodeName, new List<XElement>(), isBeginningPattern);
        }

        /// <summary>
        /// Extracts a list of children nodes from the XML input data.
        /// Apprends to an existing list.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="nodeName"></param>
        /// <param name="appendTo"></param>
        /// <param name="isBeginningPattern"></param>
        /// <returns></returns>
        /// <remarks>Is used to dispatch more efficiently the old service data to the various tests.</remarks>
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

        /// <summary>
        /// Extracts a list of children nodes from the XML input data.
        /// Apprends to an existing list.
        /// Excludes nodes with descendants.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="appendTo"></param>
        /// <param name="isBeginningPattern"></param>
        /// <returns></returns>
        /// <remarks>Is used to dispatch more efficiently the old service data to the various tests.</remarks>
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
            catch (Exception) { }

            return appendTo;
        }

        /// <summary>
        /// Extracts several values contained in a string to a list of values.
        /// </summary>
        /// <param name="valueToSplit"></param>
        /// <param name="separator"></param>
        /// <param name="appendTo"></param>
        /// <returns></returns>
        /// <remarks>Is used to convert data from the old service into a matching format to the new service.</remarks>
        public static HashSet<string> StringToList(string valueToSplit, char separator, HashSet<string> appendTo = null)
        {
            HashSet<string> values;
            string entry;

            if (appendTo != null)
            {
                values = appendTo;
            } 
            else 
            {
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
        /// Formats a raw Phone Number.
        /// </summary>
        /// <param name="unformattedPhoneNumber"></param>
        /// <param name="optionalPhoneExtension"></param>
        /// <returns></returns>
        /// <remarks>Is used to convert data from the new service to match the format returned by the old service.</remarks>
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
            return value.ToString().Trim();
        }
    }
}