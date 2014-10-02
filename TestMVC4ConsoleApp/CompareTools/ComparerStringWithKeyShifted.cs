using System.Collections.Generic;
using System.Linq;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringWithKeyShifted : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            foreach (var pairX in x)
            {
                if (!pairX.Value.SingleValueHasBeenMatched && y.ContainsKey(pairX.Key) && !y[pairX.Key].SingleValueHasBeenMatched 
                    && !string.IsNullOrEmpty(y[pairX.Key].Value) && !string.IsNullOrEmpty(pairX.Value.Value) 
                    && pairX.Value.IsOld != y[pairX.Key].IsOld)
                {
                    if (pairX.Value.Value.Contains(','))
                    {
                        string shiftedValue = pairX.Value.Value.Split(',')[1].Trim() + " " + pairX.Value.Value.Split(',')[0].Trim();

                        if (y[pairX.Key].Value == shiftedValue)
                        {
                            pairX.Value.SingleValueHasBeenMatched = true;
                            y[pairX.Key].SingleValueHasBeenMatched = true;
                            pairX.Value.MismatchDueToShiftedName = true;
                        }
                    }
                    else if (y[pairX.Key].Value.Contains(','))
                    {
                        string shiftedValue = y[pairX.Key].Value.Split(',')[1].Trim() + " " + y[pairX.Key].Value.Split(',')[0].Trim();

                        if (pairX.Value.Value == shiftedValue)
                        {
                            pairX.Value.SingleValueHasBeenMatched = true;
                            y[pairX.Key].SingleValueHasBeenMatched = true;
                            y[pairX.Key].MismatchDueToShiftedName = true;
                        }
                    }
                }
            }

            foreach (var pairY in y)
            {
                if (!pairY.Value.SingleValueHasBeenMatched && x.ContainsKey(pairY.Key) && !x[pairY.Key].SingleValueHasBeenMatched
                    && !string.IsNullOrEmpty(x[pairY.Key].Value) && !string.IsNullOrEmpty(pairY.Value.Value)
                    && pairY.Value.IsOld != y[pairY.Key].IsOld)
                {
                    if (pairY.Value.Value.Contains(','))
                    {
                        string shiftedValue = pairY.Value.Value.Split(',')[1].Trim() + " " + pairY.Value.Value.Split(',')[0].Trim();

                        if (x[pairY.Key].Value == shiftedValue)
                        {
                            pairY.Value.SingleValueHasBeenMatched = true;
                            x[pairY.Key].SingleValueHasBeenMatched = true;
                            pairY.Value.MismatchDueToShiftedName = true;
                        }
                    }
                    else if (x[pairY.Key].Value.Contains(','))
                    {
                        string shiftedValue = x[pairY.Key].Value.Split(',')[1].Trim() + " " + x[pairY.Key].Value.Split(',')[0].Trim();

                        if (pairY.Value.Value == shiftedValue)
                        {
                            pairY.Value.SingleValueHasBeenMatched = true;
                            x[pairY.Key].SingleValueHasBeenMatched = true;
                            x[pairY.Key].MismatchDueToShiftedName = true;
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
