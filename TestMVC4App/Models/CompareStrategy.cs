﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace TestMVC4App.Models
{
    public abstract class CompareStrategy
    {
        protected ResultReport resultReport;

        public CompareStrategy(List<string> oldValues, List<string> newValues, ResultReport resultReport)
        {
            this.resultReport = resultReport;
            this.resultReport.AddDetailedValues(oldValues, newValues);
        }

        /// <summary>
        /// Replaces the default characters used for describing the mismatched values of the Assert so that their content
        /// is not (mis)interpreted as HTML content.
        /// </summary>
        /// <param name="message">The string to clean.</param>
        /// <returns>The cleansed string.</returns>
        /// <remarks>This process has to take place before HTML content is generated for visualization on the report/includes exceptions to be rendered as HTML.</remarks>
        public static String ReplaceProblematicTagsForHtml(string message)
        {
            message = message.Replace("<", "<span style='color:red;'>[");
            message = message.Replace(">", "]</span>");
            // TODO : change quick fix here - because of call sequence - regex ?
            //message = message.Replace("<span style='color:red;'>[br/]</span>", "<br/>");
            //message = message.Replace("<span style='color:red;'>[/b]</span>", "</b>");
            //message = message.Replace("<span style='color:red;'>[b]</span>", "<b>");
            //message = message.Replace("[br/]", "<br/>");
            //message = message.Replace("[/b]", "</b>");
            //message = message.Replace("[b]", "<b>");
            return message;
        }

        public abstract void Investigate();
    }
}