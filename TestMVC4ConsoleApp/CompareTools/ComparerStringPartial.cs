using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    /// <summary>
    /// Compares values : whether one contains the other.
    /// </summary>
    public class ComparerStringPartial : IEqualityComparer<StringDescriptor>
    {
        bool IEqualityComparer<StringDescriptor>.Equals(StringDescriptor x, StringDescriptor y)
        {
            if (!x.HasBeenMatched && !y.HasBeenMatched && x.IsOld != y.IsOld)
            {
                if (!string.IsNullOrEmpty(x.Value) && !string.IsNullOrEmpty(y.Value))
                {
                    if (x.Value.Length > 4 && y.Value.Contains(x.Value))
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        x.PartialMatchOnly = true;
                    }
                    else if (y.Value.Length > 4 && x.Value.Contains(y.Value))
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        y.PartialMatchOnly = true;
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
