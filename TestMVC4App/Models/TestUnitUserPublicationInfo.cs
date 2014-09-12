using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class TestUnitUserPublicationInfo : TestUnit
    {
        private UsersClient newServiceAccessor;
        private IEnumerable<XElement> oldServiceData;
        private int upi;
        private int userId;

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

        public void ProvideData(IEnumerable<XElement> oldData, UsersClient newDataAccessor, int upi, int userId)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.upi = upi;
            this.userId = userId;
        }

        protected override void RunAllSingleTests()
        {
            UserPublicationInfo newServiceInfo = newServiceAccessor.GetUserPublicationsById(userId);
            this.CompareAndLog_Test(EnumTestUnitNames.UserPublicationInfo_Titles,
                        "Comparing Publication Title(s)",
                        this.userId,
                        this.upi,
                        ParsingHelper.ParseListSimpleValues(oldServiceData, "featuredPublication", "titleName"),
                        new HashSet<string>(newServiceInfo.Publications.Where(x => x != null).Select(x => x.Title)));
            this.CompareAndLog_Test(EnumTestUnitNames.UserPublicationInfo_Citations,
                        "Comparing Publication Citation(s)",
                        this.userId,
                        this.upi,
                        ParsingHelper.ParseListSimpleValues(oldServiceData, "featuredPublication", "description"),
                        new HashSet<string>(newServiceInfo.Publications.Where(x => x != null).Select(x => x.Citation)));
        }
    }
}