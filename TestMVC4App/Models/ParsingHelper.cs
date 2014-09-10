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

        public static HashSet<string> ParseListSimpleValues(IEnumerable<XElement> elements, string nodeName, string nodeAttributeName)
        {
            var values = new HashSet<string>();

            try
            {
                var tempElements = elements.Where(x=>x.Name == nodeName).Select(x=>x.Attribute(nodeAttributeName).Value);
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
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

        public static IEnumerable<XElement> ParseListNode(IEnumerable<XElement> elements ,string nodeName, bool isBeginningPattern = false)
        {
            var values = new List<XElement>();

            try
            {
                if (isBeginningPattern)
                {
                    values = elements.Where(x => x.Name.ToString().StartsWith(nodeName)).ToList();
                }
                else
                {
                    values = elements.Where(x => x.Name == nodeName).ToList();
                }
            }
            catch (Exception)
            {
                values = new List<XElement>();
            }

            return values;
        }
    }
}