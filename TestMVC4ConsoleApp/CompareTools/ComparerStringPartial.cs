using System.Collections.Generic;
using System.Linq;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringPartial : IEqualityComparer<HashSet<StringDescriptor>>
    {
        bool IEqualityComparer<HashSet<StringDescriptor>>.Equals(HashSet<StringDescriptor> x, HashSet<StringDescriptor> y)
        {
            bool areEqual = false;
            foreach (var element in x)
            {
                var potentialMatch = y.Where(n => !string.IsNullOrEmpty(element.Value) && !string.IsNullOrEmpty(element.Value)
                                                && ((n.Value.Length > 9 && n.Value.Contains(element.Value))
                                                ||(element.Value.Length > 9 && element.Value.Contains(n.Value))));

                if (potentialMatch.Count() == 1)
                {
                    potentialMatch.First().HasBeenMatched = true;
                    potentialMatch.First().MismatchDueToPartialName = true;
                    element.HasBeenMatched = true;
                    element.MismatchDueToPartialName = true;
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
