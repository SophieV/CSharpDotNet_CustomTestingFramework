using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestMVC4App.Models;

namespace TestMVC4App.Templates
{
    public class SharedProfileReportData
    {
        public Dictionary<string, ResultSeverityType> ResultSeverity_ByTestName { get; set; }

        public int UPI { get; set; }

        public string FileLinkEnd { get; set; }

        public string FileLinkBegin { get; set; }
    }
}