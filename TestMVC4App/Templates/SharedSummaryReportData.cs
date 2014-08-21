using System;
using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    public class SharedSummaryReportData
    {
        public int CountProfilesTested { get; set; }

        public int CountTestsRun { get; set; }

        public int CountTestsPerUser { get; set; }

        public int CountProfilesWithoutWarnings { get; set; }

        public Dictionary<ResultSeverityType, int> CountBySeverity { get; set; }

        public Dictionary<IdentifiedDataBehavior, int> CountByIdentifiedDataBehavior { get; set; }

        public Dictionary<string, Dictionary<ResultSeverityType, int>> CountBySeverity_ByTestName { get; set; }

        public Dictionary<string, Dictionary<IdentifiedDataBehavior, int>> CountByIdentifiedDataBehavior_ByTestName { get; set; }

        public List<string> TestNames { get; set; }

        public Dictionary<string, string> SampleData_ByTestName { get; set; }

        public TimeSpan Duration { get; set; }
    }
}