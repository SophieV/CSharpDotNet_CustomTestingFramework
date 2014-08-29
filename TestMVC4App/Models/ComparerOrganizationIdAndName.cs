using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class ComparerOrganizationIdAndName : IEqualityComparer<OrganizationTreeDescriptor>
    {
        bool IEqualityComparer<OrganizationTreeDescriptor>.Equals(OrganizationTreeDescriptor x, OrganizationTreeDescriptor y)
        {
            bool areEqual = true;

            if (x.ID != y.ID|| x.Name != y.Name)
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