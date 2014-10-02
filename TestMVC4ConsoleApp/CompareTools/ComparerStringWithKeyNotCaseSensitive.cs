using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringWithKeyNotCaseSensitive : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
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
                        pairX.Value.MismatchDueToCase = true;
                        y[pairX.Key].MismatchDueToCase = true;
                    }
                }
            }

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
                        pairY.Value.MismatchDueToCase = true;
                        x[pairY.Key].MismatchDueToCase = true;
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
