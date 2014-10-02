using System.Collections.Generic;
using System.Linq;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringNotCaseSensitive : IEqualityComparer<StringDescriptor>
    {
        bool IEqualityComparer<StringDescriptor>.Equals(StringDescriptor x, StringDescriptor y)
        {
            if (!x.HasBeenMatched && !y.HasBeenMatched && x.IsOld != y.IsOld)
            {
                if (!string.IsNullOrEmpty(x.Value) && !string.IsNullOrEmpty(y.Value))
                {
                    if (y.Value.ToLower() == x.Value)
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        y.MismatchDueToCase = true;
                    }
                    else if (y.Value == x.Value.ToLower())
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        x.MismatchDueToCase = true;
                    }
                    else if (y.Value.ToLower() == x.Value.ToLower())
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        y.MismatchDueToCase = true;
                        x.MismatchDueToCase = true;
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
