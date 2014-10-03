using System.Collections.Generic;
using System.Linq;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    /// <summary>
    /// Compares values having the same key : whether they match if one of the sides is split and reassembled.
    /// </summary>
    public class ComparerStringWithKeyShifted : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            if (x != null)
            {
                foreach (var pairX in x)
                {
                    if (!pairX.Value.HasBeenMatched && y.ContainsKey(pairX.Key) && !y[pairX.Key].HasBeenMatched
                        && !string.IsNullOrEmpty(y[pairX.Key].Value) && !string.IsNullOrEmpty(pairX.Value.Value)
                        && pairX.Value.IsOld != y[pairX.Key].IsOld)
                    {
                        if (pairX.Value.Value.Contains(','))
                        {
                            string shiftedValue = pairX.Value.Value.Split(',')[1].Trim() + " " + pairX.Value.Value.Split(',')[0].Trim();

                            if (y[pairX.Key].Value == shiftedValue)
                            {
                                pairX.Value.HasBeenMatched = true;
                                y[pairX.Key].HasBeenMatched = true;
                                pairX.Value.MatchedOnceShifted = true;
                            }
                        }
                        else if (y[pairX.Key].Value.Contains(','))
                        {
                            string shiftedValue = y[pairX.Key].Value.Split(',')[1].Trim() + " " + y[pairX.Key].Value.Split(',')[0].Trim();

                            if (pairX.Value.Value == shiftedValue)
                            {
                                pairX.Value.HasBeenMatched = true;
                                y[pairX.Key].HasBeenMatched = true;
                                y[pairX.Key].MatchedOnceShifted = true;
                            }
                        }
                    }
                }
            }

            if (y != null)
            {
                foreach (var pairY in y)
                {
                    if (!pairY.Value.HasBeenMatched && x.ContainsKey(pairY.Key) && !x[pairY.Key].HasBeenMatched
                        && !string.IsNullOrEmpty(x[pairY.Key].Value) && !string.IsNullOrEmpty(pairY.Value.Value)
                        && pairY.Value.IsOld != y[pairY.Key].IsOld)
                    {
                        if (pairY.Value.Value.Contains(','))
                        {
                            string shiftedValue = pairY.Value.Value.Split(',')[1].Trim() + " " + pairY.Value.Value.Split(',')[0].Trim();

                            if (x[pairY.Key].Value == shiftedValue)
                            {
                                pairY.Value.HasBeenMatched = true;
                                x[pairY.Key].HasBeenMatched = true;
                                pairY.Value.MatchedOnceShifted = true;
                            }
                        }
                        else if (x[pairY.Key].Value.Contains(','))
                        {
                            string shiftedValue = x[pairY.Key].Value.Split(',')[1].Trim() + " " + x[pairY.Key].Value.Split(',')[0].Trim();

                            if (pairY.Value.Value == shiftedValue)
                            {
                                pairY.Value.HasBeenMatched = true;
                                x[pairY.Key].HasBeenMatched = true;
                                x[pairY.Key].MatchedOnceShifted = true;
                            }
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
