using System.Collections.Generic;
using System.Linq;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringShifted : IEqualityComparer<HashSet<StringDescriptor>>
    {
        bool IEqualityComparer<HashSet<StringDescriptor>>.Equals(HashSet<StringDescriptor> x, HashSet<StringDescriptor> y)
        {
            bool areEqual = false;
            foreach (var element in x)
            {
                var potentialMatch = y.Where(n => !string.IsNullOrEmpty(element.Value) && !string.IsNullOrEmpty(element.Value)
                                                && ((n.Value.Contains(',') && element.Value == n.Value.Split(',')[1].Trim() + " " + n.Value.Split(',')[0].Trim())
                                                || (element.Value.Contains(',') && n.Value == element.Value.Split(',')[1].Trim() + " " + element.Value.Split(',')[0].Trim())));

                if (potentialMatch.Count() == 1)
                {
                    potentialMatch.First().SingleValueHasBeenMatched = true;
                    potentialMatch.First().MismatchDueToPartialName = true;
                    element.SingleValueHasBeenMatched = true;
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
