using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TestMVC4App.Models
{
    public abstract class TestUnit
    {
        public abstract string newServiceURLExtensionBeginning { get; }

        public abstract string newServiceURLExtensionEnding { get; }

        public TestSuite Master { get; set; }

        public TestUnit Parent { get; set; }

        public List<TestUnit> Children { get; set; }

        public List<ResultReport> DetailedResults { get; set; }

        public ResultSeverityType OverallSeverity { get { return overallSeverity; } }
        private ResultSeverityType overallSeverity;

        public void ComputeOverallSeverity()
        {
            bool keepGoing = true;

            var errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.ERROR).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if(keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                overallSeverity = ResultSeverityType.ERROR;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.ERROR_WITH_EXPLANATION).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                overallSeverity = ResultSeverityType.ERROR_WITH_EXPLANATION;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.FALSE_POSITIVE).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                overallSeverity = ResultSeverityType.FALSE_POSITIVE;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.WARNING).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                overallSeverity = ResultSeverityType.WARNING;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.SUCCESS).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                overallSeverity = ResultSeverityType.SUCCESS;
                keepGoing = false;
            }
        }

        public void ComputerOverallResults()
        {
            foreach (var child in Children)
            {
                this.DetailedResults.AddRange(child.DetailedResults);
            }
        }

        /// <summary>
        /// Creates the full path to the new web service data for this specific test.
        /// </summary>
        /// <param name="newUserId"></param>
        /// <returns></returns>
        protected string BuildNewServiceFullURL(int newUserId)
        {
            if (Master == null)
            {
                throw new NotImplementedException();
            }

            return Master.newServiceURLBase + newServiceURLExtensionBeginning + newUserId + newServiceURLExtensionEnding;
        }

        protected TestUnit(TestSuite master, TestUnit parent = null)
        {
            this.Master = master;
            this.Parent = parent;
            this.Children = new List<TestUnit>();
            this.DetailedResults = new List<ResultReport>();
        }

        public abstract void RunAllTests();

        protected static string ParseSingleOldValue(XDocument oldServiceData,string oldValueXMLPath)
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

        protected static List<string> ParseListSimpleOldValues(XDocument oldServiceData, string oldSimpleListValuesXMLPath)
        {
            var oldValues = new List<string>();

            try
            {
                var titles = oldServiceData.XPathSelectElements(oldSimpleListValuesXMLPath);

                foreach (XElement el in titles)
                {
                    oldValues.Add(el.Element("titleName").Value);
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