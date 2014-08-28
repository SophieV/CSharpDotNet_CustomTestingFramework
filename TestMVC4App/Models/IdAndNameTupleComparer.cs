using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class IdAndNameTupleComparer : IEqualityComparer<Tuple<string,string>>
    {
        bool IEqualityComparer<Tuple<string, string>>.Equals(Tuple<string, string> x, Tuple<string, string> y)
        {
            bool areEqual = true;

            if (x.Item1 != y.Item1 || x.Item2 != y.Item2)
            {
                areEqual = false;
            }

            return areEqual;
        }

        int IEqualityComparer<Tuple<string, string>>.GetHashCode(Tuple<string, string> obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}