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
    public class OrganizationDescriptor
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public OrganizationDescriptor Parent{ get; set; }

        public string ParentId { get; set; }
        public bool IsPrimary { get; set; }
        public List<OrganizationDescriptor> Children { get; set; }
        public int Depth { get; set; }

        public OrganizationDescriptor()
        {
            this.Children = new List<OrganizationDescriptor>();
        }
    }

    public class OrganizationTestUnit : TestUnit
    {
        private IEnumerable<Organization> newServiceOrganizations = new List<Organization>();
        private IEnumerable<XElement> oldServiceDepartments;
        private IEnumerable<XElement> oldServiceTreeDepartments;

        private int userId;
        private int upi;

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

            if(oldServiceDepartments != null)
            {
                this.oldServiceDepartments = oldServiceDepartments;
            }

            if (oldServiceTreeDepartments != null)
            {
                this.oldServiceTreeDepartments = oldServiceTreeDepartments;
            }
        }

        protected override void RunAllSingleTests()
        {
            List<string> oldOrganizationIdValues;
            List<string> oldOrganizationNameValues;
            OrganizationDescriptor rootOldDescriptor;
            List<OrganizationDescriptor> oldDescriptors;
        
            ParseOldServiceData(out oldOrganizationIdValues, out oldOrganizationNameValues, out oldDescriptors, out rootOldDescriptor);

            List<string> newOrganizationIdValues;
            List<string> newOrganizationNameValues;
            OrganizationDescriptor rootNewDescriptor;
            List<OrganizationDescriptor> newDescriptors;
            ParseNewServiceData(out newOrganizationIdValues, out newOrganizationNameValues, out newDescriptors, out rootNewDescriptor);

            //Task[] tasks = new Task[2]
            //{
            //Task.Factory.StartNew(() => UserGeneralInfo_Organization_Id_Test(oldOrganizationIdValues, newOrganizationIdValues)),
            //Task.Factory.StartNew(() => UserGeneralInfo_Organization_Name_Test(oldOrganizationNameValues, newOrganizationNameValues))
            //};

            //Task.WaitAll(tasks);

            UserGeneralInfo_Organization_Id_Test(oldOrganizationIdValues, newOrganizationIdValues);
            UserGeneralInfo_Organization_Name_Test(oldOrganizationNameValues, newOrganizationNameValues);
            UserGeneralInfo_Organization_CheckTreeDepthCoherence_Test(oldDescriptors, newDescriptors);
            UserGeneralInfo_Organization_CheckIsPrimary_Test(oldDescriptors, newDescriptors);

            ComputeOverallSeverity();
        }

        private void ParseNewServiceData(out List<string> newOrganizationIdValues, 
                                         out List<string> newOrganizationNameValues,
                                         out List<OrganizationDescriptor> newOrganizationDescriptors,
                                         out OrganizationDescriptor rootContainerOrganizationDescriptors)
        {
            newOrganizationIdValues = new List<string>();
            newOrganizationNameValues = new List<string>();

            newOrganizationDescriptors = new List<OrganizationDescriptor>();
            OrganizationDescriptor organizationDescriptor;

            rootContainerOrganizationDescriptors = new OrganizationDescriptor();
            rootContainerOrganizationDescriptors.Depth = -1;

            if (this.newServiceOrganizations.Count() > 0)
            {
                foreach (var organization in this.newServiceOrganizations)
                {
                    newOrganizationIdValues.Add(organization.OrganizationId.ToString());
                    newOrganizationNameValues.Add(organization.Name);

                    organizationDescriptor = new OrganizationDescriptor()
                    {
                        ID = organization.OrganizationId.ToString(),
                        Name = organization.Name,
                        IsPrimary = organization.IsImported,
                        ParentId = organization.OrganizationParentId.ToString()
                    };

                    newOrganizationDescriptors.Add(organizationDescriptor);
                }

                CreateOrganizationTree(newOrganizationDescriptors);

                var root = newOrganizationDescriptors.Where(z => z.Parent == null).First();
                rootContainerOrganizationDescriptors.Children.Add(root);
                root.Parent = rootContainerOrganizationDescriptors;

                AssignDepthProperty(root);
            }
        }

        private void AssignDepthProperty(OrganizationDescriptor org, int depth = 0)
        {
            org.Depth = depth;
            foreach (var child in org.Children)
            {
                AssignDepthProperty(child, depth + 1);
            }
        }

        private void ParseOldServiceData(out List<string> oldOrganizationIdValues, 
                                         out List<string> oldOrganizationNameValues,
                                         out List<OrganizationDescriptor> oldOrganizationDescriptors,
                                         out OrganizationDescriptor rootContainerOrganizationDescriptors)
        {
            oldOrganizationIdValues = new List<string>();
            oldOrganizationNameValues = new List<string>();
            rootContainerOrganizationDescriptors = new OrganizationDescriptor();
            oldOrganizationDescriptors = new List<OrganizationDescriptor>();
            rootContainerOrganizationDescriptors.Depth = -1;

            try
            {
                //oldServiceXMLContent.XPathSelectElements("/Faculty/facultyMember/department");

                foreach (XElement el in oldServiceDepartments)
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

            try
            {
                foreach (XElement el in oldServiceTreeDepartments)
                {
                    try
                    {
                        ProcessRecursiveTree(el, rootContainerOrganizationDescriptors, oldOrganizationDescriptors);
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
        }

        private void ProcessRecursiveTree(XElement element, OrganizationDescriptor parent, List<OrganizationDescriptor> oldOrganizationDescriptors, int depth = 0)
        {
            var orgDesc = new OrganizationDescriptor();

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

            oldOrganizationDescriptors.Add(orgDesc);

            if (element.Elements("treeDepartment").Count() > 0)
            {
                foreach (XElement child in element.Elements("treeDepartment"))
                {
                    ProcessRecursiveTree(child, orgDesc, oldOrganizationDescriptors, depth + 1);
                }
            }
        }

        private void CreateOrganizationTree (List<OrganizationDescriptor> puzzlePieces)
        {
            OrganizationDescriptor potentialParent;
            var puzzlePiecesNew = new List<OrganizationDescriptor>();
            puzzlePiecesNew.AddRange(puzzlePieces);

            // get list of orphans
            List<OrganizationDescriptor> orphans = new List<OrganizationDescriptor>();

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
                OrganizationDescriptor downloadedParentDescriptor;

                foreach (OrganizationDescriptor orphan in orphans)
                {
                    System.Diagnostics.Debug.WriteLine(orphan.ID + " can't find parent " + orphan.ParentId);
                    downloadedParent = organizationsClient.GetOrganization(int.Parse(orphan.ParentId));
                    downloadedParentDescriptor = new OrganizationDescriptor()
                    {
                        ID = downloadedParent.OrganizationId.ToString(),
                        Name = downloadedParent.Name,
                        IsPrimary = downloadedParent.IsImported,
                        ParentId = downloadedParent.OrganizationParentId.ToString()
                    };
                    puzzlePiecesNew.Add(downloadedParentDescriptor);
                }

                CreateOrganizationTree(puzzlePiecesNew);
            }
        }

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

        private void UserGeneralInfo_Organization_CheckTreeDepthCoherence_Test(List<OrganizationDescriptor> oldTree, List<OrganizationDescriptor> newTree)
        {
            bool keepGoing = true;
            int index = 0;
            IEnumerable<OrganizationDescriptor> oldEntriesSameLevel;
            IEnumerable<OrganizationDescriptor> newEntriesSameLevel;
            int oldCount;
            int newCount;

            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_Organization_CheckTreeDepthCoherence_Test", "Comparing Organization Tree Depth");

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
                    // cannot be compared equal number because the new tree may contain more entries - because some where manually excluded from the old one
                    Assert.IsTrue(oldCount >= newCount, "Comparing level index " + index);
                } 
                catch (AssertFailedException e)
                {
                    resultReport.UpdateResult(ResultSeverityType.ERROR);
                    resultReport.ErrorMessage = e.Message;
                    resultReport.IdentifedDataBehaviors.Add(IdentifiedDataBehavior.OLD_TREE_HAS_MORE_CHILDREN);
                    resultReport.OldValues.AddRange(oldEntriesSameLevel.Select(x=>x.ID));
                    resultReport.NewValues.AddRange(newEntriesSameLevel.Select(x => x.ID));
                }

                index++;
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

    private void UserGeneralInfo_Organization_CheckIsPrimary_Test(List<OrganizationDescriptor> oldTree, List<OrganizationDescriptor> newTree)
        {
        // TODO : NO, needs to be based on departments outside of TREE.
            IEnumerable<OrganizationDescriptor> oldEntriesIsPrimary;
            IEnumerable<OrganizationDescriptor> newEntriesIsPrimary;
            int oldCount;
            int newCount;

            var watch = new Stopwatch();
            watch.Start();
            var resultReport = new ResultReport("UserGeneralInfo_Organization_CheckIsPrimary_Test", "Comparing Organization IsImported/Primary");

                oldEntriesIsPrimary = oldTree.Where(x => x.IsPrimary == true);
                newEntriesIsPrimary = newTree.Where(s => s.IsPrimary == true);

                oldCount = oldEntriesIsPrimary.Count();
                newCount = newEntriesIsPrimary.Count();

                try
                {
                    Assert.AreEqual(oldCount,newCount);
                } 
                catch (AssertFailedException e)
                {
                    resultReport.UpdateResult(ResultSeverityType.ERROR);
                    resultReport.ErrorMessage = e.Message;
                    resultReport.OldValues.AddRange(oldEntriesIsPrimary.Select(x=>x.ID));
                    resultReport.NewValues.AddRange(newEntriesIsPrimary.Select(x => x.ID));
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
    }
}