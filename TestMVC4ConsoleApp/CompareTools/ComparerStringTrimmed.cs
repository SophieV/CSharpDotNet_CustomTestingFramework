
using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public class ComparerStringTrimmed : IEqualityComparer<StringDescriptor>
    {
        bool IEqualityComparer<StringDescriptor>.Equals(StringDescriptor x, StringDescriptor y)
        {
            if (!x.HasBeenMatched && !y.HasBeenMatched && x.IsOld != y.IsOld)
            {
                if (!string.IsNullOrEmpty(x.Value) && !string.IsNullOrEmpty(y.Value))
                {
                    if (x.Value == y.Value.Trim())
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        y.MismatchDueToTrailingSpaces = true;
                    }
                    else if (x.Value.Trim() == y.Value)
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        x.MismatchDueToTrailingSpaces = true;
                    }
                    else if (x.Value.Trim() == y.Value.Trim())
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        x.MismatchDueToTrailingSpaces = true;
                        y.MismatchDueToTrailingSpaces = true;
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