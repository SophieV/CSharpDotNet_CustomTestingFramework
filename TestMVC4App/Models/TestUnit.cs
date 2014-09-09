﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public TestSuite Container { get; set; }

        public TestUnit Parent { get; set; }

        public HashSet<TestUnit> Children { get; set; }

        public HashSet<ResultReport> DetailedResults { get; set; }

        public ResultSeverityType OverallSeverity { get; private set; }

        public void ComputeOverallSeverity()
        {
            bool keepGoing = true;

            var errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.ERROR).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if(keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = ResultSeverityType.ERROR;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.ERROR_WITH_EXPLANATION).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = ResultSeverityType.ERROR_WITH_EXPLANATION;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.FALSE_POSITIVE).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = ResultSeverityType.FALSE_POSITIVE;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.WARNING).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = ResultSeverityType.WARNING;
                keepGoing = false;
            }

            errors = this.DetailedResults.Where(r => r.Result == ResultSeverityType.SUCCESS).GroupBy(results => results.Result).Select(x => new { Severity = x.Key, Count = x.Count() });
            if (keepGoing && errors.Count() > 0 && errors.First().Count > 0)
            {
                OverallSeverity = ResultSeverityType.SUCCESS;
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
        protected string BuildNewServiceURL(int userId)
        {
            if (Container == null)
            {
                throw new NotImplementedException();
            }

            return Container.newServiceURLBase + newServiceURLExtensionBeginning + userId + newServiceURLExtensionEnding;
        }

        protected TestUnit(TestSuite container, TestUnit parent = null)
        {
            this.Container = container;
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

        public void CompareAndLog_Test(TestUnitNames testFullName, string testDescription, int userId, int upi, HashSet<string> oldValues, HashSet<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(userId),
                                              resultReport);
        }

        public void CompareAndLog_Test(TestUnitNames testFullName, string testDescription, int userId, int upi, Dictionary<HashSet<string>,HashSet<string>> newAndOldValues, bool stringPartialMatch = false)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(newAndOldValues,resultReport,stringPartialMatch);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(userId),
                                              resultReport);
        }

        public void CompareAndLog_Test(TestUnitNames testFullName, string testDescription, int userId, int upi, XDocument oldServiceData, string oldSingleStringPath, string newValue)
        {
            string oldValue = ParsingHelper.ParseSingleOldValue(oldServiceData, oldSingleStringPath);

            this.CompareAndLog_Test(testFullName, testDescription, userId, upi, oldValue, newValue);
        }

        public void CompareAndLog_Test(TestUnitNames testFullName, string testDescription, int userId, int upi, string oldValue, string newValue)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(testFullName, testDescription);
            var compareStrategy = new CompareStrategyContextSwitcher(oldValue, newValue, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Container.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceURL(userId),
                                              resultReport);
        }
    }
}