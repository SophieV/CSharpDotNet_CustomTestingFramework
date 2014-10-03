using System;
using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    /// <summary>
    /// Object shared between the application logic and the UI template.
    /// It contains the necessary data needed for display on the Report.
    /// </summary>
    public class SummaryReportSharedData
    {
        public HashSet<EnumTestUnitNames> Not100PercentReturned { get; set; }

        public double CountProfilesTested { get; set; }

        public double CountProfilesIgnored { get; set; }

        public double CountTestsRun { get; set; }

        public int CountTestsPerUser { get; set; }

        public int CountProfilesWithoutWarnings { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan AverageDurationPerProfile { get; set; }

        public Dictionary<EnumResultSeverityType, int> CountBySeverity { get; set; }

        public Dictionary<EnumIdentifiedDataBehavior, int> CountByIdentifiedDataBehavior { get; set; }

        public Dictionary<EnumTestUnitNames, TimeSpan> AverageDurationByTestName { get; set; }

        public Dictionary<EnumTestUnitNames, Dictionary<EnumResultSeverityType, int>> CountSeverityByTestName { get; set; }

        public Dictionary<EnumTestUnitNames, Dictionary<EnumIdentifiedDataBehavior, int>> CountIdentifiedDataBehaviorByTestName { get; set; }

        public Dictionary<EnumTestUnitNames, double> FrequencySuccessByTestName { get; set; }

        public List<EnumTestUnitNames> AllTestNames { get; set; }

        public Dictionary<EnumTestUnitNames, string> SampleDataByTestName { get; set; }

        public string LinkEnd2TestNameFile { get; set; }

        public string Link2ProfileFile { get; set; }

        public string ErrorHappened { get; set; }

        public string ErrorMessage { get; set; }
    }
}