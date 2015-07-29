using System;
using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    /// <summary>
    /// Object shared between the application logic and the UI template.
    /// It contains the necessary data needed for display on the Report.
    /// </summary>
    public class ProfileReportSharedData
    {
        public Dictionary<EnumTestUnitNames, EnumResultSeverityType> SeverityByTestName { get; set; }

        public int OldId { get; set; }

        public string LinkEnd2TestNameFile { get; set; }

        public TimeSpan DurationTestingProfile { get; set; }

        public TimeSpan DurationDownloadingDataFromOldService { get; set; }

        public TimeSpan DurationDownloadingDataFromNewService { get; set; }
    }
}