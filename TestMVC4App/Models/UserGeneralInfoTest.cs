using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Xml.XPath;
using TestMVC4App.Templates;
using YSM.PMS.Service.Common.DataTransfer;

namespace TestMVC4App.Models
{
    public class UserGeneralInfoTest : ITestStructure
    {
        private const string NEW_SERVICE_SUB_URL_BASE = "Users/";
        private const string NEW_SERVICE_SUB_URL_ENDING = "/GeneralInfo";
        private string NEW_SERVICE_URL_BASE = WebConfigurationManager.AppSettings["ProfileServiceBaseAddress"] + NEW_SERVICE_SUB_URL_BASE;

        void ITestStructure.RunAllTests(YSM.PMS.Web.Service.Clients.IUsersClient newServiceAccessor, int upi, System.Xml.Linq.XDocument oldServiceXMLContent, int userId)
        {
            var newUserGeneralInfo = newServiceAccessor.GetUserGeneralInfoById(userId);

            UserGeneralInfo_Bio_Test(newUserGeneralInfo, oldServiceXMLContent);
            UserGeneralInfo_Titles_Test(newUserGeneralInfo, oldServiceXMLContent);
            UserGeneralInfo_Organizations_Test(newUserGeneralInfo, oldServiceXMLContent);
        }

        #region Field Comparison Tests

        private bool UserGeneralInfo_Bio_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            return UserServiceTestSuite.HandleSimpleStringCompare(oldServiceData, 
                                                                   "/Faculty/facultyMember/biography", 
                                                                   newServiceData.Bio, 
                                                                   newServiceData.UserId, 
                                                                   newServiceData.Upi, 
                                                                   "Comparing Bio", 
                                                                   NEW_SERVICE_URL_BASE + newServiceData.UserId);
        }

        private bool UserGeneralInfo_Titles_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            List<string> oldValues = new List<string>();
            List<string> newValues = new List<string>();

            try
            {
                var titles = oldServiceData.XPathSelectElements("/Faculty/facultyMember/title");

                foreach(XElement el in titles)
                {
                    oldValues.Add(el.Element("titleName").Value);
                }
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }
            
            if(newServiceData.Titles.Count() > 0)
            {
                foreach(var title in newServiceData.Titles)
                {
                    newValues.Add(title.TitleName);
                }
            }

            return UserServiceTestSuite.HandleComparingSimpleCollectionString(oldValues, 
                                                                              newValues, 
                                                                              newServiceData.UserId, 
                                                                              newServiceData.Upi, 
                                                                              "Comparing Titles",
                                                                              NEW_SERVICE_URL_BASE + newServiceData.UserId + NEW_SERVICE_SUB_URL_ENDING);
        }

        /// <summary>
        /// TODO : Considering moving Org testing to a separate class
        /// </summary>
        /// <param name="newServiceData"></param>
        /// <param name="oldServiceData"></param>
        /// <returns></returns>
        private bool UserGeneralInfo_Organizations_Test(UserGeneralInfo newServiceData, XDocument oldServiceData)
        {
            bool success = true;

            List<string> oldOrganizationIdValues = new List<string>();
            List<string> newOrganizationIdValues = new List<string>();

            List<string> oldOrganizationNameValues = new List<string>();
            List<string> newOrganizationNameValues = new List<string>();

            try
            {
                var departments = oldServiceData.XPathSelectElements("/Faculty/facultyMember/department");

                foreach (XElement el in departments)
                {
                    try
                    {
                        oldOrganizationIdValues.Add(el.Element("OrgID").Value);
                    } 
                    catch (Exception)
                    {
                        // no value to parse
                    }

                    try
                    {
                        oldOrganizationNameValues.Add(el.Element("departmentName").Value);
                    }
                    catch (Exception)
                    {
                        // no value to parse
                    }
                }
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            if (newServiceData.Organizations.Count() > 0)
            {
                foreach (var organization in newServiceData.Organizations)
                {
                    newOrganizationIdValues.Add(organization.OrganizationId.ToString());
                    newOrganizationNameValues.Add(organization.Name);
                }
            }

            success &= UserServiceTestSuite.HandleComparingSimpleCollectionString(oldOrganizationIdValues, 
                                                                                  newOrganizationIdValues, 
                                                                                  newServiceData.UserId, 
                                                                                  newServiceData.Upi, 
                                                                                  "Comparing OrganizationId",
                                                                                  NEW_SERVICE_URL_BASE + newServiceData.UserId + NEW_SERVICE_SUB_URL_ENDING);

            success &= UserServiceTestSuite.HandleComparingSimpleCollectionString(oldOrganizationNameValues, 
                                                                                  newOrganizationNameValues, 
                                                                                  newServiceData.UserId, 
                                                                                  newServiceData.Upi, 
                                                                                  "Comparing OrganizationName",
                                                                                  NEW_SERVICE_URL_BASE + newServiceData.UserId + NEW_SERVICE_SUB_URL_ENDING);
            return success;
        }

        #endregion
    }
}