using System;
using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    public class SharedProfileReportData
    {
        public Dictionary<TestUnitNames, ResultSeverityType> ResultSeverity_ByTestName { get; set; }

        public int UPI { get; set; }

        public string FileLinkEnd { get; set; }

        public TimeSpan Duration { get; set; }
    }
}