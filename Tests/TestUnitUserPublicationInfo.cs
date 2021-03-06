﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YSM.PMS.Web.Service.DataTransfer.Models;

namespace TestMVC4App.Models
{
    public class TestUnitUserPublicationInfo : TestUnit
    {
        private IEnumerable<UserPublication> newData;

        public TestUnitUserPublicationInfo(TestSuite parent, IEnumerable<UserPublication> newData) : base(parent)
        {
            this.newData = newData;
        }

        protected override void RunAllSingleTests()
        {
            UserPublicationInfo_Titles();
            UserPublicationInfo_Description();
        }

        private HashSet<string> UserPublicationInfo_Titles()
        {
            HashSet<string> newValues;

            if (this.newData != null)
            {
                newValues = new HashSet<string>(newData.Where(x => x != null).Select(x => HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(x.Publication.Title))));
            }
            else
            {
                newValues = new HashSet<string>();
            }
            this.CompareAndLog_Test(
                        EnumTestUnitNames.UserPublicationInfo_Titles,
                        "Comparing Publication Title(s)",
                        ParsingHelper.ParseUnstructuredListOfValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.featuredPublication.ToString(), EnumOldServiceFieldsAsKeys.titleName.ToString()),
                        newValues);
            return newValues;
        }

        private void UserPublicationInfo_Description()
        {
            var newValues = new HashSet<string>();

            try 
            {
                if (this.newData != null)
                {
                    newValues = new HashSet<string>(this.newData.Where(x => x != null).Select(x => HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(x.Publication.Citation))));
                }
            }
            catch (Exception) { }

            this.CompareAndLog_Test(
                        EnumTestUnitNames.UserPublicationInfo_Citations,
                        "Comparing Publication Citation(s)",
                        ParsingHelper.ParseUnstructuredListOfValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.featuredPublication.ToString(), EnumOldServiceFieldsAsKeys.description.ToString()),
                        newValues);
        }
    }
}