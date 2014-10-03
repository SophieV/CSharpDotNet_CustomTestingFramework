using System.Collections.Generic;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Compares values : whether they match or are both empty.
    /// </summary>
    public class ComparerString : IEqualityComparer<StringDescriptor>
    {
        bool IEqualityComparer<StringDescriptor>.Equals(StringDescriptor x, StringDescriptor y)
        {
            if (x != null && y != null)
            {
                if (!x.HasBeenMatched && !y.HasBeenMatched && x.IsOld != y.IsOld)
                {
                    if (string.IsNullOrEmpty(x.Value) && string.IsNullOrEmpty(y.Value))
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                    }
                    else if (x.Value == y.Value)
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
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