using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMVC4App.Models;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class TestUnitUserGeneralContact : TestUnit
    {
        private GeneralContact newData;

        public TestUnitUserGeneralContact(TestSuite parent, GeneralContact newData)
            : base(parent)
        {
            this.newData = newData;
        }

        protected override void RunAllSingleTests()
        {
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_OfficePhone, "Comparing General Contact", this.OldDataNodes, EnumOldServiceFieldsAsKeys.officePhone.ToString(),ParsingHelper.FormatPhoneNumber(this.newData.AcademicPhone));
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_ClinicPhone, "Comparing General Contact", this.OldDataNodes, EnumOldServiceFieldsAsKeys.clinicPhone.ToString(), ParsingHelper.FormatPhoneNumber(this.newData.ClinicPhone));
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_OfficeFax, "Comparing General Contact", this.OldDataNodes, EnumOldServiceFieldsAsKeys.officeFax.ToString(), string.Empty);
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_ClinicFax, "Comparing General Contact", this.OldDataNodes, EnumOldServiceFieldsAsKeys.clinicFax.ToString(), ParsingHelper.FormatPhoneNumber(this.newData.ClinicFax));

            ComputeOverallSeverity();
        }
    }
}
