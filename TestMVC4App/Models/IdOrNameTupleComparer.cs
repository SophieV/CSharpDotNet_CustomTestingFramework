using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC4App.Models
{
    public class IdOrNameTupleComparer : IEqualityComparer<Tuple<string, string>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Data from new service.</param>
        /// <param name="y">Data from old service.</param>
        /// <returns></returns>
        bool IEqualityComparer<Tuple<string, string>>.Equals(Tuple<string, string> x, Tuple<string, string> y)
        {
            bool areEqual = false;

            // if IDs match
            if (x.Item1 == y.Item1)
            {
                areEqual = true;
            }

            // if no ID on the old side and names match
            if (string.IsNullOrEmpty(y.Item1) && x.Item2 == y.Item2)
            {
                areEqual = true;
            }

            System.Diagnostics.Debug.WriteLine("Comparing {" + x.Item1 + "," + x.Item2 + "} and {" + y.Item1 + "," + y.Item2 + "} is " + areEqual);

            return areEqual;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks>Implementations are required to ensure that if the Equals method returns true for two objects x and y, then the value returned by the GetHashCode method for x must equal the value returned for y.</remarks>
        int IEqualityComparer<Tuple<string, string>>.GetHashCode(Tuple<string, string> obj)
        {
            System.Diagnostics.Debug.WriteLine("Hash {" + obj.Item1 + "," + obj.Item2 + "}");

            return obj.Item2.ToLower().GetHashCode();
        }
    }
}