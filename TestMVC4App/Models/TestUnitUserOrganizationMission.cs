using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserOrganizationMission : TestUnit
    {
                # region Data Provided by Parent Test Unit

        private IEnumerable<XElement> oldServiceMissions;
        private IEnumerable<OrganizationMission> newServiceMissions;
        private int userId;
        private int upi;

        public void ProvideData(int userId,
                                            int upi,
                                            IEnumerable<XElement> oldServiceMissions,
                                            IEnumerable<OrganizationMission> newServiceMissions)
        {
            this.userId = userId;
            this.upi = upi;

            if (newServiceMissions != null)
            {
                this.newServiceMissions = newServiceMissions;
            }

            if (oldServiceMissions != null)
            {
                this.oldServiceMissions = oldServiceMissions;
            }
        }

        #endregion

        public override string newServiceURLExtensionBeginning
        {
            get { return Parent.newServiceURLExtensionBeginning; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return Parent.newServiceURLExtensionEnding; }
        }

        public TestUnitUserOrganizationMission(TestSuite parent, TestUnit bigBrother) 
            : base (parent,bigBrother)
        {

        }

        protected override void RunAllSingleTests()
        {
            var oldValues = new HashSet<string>();
            try 
            { 
                if (this.oldServiceMissions.Count() > 0)
                {
                    oldValues = new HashSet<string>(this.oldServiceMissions.SelectMany(s => s.ToString().Replace(" ","").Split(',')));
                }
            } 
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }

            var newValues = new HashSet<string>(this.newServiceMissions.Where(s=>!string.IsNullOrEmpty(s.MissionName)).Select(x=>x.MissionName));

            this.CompareAndLog_Test("UserGeneralInfo_OrganizationMission_Name_Test", "Comparing Organization Missions", userId, upi, oldValues, newValues);

            ComputeOverallSeverity();
        }
    }
}