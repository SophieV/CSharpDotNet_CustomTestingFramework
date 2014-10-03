using System.Collections.Generic;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Compares values having the same key : whether they match or are both empty.
    /// </summary>
    public class ComparerStringWithKey : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys,StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            if (x != null)
            {
                foreach (var pairX in x)
                {
                    if (!pairX.Value.HasBeenMatched && y.ContainsKey(pairX.Key) && !y[pairX.Key].HasBeenMatched && pairX.Value.IsOld != y[pairX.Key].IsOld)
                    {
                        if (string.IsNullOrEmpty(pairX.Value.Value) && string.IsNullOrEmpty(y[pairX.Key].Value))
                        {
                            pairX.Value.HasBeenMatched = true;
                            y[pairX.Key].HasBeenMatched = true;
                        }
                        else if (pairX.Value.Value == y[pairX.Key].Value)
                        {
                            pairX.Value.HasBeenMatched = true;
                            y[pairX.Key].HasBeenMatched = true;
                        }
                    }
                }
            }

            if (y != null)
            {
                foreach (var pairY in y)
                {
                    if (!pairY.Value.HasBeenMatched && x.ContainsKey(pairY.Key) && !x[pairY.Key].HasBeenMatched && pairY.Value.IsOld != y[pairY.Key].IsOld)
                    {
                        if (string.IsNullOrEmpty(pairY.Value.Value) && string.IsNullOrEmpty(x[pairY.Key].Value))
                        {
                            pairY.Value.HasBeenMatched = true;
                            x[pairY.Key].HasBeenMatched = true;
                        }
                        else if (pairY.Value.Value == x[pairY.Key].Value)
                        {
                            pairY.Value.HasBeenMatched = true;
                            x[pairY.Key].HasBeenMatched = true;
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