
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Compares values having the same key : whether they match if one or both sides have trailing spaces removed.
    /// </summary>
    public class ComparerStringWithKeyTrimmed : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            foreach (var pairX in x)
            {
                if (!pairX.Value.HasBeenMatched && y.ContainsKey(pairX.Key) && !y[pairX.Key].HasBeenMatched 
                    && !string.IsNullOrEmpty(pairX.Value.Value) && !string.IsNullOrEmpty(y[pairX.Key].Value)
                    && pairX.Value.IsOld != y[pairX.Key].IsOld)
                {
                    if (pairX.Value.Value.Trim() == y[pairX.Key].Value)
                    {
                        pairX.Value.HasBeenMatched = true;
                        y[pairX.Key].HasBeenMatched = true;
                        pairX.Value.MatchedOnceTrailingSpacesRemoved = true;
                    }
                    else if (pairX.Value.Value == y[pairX.Key].Value.Trim())
                    {
                        pairX.Value.HasBeenMatched = true;
                        y[pairX.Key].HasBeenMatched = true;
                        y[pairX.Key].MatchedOnceTrailingSpacesRemoved = true;
                    }
                    else if (pairX.Value.Value.Trim() == y[pairX.Key].Value.Trim())
                    {
                        pairX.Value.HasBeenMatched = true;
                        y[pairX.Key].HasBeenMatched = true;
                        pairX.Value.MatchedOnceTrailingSpacesRemoved = true;
                        y[pairX.Key].MatchedOnceTrailingSpacesRemoved = true;
                    }
                }
            }

            foreach (var pairY in y)
            {
                if (!pairY.Value.HasBeenMatched && x.ContainsKey(pairY.Key) && !x[pairY.Key].HasBeenMatched
                    && !string.IsNullOrEmpty(pairY.Value.Value) && !string.IsNullOrEmpty(x[pairY.Key].Value)
                    && pairY.Value.IsOld != y[pairY.Key].IsOld)
                {
                    if (pairY.Value.Value.ToLower() == x[pairY.Key].Value.Trim())
                    {
                        pairY.Value.HasBeenMatched = true;
                        x[pairY.Key].HasBeenMatched = true;
                        pairY.Value.MatchedOnceTrailingSpacesRemoved = true;
                    }
                    else if (pairY.Value.Value == x[pairY.Key].Value.Trim())
                    {
                        pairY.Value.HasBeenMatched = true;
                        x[pairY.Key].HasBeenMatched = true;
                        x[pairY.Key].MatchedOnceTrailingSpacesRemoved = true;
                    }
                    else if (pairY.Value.Value.Trim() == x[pairY.Key].Value.Trim())
                    {
                        pairY.Value.HasBeenMatched = true;
                        x[pairY.Key].HasBeenMatched = true;
                        pairY.Value.MatchedOnceTrailingSpacesRemoved = true;
                        x[pairY.Key].MatchedOnceTrailingSpacesRemoved = true;
                    }
                }
            }

            return false;
        }

        int IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.GetHashCode(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> obj)
        {
            return obj.Values.ToString().ToLower().GetHashCode();
        }
    }
}