using System.Collections.Generic;
using System.Linq;

namespace TestMVC4App.Models
{
    public class ComparerStringWithKey : IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys,StringDescriptor>>
    {
        bool IEqualityComparer<Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor>>.Equals(Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> x, Dictionary<EnumOldServiceFieldsAsKeys, StringDescriptor> y)
        {
            bool areEqual = true;

            foreach( var pair in x)
            {
                if (!pair.Value.HasBeenMatched)
                {
                    if (y.ContainsKey(pair.Key) && !y[pair.Key].HasBeenMatched)
                    {
                        if (string.IsNullOrEmpty(pair.Value.Value) && string.IsNullOrEmpty(y[pair.Key].Value))
                        {
                            pair.Value.HasBeenMatched = true;
                            y[pair.Key].HasBeenMatched = true;
                        }
                        else if (pair.Value.Value == y[pair.Key].Value)
                        {
                            pair.Value.HasBeenMatched = true;
                            y[pair.Key].HasBeenMatched = true;
                        }
                        else
                        {
                            areEqual = false;
                        }
                    }
                }
            }

            foreach (var pair in y)
            {
                if (!pair.Value.HasBeenMatched)
                {
                    if (x.ContainsKey(pair.Key) && !x[pair.Key].HasBeenMatched)
                    {
                        if (string.IsNullOrEmpty(pair.Value.Value) && string.IsNullOrEmpty(x[pair.Key].Value))
                        {
                            pair.Value.HasBeenMatched = true;
                            x[pair.Key].HasBeenMatched = true;
                        }
                        else if (pair.Value.Value == y[pair.Key].Value)
                        {
                            pair.Value.HasBeenMatched = true;
                            x[pair.Key].HasBeenMatched = true;
                        }
                        else
                        {
                            areEqual = false;
                        }
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