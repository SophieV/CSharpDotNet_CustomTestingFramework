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
    using System.Linq;
    using TestMVC4App.Models;
    using HtmlDiff;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public partial class TestNameDetailedReport : TestNameDetailedReportBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("<tr>\r\n\t<td class=\"td_main\"><b>");
            
            #line 9 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.TestDescription));
            
            #line default
            #line hidden
            this.Write("</b><br/>\r\n\t\t<a id=\"");
            
            #line 10 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.UPI));
            
            #line default
            #line hidden
            this.Write("\">");
            
            #line 10 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.TestName));
            
            #line default
            #line hidden
            this.Write("</a>\r\n\t</td>\r\n\t");
            
            #line 12 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 switch (DetailedReportDataObject.Result) {
		case ResultSeverityType.FALSE_POSITIVE : 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"td_main false_positive\">");
            
            #line 14 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.Result));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 15 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 break;
		case ResultSeverityType.WARNING: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"td_main warning\">");
            
            #line 17 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.Result));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 18 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 break;
		case ResultSeverityType.WARNING_NO_DATA: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"td_main warning_no_data\">");
            
            #line 20 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.Result));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 21 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 break;
		case ResultSeverityType.ERROR_WITH_EXPLANATION: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"td_main error_with_explanation\">");
            
            #line 23 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.Result));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 24 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 break;
		case ResultSeverityType.ERROR: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"td_main error\">");
            
            #line 26 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.Result));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 27 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 break;
		default: 
            
            #line default
            #line hidden
            this.Write("\t\t<td class=\"td_main success\">");
            
            #line 29 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.Result));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 30 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 break;
	} 
            
            #line default
            #line hidden
            this.Write("\t</td>\r\n\t<td class=\"td_main\" style=\"width: 20%;\">\r\n\t\t<a href=\"");
            
            #line 34 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.OldUrl));
            
            #line default
            #line hidden
            this.Write("\">Profile data of UPI ");
            
            #line 34 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.UPI));
            
            #line default
            #line hidden
            this.Write("</a><br/>\r\n\t\t<a href=\"");
            
            #line 35 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.NewUrl));
            
            #line default
            #line hidden
            this.Write("\">User data of ID is ");
            
            #line 35 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.UserId));
            
            #line default
            #line hidden
            this.Write("</a>\r\n\t</td>\r\n\t<td class=\"td_main\" style=\"width: 60%;\">\r\n\t\t");
            
            #line 38 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.ErrorMessage));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t<br/><br/>\r\n\t\t<table class=\"table_main\">\r\n\t\t<tr>\r\n\t\t\t<td class=\"th_main\">OLD " +
                    "SERVICE DATA ");
            
            #line 42 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(DetailedReportDataObject.OldOrganizationValues.Count <= 0) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"error_color\">[COUNT ELEMENTS: ");
            
            #line 42 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.OldValues.Count));
            
            #line default
            #line hidden
            this.Write("]</span>");
            
            #line 42 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t\t\t<td class=\"th_main\">NEW SERVICE DATA ");
            
            #line 43 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(DetailedReportDataObject.NewOrganizationValues.Count <= 0) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"error_color\">[COUNT ELEMENTS: ");
            
            #line 43 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.NewValues.Count));
            
            #line default
            #line hidden
            this.Write("]</span>");
            
            #line 43 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("</td>\r\n\t\t</tr>\r\n\t\t<tr>\r\n\t\t\t<td style=\"border-right: 1px lightgrey solid;\">\r\n\t\t\t\t<" +
                    "ul>\r\n\t\t\t\t\t");
            
            #line 48 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(DetailedReportDataObject.OldOrganizationValues.Count > 0) {
					string entry = string.Empty;
					foreach (OrganizationTreeDescriptor element in DetailedReportDataObject.OldOrganizationValues) {
						if (element.Depth < 0) {
							entry = "[DEPTH NOT ASSIGNED] " + element.Name + " (" + element.ID + ")"; 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t\t\t<li>");
            
            #line 53 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entry));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t\t\t\t");
            
            #line 54 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(element.IsMissing) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"missing\">[MISSING IN NEW SERVICE]</span>");
            
            #line 54 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t\t\t");
            
            #line 55 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(element.IsDuplicate) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"duplicate\">[DUPLICATE]</span>");
            
            #line 55 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t\t</li>");
            
            #line 56 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"

					} } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t<br/><br/>\r\n\t\t\t\t\t");
            
            #line 59 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 var flattenedTree = new OrganizationTreeDescriptor[] { DetailedReportDataObject.OldTreeRoot }.SelectNestedChildren(t => t.Children).ToList();
					foreach (OrganizationTreeDescriptor element in flattenedTree) {
						entry = string.Empty;
						for (int i = 0; i < element.Depth; i++) { entry += "- "; }
						if (DetailedReportDataObject.TreeComparisonIndexError > 0 && element.Depth == DetailedReportDataObject.TreeComparisonIndexError) { entry += "<span class=\"tree_depth_mismatch\">"; }
						entry += "[DEPTH " + element.Depth + "] " + element.Name + " (" + element.ID + ")";
						if (DetailedReportDataObject.TreeComparisonIndexError > 0 && element.Depth == DetailedReportDataObject.TreeComparisonIndexError) { entry += " [MISMATCH WITH NEW SERVICE]</span>"; } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t<li>");
            
            #line 66 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entry));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t\t");
            
            #line 67 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(element.IsMissing) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"missing\">[MISSING IN NEW SERVICE]</span>");
            
            #line 67 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t");
            
            #line 68 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(element.IsDuplicate) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"duplicate\">[DUPLICATE]</span>");
            
            #line 68 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t</li>\r\n\t\t\t\t\t");
            
            #line 70 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } } else if(DetailedReportDataObject.OldValues.Count > 0) {
					var potentialDuplicates = DetailedReportDataObject.OldValues.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key);
					foreach (string oldValue in DetailedReportDataObject.OldValues) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t<li>");
            
            #line 73 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(!string.IsNullOrEmpty(oldValue)) { 
            
            #line default
            #line hidden
            
            #line 73 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(oldValue));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t\t");
            
            #line 74 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(!DetailedReportDataObject.NewValues.Contains(oldValue)) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"missing\">[MISSING IN NEW SERVICE]</span>");
            
            #line 74 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t");
            
            #line 75 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(potentialDuplicates.Contains(oldValue)) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"duplicate\">[DUPLICATE]</span>");
            
            #line 75 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t");
            
            #line 76 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } else { 
            
            #line default
            #line hidden
            this.Write("NULL");
            
            #line 76 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("</li>\r\n\t\t\t\t\t");
            
            #line 77 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t</ul>\r\n\t\t\t</td>\r\n\t\t\t<td>\r\n\t\t\t\t<ul>\r\n\t\t\t\t\t");
            
            #line 82 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(DetailedReportDataObject.NewOrganizationValues.Count > 0) {
					string entry = string.Empty;
					foreach (OrganizationTreeDescriptor element in DetailedReportDataObject.NewOrganizationValues) {
						if (element.Depth < 0) {
							entry = "[DEPTH NOT ASSIGNED] " + element.Name + " (" + element.ID + ")"; 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t\t\t<li>");
            
            #line 87 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entry));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t\t\t\t");
            
            #line 88 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(element.IsMissing) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"missing\">[MISSING IN OLD SERVICE]</span>");
            
            #line 88 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t\t\t");
            
            #line 89 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(element.IsDuplicate) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"duplicate\">[DUPLICATE]</span>");
            
            #line 89 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t\t\t</li>");
            
            #line 90 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"

					} } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t<br/><br/>\r\n\t\t\t\t\t");
            
            #line 93 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 var flattenedTree2 = new OrganizationTreeDescriptor[] { DetailedReportDataObject.NewTreeRoot }.SelectNestedChildren(t => t.Children).ToList();
					foreach (OrganizationTreeDescriptor element in flattenedTree2) {
						entry = string.Empty;
						for (int i = 0; i < element.Depth; i++) { entry += "- "; }
						if (DetailedReportDataObject.TreeComparisonIndexError > 0 && element.Depth == DetailedReportDataObject.TreeComparisonIndexError) { entry += "<span class=\"tree_depth_mismatch\">"; }
						entry += "[DEPTH " + element.Depth + "] " + element.Name + " (" + element.ID + ")";
						if (DetailedReportDataObject.TreeComparisonIndexError > 0 && element.Depth == DetailedReportDataObject.TreeComparisonIndexError) { entry += " [MISMATCH WITH OLD SERVICE]</span>"; } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t<li>");
            
            #line 100 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entry));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t\t");
            
            #line 101 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(element.IsMissing) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"missing\">[MISSING IN OLD SERVICE]</span>");
            
            #line 101 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t");
            
            #line 102 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(element.IsDuplicate) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"duplicate\">[DUPLICATE]</span>");
            
            #line 102 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t</li>\r\n\t\t\t\t\t");
            
            #line 104 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } } else if(DetailedReportDataObject.NewValues.Count > 0) {
					var potentialDuplicates = DetailedReportDataObject.NewValues.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key);
					foreach (string newValue in DetailedReportDataObject.NewValues) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t<li>");
            
            #line 107 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(!string.IsNullOrEmpty(newValue)) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t");
            
            #line 108 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 string newValueToDisplay = newValue;
					if(DetailedReportDataObject.OldValues.Count <= 1 && DetailedReportDataObject.NewValues.Count <= 1) {
						string oldValue = string.Empty;
						if(DetailedReportDataObject.OldValues.Count > 0) { oldValue = DetailedReportDataObject.OldValues.First(); if(oldValue == null) { oldValue = string.Empty; } }
						HtmlDiff diffHelper = new HtmlDiff(oldValue, newValue);
						newValueToDisplay = diffHelper.Build();
					} 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t");
            
            #line 115 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(newValueToDisplay));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t\t");
            
            #line 116 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(!DetailedReportDataObject.OldValues.Contains(newValue)) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"missing\">[MISSING IN OLD SERVICE]</span>");
            
            #line 116 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t");
            
            #line 117 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(potentialDuplicates.Contains(newValue)) { 
            
            #line default
            #line hidden
            this.Write("<span class=\"duplicate\">[DUPLICATE]</span>");
            
            #line 117 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t");
            
            #line 118 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } else { 
            
            #line default
            #line hidden
            this.Write("NULL");
            
            #line 118 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } 
            
            #line default
            #line hidden
            this.Write("</li>\r\n\t\t\t\t\t");
            
            #line 119 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t</ul>\r\n\r\n\r\n\r\n\r\n\r\n\t\t\t</td>\r\n\t\t</tr>\r\n\t\t</table>\r\n\t</td>\r\n\t<td class=\"td_main\" " +
                    "style=\"width: 30%;\">\r\n\t<p>Duration : ");
            
            #line 131 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DetailedReportDataObject.Duration.ToString("mm'mn:'ss's:'FFFFFFF")));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t<br/>\r\n\t<ul>\r\n\t");
            
            #line 134 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 if(DetailedReportDataObject.IdentifiedDataBehaviors != null) {
		foreach(var observation in DetailedReportDataObject.IdentifiedDataBehaviors) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t<li>");
            
            #line 136 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(LogManager.GetDescription(observation)));
            
            #line default
            #line hidden
            this.Write("</li><br/>\r\n\t");
            
            #line 137 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"
 } } 
            
            #line default
            #line hidden
            this.Write("\t\t</ul>\r\n\t</td>\r\n</tr>");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "\\psf\Home\Desktop\TestMVC4App\Profile-System-Testing\TestMVC4App\Templates\TestNameDetailedReport.tt"

private global::TestMVC4App.Templates.SharedDetailedReportData _DetailedReportDataObjectField;

/// <summary>
/// Access the DetailedReportDataObject parameter of the template.
/// </summary>
private global::TestMVC4App.Templates.SharedDetailedReportData DetailedReportDataObject
{
    get
    {
        return this._DetailedReportDataObjectField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool DetailedReportDataObjectValueAcquired = false;
if (this.Session.ContainsKey("DetailedReportDataObject"))
{
    this._DetailedReportDataObjectField = ((global::TestMVC4App.Templates.SharedDetailedReportData)(this.Session["DetailedReportDataObject"]));
    DetailedReportDataObjectValueAcquired = true;
}
if ((DetailedReportDataObjectValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("DetailedReportDataObject");
    if ((data != null))
    {
        this._DetailedReportDataObjectField = ((global::TestMVC4App.Templates.SharedDetailedReportData)(data));
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
    public class TestNameDetailedReportBase
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
