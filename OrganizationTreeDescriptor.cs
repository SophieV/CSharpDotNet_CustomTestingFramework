using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public class OrganizationTreeDescriptor
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public OrganizationTreeDescriptor Parent { get; set; }

        public string ParentId { get; set; }
        public bool IsPrimary { get; set; }
        public HashSet<OrganizationTreeDescriptor> Children { get; set; }
        public int Depth { get; set; }

        public bool IsDuplicate { get; set; }

        public bool IsMissing { get; set; }

        public bool HasBeenMatched { get; set; }

        public bool UsedMoreThanOnce { get; set; }

        public bool WasOnlyOption { get; set; }

        public OrganizationTreeDescriptor MatchedPartner { get; set; }

        public bool IsImportedFromNewService { get; set; }

        public bool IsDisplayedOnYmg { get; set; }

        public string Type { get; set; }

        public HashSet<string> Missions { get; set; }

        public string FacultyType { get; set; }

        public OrganizationTreeDescriptor()
        {
            this.Children = new HashSet<OrganizationTreeDescriptor>();
            this.Missions = new HashSet<string>();
            // default value - for orphans - should not mess up the search at level index
            this.Depth = -1;
            this.HasBeenMatched = false;
        }

        /// <summary>
        /// Copies an element including its children (recursive).
        /// </summary>
        /// <returns></returns>
        public OrganizationTreeDescriptor DeepClone()
        {
            var copy = (OrganizationTreeDescriptor)MemberwiseClone();

            // deep copy of children
            copy.Children = new HashSet<OrganizationTreeDescriptor>(Children.ToList().Select(x => x.DeepClone()));
            return copy;
        }
    }
}