using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class OrganizationTreeDescriptor
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public OrganizationTreeDescriptor Parent { get; set; }

        public string ParentId { get; set; }
        public bool IsPrimary { get; set; }
        public List<OrganizationTreeDescriptor> Children { get; set; }
        public int Depth { get; set; }

        public bool IsDuplicate { get; set; }

        public bool IsMissing { get; set; }

        public bool HasBeenMatched { get; set; }

        public bool IsImportedFromNewService { get; set; }

        public OrganizationTreeDescriptor()
        {
            this.Children = new List<OrganizationTreeDescriptor>();
            // default value - for orphans - should not mess up the search at level index
            this.Depth = -1;
            this.HasBeenMatched = false;
        }
    }
}