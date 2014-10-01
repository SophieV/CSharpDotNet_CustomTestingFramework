using System.Collections.Generic;
using System.Linq;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringWithKeyShifted : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            bool areEqual = true;
            foreach (var key in x.Keys)
            {
                if (x.ContainsKey(key) && y.ContainsKey(key) && !x[key].HasBeenMatched && !y[key].HasBeenMatched && !string.IsNullOrEmpty(x[key].Value) && !string.IsNullOrEmpty(y[key].Value))
                {
                    if (x[key].Value.Contains(','))
                    {
                        string shiftedValue = x[key].Value.Split(',')[1].Trim() + " " + x[key].Value.Split(',')[0].Trim();

                        if (y[key].Value == shiftedValue)
                        {
                            areEqual &= true;
                            x[key].HasBeenMatched = true;
                            y[key].HasBeenMatched = true;
                            x[key].MismatchDueToShiftedName = true;
                        }
                    }
                    else if (y[key].Value.Contains(','))
                    {
                        string shiftedValue = y[key].Value.Split(',')[1].Trim() + " " + y[key].Value.Split(',')[0].Trim();

                        if (x[key].Value == shiftedValue)
                        {
                            areEqual &= true;
                            x[key].HasBeenMatched = true;
                            y[key].HasBeenMatched = true;
                            y[key].MismatchDueToShiftedName = true;
                        }
                    }
                    else
                    {
                        areEqual &= false;
                    }
                }
                else
                {
                    areEqual &= false;
                }
            }

            return areEqual;
        }

        int IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.GetHashCode(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> obj)
        {
            return obj.Values.ToString().ToLower().GetHashCode();
        }
    }
}
