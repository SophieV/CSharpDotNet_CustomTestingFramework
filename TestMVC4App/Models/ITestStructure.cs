using System;
namespace TestMVC4App.Models
{
    interface ITestStructure
    {
        /// <summary>
        /// Main entry point that manages all the individuals tests for the fields 
        /// of the <see cref="YSM.PMS.Service.Common.DataTransfer.UserGeneralInfo"/> class.
        /// </summary>
        /// <param name="newServiceAccessor">Instance on which the call to the specific
        /// <see cref="YSM.PMS.Service.Common.DataTransfer.IUserService.GetUserById()"/> method tested will be made.</param>
        /// <param name="upi">Old Identifier of the User.</param>
        /// <param name="oldServiceXMLContent">Result returned by the old service - to be parsed.</param>
        /// <param name="userId">New Identifier of the User.</param>
        void RunAllTests(YSM.PMS.Web.Service.Clients.IUsersClient newServiceAccessor, int upi, System.Xml.Linq.XDocument oldServiceXMLContent, int userId = 0);
    }
}
