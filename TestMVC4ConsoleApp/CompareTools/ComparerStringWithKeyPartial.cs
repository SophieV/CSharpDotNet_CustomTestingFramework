using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringWithKeyPartial : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            foreach (var pairX in x)
            {
                if (!pairX.Value.SingleValueHasBeenMatched && y.ContainsKey(pairX.Key) && !y[pairX.Key].SingleValueHasBeenMatched
                    && !string.IsNullOrEmpty(pairX.Value.Value) && !string.IsNullOrEmpty(y[pairX.Key].Value)
                    && pairX.Value.IsOld != y[pairX.Key].IsOld)
                {
                    if (pairX.Value.Value.Length > 4 && pairX.Value.Value.Trim().Contains(y[pairX.Key].Value.Trim()))
                    {
                        pairX.Value.SingleValueHasBeenMatched = true;
                        y[pairX.Key].SingleValueHasBeenMatched = true;
                        y[pairX.Key].MismatchDueToPartialName = true;
                    }
                    else if (y[pairX.Key].Value.Length > 4 && y[pairX.Key].Value.Trim().Contains(pairX.Value.Value.Trim()))
                    {
                        pairX.Value.SingleValueHasBeenMatched = true;
                        y[pairX.Key].SingleValueHasBeenMatched = true;
                        pairX.Value.MismatchDueToPartialName = true;
                    }
                }
            }

            foreach (var pairY in y)
            {
                if (!pairY.Value.SingleValueHasBeenMatched && x.ContainsKey(pairY.Key) && !x[pairY.Key].SingleValueHasBeenMatched
                    && !string.IsNullOrEmpty(pairY.Value.Value) && !string.IsNullOrEmpty(x[pairY.Key].Value)
                    && pairY.Value.IsOld != y[pairY.Key].IsOld)
                {
                    if (pairY.Value.Value.Length > 4 && pairY.Value.Value.Trim().Contains(x[pairY.Key].Value.Trim()))
                    {
                        pairY.Value.SingleValueHasBeenMatched = true;
                        x[pairY.Key].SingleValueHasBeenMatched = true;
                        x[pairY.Key].MismatchDueToPartialName = true;
                    }
                    else if (x[pairY.Key].Value.Length > 4 && x[pairY.Key].Value.Trim().Contains(pairY.Value.Value.Trim()))
                    {
                        pairY.Value.SingleValueHasBeenMatched = true;
                        x[pairY.Key].SingleValueHasBeenMatched = true;
                        pairY.Value.MismatchDueToPartialName = true;
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
