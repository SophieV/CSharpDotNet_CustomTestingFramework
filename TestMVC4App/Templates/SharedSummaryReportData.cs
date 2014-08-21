using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    public class SharedSummaryReportData
    {
        public int CountProfilesTested { get; set; }

        public int CountTestsRun { get; set; }

        public int CountTestsPerUser { get; set; }

        public int CountProfilesWithoutWarnings { get; set; }

        public Dictionary<SeverityState, int> OverviewCountBySeverityState { get; set; }

        public Dictionary<ObservationLabel, int> OverviewCountByObservationType { get; set; }

        public Dictionary<string, Dictionary<SeverityState, int>> ByTestNameCountBySeverityState { get; set; }

        public Dictionary<string, Dictionary<ObservationLabel, int>> ByTestNameCountByObservationType { get; set; }

        public List<string> TestNames { get; set; }

        public Dictionary<string, string> SampleDataByTestName { get; set; }
    }
}