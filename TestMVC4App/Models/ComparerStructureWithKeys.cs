using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class ComparerStructureWithKeys : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys,string>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, string>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, string> x, Dictionary<EnumOldServiceFieldsAsKeys, string> y)
        {
            bool areEqual = true;
            foreach (var key in x.Keys)
            {
                if (areEqual)
                {
                    if (x.ContainsKey(key) && y.ContainsKey(key))
                    {
                        if (string.IsNullOrEmpty(x[key]) && string.IsNullOrEmpty(y[key]))
                        {
                            areEqual = true;
                        }
                        else if (string.IsNullOrEmpty(x[key]) || string.IsNullOrEmpty(y[key]))
                        {
                            areEqual = false;
                        }
                        else if (x[key] == y[key])
                        {
                            areEqual = true;
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

        int IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, string>>.GetHashCode(Dictionary<EnumOldServiceFieldsAsKeys, string> obj)
        {
            return obj.Values.ToString().ToLower().GetHashCode();
        }
    }
}