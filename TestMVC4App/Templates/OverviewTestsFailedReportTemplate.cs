﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 12.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace TestMVC4App.Templates
{
    using System.Collections.Generic;
    using System.Linq;
    using TestMVC4App.Models;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public partial class OverviewTestsFailedReportTemplate : OverviewTestsFailedReportTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\t<html>\r\n\t<head></head>\r\n\t<body>\r\n\t<h1>Overview Report</h1>\r\n\t<hr/>\r\n\t<h2>Global<" +
                    "/h2>\r\n\t<p>Count of user profiles tested : ");
            
            #line 14 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(StatsReport.CountProfilesTested));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t<p>Total count of tests run : ");
            
            #line 15 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(StatsReport.CountTestsRun));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t<p>Count of tests per user profile : ");
            
            #line 16 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(StatsReport.CountTestsPerUser));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t<p>Count of user profiles free from any kind of warning : ");
            
            #line 17 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(StatsReport.CountProfilesWithoutWarnings));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t<p>Total count of errors : ");
            
            #line 18 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(StatsReport.CountErrors));
            
            #line default
            #line hidden
            this.Write(@"</p>
	<br/>
	<table style=""border:solid 2px lightgrey;border-collapse:collapse;"">
	<tr>
	<th style=""background-color:lightgrey;"">Severity of Failure</th>
	<th style=""background-color:lightgrey;"">Count</th>
	<th style=""background-color:lightgrey;"">Description of the category</th>
	</tr>

	");
            
            #line 27 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
 foreach (KeyValuePair<SeverityState,int> countPerSeverityState in StatsReport.OverviewCountBySeverityState)
	{
	
            
            #line default
            #line hidden
            this.Write("\t<tr>\r\n\r\n\t\t");
            
            #line 32 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
 switch (countPerSeverityState.Key)
	{
		case SeverityState.FALSE_POSITIVE :
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;background-color: green;color:white;border-width:2px;b" +
                    "order-color:lightgrey;border-style:solid;font-weight:bold;\">");
            
            #line 36 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 37 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
		case SeverityState.WARNING:
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;background-color: beige;color:black;border-width:2px;b" +
                    "order-color:lightgrey;border-style:solid;font-weight:bold;\">");
            
            #line 41 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 42 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
		case SeverityState.ERROR_WITH_EXPLANATION:
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;background-color: orange;color:white;border-width:2px;" +
                    "border-color:lightgrey;border-style:solid;font-weight:bold;\">");
            
            #line 46 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 47 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
		case SeverityState.ERROR:
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;background-color: Red;color:white;border-width:2px;bor" +
                    "der-color:lightgrey;border-style:solid;font-weight:bold;\">");
            
            #line 51 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 52 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
		default:
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;color:black;border-width:2px;border-color:lightgrey;bo" +
                    "rder-style:solid;font-weight:bold;\">");
            
            #line 56 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 57 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
	}
	
            
            #line default
            #line hidden
            this.Write("\t</td>\r\n\t<td style=\"padding: 10px;border-width:2px;border-color:lightgrey;border-" +
                    "style:solid;\">");
            
            #line 62 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Value));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t<td style=\"padding: 10px;border-width:2px;border-color:lightgrey;border-s" +
                    "tyle:solid;color:darkgrey;\">");
            
            #line 63 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(AssertFailedReport.GetDescription(countPerSeverityState.Key)));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t</tr>\r\n\t");
            
            #line 65 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

	} 
            
            #line default
            #line hidden
            this.Write(@"
	</table>
	<br/>
	<p>There is a one-to-one relationship between a Failure and a user profile. A given test is run once for the given user profile.</p>
	<br/>
	<table style=""border:solid 2px lightgrey;border-collapse:collapse;"">
	<tr>
	<th style=""background-color:lightgrey;"">Observation Type</th>
	<th style=""background-color:lightgrey;"">Count</th>
	</tr>

	");
            
            #line 78 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
 
	var sortedDictionary = StatsReport.OverviewCountByObservationType.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
	foreach (KeyValuePair<ObservationLabel,int> countPerObservationType in sortedDictionary)
	{
	
            
            #line default
            #line hidden
            this.Write("\t<tr>\r\n\t<td style=\"padding: 10px;border-width:2px;border-color:lightgrey;border-s" +
                    "tyle:solid;\">");
            
            #line 84 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(AssertFailedReport.GetDescription(countPerObservationType.Key)));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t<td style=\"padding: 10px;border-width:2px;border-color:lightgrey;border-s" +
                    "tyle:solid;\">");
            
            #line 85 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerObservationType.Value));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t</tr>\r\n\t");
            
            #line 87 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

	} 
            
            #line default
            #line hidden
            this.Write(@"
	</table>
	<br/>
	<p>There is no straightforward relationship between an observation and a Failure. An observation is made regardless of the test result. 
	A given observation can only be made once for a given test on a given user profile.</p>
	<br/>
	<br/>

	");
            
            #line 97 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
 foreach (string testName in StatsReport.TestNames)
	{
	
            
            #line default
            #line hidden
            this.Write("\t<hr/>\r\n\t\t<h2>");
            
            #line 101 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(testName));
            
            #line default
            #line hidden
            this.Write(@"</h2>

	<table style=""border:solid 2px lightgrey;border-collapse:collapse;"">
	<tr>
	<th style=""background-color:lightgrey;"">Severity of Failure</th>
	<th style=""background-color:lightgrey;"">Count</th>
	<th style=""background-color:lightgrey;"">Frequency</th>
	</tr>

	");
            
            #line 110 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
 foreach (KeyValuePair<SeverityState,int> countPerSeverityState in StatsReport.ByTestNameCountBySeverityState[testName])
	{
	
            
            #line default
            #line hidden
            this.Write("\t<tr>\r\n\r\n\t\t");
            
            #line 115 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
 switch (countPerSeverityState.Key)
	{
		case SeverityState.FALSE_POSITIVE :
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;background-color: green;color:white;border-width:2px;b" +
                    "order-color:lightgrey;border-style:solid;font-weight:bold;\">");
            
            #line 119 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 120 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
		case SeverityState.WARNING:
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;background-color: beige;color:black;border-width:2px;b" +
                    "order-color:lightgrey;border-style:solid;font-weight:bold;\">");
            
            #line 124 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 125 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
		case SeverityState.ERROR_WITH_EXPLANATION:
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;background-color: orange;color:white;border-width:2px;" +
                    "border-color:lightgrey;border-style:solid;font-weight:bold;\">");
            
            #line 129 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 130 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
		case SeverityState.ERROR:
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;background-color: Red;color:white;border-width:2px;bor" +
                    "der-color:lightgrey;border-style:solid;font-weight:bold;\">");
            
            #line 134 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 135 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
		default:
		
            
            #line default
            #line hidden
            this.Write("\t\t<td style=\"padding: 10px;color:black;border-width:2px;border-color:lightgrey;bo" +
                    "rder-style:solid;font-weight:bold;\">");
            
            #line 139 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Key));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 140 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		break;
	}
	
            
            #line default
            #line hidden
            this.Write("\t</td>\r\n\t<td style=\"padding: 10px;border-width:2px;border-color:lightgrey;border-" +
                    "style:solid;\">");
            
            #line 145 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerSeverityState.Value));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t<td style=\"padding: 10px;border-width:2px;border-color:lightgrey;border-s" +
                    "tyle:solid;\">\r\n\t");
            
            #line 147 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
 double frequency = (double)countPerSeverityState.Value /(double) StatsReport.CountProfilesTested;
	if (frequency > 0)
	{
	
            
            #line default
            #line hidden
            this.Write("\t");
            
            #line 151 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(frequency.ToString("P")));
            
            #line default
            #line hidden
            this.Write("\r\n\t");
            
            #line 152 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

	} 
            
            #line default
            #line hidden
            this.Write("\t</td>\r\n\t</tr>\r\n\t");
            
            #line 156 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

	} 
            
            #line default
            #line hidden
            this.Write("\r\n\t</table>\r\n\t<br/>\r\n\t<br/>\r\n\t<table style=\"border:solid 2px lightgrey;border-col" +
                    "lapse:collapse;\">\r\n\t<tr>\r\n\t<th style=\"background-color:lightgrey;\">Observation T" +
                    "ype</th>\r\n\t<th style=\"background-color:lightgrey;\">Count</th>\r\n\t</tr>\r\n\r\n\t");
            
            #line 168 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
 
	if(StatsReport.ByTestNameCountByObservationType.ContainsKey(testName))
	{
	var sortedDictionary2 =StatsReport.ByTestNameCountByObservationType[testName].OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
	foreach (KeyValuePair<ObservationLabel,int> countPerObservationType in sortedDictionary2)
	{
		if(countPerObservationType.Value > 0)
		{
	
            
            #line default
            #line hidden
            this.Write("\t<tr>\r\n\t<td style=\"padding: 10px;border-width:2px;border-color:lightgrey;border-s" +
                    "tyle:solid;\">");
            
            #line 178 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(AssertFailedReport.GetDescription(countPerObservationType.Key)));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t<td style=\"padding: 10px;border-width:2px;border-color:lightgrey;border-s" +
                    "tyle:solid;\">");
            
            #line 179 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(countPerObservationType.Value));
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t</tr>\r\n\t");
            
            #line 181 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

		}
	}
	} 
            
            #line default
            #line hidden
            this.Write("\r\n\t</table>\r\n\t");
            
            #line 187 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

	}
	
            
            #line default
            #line hidden
            this.Write("\t</body>\r\n\t</html>\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\OverviewTestsFailedReportTemplate.tt"

private global::TestMVC4App.Templates.OverviewStatsReport _StatsReportField;

/// <summary>
/// Access the StatsReport parameter of the template.
/// </summary>
private global::TestMVC4App.Templates.OverviewStatsReport StatsReport
{
    get
    {
        return this._StatsReportField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool StatsReportValueAcquired = false;
if (this.Session.ContainsKey("StatsReport"))
{
    this._StatsReportField = ((global::TestMVC4App.Templates.OverviewStatsReport)(this.Session["StatsReport"]));
    StatsReportValueAcquired = true;
}
if ((StatsReportValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("StatsReport");
    if ((data != null))
    {
        this._StatsReportField = ((global::TestMVC4App.Templates.OverviewStatsReport)(data));
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
    public class OverviewTestsFailedReportTemplateBase
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