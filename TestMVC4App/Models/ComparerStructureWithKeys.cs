using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class ComparerStructureWithKeys : IEqualityComparer<Dictionary<OldServiceFieldsAsKeys,string>>
    {
        bool IEqualityComparer<Dictionary<OldServiceFieldsAsKeys, string>>.Equals(Dictionary<OldServiceFieldsAsKeys, string> x, Dictionary<OldServiceFieldsAsKeys, string> y)
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

        int IEqualityComparer<Dictionary<OldServiceFieldsAsKeys, string>>.GetHashCode(Dictionary<OldServiceFieldsAsKeys, string> obj)
        {
            System.Diagnostics.Debug.WriteLine(obj.Values.ToString().ToLower().GetHashCode());
            return obj.Values.ToString().ToLower().GetHashCode();
        }
    }
}