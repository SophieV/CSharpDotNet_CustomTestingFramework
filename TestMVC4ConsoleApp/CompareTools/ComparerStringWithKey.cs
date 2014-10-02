using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public class ComparerStringWithKey : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys,StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            foreach (var pairX in x)
            {
                if (!pairX.Value.SingleValueHasBeenMatched && y.ContainsKey(pairX.Key) && !y[pairX.Key].SingleValueHasBeenMatched && pairX.Value.IsOld != y[pairX.Key].IsOld)
                    {
                        if (string.IsNullOrEmpty(pairX.Value.Value) && string.IsNullOrEmpty(y[pairX.Key].Value))
                        {
                            pairX.Value.SingleValueHasBeenMatched = true;
                            y[pairX.Key].SingleValueHasBeenMatched = true;
                        }
                        else if (pairX.Value.Value == y[pairX.Key].Value)
                        {
                            pairX.Value.SingleValueHasBeenMatched = true;
                            y[pairX.Key].SingleValueHasBeenMatched = true;
                        }
                    }
            }

            foreach (var pairY in y)
            {
                if (!pairY.Value.SingleValueHasBeenMatched && x.ContainsKey(pairY.Key) && !x[pairY.Key].SingleValueHasBeenMatched && pairY.Value.IsOld != y[pairY.Key].IsOld)
                {
                    if (string.IsNullOrEmpty(pairY.Value.Value) && string.IsNullOrEmpty(x[pairY.Key].Value))
                    {
                        pairY.Value.SingleValueHasBeenMatched = true;
                        x[pairY.Key].SingleValueHasBeenMatched = true;
                    }
                    else if (pairY.Value.Value == x[pairY.Key].Value)
                    {
                        pairY.Value.SingleValueHasBeenMatched = true;
                        x[pairY.Key].SingleValueHasBeenMatched = true;
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