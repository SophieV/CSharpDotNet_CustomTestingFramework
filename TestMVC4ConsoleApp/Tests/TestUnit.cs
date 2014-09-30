using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace TestMVC4App.Models
{
    public abstract class TestUnit
    {
        # region Properties

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

        public Dictionary<EnumTestUnitNames,ResultReport> DetailedResults { get; set; }

        public EnumResultSeverityType OverallSeverity { get; private set; }

        protected int Upi { get; private set; }

        public int UserId { get; protected set; }

        protected string PageName { get; private set; }

        protected IEnumerable<XElement> OldDataNodes { get; private set; }

        #endregion Properties

        /// <summary>
        /// Defines the overall Result Severity by counting the occurences of Severity Types in the detailed Results.
        /// </summary>
        public void ComputeOverallSeverity()
        {
            bool keepGoing = true;

            var errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.ERROR).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if(keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.ERROR;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.ERROR_WITH_EXPLANATION).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.ERROR_WITH_EXPLANATION;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.FALSE_POSITIVE).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.FALSE_POSITIVE;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.WARNING).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.WARNING;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Value.Severity == EnumResultSeverityType.SUCCESS).GroupBy(results => results.Value.Severity).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = EnumResultSeverityType.SUCCESS;
                keepGoing = false;
            }
        }

        /// <summary>
        /// Adds the children of children to this level so that results can be computed together.
        /// </summary>
        public void ComputerOverallResults()
        {
            foreach (var child in Children)
            {
                Array.ForEach(child.DetailedResults.ToArray(), x => this.DetailedResults.Add(x.Key,x.Value));
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
            this.DetailedResults = new Dictionary<EnumTestUnitNames,ResultReport>();
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
            var resultReport = new ResultReport(this.UserId,this.Upi, testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> oldValues, HashSet<Dictionary<EnumOldServiceFieldsAsKeys, string>> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, Dictionary<Dictionary<EnumOldServiceFieldsAsKeys, string>,Dictionary<EnumOldServiceFieldsAsKeys, string>> oldAndNewValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(oldAndNewValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        public void CompareAndLog_Test(EnumTestUnitNames testFullName, string testDescription, Dictionary<HashSet<string>,HashSet<string>> newAndOldValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(newAndOldValues,resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
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
            var resultReport = new ResultReport(this.UserId, this.Upi, testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newValue, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport.TestName, resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }
    }
}