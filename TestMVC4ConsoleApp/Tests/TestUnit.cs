using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public abstract class TestUnit
    {
        public bool HttpErrorHappened { get; set; }

        public bool UnknownErrorHappened { get; set; }

        public string ErrorMessage { get; set; }

        public virtual string NewServiceURLExtensionBeginning
        {
            get { return "Users/PageName/"; }
        }

        public virtual string NewServiceURLExtensionEnding
        {
            get { return "/Complete"; }
        }

        public TestSuite Container { get; set; }

        public TestUnit Parent { get; set; }

        public HashSet<TestUnit> Children { get; set; }

        public HashSet<ResultReport> DetailedResults { get; set; }

        public EnumResultSeverityType OverallSeverity { get; private set; }

        protected int Upi { get; private set; }

        public int UserId { get; protected set; }

        protected string PageName { get; private set; }

        protected IEnumerable<XElement> OldDataNodes { get; private set; }

        public void ComputeOverallSeverity()
        {
            bool keepGoing = true;

            var errors = this.DetailedResults.Where(r => r.Result == EnumResultSeverityType.ERROR).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if(keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.ERROR;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == EnumResultSeverityType.ERROR_WITH_EXPLANATION).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.ERROR_WITH_EXPLANATION;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == EnumResultSeverityType.FALSE_POSITIVE).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.FALSE_POSITIVE;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == EnumResultSeverityType.WARNING).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.WARNING;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == EnumResultSeverityType.SUCCESS).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.SUCCESS;
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
        /// <param name="userId"></param>
        /// <returns></returns>
        protected virtual string BuildNewServiceURL(string pageName)
        {
            if (this.Container == null)
            {
                throw new NotImplementedException();
            }

            return Container.newServiceURLBase + NewServiceURLExtensionBeginning + pageName + NewServiceURLExtensionEnding;
        }

        protected TestUnit(TestSuite container, TestUnit parent = null)
        {
            this.Container = container;
            this.Parent = parent;
            this.Children = new HashSet<TestUnit>();
            this.DetailedResults = new HashSet<ResultReport>();
        }

        protected abstract void RunAllSingleTests();

        public void ProvideData(int upi, IEnumerable<XElement> oldDataNodes, int userId, string pageName)
        {
            this.Upi = upi;
            this.OldDataNodes = oldDataNodes;
            this.UserId = userId;
            this.PageName = pageName;
        }

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
                    System.Console.Out.WriteLine(e.StackTrace);

                    HttpErrorHappened = false;
                    ErrorMessage = e.StackTrace;
                }
            }
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, HashSet<string> oldValues, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.UserId,
                                              this.Upi,
                                              this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.UserId,
                                              this.Upi,
                                              this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, Dictionary<HashSet<string>,HashSet<string>> newAndOldValues, bool stringPartialMatch = false)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(newAndOldValues,resultReport,stringPartialMatch);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.UserId,
                                              this.Upi,
                                              this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, IEnumerable<XElement> oldServiceData, string oldSingleStringPath, string newValue)
        {
            string oldValue = ParsingHelper.ParseSingleValue(oldServiceData, oldSingleStringPath);

            this.CompareAndLog_Test(testFullName, testDescription, oldValue, newValue);
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, string oldValue, string newValue)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newValue, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.UserId,
                                              this.Upi,
                                              this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }
    }
}