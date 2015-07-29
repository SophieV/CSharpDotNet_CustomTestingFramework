using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    /// <summary>
    /// Compares values having the same key : whether they match if both sides are made case-insensitive.
    /// </summary>
    public class ComparerStringWithKeyNotCaseSensitive : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            if (x != null)
            {
                foreach (var pairX in x)
                {
                    if (!pairX.Value.HasBeenMatched && y.ContainsKey(pairX.Key) && !y[pairX.Key].HasBeenMatched
                        && !string.IsNullOrEmpty(pairX.Value.Value) && !string.IsNullOrEmpty(y[pairX.Key].Value)
                        && pairX.Value.IsOld != y[pairX.Key].IsOld)
                    {
                        if (pairX.Value.Value.ToLower() == y[pairX.Key].Value.ToLower())
                        {
                            pairX.Value.HasBeenMatched = true;
                            y[pairX.Key].HasBeenMatched = true;
                            pairX.Value.MatchedOnceCaseCorrected = true;
                            y[pairX.Key].MatchedOnceCaseCorrected = true;
                        }
                    }
                }
            }

            if (y != null)
            {
                foreach (var pairY in y)
                {
                    if (!pairY.Value.HasBeenMatched && x.ContainsKey(pairY.Key) && !x[pairY.Key].HasBeenMatched
                        && !string.IsNullOrEmpty(pairY.Value.Value) && !string.IsNullOrEmpty(x[pairY.Key].Value)
                        && pairY.Value.IsOld != y[pairY.Key].IsOld)
                    {
                        if (pairY.Value.Value.ToLower() == x[pairY.Key].Value.ToLower())
                        {
                            pairY.Value.HasBeenMatched = true;
                            x[pairY.Key].HasBeenMatched = true;
                            pairY.Value.MatchedOnceCaseCorrected = true;
                            x[pairY.Key].MatchedOnceCaseCorrected = true;
                        }
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
