﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class TestUnitUserContactLocationInfo : TestUnit
    {
        private UsersClient newServiceAccessor;
        private XDocument oldServiceData;
        private int upi;
        private int userId;

        public override string newServiceURLExtensionBeginning
        {
            get { return "Users/"; }
        }

        public override string newServiceURLExtensionEnding
        {
            get { return "/ContactLocation"; }
        }

        public TestUnitUserContactLocationInfo(TestSuite parent) : base(parent)
        {

        }

        protected override void RunAllSingleTests()
        {
            var newUserContactLocationInfo = newServiceAccessor.GetUserContactLocationById(userId);

            UserContactLocationInfo_Assistants_Test(newUserContactLocationInfo, oldServiceData);
            UserContactLocationInfo_LabWebsites_Test(newUserContactLocationInfo, oldServiceData);
            UserContactLocationInfo_Addresses_Test(newUserContactLocationInfo, oldServiceData);

            ComputeOverallSeverity();
        }

        public void ProvideUserData(XDocument oldData, UsersClient newDataAccessor, int upi, int userId)
        {
            this.newServiceAccessor = newDataAccessor;
            this.oldServiceData = oldData;
            this.userId = userId;
            this.upi = upi;
        }

        /// <summary>
        /// The Assistant Name is a combination of the lastname and firstname.
        /// We are interested in checking :
        /// - whether the amount of persons listed as assistants is a match
        /// - whether the name returned by the new service contains the firstname returned by the old service (defined as good enough match)
        /// </summary>
        /// <param name="newServiceData"></param>
        /// <param name="oldServiceData"></param>
        /// <remarks>Special syntax.</remarks>
        private void UserContactLocationInfo_Assistants_Test(UserContactLocationInfo newServiceData, XDocument oldServiceData)
        {
            HashSet<string> oldValues = ParsingHelper.ParseListSimpleOldValues(oldServiceData, "/Faculty/facultyMember/assistant", "fname");

            HashSet<string> newValues = new HashSet<string>();
            if(newServiceData.Assistants.Count() > 0)
            {
                foreach(var assistant in newServiceData.Assistants)
                {
                    if (!string.IsNullOrEmpty(assistant.UserMinimalInfo.Name))
                    {
                        newValues.Add(assistant.UserMinimalInfo.Name);
                    }
                }
            }

            var pairs = new Dictionary<HashSet<string>, HashSet<string>>();
            if (oldValues.Count() > 0 || newValues.Count() > 0)
            {
                pairs.Add(oldValues,newValues);
            }

            this.CompareAndLog_Test("UserContactLocationInfo_Assistants_Test", "Comparing Assistant Name(s)",userId,upi, pairs,true);
        }

        private void UserContactLocationInfo_LabWebsites_Test(UserContactLocationInfo newServiceData, XDocument oldServiceData)
        {
            IEnumerable<XElement> labWebsites;

            var labWesitesTest = new TestUnitUserLabWebsite(this.Container, this);
            this.Children.Add(labWesitesTest);

            try
            {
                labWebsites = oldServiceData.XPathSelectElements("/Faculty/facultyMember/labWebsite");
            }
            catch (Exception)
            {
                labWebsites = new List<XElement>();
            }

            labWesitesTest.ProvideData(userId,
                                        upi,
                                        labWebsites,
                                        newServiceData.LabWebsites);
            labWesitesTest.RunAllTests();
        }

        private void UserContactLocationInfo_Addresses_Test(UserContactLocationInfo newServiceData, XDocument oldServiceData)
        {
            IEnumerable<XElement> addresses;
            IEnumerable<XElement> mailingInfo;

            var addressesTest = new TestUnitUserAddress(this.Container, this);
            this.Children.Add(addressesTest);

            try
            {
                addresses = oldServiceData.XPathSelectElements("/Faculty/facultyMember/location");
            }
            catch (Exception)
            {
                addresses = new List<XElement>();
            }

            try
            {
                mailingInfo = oldServiceData.XPathSelectElements("/Faculty/facultyMember/*[starts-with(name(),'mailing')]");
            }
            catch (Exception)
            {
                mailingInfo = new List<XElement>();
            }

            addressesTest.ProvideOrganizationData(userId,
                                                     upi,
                                                     addresses,
                                                     mailingInfo,
                                                     newServiceData.UserAddresses);
            addressesTest.RunAllTests();
        }
    }
}