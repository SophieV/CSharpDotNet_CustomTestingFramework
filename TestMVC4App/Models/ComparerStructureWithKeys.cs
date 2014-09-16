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
            System.Diagnostics.Debug.WriteLine("TEST");
            foreach (var key in x.Keys)
            {
                System.Diagnostics.Debug.WriteLine(key);
                if (areEqual)
                {
                    if (x.ContainsKey(key) && y.ContainsKey(key))
                    {
                        System.Diagnostics.Debug.WriteLine(x[key].ToString());
                        if (x[key] == y[key])
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
            System.Diagnostics.Debug.WriteLine(obj.Values.ToString().ToLower().GetHashCode());
            return obj.Values.ToString().ToLower().GetHashCode();
        }
    }
}