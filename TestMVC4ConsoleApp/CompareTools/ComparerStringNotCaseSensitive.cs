using System.Collections.Generic;
using System.Linq;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringNotCaseSensitive : IEqualityComparer<HashSet<StringDescriptor>>
    {
        bool IEqualityComparer<HashSet<StringDescriptor>>.Equals(HashSet<StringDescriptor> x, HashSet<StringDescriptor> y)
        {
            bool areEqual = false;
            foreach (var element in x)
            {
                var potentialMatch = y.Where(n => n.Value.ToLowerInvariant() == element.Value.ToLowerInvariant());

                if (potentialMatch.Count() == 1)
                {
                    potentialMatch.First().HasBeenMatched = true;
                    potentialMatch.First().MismatchDueToCase = true;
                    element.HasBeenMatched = true;
                    element.MismatchDueToCase = true;
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
