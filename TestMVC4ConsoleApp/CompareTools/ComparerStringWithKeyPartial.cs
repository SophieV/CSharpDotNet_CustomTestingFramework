using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    /// <summary>
    /// Compares values having the same key : whether one contains the other.
    /// </summary>
    public class ComparerStringWithKeyPartial : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
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
                        if (pairX.Value.Value.Length > 4 && pairX.Value.Value.Trim().Contains(y[pairX.Key].Value.Trim()))
                        {
                            pairX.Value.HasBeenMatched = true;
                            y[pairX.Key].HasBeenMatched = true;
                            y[pairX.Key].PartialMatchOnly = true;
                        }
                        else if (y[pairX.Key].Value.Length > 4 && y[pairX.Key].Value.Trim().Contains(pairX.Value.Value.Trim()))
                        {
                            pairX.Value.HasBeenMatched = true;
                            y[pairX.Key].HasBeenMatched = true;
                            pairX.Value.PartialMatchOnly = true;
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
                        if (pairY.Value.Value.Length > 4 && pairY.Value.Value.Trim().Contains(x[pairY.Key].Value.Trim()))
                        {
                            pairY.Value.HasBeenMatched = true;
                            x[pairY.Key].HasBeenMatched = true;
                            x[pairY.Key].PartialMatchOnly = true;
                        }
                        else if (x[pairY.Key].Value.Length > 4 && x[pairY.Key].Value.Trim().Contains(pairY.Value.Value.Trim()))
                        {
                            pairY.Value.HasBeenMatched = true;
                            x[pairY.Key].HasBeenMatched = true;
                            pairY.Value.PartialMatchOnly = true;
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
