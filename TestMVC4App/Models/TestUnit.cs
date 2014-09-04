using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TestMVC4App.Models
{
    public abstract class TestUnit
    {
        public bool HttpErrorHappened { get; set; }

        public bool UnknownErrorHappened { get; set; }

        public string ErrorMessage { get; set; }

        public abstract string newServiceURLExtensionBeginning { get; }

        public abstract string newServiceURLExtensionEnding { get; }

        public TestSuite Master { get; set; }

        public TestUnit Parent { get; set; }

        public HashSet<TestUnit> Children { get; set; }

        public HashSet<ResultReport> DetailedResults { get; set; }

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
                Array.ForEach(child.DetailedResults.ToArray(), x => this.DetailedResults.Add(x));
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
            this.Children = new HashSet<TestUnit>();
            this.DetailedResults = new HashSet<ResultReport>();
        }

        protected abstract void RunAllSingleTests();

        public void RunAllTests()
        {
            try
            {
                RunAllSingleTests();
            } 
            catch (HttpRequestException httpe)
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    System.Diagnostics.Debug.WriteLine("There were problems accessing the services.");
                    System.Diagnostics.Debug.WriteLine(httpe.StackTrace);

                    HttpErrorHappened = true;
                    ErrorMessage = httpe.StackTrace;
                }
            }
            catch(Exception e)
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);

                    HttpErrorHappened = false;
                    ErrorMessage = e.StackTrace;
                }
            }
        }

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

        protected static string ParseSingleOldValue(IEnumerable<XElement> oldServiceData, string oldValueXMLPath)
        {
            string oldValue = string.Empty;

            try
            {
                oldValue = oldServiceData.Where(x=>x.Name == oldValueXMLPath).Select(x=>x.Value).First();
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            return oldValue;
        }

        protected static HashSet<string> ParseListSimpleOldValues(XDocument oldServiceData, string listNodePath, string listEntryNodeName)
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

        protected static HashSet<string> ParseListSimpleOldValues(IEnumerable<XElement> elements, string nodeName)
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

        protected static bool IsContentOfCollectionItemsSubsetOfOtherCollection(ICollection<string> collectionA, ICollection<string> collectionB)
        {
            bool wasFound = true;
            bool wasFoundItem;

            if (collectionA.Count() != collectionB.Count())
            {
                wasFound = false;
            }

            if (wasFound)
            {
                foreach (var a in collectionA)
                {
                    wasFoundItem = false;
                    foreach (var b in collectionB)
                    {
                        if (!wasFoundItem && (a.Trim().Contains(b.Trim()) || b.Trim().Contains(a.Trim())))
                        {
                            wasFoundItem = true;
                        }
                    }

                    wasFound &= wasFoundItem;
                }
            }

            return wasFound;
        }
    }
}