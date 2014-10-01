using System.Collections.Generic;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringWithKeyPartial : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            bool areEqual = true;
            foreach (var key in x.Keys)
            {
                if (x.ContainsKey(key) && y.ContainsKey(key) && !x[key].HasBeenMatched && !y[key].HasBeenMatched)
                {
                    if (!string.IsNullOrEmpty(x[key].Value) && x[key].Value.Length > 4 && !string.IsNullOrEmpty(y[key].Value) && x[key].Value.Trim().Contains(y[key].Value.Trim()))
                    {
                        areEqual = true;
                        x[key].HasBeenMatched = true;
                        y[key].HasBeenMatched = true;
                        y[key].MismatchDueToPartialName = true;
                    }
                    else if (!string.IsNullOrEmpty(y[key].Value) && y[key].Value.Length > 4 && !string.IsNullOrEmpty(x[key].Value) && y[key].Value.Trim().Contains(x[key].Value.Trim()))
                    {
                        areEqual = true;
                        x[key].HasBeenMatched = true;
                        y[key].HasBeenMatched = true;
                        x[key].MismatchDueToPartialName = true;
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
