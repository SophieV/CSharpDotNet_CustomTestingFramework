using System;
using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    public class SharedSummaryReportData
    {
        public double CountProfilesTested { get; set; }

        public double CountTestsRun { get; set; }

        public int CountTestsPerUser { get; set; }

        public int CountProfilesWithoutWarnings { get; set; }

        public Dictionary<EnumResultSeverityType, int> CountBySeverity { get; set; }

        public Dictionary<EnumIdentifiedDataBehavior, int> CountByIdentifiedDataBehavior { get; set; }

        public Dictionary<EnumTestUnitNames, TimeSpan> AverageDuration_ByTestName { get; set; }

        public Dictionary<EnumTestUnitNames, Dictionary<EnumResultSeverityType, int>> CountBySeverity_ByTestName { get; set; }

        public Dictionary<EnumTestUnitNames, Dictionary<EnumIdentifiedDataBehavior, int>> CountByIdentifiedDataBehavior_ByTestName { get; set; }

        public Dictionary<EnumTestUnitNames, double> FrequencySuccess_ByTestName { get; set; }

        public List<EnumTestUnitNames> TestNames { get; set; }

        public Dictionary<EnumTestUnitNames, string> SampleData_ByTestName { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan AverageDurationPerProfile { get; set; }

        public string FileLinkEnd { get; set; }

        public string FileByProfileLink { get; set; }

        public string ErrorHappened { get; set; }

        public string ErrorMessage { get; set; }
    }
}