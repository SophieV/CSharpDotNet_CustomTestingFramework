﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 12.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace TestMVC4ConsoleApp.Templates
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    using TestMVC4App.Models;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public partial class SummaryReport : SummaryReportBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"<html>
<head>
<style>
.table_main {border:solid 2px darkgrey;border-collapse:collapse;}
.title {text-align: center;}
.th_main {padding: 10px;font-variant: small-caps;text-align: center;border-width:2px;border-color:darkgrey;border-style:solid;background-color: lightgrey;}
.td_main {padding: 10px;border-width:2px;border-color:darkgrey;border-style:solid;}
.warning {background-color: bisque;color:black;font-weight:bold;}
.warning_no_data {background-color: beige;color:black;font-weight:bold;}
.false_positive {background-color: green;color:white;font-weight:bold;}
.error {background-color: Red;color:white;font-weight:bold;}
.success {color:black;font-weight:bold;}
.error_with_explanation {background-color: orange;color:white;font-weight:bold;}
.error_data {color: white;font-weight: bold;background-color: red;font-size: 2em; text-align: center;}
</style>
</head>
<body>
	<h1 id=""top"" class=""title"">Summary Report</h1>
	<hr/>

	");
            
            #line 29 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 if(!string.IsNullOrEmpty(SharedDataObject.ErrorHappened)) { 
	string message = "UNKNOWN ERROR(S) MAY MAKE THESE RESULTS INCONSISTENT/UNRELIABLE !";
	if (SharedDataObject.ErrorHappened == "HTTP") { message = "PROBLEMS CONNECTING TO DATA SERVICES MAKE THESE RESULTS INCONSISTENT/UNRELIABLE !"; } 
            
            #line default
            #line hidden
            this.Write("\t <p class=\"error_data\">");
            
            #line 32 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(message));
            
            #line default
            #line hidden
            this.Write("<br/>");
            
            #line 32 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SharedDataObject.ErrorMessage));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t <hr/>\r\n\t");
            
            #line 34 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n\t<h2>Overview</h2>\r\n\t<p>Total count of tests run : ");
            
            #line 37 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(String.Format("{0:0,0}",SharedDataObject.CountTestsRun)));
            
            #line default
            #line hidden
            this.Write("<br/>\r\n\tCount of tests per user profile : ");
            
            #line 38 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(String.Format("{0:0,0}",SharedDataObject.CountTestsPerUser)));
            
            #line default
            #line hidden
            this.Write("<br/>\r\n\tCount of user profiles tested : ");
            
            #line 39 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(String.Format("{0:0,0}",SharedDataObject.CountProfilesTested)));
            
            #line default
            #line hidden
            this.Write("<br/>\r\n\tCount of user profiles ignored : ");
            
            #line 40 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(String.Format("{0:0,0}",SharedDataObject.CountProfilesIgnored)));
            
            #line default
            #line hidden
            this.Write("<br/>\r\n\tCount of user profiles free from any kind of warning : ");
            
            #line 41 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(String.Format("{0:0,0}",SharedDataObject.CountProfilesWithoutWarnings)));
            
            #line default
            #line hidden
            this.Write("<br/>\r\n\tProfile Average Testing Duration : ");
            
            #line 42 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SharedDataObject.Duration.ToString("hh'h: 'mm'mn:'ss's:'FFFFFFF")));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t<p>Average Duration by User Profile : ");
            
            #line 43 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SharedDataObject.AverageDurationPerProfile.ToString("hh'h: 'mm'mn:'ss's:'FFFFFFF")));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t<br/>\r\n\t<table class=\"table_main\">\r\n\t\t<tr>\r\n\t\t<th class=\"th_main\">Result</" +
                    "th>\r\n\t\t<th class=\"th_main\">Count</th>\r\n\t\t<th class=\"th_main\">Description</th>\r\n\t" +
                    "\t</tr>\r\n\t\t");
            
            #line 51 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 foreach (KeyValuePair<EnumResultSeverityType,int> countPerSeverity in SharedDataObject.CountBySeverity) { 
            
            #line default
            #line hidden
            this.Write("\t\t<tr>\r\n\t\t\t");
            
            #line 53 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 switch (countPerSeverity.Key) {
			case EnumResultSeverityType.FALSE_POSITIVE : 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"false_positive td_main\">\r\n\t\t\t");
            
            #line 56 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
			case EnumResultSeverityType.WARNING:
			case EnumResultSeverityType.WARNING_ONLY_NEW: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"warning td_main\">\r\n\t\t\t");
            
            #line 60 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
			case EnumResultSeverityType.WARNING_NO_DATA: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"warning_no_data td_main\">\r\n\t\t\t");
            
            #line 63 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
			case EnumResultSeverityType.ERROR_WITH_EXPLANATION:
			case EnumResultSeverityType.ERROR_ONLY_OLD: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"error_with_explanation td_main\">\r\n\t\t\t");
            
            #line 67 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
			case EnumResultSeverityType.ERROR: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"error td_main\">\r\n\t\t\t");
            
            #line 70 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
			default: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"success td_main\">\r\n\t\t\t");
            
            #line 73 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
		} 
            
            #line default
            #line hidden
            this.Write("\t\t");
            
            #line 75 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverity.Key));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t\t<td class=\"td_main\">");
            
            #line 76 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(String.Format("{0:0,0}",countPerSeverity.Value)));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t\t<td class=\"td_main\" style=\"color:darkgrey;\">");
            
            #line 77 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ParsingHelper.GetDescription(countPerSeverity.Key)));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t</tr>\r\n\t");
            
            #line 79 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t</table>\r\n\t<br/>\r\n\t<br/>\r\n\t<hr/>\r\n\t<h2>Overview By Test Name Per User Profile</h" +
                    "2>\r\n\t<a href=\"");
            
            #line 85 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SharedDataObject.Link2ProfileFile));
            
            #line default
            #line hidden
            this.Write(@""">Go to Overview of Results by UPI</a>
	<br/>
	<hr/>
	<h2>Overview By Test Name</h2>
	<br/>
	<table class=""table_main"">
	<tr>
		<th class=""th_main"">Test Name</th>
		<th class=""th_main"">Overall Success</th>
		<th class=""th_main"">Result Severity</th>
		<th class=""th_main"">Observations</th>
		<th class=""th_main"">Sample Data</th>
		<th class=""th_main"">Average Duration</th>
		<th class=""th_main"">More Info</th>
	</tr>
	");
            
            #line 100 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 foreach (var testName in SharedDataObject.AllTestNames) { 
            
            #line default
            #line hidden
            this.Write("\t<tr>\r\n\t\t<td class=\"td_main\">");
            
            #line 102 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(testName));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t\t<td class=\"th_main\">\r\n\t\t\t");
            
            #line 104 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SharedDataObject.FrequencySuccessByTestName[testName].ToString("P")));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t</td>\r\n\t\t<td class=\"td_main\">\r\n\t\t\t<table class=\"table_main\">\r\n\t\t\t");
            
            #line 108 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 foreach (KeyValuePair<EnumResultSeverityType,int> countPerSeverity in SharedDataObject.CountSeverityByTestName[testName]) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t<tr>\r\n\t\t\t");
            
            #line 110 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 switch (countPerSeverity.Key) {
				case EnumResultSeverityType.FALSE_POSITIVE : 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t<td class=\"false_positive td_main\">\r\n\t\t\t\t");
            
            #line 113 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
				case EnumResultSeverityType.WARNING:
				case EnumResultSeverityType.WARNING_ONLY_NEW: 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t<td class=\"warning td_main\">\r\n\t\t\t\t");
            
            #line 117 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
				case EnumResultSeverityType.WARNING_NO_DATA: 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t<td class=\"warning_no_data td_main\">\r\n\t\t\t\t");
            
            #line 120 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
				case EnumResultSeverityType.ERROR_WITH_EXPLANATION:
				case EnumResultSeverityType.ERROR_ONLY_OLD: 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t<td class=\"error_with_explanation td_main\">\r\n\t\t\t\t");
            
            #line 124 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
				case EnumResultSeverityType.ERROR: 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t<td class=\"error td_main\">\r\n\t\t\t\t");
            
            #line 127 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
				default: 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t<td class=\"success td_main\">\r\n\t\t\t\t");
            
            #line 130 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 break;
			} 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t");
            
            #line 132 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverity.Key));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t\t\t\t<td class=\"td_main\">");
            
            #line 133 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(String.Format("{0:0,0}", countPerSeverity.Value)));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t\t\t\t<td class=\"td_main\">\r\n\t\t\t\t\t");
            
            #line 135 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 double frequency = (double)countPerSeverity.Value /(double) SharedDataObject.CountProfilesTested;
					if (frequency > 0) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t");
            
            #line 137 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(frequency.ToString("P")));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t\t");
            
            #line 138 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t\t");
            
            #line 141 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t</table>\r\n\t\t</td>\r\n\t\t<td class=\"td_main\">\r\n\t\t\t<table class=\"table_main\">\r\n\t\t\t\t" +
                    "<tr>\r\n\t\t\t\t\t<th class=\"th_main\">Identified Data Behavior</th>\r\n\t\t\t\t\t<th class=\"th" +
                    "_main\">Count</th>\r\n\t\t\t\t</tr>\r\n\t\t\t\t");
            
            #line 150 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 if(SharedDataObject.CountIdentifiedDataBehaviorByTestName.ContainsKey(testName)) {
				foreach (KeyValuePair<EnumIdentifiedDataBehavior,int> countPerIdentifiedDataBehavior in SharedDataObject.CountIdentifiedDataBehaviorByTestName[testName]) {
					if(countPerIdentifiedDataBehavior.Value > 0) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t<tr>\r\n\t\t\t\t\t<td class=\"td_main\">");
            
            #line 154 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(LogManager.IdentifiedBehaviorsDescriptions[countPerIdentifiedDataBehavior.Key]));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t\t\t\t\t<td class=\"td_main\">");
            
            #line 155 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(String.Format("{0:0,0}", countPerIdentifiedDataBehavior.Value)));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t\t\t\t</tr>\r\n\t\t\t\t");
            
            #line 157 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 } } } 
            
            #line default
            #line hidden
            this.Write("\t\t\t</table>\r\n\t\t</td>\r\n\t\t<td class=\"td_main\">\r\n\t\t\t");
            
            #line 161 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 if(SharedDataObject.SampleDataByTestName.ContainsKey(testName)) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t");
            
            #line 162 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SharedDataObject.SampleDataByTestName[testName]));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t");
            
            #line 163 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t</td>\r\n\t\t<td class=\"td_main\">\r\n\t\t");
            
            #line 166 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 if(SharedDataObject.AverageDurationByTestName.ContainsKey(testName)) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t");
            
            #line 167 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SharedDataObject.AverageDurationByTestName[testName].ToString("hh'h: 'mm'mn:'ss's:'FFFFFFF")));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t");
            
            #line 168 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t</td>\r\n\t\t<td class=\"td_main\">\r\n\t\t\t<a href=\"");
            
            #line 171 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(testName));
            
            #line default
            #line hidden
            
            #line 171 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SharedDataObject.LinkEnd2TestNameFile));
            
            #line default
            #line hidden
            this.Write("\">Test Details</a>\r\n\t\t</td>\r\n\t</tr>\r\n\t");
            
            #line 174 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t</table>\r\n</body>\r\n</html>\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4ConsoleApp\Templates\SummaryReport.tt"

private global::TestMVC4App.Templates.SummaryReportSharedData _SharedDataObjectField;

/// <summary>
/// Access the SharedDataObject parameter of the template.
/// </summary>
private global::TestMVC4App.Templates.SummaryReportSharedData SharedDataObject
{
    get
    {
        return this._SharedDataObjectField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool SharedDataObjectValueAcquired = false;
if (this.Session.ContainsKey("SharedDataObject"))
{
    this._SharedDataObjectField = ((global::TestMVC4App.Templates.SummaryReportSharedData)(this.Session["SharedDataObject"]));
    SharedDataObjectValueAcquired = true;
}
if ((SharedDataObjectValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("SharedDataObject");
    if ((data != null))
    {
        this._SharedDataObjectField = ((global::TestMVC4App.Templates.SummaryReportSharedData)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public class SummaryReportBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}