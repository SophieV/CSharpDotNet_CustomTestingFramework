using System.Collections.Generic;
using System.Linq;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserPublicationInfo : TestUnit
    {
        public override string newServiceURLExtensionBeginning
        {
            get { return "Users/"; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return "/Publications"; }
        }

        public TestUnitUserPublicationInfo(TestSuite parent) : base(parent)
        {
        }

        protected override void RunAllSingleTests()
        {
            UserPublicationInfo newServiceInfo = this.NewDataAccessor.GetUserPublicationsById(this.UserId);
            HashSet<string> newValues;

            if (newServiceInfo.Publications != null)
            {
                newValues = new HashSet<string>(newServiceInfo.Publications.Where(x => x != null).Select(x => x.Title));
            } 
            else
            {
                newValues = new HashSet<string>();
            }
            this.CompareAndLog_Test(
                        EnumTestUnitNames.UserPublicationInfo_Titles,
                        "Comparing Publication Title(s)",
                        this.UserId,
                        this.Upi,
                        ParsingHelper.ParseListSimpleValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.featuredPublication.ToString(), EnumOldServiceFieldsAsKeys.titleName.ToString()),
                        newValues);


            if (newServiceInfo.Publications != null)
            {
                newValues = new HashSet<string>(newServiceInfo.Publications.Where(x => x != null).Select(x => x.Citation));
            }
            else
            {
                newValues = new HashSet<string>();
            }
            this.CompareAndLog_Test(
                        EnumTestUnitNames.UserPublicationInfo_Citations,
                        "Comparing Publication Citation(s)",
                        this.UserId,
                        this.Upi,
                        ParsingHelper.ParseListSimpleValues(this.OldDataNodes, EnumOldServiceFieldsAsKeys.featuredPublication.ToString(), EnumOldServiceFieldsAsKeys.description.ToString()),
                        newValues);
        }
    }
}