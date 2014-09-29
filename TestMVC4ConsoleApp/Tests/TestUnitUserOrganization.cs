using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class TestUnitUserOrganization : TestUnit
    {
        private IEnumerable<OrganizationTreeInfo> newData;
        private HashSet<OrganizationTreeDescriptor> newServiceOrganizationDescriptors;
        private HashSet<OrganizationTreeDescriptor> oldServiceOrganizationDescriptors;

        public TestUnitUserOrganization(TestSuite parent, IEnumerable<OrganizationTreeInfo> newData)
            :base(parent)
        {
            this.newData = newData;
            this.newServiceOrganizationDescriptors = new HashSet<OrganizationTreeDescriptor>();
            this.oldServiceOrganizationDescriptors = new HashSet<OrganizationTreeDescriptor>();
        }

        protected override void RunAllSingleTests()
        {
            OrganizationTreeDescriptor oldTreeRoot = null;

            try 
            { 
                oldTreeRoot = ParseOldServiceData(); 
            } 
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }

            if (this.oldServiceOrganizationDescriptors.Count == 1)
            {
                // it means that there is only the root node, which is equivalent to no tree
                this.oldServiceOrganizationDescriptors.Clear();
            }

            OrganizationTreeDescriptor newTreeRoot = ParseNewServiceData();

            UserGeneralInfo_Organization_Id_Test(new HashSet<string>(this.oldServiceOrganizationDescriptors.Where(a => !string.IsNullOrEmpty(a.ID)).Select(a => a.ID)),
                                                 new HashSet<string>(this.newServiceOrganizationDescriptors.Where(a => !string.IsNullOrEmpty(a.ID)).Select(a => a.ID)));

            UserGeneralInfo_Organization_Name_Test(new HashSet<string>(this.oldServiceOrganizationDescriptors.Where(a => !string.IsNullOrEmpty(a.Name)).Select(a => a.Name)),
                                                   new HashSet<string>(this.newServiceOrganizationDescriptors.Where(a => !string.IsNullOrEmpty(a.Name)).Select(a => a.Name)));

            UserGeneralInfo_Organization_Type_Test();

            UserGeneralInfo_Organization_IdAndNameTogether_Test(oldTreeRoot,newTreeRoot);
            UserGeneralInfo_Organization_CheckTreeDepthCoherence_Test(oldTreeRoot, newTreeRoot);
            UserGeneralInfo_Organization_MergingNewTreeToOldOne_Test(oldTreeRoot, newTreeRoot);

            UserGeneralInfo_Organization_Missions_Test();
            UserGeneralInfo_Organizations_YmgStatus_Test();

            ComputeOverallSeverity();
        }

        #region Parsing Input Data

        private OrganizationTreeDescriptor ParseNewServiceData()
        {
            var rootContainer = new OrganizationTreeDescriptor();
            rootContainer.Depth = 0;
            this.newServiceOrganizationDescriptors.Add(rootContainer);

            if (this.newData != null && this.newData.Count() > 0)
            {
                foreach (var organization in this.newData)
                {
                    PopulateTreeDescriptorsFromNew(organization, rootContainer);
                }
            }

            return rootContainer;
        }

        private void PopulateTreeDescriptorsFromNew(OrganizationTreeInfo newData, OrganizationTreeDescriptor parent, int depth = 0)
        {
            var organizationDescriptor = new OrganizationTreeDescriptor()
            {
                ID = newData.OrganizationId.ToString(),
                Name = newData.Name,
                Type = newData.Type,
                Depth = depth + 1,
                IsDisplayedOnYmg = newData.IsDisplayedOnYmg
            };

            if (newData.Missions != null)
            {
                organizationDescriptor.Missions = new HashSet<string>(newData.Missions.Select(x => x.Name));
            }

            if (parent != null)
            {
                organizationDescriptor.ParentId = parent.ID;
                organizationDescriptor.Parent = parent;

                parent.Children.Add(organizationDescriptor);
            }

            this.newServiceOrganizationDescriptors.Add(organizationDescriptor);

            foreach (var child in newData.Children)
            {
                PopulateTreeDescriptorsFromNew(child, organizationDescriptor, depth + 1);
            }
        }

        private OrganizationTreeDescriptor ParseOldServiceData()
        {
            var rootContainer = new OrganizationTreeDescriptor();
            rootContainer.Depth = 0;
            this.oldServiceOrganizationDescriptors.Add(rootContainer);

            string parsedOrgId = string.Empty;
            string parsedOrgName = string.Empty;
            string parsedMissions = string.Empty;

            // first get the tree and its elements
            try
            {
                PopulateTreeDescriptorsFromOld(this.OldDataNodes.First(), rootContainer, this.oldServiceOrganizationDescriptors);
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }

            return rootContainer;
        }

        private static void PopulateTreeDescriptorsFromOld(XElement element, OrganizationTreeDescriptor parent, HashSet<OrganizationTreeDescriptor> allPuzzlePieces, int depth = 0)
        {
            OrganizationTreeDescriptor orgDesc = parent;
            string tempValue;

            if (element != null && element.Name == EnumOldServiceFieldsAsKeys.treeDepartment.ToString()) 
            {
                orgDesc = new OrganizationTreeDescriptor();

                try
                {
                    orgDesc.ID = element.Element(EnumOldServiceFieldsAsKeys.orgID.ToString()).Value.Trim();
                }
                catch (Exception)
                {
                    // no value to parse
                    orgDesc.ID = string.Empty;
                }

                try
                {
                    orgDesc.Name = element.Element(EnumOldServiceFieldsAsKeys.name.ToString()).Value.Trim();
                }
                catch (Exception)
                {
                    // no value to parse
                    orgDesc.Name = string.Empty;
                }

                try
                {
                    orgDesc.Type = element.Element(EnumOldServiceFieldsAsKeys.orgType.ToString()).Value.Trim();
                }
                catch (Exception)
                {
                    // no value to parse
                    orgDesc.Type = string.Empty;
                }

                try
                {
                    orgDesc.IsDisplayedOnYmg = (element.Element(EnumOldServiceFieldsAsKeys.ymgStatus.ToString()).Value.Trim() == "Y");
                }
                catch (Exception)
                {
                    // no value to parse
                    orgDesc.IsDisplayedOnYmg = false;
                }

                try
                {

                    tempValue = element.Element(EnumOldServiceFieldsAsKeys.mission.ToString()).Value;

                    if (!string.IsNullOrEmpty(tempValue))
                    {
                        var tempValues = tempValue.Split(',');

                        foreach(var temp in tempValues)
                        {
                            orgDesc.Missions.Add(temp);
                        }
                    }
                }
                catch (Exception)
                {
                    // no value to parse
                }

                try
                {
                    string isPrimary = element.Element(EnumOldServiceFieldsAsKeys.primaryDept.ToString()).Value.Trim();
                    if(isPrimary == "Y")
                    {
                        orgDesc.IsPrimary = true;
                    }
                    else
                    {
                        orgDesc.IsPrimary = false;
                    }
                }
                catch (Exception)
                {
                    // no value to parse
                }

                if(depth > 0)
                {
                    orgDesc.Parent = parent;
                }

                orgDesc.Depth = depth;

                if(parent != null)
                {
                    parent.Children.Add(orgDesc);
                }

                allPuzzlePieces.Add(orgDesc);
        }

            if (element.Elements(EnumOldServiceFieldsAsKeys.treeDepartment.ToString()).Count() > 0)
            {
                foreach (XElement child in element.Elements(EnumOldServiceFieldsAsKeys.treeDepartment.ToString()))
                {
                    PopulateTreeDescriptorsFromOld(child, orgDesc, allPuzzlePieces, depth + 1);
                }
            }
        }

        private static void PopulateGapsOfOldTree(HashSet<OrganizationTreeDescriptor> oldTree, HashSet<OrganizationTreeDescriptor> newTree, ResultReport resultReport, bool excludeMatchedElements, int previousCountMissingElements = -1)
        {
            int countUnmatchedChildrenOfMissingElement = 42;
            bool onlyOneOptionAvailable = false;
            bool matchingElementFound = false;
            OrganizationTreeDescriptor matchingElement;

            var missingElementsByDepth = oldTree.Where(x => x.HasBeenMatched == false
                                                && string.IsNullOrEmpty(x.ID)
                                                && string.IsNullOrEmpty(x.Name)
                                                && x.Depth >= 0)
                                   .GroupBy(x => x.Depth)
                                   .ToDictionary(t => t.Key, t => t.ToList());

            int countMissingElements = missingElementsByDepth.Count();

            Dictionary<int, List<OrganizationTreeDescriptor>> potentialMatchingElementsByDepth;

            if (excludeMatchedElements)
            {
                potentialMatchingElementsByDepth = newTree.Where(x => x.HasBeenMatched == false).GroupBy(g => g.Depth).ToDictionary(t => t.Key, t => t.ToList());
            }
            else
            {
                potentialMatchingElementsByDepth = newTree.GroupBy(g => g.Depth).ToDictionary(t => t.Key, t => t.ToList());
            }

            if (countMissingElements != previousCountMissingElements && countMissingElements > 0 && potentialMatchingElementsByDepth.Count() > 0)
            {
                foreach (var missingElementsPair in missingElementsByDepth)
                {
                    foreach (var missingElement in missingElementsPair.Value)
                    {
                        if (potentialMatchingElementsByDepth.ContainsKey(missingElementsPair.Key))
                        {
                            matchingElementFound = false;
                            matchingElement = null;

                            foreach (var potentialElement in potentialMatchingElementsByDepth[missingElementsPair.Key])
                            {
                                onlyOneOptionAvailable = potentialMatchingElementsByDepth[missingElementsPair.Key].Count() == 1;

                                if ((potentialElement.ParentId == missingElement.ParentId
                                    || string.IsNullOrEmpty(missingElement.ParentId))
                                    && potentialElement.Children.Count() >= missingElement.Children.Count())
                                {
                                    if (potentialElement.Children.Select(x => x.MatchedPartner).Count() > 0)
                                    {
                                        countUnmatchedChildrenOfMissingElement = missingElement.Children.Except(potentialElement.Children.Select(x => x.MatchedPartner).Union(potentialElement.Children)).Count();
                                    }
                                    else
                                    {
                                        countUnmatchedChildrenOfMissingElement = missingElement.Children.Select(y => y.ID).Except(potentialElement.Children.Select(x => x.ID)).Count();
                                    }

                                    if (countUnmatchedChildrenOfMissingElement == 0)
                                    {
                                        matchingElementFound = true;
                                        matchingElement = potentialElement;
                                    }
                                }

                                // if there is only one potential match, don't be picky
                                if (onlyOneOptionAvailable && !matchingElementFound)
                                {
                                    matchingElementFound = true;
                                    matchingElement = potentialElement;
                                    matchingElement.WasOnlyOption = true;
                                    resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.MATCHING_SINGLE_ELEMENT_GIVEN_DEPTH_MISMATCH);
                                    resultReport.UpdateResult(EnumResultSeverityType.WARNING);
                                }
                            }

                            if (matchingElementFound)
                            {
                                if (!excludeMatchedElements)
                                {
                                    matchingElement.UsedMoreThanOnce = true;
                                    resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.REUSED_ELEMENT_TO_FILL_GAP);
                                }

                                matchingElement.HasBeenMatched = true;
                                matchingElement.MatchedPartner = missingElement;
                                missingElement.HasBeenMatched = true;
                                missingElement.MatchedPartner = matchingElement;
                                matchingElement.IsImportedFromNewService = true;
                                missingElement.IsMissing = false;
                                matchingElement.IsMissing = false;

                                matchingElement.Children.Clear();
                                matchingElement.Parent = missingElement.Parent;

                                // replace in old tree
                                foreach (var childFromMissing in missingElement.Children)
                                {
                                    childFromMissing.Parent = matchingElement;
                                    matchingElement.Children.Add(childFromMissing);
                                }

                                if (missingElement.Parent != null && missingElement.Parent.Children != null)
                                {
                                    missingElement.Parent.Children.Add(matchingElement);
                                    missingElement.Parent.Children.Remove(missingElement);
                                }

                                oldTree.Add(matchingElement);
                                oldTree.Remove(missingElement);
                            }
                        }
                    }
                }

                PopulateGapsOfOldTree(oldTree, newTree, resultReport, excludeMatchedElements, countMissingElements);
            }
        }

        #endregion

        #region Single Tests

        private void UserGeneralInfo_Organization_IdAndNameTogether_Test(OrganizationTreeDescriptor oldTreeRoot, OrganizationTreeDescriptor newTreeRoot)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_Organizations_IdAndName, "Comparing Organization Id+Name Combinations");
            var compareStrategy = new CompareStrategyContextSwitcher(
                oldServiceOrganizationDescriptors,
                oldTreeRoot,
                newServiceOrganizationDescriptors,
                newTreeRoot,
                resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserGeneralInfo_Organization_Id_Test(HashSet<string> oldValues, HashSet<string> newValues)
        {
            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Organizations_Id, "Comparing Organization Ids", oldValues, newValues);
        }

        private void UserGeneralInfo_Organization_Name_Test(HashSet<string> oldValues, HashSet<string> newValues)
        {
            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Organizations_Name, "Comparing Organization Names", oldValues, newValues);
        }

        private void UserGeneralInfo_Organization_CheckTreeDepthCoherence_Test(OrganizationTreeDescriptor oldTreeRoot, OrganizationTreeDescriptor newTreeRoot)
        {
            bool keepGoing = true;
            int index = 0;
            IEnumerable<OrganizationTreeDescriptor> oldEntriesSameDepth;
            IEnumerable<OrganizationTreeDescriptor> newEntriesSameDepth;
            int oldCount;
            int newCount;

            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_Organizations_TreeDepthCoherence, "Comparing Organization Tree Depth Coherence");

            while (keepGoing)
            {
                oldEntriesSameDepth = this.oldServiceOrganizationDescriptors.Where(x => x.Depth == index);
                newEntriesSameDepth = this.newServiceOrganizationDescriptors.Where(s => s.Depth == index);

                oldCount = oldEntriesSameDepth.Count();
                newCount = newEntriesSameDepth.Count();

                if (oldCount == 0 && newCount == 0)
                {
                    keepGoing = false;
                }

                try
                {
                    // the only we can compare is that the old tree does not return more entries than the new one at a given depth of depth
                    // the new service may return more because it has enriched the old tree where some of the values may have been manually - ? - excluded
                    Assert.IsFalse(oldCount > newCount, "Comparing at depth index " + index);

                    resultReport.UpdateResult(EnumResultSeverityType.SUCCESS);
                }
                catch (AssertFailedException e)
                {
                    resultReport.UpdateResult(EnumResultSeverityType.ERROR);
                    resultReport.ErrorMessage = e.Message;
                    resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.OLD_TREE_HAS_MORE_CHILDREN_GIVEN_DEPTH);
                    resultReport.AddDetailedValues(this.oldServiceOrganizationDescriptors, oldTreeRoot, this.newServiceOrganizationDescriptors, newTreeRoot);
                    resultReport.TreeComparisonIndexError = index;

                    if (resultReport.Result == EnumResultSeverityType.ERROR)
                    {
                        keepGoing = false;
                    }
                }

                index++;
            }

            if(resultReport.Result == EnumResultSeverityType.SUCCESS)
            {
                resultReport.IdentifedDataBehaviors.Add(EnumIdentifiedDataBehavior.NEW_TREE_COUNT_CONSISTENT);
                resultReport.AddDetailedValues(this.oldServiceOrganizationDescriptors, oldTreeRoot, this.newServiceOrganizationDescriptors, newTreeRoot);
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserGeneralInfo_Organization_MergingNewTreeToOldOne_Test(OrganizationTreeDescriptor oldTreeRoot, OrganizationTreeDescriptor newTreeRoot)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport(this.UserId, this.Upi, EnumTestUnitNames.UserGeneralInfo_Organizations_TreeMerged, "Trying to merge Organization Trees together");

            if (newTreeRoot != null && oldTreeRoot != null)
            {

                var newTreeRootDeepClone = newTreeRoot.DeepClone();

                PopulateGapsOfOldTree(this.oldServiceOrganizationDescriptors, this.newServiceOrganizationDescriptors, resultReport, true);

                var missingElements = this.oldServiceOrganizationDescriptors.Where(x => x.HasBeenMatched == false
                                                                && string.IsNullOrEmpty(x.ID)
                                                                && string.IsNullOrEmpty(x.Name)
                                                                && x.Depth >= 0)
                                                   .GroupBy(x => x.Depth)
                                                   .ToDictionary(t => t.Key, t => t.ToList());
                int countMissingElements = missingElements.Count();

                if (countMissingElements == 0)
                {
                    resultReport.UpdateResult(EnumResultSeverityType.SUCCESS);
                }
                else
                {
                    //if (countMissingElements > 0)
                    //{
                    //    PopulateGapsOfOldTree(this.oldServiceOrganizationDescriptors, this.newServiceOrganizationDescriptors, resultReport, false);

                    //    missingElements = this.oldServiceOrganizationDescriptors.Where(x => x.HasBeenMatched == false
                    //                                                    && string.IsNullOrEmpty(x.ID)
                    //                                                    && string.IsNullOrEmpty(x.Name)
                    //                                                    && x.Depth >= 0)
                    //                                       .GroupBy(x => x.Depth)
                    //                                       .ToDictionary(t => t.Key, t => t.ToList());
                    //    countMissingElements = missingElements.Count();
                    //}

                    if (countMissingElements == 0)
                    {
                        resultReport.UpdateResult(EnumResultSeverityType.FALSE_POSITIVE);
                    }
                    else
                    {
                        resultReport.UpdateResult(EnumResultSeverityType.ERROR);
                    }
                }

                OrganizationTreeDescriptor root = null;

                try
                {
                    root = this.oldServiceOrganizationDescriptors.Where(x => x.Depth == 0).First();
                } catch (Exception)
                { }

                resultReport.AddDetailedValues(null, root, null, newTreeRootDeepClone);
            }
            else
            {
                resultReport.UpdateResult(EnumResultSeverityType.WARNING_NO_DATA);
            }


            watch.Stop();
            resultReport.Duration = watch.Elapsed;
            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(this.Container.BuildOldServiceFullURL(this.Upi),
                                              this.BuildNewServiceURL(this.PageName),
                                              resultReport);
        }

        private void UserGeneralInfo_Organization_Missions_Test()
        {
            var watch = new Stopwatch();
            watch.Start();
            var missionsPerOrg = new Dictionary<HashSet<string>, HashSet<string>>();
            
            foreach (var entry in this.newServiceOrganizationDescriptors)
            {
                if (entry.MatchedPartner != null && (entry.Missions.Count() > 0 || entry.MatchedPartner.Missions.Count() > 0))
                {
                    // matched partner is old service data
                    missionsPerOrg.Add(entry.MatchedPartner.Missions, entry.Missions);
                }
            }
            
            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Organizations_Missions, "Comparing Organization Missions", missionsPerOrg);
        }

        private void UserGeneralInfo_Organization_Type_Test()
        {
            var watch = new Stopwatch();
            watch.Start();
            var valuesPerOrg = new Dictionary<HashSet<string>, HashSet<string>>();

            foreach (var entry in this.newServiceOrganizationDescriptors)
            {
                // TODO: implement no data on old or new side
                if (entry.MatchedPartner != null)
                {
                    // matched partner is old service data
                    valuesPerOrg.Add(new HashSet<string>(){entry.MatchedPartner.Type}, new HashSet<string>() {entry.Type});
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Organizations_Type, "Comparing Organization Types", valuesPerOrg);
        }

        private void UserGeneralInfo_Organizations_YmgStatus_Test()
        {
            var watch = new Stopwatch();
            watch.Start();
            var valuesPerOrg = new Dictionary<HashSet<string>, HashSet<string>>();

            foreach (var entry in this.newServiceOrganizationDescriptors)
            {
                if (entry.MatchedPartner != null)
                {
                    // matched partner is old service data
                    valuesPerOrg.Add(new HashSet<string>() { entry.MatchedPartner.IsDisplayedOnYmg.ToString() }, new HashSet<string>() { entry.IsDisplayedOnYmg.ToString() });
                }
            }

            this.CompareAndLog_Test(EnumTestUnitNames.UserGeneralInfo_Organizations_YmgStatus, "Comparing Display On YMG Status", valuesPerOrg);
        }
    }

    #endregion
}