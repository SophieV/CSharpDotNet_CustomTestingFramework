﻿using System.Collections.Generic;
using System.Linq;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserPublicationInfo : TestUnit
    {
        private IEnumerable<Publication> newData;

        public TestUnitUserPublicationInfo(TestSuite parent, IEnumerable<Publication> newData) : base(parent)
        {
            this.newData = newData;
        }

        protected override void RunAllSingleTests()
        {
            UserPublicationInfo_Titles();
            UserPublicationInfo_Citations();
        }

        private HashSet<string> UserPublicationInfo_Titles()
        {
            HashSet<string> newValues;

            if (this.newData != null)
            {
                newValues = new HashSet<string>(newData.Where(x => x != null).Select(x => x.Title));
            }
            else
            {
                newValues = new HashSet<string>();
            }
            this.CompareAndLog_Test(
                        EnumTestUnitNames.UserPublicationInfo_Titles,
                        "Comparing Publication Title(s)",
                        ParsingHelper.ParseListSimpleValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.featuredPublication.ToString(), EnumOldServiceFieldsAsKeys.titleName.ToString()),
                        newValues);
            return newValues;
        }

        private void UserPublicationInfo_Citations()
        {
            HashSet<string> newValues;

            if (this.newData != null)
            {
                newValues = new HashSet<string>(this.newData.Where(x => x != null).Select(x => x.Citation));
            }
            else
            {
                newValues = new HashSet<string>();
            }
            this.CompareAndLog_Test(
                        EnumTestUnitNames.UserPublicationInfo_Citations,
                        "Comparing Publication Citation(s)",
                        ParsingHelper.ParseListSimpleValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.featuredPublication.ToString(), EnumOldServiceFieldsAsKeys.description.ToString()),
                        newValues);
        }
    }
}