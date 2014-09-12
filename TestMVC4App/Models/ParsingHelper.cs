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
                oldValue = HttpUtility.HtmlDecode(oldServiceData.XPathSelectElement(oldValueXMLPath).Value);
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
                value = HttpUtility.HtmlDecode(elements.Where(x => x.Name == nodeName).Select(x => x.Value).First());
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            return value;
        }

        public static HashSet<string> ParseListSimpleValues(IEnumerable<XElement> elements, string nodeName, string childNodeName)
        {
            HashSet<string> values;

            try
            {
                values = new HashSet<string>(elements.Where(x => x.Name == nodeName).Descendants(childNodeName).Select(x => x.Value));
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
    }
}