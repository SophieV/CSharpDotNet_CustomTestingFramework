using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class ComparerOrganizationIdOrName : IEqualityComparer<OrganizationTreeDescriptor>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Data from new service. Or vice versa !</param>
        /// <param name="y">Data from old service. Or vice versa !</param>
        /// <returns></returns>
        bool IEqualityComparer<OrganizationTreeDescriptor>.Equals(OrganizationTreeDescriptor x, OrganizationTreeDescriptor y)
        {
            bool areEqual = false;

            // if IDs match
            if (x.ID == y.ID)
            {
                areEqual = true;

                if (x.Depth == y.Depth)
                {
                    x.HasBeenMatched = true;
                    y.HasBeenMatched = true;
                }
            }

            // if no ID on the old side and names match
            if ((string.IsNullOrEmpty(y.ID) && x.Name == y.Name) || (string.IsNullOrEmpty(x.ID) && x.Name == y.Name))
            {
                areEqual = true;

                if (x.Depth == y.Depth)
                {
                    x.HasBeenMatched = true;
                    y.HasBeenMatched = true;
                }
            }

            return areEqual;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks>Implementations are required to ensure that if the Equals method returns true for two objects x and y, then the value returned by the GetHashCode method for x must equal the value returned for y.</remarks>
        int IEqualityComparer<OrganizationTreeDescriptor>.GetHashCode(OrganizationTreeDescriptor obj)
        {
            return obj.Name.ToLower().GetHashCode();
        }
    }
}