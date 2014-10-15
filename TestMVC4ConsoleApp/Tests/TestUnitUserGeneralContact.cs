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
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_OfficePhone, "Comparing General Contact", this.OldDataNodes, EnumOldServiceFieldsAsKeys.officePhone.ToString(),ParsingHelper.FormatPhoneNumber((this.newData != null?this.newData.AcademicPhone:string.Empty), (this.newData != null?this.newData.AcademicPhoneExtension:string.Empty)));
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_ClinicPhone, "Comparing General Contact", this.OldDataNodes, EnumOldServiceFieldsAsKeys.clinicPhone.ToString(), ParsingHelper.FormatPhoneNumber((this.newData != null?this.newData.ClinicPhone:string.Empty), (this.newData != null?this.newData.ClinicPhoneExtension:string.Empty)));
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_LabPhone, "Comparing General Contact", this.OldDataNodes, EnumOldServiceFieldsAsKeys.labPhone.ToString(), ParsingHelper.FormatPhoneNumber((this.newData != null?this.newData.LabPhone:string.Empty), (this.newData != null?this.newData.LabPhoneExtension:string.Empty))); 
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_OfficeFax, "Comparing General Contact", this.OldDataNodes, EnumOldServiceFieldsAsKeys.officeFax.ToString(), ParsingHelper.FormatPhoneNumber((this.newData != null?this.newData.OfficeFax:string.Empty)));
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_ClinicFax, "Comparing General Contact", this.OldDataNodes, EnumOldServiceFieldsAsKeys.clinicFax.ToString(), ParsingHelper.FormatPhoneNumber((this.newData != null?this.newData.ClinicFax:string.Empty)));
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_MobilePhone, "Comparing General Contact", "false", (this.newData != null ? this.newData.IsMobilePhoneDisplayed.ToString() : string.Empty));
            this.CompareAndLog_Test(EnumTestUnitNames.UserContactLocationInfo_GeneralContact_Pager, "Comparing General Contact", "false", (this.newData != null ? this.newData.IsPagerDisplayed.ToString() : string.Empty));
            ComputeOverallSeverity();
        }
    }
}
