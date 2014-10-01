using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public class ComparerString : IEqualityComparer<HashSet<StringDescriptor>>
    {
        bool IEqualityComparer<HashSet<StringDescriptor>>.Equals(HashSet<StringDescriptor> x, HashSet<StringDescriptor> y)
        {
            bool areEqual = false;
            foreach( var element in x)
            {
                var potentialMatch = y.Where(n => n.Value == element.Value);

                if(potentialMatch.Count() == 1)
                {
                    areEqual = true;
                    potentialMatch.First().HasBeenMatched = true;
                    element.HasBeenMatched = true;
                }
            }

            return areEqual;
        }

        int IEqualityComparer<HashSet<StringDescriptor>>.GetHashCode(HashSet<StringDescriptor> obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}