
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class ComparerStringWithKeyTrimmed : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            bool areEqual = true;

            foreach (var key in x.Keys)
            {
                if (x.ContainsKey(key) && y.ContainsKey(key) && !x[key].HasBeenMatched && !y[key].HasBeenMatched)
                {
                    if (!string.IsNullOrEmpty(x[key].Value) && !string.IsNullOrEmpty(y[key].Value))
                    {
                        if (x[key].Value.Trim() == y[key].Value)
                        {
                            areEqual &= true;
                            x[key].HasBeenMatched = true;
                            x[key].MismatchDueToTrailingSpaces = true;
                            y[key].HasBeenMatched = true;
                        }
                        else if (x[key].Value == y[key].Value.Trim())
                        {
                            areEqual &= true;
                            x[key].HasBeenMatched = true;
                            y[key].HasBeenMatched = true;
                            y[key].MismatchDueToTrailingSpaces = true;
                        }
                        else if (x[key].Value.Trim() == y[key].Value.Trim())
                        {
                            areEqual &= true;
                            x[key].HasBeenMatched = true;
                            x[key].MismatchDueToTrailingSpaces = true;
                            y[key].HasBeenMatched = true;
                            y[key].MismatchDueToTrailingSpaces = true;
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