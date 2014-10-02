using System.Collections.Generic;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Compares values : whether they match if one or both sides have trailing spaces removed.
    /// </summary>
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
                        y.MatchedOnceTrailingSpacesRemoved = true;
                    }
                    else if (x.Value.Trim() == y.Value)
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        x.MatchedOnceTrailingSpacesRemoved = true;
                    }
                    else if (x.Value.Trim() == y.Value.Trim())
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        x.MatchedOnceTrailingSpacesRemoved = true;
                        y.MatchedOnceTrailingSpacesRemoved = true;
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