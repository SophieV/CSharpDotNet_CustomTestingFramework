using System;
using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    public class ProfileReportSharedData
    {
        public Dictionary<EnumTestUnitNames, EnumResultSeverityType> SeverityByTestName { get; set; }

        public int UPI { get; set; }

        public string LinkEnd2TestNameFile { get; set; }

        public TimeSpan DurationTestingProfile { get; set; }

        public TimeSpan DurationDownloadingDataFromOldService { get; set; }

        public TimeSpan DurationDownloadingDataFromNewService { get; set; }
    }
}