﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (areEqual)
                {
                    if (x.ContainsKey(key) && y.ContainsKey(key))
                    {
                        if (!string.IsNullOrEmpty(x[key].Value) && x[key].Value.Length > 9 && !string.IsNullOrEmpty(y[key].Value) && x[key].Value.Contains(y[key].Value)
                            || !string.IsNullOrEmpty(y[key].Value) && y[key].Value.Length > 9 && !string.IsNullOrEmpty(x[key].Value) && y[key].Value.Contains(x[key].Value))
                        {
                            areEqual = true;
                            x[key].HasBeenMatched = true;
                            y[key].HasBeenMatched = true;
                            x[key].MismatchDueToTrailingSpaces = true;
                            x[key].MismatchDueToTrailingSpaces = true;

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
