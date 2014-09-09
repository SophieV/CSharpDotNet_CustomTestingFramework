using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

        public static string ParseSingleOldValue(XDocument oldServiceData, string oldValueXMLPath)
        {
            string oldValue = string.Empty;

            try
            {
                oldValue = oldServiceData.XPathSelectElement(oldValueXMLPath).Value;
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            return oldValue;
        }

        public static string ParseSingleOldValue(IEnumerable<XElement> oldServiceData, string oldValueXMLPath)
        {
            string oldValue = string.Empty;

            try
            {
                oldValue = oldServiceData.Where(x => x.Name == oldValueXMLPath).Select(x => x.Value).First();
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            return oldValue;
        }

        public static HashSet<string> ParseListSimpleOldValues(XDocument oldServiceData, string listNodePath, string listEntryNodeName)
        {
            var oldValues = new HashSet<string>();

            try
            {
                var elements = oldServiceData.XPathSelectElements(listNodePath);

                foreach (XElement element in elements)
                {
                    oldValues.Add(element.Element(listEntryNodeName).Value);
                }
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            return oldValues;
        }

        public static HashSet<string> ParseListSimpleOldValues(IEnumerable<XElement> elements, string nodeName)
        {
            var oldValues = new HashSet<string>();

            try
            {
                foreach (XElement element in elements)
                {
                    oldValues.Add(element.Element(nodeName).Value);
                }
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            return oldValues;
        }
    }
}