using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using YSM.PMS.Service.Common.DataTransfer;
using YSM.PMS.Web.Service.Clients;

namespace TestMVC4App.Models
{
    public class OrganizationTreeDescriptor
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public OrganizationTreeDescriptor Parent{ get; set; }

        public string ParentId { get; set; }
        public bool IsPrimary { get; set; }
        public List<OrganizationTreeDescriptor> Children { get; set; }
        public int Depth { get; set; }

        public OrganizationTreeDescriptor()
        {
            this.Children = new List<OrganizationTreeDescriptor>();
            // default value - for orphans - should not mess up the search at level index
            this.Depth = -1;
        }
    }

    public class OrganizationTestUnit : TestUnit
    {
        # region Data Provided by Parent Test Unit

        private IEnumerable<Organization> newServiceOrganizations = new List<Organization>();
        private IEnumerable<XElement> oldServiceDepartments;
        private IEnumerable<XElement> oldServiceTreeDepartments;
        private int userId;
        private int upi;

        public void ProvideOrganizationData(int userId,
                                            int upi,
                                            IEnumerable<XElement> oldServiceDepartments,
                                            IEnumerable<XElement> oldServiceTreeDepartments,
                                            IEnumerable<Organization> newServiceOrganizations)
        {
            this.userId = userId;
            this.upi = upi;

            if (newServiceOrganizations != null)
            {
                this.newServiceOrganizations = newServiceOrganizations;
            }

            if (oldServiceDepartments != null)
            {
                this.oldServiceDepartments = oldServiceDepartments;
            }

            if (oldServiceTreeDepartments != null)
            {
                this.oldServiceTreeDepartments = oldServiceTreeDepartments;
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

        public OrganizationTestUnit(TestSuite parent, TestUnit bigBrother) 
            : base (parent,bigBrother)
        {

        }

        protected override void RunAllSingleTests()
        {
            OrganizationTreeDescriptor oldTreeRootContainer;
            List<OrganizationTreeDescriptor> oldTreeOrganizations;
            OrganizationTreeDescriptor newTreeRootContainer;
            List<OrganizationTreeDescriptor> newTreeOrganizations;
        
            ParseOldServiceData(out oldTreeOrganizations, out oldTreeRootContainer);

            ParseNewServiceData(out newTreeOrganizations, out newTreeRootContainer);

            //Task[] tasks = new Task[2]
            //{
            //Task.Factory.StartNew(() => UserGeneralInfo_Organization_Id_Test(oldOrganizationIdValues, newOrganizationIdValues)),
            //Task.Factory.StartNew(() => UserGeneralInfo_Organization_Name_Test(oldOrganizationNameValues, newOrganizationNameValues))
            //};

            //Task.WaitAll(tasks);

            UserGeneralInfo_Organization_Id_Test(oldTreeOrganizations.Where(a => !string.IsNullOrEmpty(a.ID)).Select(a => a.ID).ToList(), 
                                                 newTreeOrganizations.Where(a => !string.IsNullOrEmpty(a.ID)).Select(a => a.ID).ToList());
            UserGeneralInfo_Organization_Name_Test(oldTreeOrganizations.Where(a => !string.IsNullOrEmpty(a.Name)).Select(a => a.Name).ToList(), 
                                                   newTreeOrganizations.Where(a => !string.IsNullOrEmpty(a.Name)).Select(a => a.Name).ToList());
            UserGeneralInfo_Organization_CheckTreeDepthCoherence_Test(oldTreeOrganizations, newTreeOrganizations);
            UserGeneralInfo_Organization_CheckIsPrimary_Test(oldTreeOrganizations, newTreeOrganizations);

            ComputeOverallSeverity();
        }

        #region Parsing Input Data

        private void ParseNewServiceData(out List<OrganizationTreeDescriptor> newTreeOrganizationElements,
                                         out OrganizationTreeDescriptor rootTreeContainerOrganization)
        {
            newTreeOrganizationElements = new List<OrganizationTreeDescriptor>();
            OrganizationTreeDescriptor organizationDescriptor;

            rootTreeContainerOrganization = new OrganizationTreeDescriptor();
            rootTreeContainerOrganization.Depth = -1;

            if (this.newServiceOrganizations.Count() > 0)
            {
                foreach (var organization in this.newServiceOrganizations)
                {
                    organizationDescriptor = new OrganizationTreeDescriptor()
                    {
                        ID = organization.OrganizationId.ToString(),
                        Name = organization.Name,
                        IsPrimary = organization.IsImported,
                        ParentId = organization.OrganizationParentId.ToString()
                    };

                    newTreeOrganizationElements.Add(organizationDescriptor);
                }

                newTreeOrganizationElements = LinkElementsOfOrganizationTree(newTreeOrganizationElements);

                var root = newTreeOrganizationElements.Where(z => z.Parent == null).First();
                rootTreeContainerOrganization.Children.Add(root);
                root.Parent = rootTreeContainerOrganization;

                AssignDepthProperty(root);
            }
        }

        private void AssignDepthProperty(OrganizationTreeDescriptor treeOrganizationElement, int depth = 0)
        {
            treeOrganizationElement.Depth = depth;

            foreach (var child in treeOrganizationElement.Children)
            {
                AssignDepthProperty(child, depth + 1);
            }
        }

        private void ParseOldServiceData(out List<OrganizationTreeDescriptor> oldTreeOrganizationElements,
                                         out OrganizationTreeDescriptor rootTreeContainerOrganization)
        {
            rootTreeContainerOrganization = new OrganizationTreeDescriptor();
            oldTreeOrganizationElements = new List<OrganizationTreeDescriptor>();
            rootTreeContainerOrganization.Depth = -1;

            string parsedOrgId = string.Empty;
            string parsedOrgName = string.Empty;
            bool parsedIsPrimary = false;

            // first get the tree and its elements
            try
            {
                foreach (XElement el in oldServiceTreeDepartments)
                {
                    try
                    {
                        ParseRecursiveTree(el, rootTreeContainerOrganization, oldTreeOrganizationElements);
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

            // then try to complete the info about the elements of the tree
            // or add the elements separately to the list if not be found
            try
            {
                foreach (XElement el in oldServiceDepartments)
                {
                    try
                    {
                        parsedOrgId = el.Element("OrgID").Value;
                    }
                    catch (Exception)
                    {
                        // no value to parse
                        parsedOrgId = string.Empty;
                    }

                    try
                    {
                        parsedOrgName = el.Element("departmentName").Value;
                    }
                    catch (Exception)
                    {
                        // no value to parse
                        parsedOrgName = string.Empty;
                    }

                    try
                    {
                        string isPrimary = el.Element("primaryDept").Value.Trim();
                        if (isPrimary == "Y")
                        {
                             parsedIsPrimary = true;
                        }
                        else
                        {
                            parsedIsPrimary = false;
                        }
                    }
                    catch (Exception)
                    {
                        // no value to parse
                        parsedIsPrimary = false;
                    }

                    IEnumerable<OrganizationTreeDescriptor> findOrganizationResults = null;
                    if (!string.IsNullOrWhiteSpace(parsedOrgId))
                    {
                        findOrganizationResults = oldTreeOrganizationElements.Where(i => i.ID == parsedOrgId);
                    }

                    if (findOrganizationResults == null || findOrganizationResults.Count() == 0)
                    {
                        findOrganizationResults = oldTreeOrganizationElements.Where(i => i.Name == parsedOrgName);
                    }

                        if(findOrganizationResults.Count() == 0)
                        {
                            // was not found, create new one
                            var organizationDescriptor = new OrganizationTreeDescriptor() { ID = parsedOrgId, Name = parsedOrgName, IsPrimary = parsedIsPrimary };
                            oldTreeOrganizationElements.Add(organizationDescriptor);
                        }
                        else if (findOrganizationResults.Count() == 1)
                        {
                            var organizationDescriptor = findOrganizationResults.First();

                            if (string.IsNullOrWhiteSpace(organizationDescriptor.ID))
                            {
                                organizationDescriptor.ID = parsedOrgId;
                            }

                            if (string.IsNullOrWhiteSpace(organizationDescriptor.Name))
                            {
                                organizationDescriptor.Name = parsedOrgName;
                            }

                            if (!organizationDescriptor.IsPrimary)
                            {
                                organizationDescriptor.IsPrimary = parsedIsPrimary;
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("More than one results returned for the unique organization. PROBLEM !");
                        }
                }
            }
            catch (Exception)
            {
                // there is no existing attribute to parse
            }
        }

        private static void ParseRecursiveTree(XElement element, OrganizationTreeDescriptor parent, List<OrganizationTreeDescriptor> allPuzzlePieces, int depth = 0)
        {
            var orgDesc = new OrganizationTreeDescriptor();

            try
            {
                orgDesc.ID = element.Element("orgID").Value.Trim();
            }
            catch (Exception)
            {
                // no value to parse
            }

            try
            {
                orgDesc.Name = element.Element("name").Value.Trim();
            }
            catch (Exception)
            {
                // no value to parse
            }

            try
            {
                string isPrimary = element.Element("primaryDept").Value.Trim();
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

            if (element.Elements("treeDepartment").Count() > 0)
            {
                foreach (XElement child in element.Elements("treeDepartment"))
                {
                    ParseRecursiveTree(child, orgDesc, allPuzzlePieces, depth + 1);
                }
            }
        }

        private static List<OrganizationTreeDescriptor> LinkElementsOfOrganizationTree(List<OrganizationTreeDescriptor> puzzlePieces)
        {
            OrganizationTreeDescriptor potentialParent;
            var puzzlePiecesNew = new List<OrganizationTreeDescriptor>();
            puzzlePiecesNew.AddRange(puzzlePieces);

            // get list of orphans
            List<OrganizationTreeDescriptor> orphans = new List<OrganizationTreeDescriptor>();

            // try to stitch pieces together
            foreach (var piece in puzzlePieces)
            {
                if (piece.Parent == null && !string.IsNullOrEmpty(piece.ParentId))
                {
                    try
                    {
                        potentialParent = puzzlePieces.Where(x => x.ID == piece.ParentId).First();
                    }
                    catch (Exception)
                    {
                        potentialParent = null;
                    }

                    if (potentialParent != null)
                    {
                        piece.Parent = potentialParent;
                        potentialParent.Children.Add(piece);
                    }
                    else
                    {
                        orphans.Add(piece);
                    }
                }
            }

            if (orphans.Count() > 0)
            {
                var organizationsClient = new OrganizationsClient();
                Organization downloadedParent;
                OrganizationTreeDescriptor downloadedParentDescriptor;

                foreach (string orphanParentId in orphans.Select(x=>x.ParentId).Distinct())
                {
                    System.Diagnostics.Debug.WriteLine("can't find parent " + orphanParentId);
                    downloadedParent = organizationsClient.GetOrganization(int.Parse(orphanParentId));
                    downloadedParentDescriptor = new OrganizationTreeDescriptor()
                    {
                        ID = downloadedParent.OrganizationId.ToString(),
                        Name = downloadedParent.Name,
                        IsPrimary = downloadedParent.IsImported,
                        ParentId = downloadedParent.OrganizationParentId.ToString(),
                        Parent = null
                    };
                    puzzlePiecesNew.Add(downloadedParentDescriptor);
                }

                LinkElementsOfOrganizationTree(puzzlePiecesNew);
            }

            return puzzlePiecesNew;
        }

        #endregion

        #region Single Tests

        private void UserGeneralInfo_Organization_Id_Test(List<string> oldValues, List<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_Organization_Id_Test", "Comparing Organization Ids");
            var compareStrategy = new SimpleCollectionCompareStrategy(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(userId),
                                              resultReport);
        }

        private void UserGeneralInfo_Organization_Name_Test(List<string> oldValues, List<string> newValues)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_Organization_Name_Test", "Comparing Organization Names");
            var compareStrategy = new SimpleCollectionCompareStrategy(oldValues, newValues, resultReport);
            compareStrategy.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(userId),
                                              resultReport);
        }

        private void UserGeneralInfo_Organization_CheckTreeDepthCoherence_Test(List<OrganizationTreeDescriptor> oldTree, List<OrganizationTreeDescriptor> newTree)
        {
            bool keepGoing = true;
            int index = 0;
            IEnumerable<OrganizationTreeDescriptor> oldEntriesSameLevel;
            IEnumerable<OrganizationTreeDescriptor> newEntriesSameLevel;
            int oldCount;
            int newCount;

            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_Organization_CheckTreeDepthCoherence_Test", "Comparing Organization Tree Depth Coherence");

            while (keepGoing)
            {
                oldEntriesSameLevel = oldTree.Where(x => x.Depth == index);
                newEntriesSameLevel = newTree.Where(s => s.Depth == index);

                oldCount = oldEntriesSameLevel.Count();
                newCount = newEntriesSameLevel.Count();

                if (oldCount == 0 && newCount == 0)
                {
                    keepGoing = false;
                }

                try
                {
                    // the only we can compare is that the old tree does not return more entries than the new one at a given level of depth
                    // the new service may return more because it has enriched the old tree where some of the values may have been manually - ? - excluded
                    Assert.IsFalse(oldCount > newCount, "Comparing at level index " + index);

                    resultReport.UpdateResult(ResultSeverityType.SUCCESS);
                }
                catch (AssertFailedException e)
                {
                    resultReport.UpdateResult(ResultSeverityType.ERROR);
                    resultReport.ErrorMessage = e.Message;
                    resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.OLD_TREE_HAS_MORE_CHILDREN);
                    resultReport.AddDetailedValues(oldTree.OrderBy(x => x.Depth).Select(x => (x.Depth == -1 ? "[UNKNOWN]" : "[L" + x.Depth + "]") + " - " + x.Name + " (" + x.ID + ")").ToList(),
                                                   newTree.OrderBy(x => x.Depth).Select(x => (x.Depth == -1 ? "[UNKNOWN]" : "[L" + x.Depth + "]") + " - " + x.Name + " (" + x.ID + ")").ToList());

                    if (resultReport.Result == ResultSeverityType.ERROR)
                    {
                        keepGoing = false;
                    }
                }

                index++;
            }

            if(resultReport.Result == ResultSeverityType.SUCCESS)
            {
                resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.NEW_TREE_COUNT_CONSISTENT);
                resultReport.AddDetailedValues(oldTree.OrderBy(x => x.Depth).Select(x => (x.Depth == -1 ? "[UNKNOWN]" : "[L" + x.Depth + "]") + " - " + x.Name + " (" + x.ID + ")").ToList(),
                                               newTree.OrderBy(x => x.Depth).Select(x => (x.Depth == -1 ? "[UNKNOWN]" : "[L" + x.Depth + "]") + " - " + x.Name + " (" + x.ID + ")").ToList());
            }

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(userId),
                                              resultReport);
        }

        private void UserGeneralInfo_Organization_CheckIsPrimary_Test(List<OrganizationTreeDescriptor> oldTree, List<OrganizationTreeDescriptor> newTree)
        {
            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_Organization_CheckIsPrimary_Test", "Comparing Organization IsImported/Primary");

            var oldEntriesIsPrimary = oldTree.Where(x => x.IsPrimary == true).Select(x=>x.Name).ToList();
            var newEntriesIsPrimary = newTree.Where(s => s.IsPrimary == true).Select(x => x.Name).ToList();

            // this comparison can work because all the primary departments have a name assigned in the old service !
            var collectionComparison = new SimpleCollectionCompareStrategy(oldEntriesIsPrimary, newEntriesIsPrimary, resultReport);
            collectionComparison.Investigate();

            watch.Stop();
            resultReport.Duration = watch.Elapsed;

            this.DetailedResults.Add(resultReport);

            LogManager.Instance.LogTestResult(userId,
                                              upi,
                                              this.Master.BuildOldServiceFullURL(upi),
                                              this.BuildNewServiceFullURL(userId),
                                              resultReport);
        }
    }

    #endregion
}