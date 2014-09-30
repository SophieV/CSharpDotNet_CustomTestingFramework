
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class ComparerStringWithKeyTrimmed : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            bool areEqual = true;

            foreach (var key in x.Keys)
            {
                if (areEqual)
                {
                    if (x.ContainsKey(key) && y.ContainsKey(key))
                    {
                        if (string.IsNullOrEmpty(x[key].Value) && string.IsNullOrEmpty(y[key].Value))
                        {
                            areEqual = true;
                        }
                        else if (string.IsNullOrEmpty(x[key].Value) || string.IsNullOrEmpty(y[key].Value))
                        {
                            areEqual = false;
                        }
                        else if (x[key].Value.Trim() == y[key].Value.Trim())
                        {
                            areEqual = true;
                            x[key].HasBeenMatched = true;
                            x[key].MismatchDueToTrailingSpaces = true;
                            y[key].HasBeenMatched = true;
                            y[key].MismatchDueToTrailingSpaces = true;
                        }
                        else
                        {
                            areEqual = false;
                        }
                    }
                    else
                    {
                        areEqual = false;
                    }
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