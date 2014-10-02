
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class ComparerStringWithKeyTrimmed : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            foreach (var pairX in x)
            {
                if (!pairX.Value.SingleValueHasBeenMatched && y.ContainsKey(pairX.Key) && !y[pairX.Key].SingleValueHasBeenMatched 
                    && !string.IsNullOrEmpty(pairX.Value.Value) && !string.IsNullOrEmpty(y[pairX.Key].Value)
                    && pairX.Value.IsOld != y[pairX.Key].IsOld)
                {
                    if (pairX.Value.Value.Trim() == y[pairX.Key].Value)
                    {
                        pairX.Value.SingleValueHasBeenMatched = true;
                        y[pairX.Key].SingleValueHasBeenMatched = true;
                        pairX.Value.MismatchDueToTrailingSpaces = true;
                    }
                    else if (pairX.Value.Value == y[pairX.Key].Value.Trim())
                    {
                        pairX.Value.SingleValueHasBeenMatched = true;
                        y[pairX.Key].SingleValueHasBeenMatched = true;
                        y[pairX.Key].MismatchDueToTrailingSpaces = true;
                    }
                    else if (pairX.Value.Value.Trim() == y[pairX.Key].Value.Trim())
                    {
                        pairX.Value.SingleValueHasBeenMatched = true;
                        y[pairX.Key].SingleValueHasBeenMatched = true;
                        pairX.Value.MismatchDueToTrailingSpaces = true;
                        y[pairX.Key].MismatchDueToTrailingSpaces = true;
                    }
                }
            }

            foreach (var pairY in y)
            {
                if (!pairY.Value.SingleValueHasBeenMatched && x.ContainsKey(pairY.Key) && !x[pairY.Key].SingleValueHasBeenMatched
                    && !string.IsNullOrEmpty(pairY.Value.Value) && !string.IsNullOrEmpty(x[pairY.Key].Value)
                    && pairY.Value.IsOld != y[pairY.Key].IsOld)
                {
                    if (pairY.Value.Value.ToLower() == x[pairY.Key].Value.Trim())
                    {
                        pairY.Value.SingleValueHasBeenMatched = true;
                        x[pairY.Key].SingleValueHasBeenMatched = true;
                        pairY.Value.MismatchDueToTrailingSpaces = true;
                    }
                    else if (pairY.Value.Value == x[pairY.Key].Value.Trim())
                    {
                        pairY.Value.SingleValueHasBeenMatched = true;
                        x[pairY.Key].SingleValueHasBeenMatched = true;
                        x[pairY.Key].MismatchDueToTrailingSpaces = true;
                    }
                    else if (pairY.Value.Value.Trim() == x[pairY.Key].Value.Trim())
                    {
                        pairY.Value.SingleValueHasBeenMatched = true;
                        x[pairY.Key].SingleValueHasBeenMatched = true;
                        pairY.Value.MismatchDueToTrailingSpaces = true;
                        x[pairY.Key].MismatchDueToTrailingSpaces = true;
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