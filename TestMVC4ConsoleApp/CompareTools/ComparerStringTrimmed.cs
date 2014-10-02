
using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public class ComparerStringTrimmed : IEqualityComparer<HashSet<StringDescriptor>>
    {
        bool IEqualityComparer<HashSet<StringDescriptor>>.Equals(HashSet<StringDescriptor> x, HashSet<StringDescriptor> y)
        {
            bool areEqual = false;
            foreach (var element in x)
            {
                var potentialMatch = y.Where(n => n.Value.Trim() == element.Value.Trim());

                if (potentialMatch.Count() == 1)
                {
                    potentialMatch.First().SingleValueHasBeenMatched = true;
                    potentialMatch.First().MismatchDueToTrailingSpaces = true;
                    element.SingleValueHasBeenMatched = true;
                    element.MismatchDueToTrailingSpaces = true;
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