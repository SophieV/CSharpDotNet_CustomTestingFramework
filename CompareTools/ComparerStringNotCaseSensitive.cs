using System.Collections.Generic;
using TestMVC4App.Models;

/// <summary>
/// Compares values : whether they match if both sides are made-case insensitive.
/// </summary>
namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringNotCaseSensitive : IEqualityComparer<StringDescriptor>
    {
        bool IEqualityComparer<StringDescriptor>.Equals(StringDescriptor x, StringDescriptor y)
        {
            if (x != null && y != null)
            {
                if (!x.HasBeenMatched && !y.HasBeenMatched && x.IsOld != y.IsOld)
                {
                    if (!string.IsNullOrEmpty(x.Value) && !string.IsNullOrEmpty(y.Value))
                    {
                        if (y.Value.ToLower() == x.Value.ToLower())
                        {
                            x.HasBeenMatched = true;
                            y.HasBeenMatched = true;
                            y.MatchedOnceCaseCorrected = true;
                            x.MatchedOnceCaseCorrected = true;
                        }
                    }
                }
            }

            return false;
        }

        int IEqualityComparer<StringDescriptor>.GetHashCode(StringDescriptor obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}
