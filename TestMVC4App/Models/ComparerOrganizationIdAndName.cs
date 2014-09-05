using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class ComparerOrganizationIdAndName : IEqualityComparer<OrganizationTreeDescriptor>
    {
        bool IEqualityComparer<OrganizationTreeDescriptor>.Equals(OrganizationTreeDescriptor x, OrganizationTreeDescriptor y)
        {
            bool areEqual = true;

            if (x.ID == y.ID && x.Name == y.Name)
            {
                areEqual = true;

                if (x.Depth == y.Depth)
                {
                    x.HasBeenMatched = true;
                    x.MatchedPartner = y;
                    y.HasBeenMatched = true;
                    y.MatchedPartner = x;
                }
            }
            else
            {
                areEqual = false;
            }

            return areEqual;
        }

        int IEqualityComparer<OrganizationTreeDescriptor>.GetHashCode(OrganizationTreeDescriptor obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}