using System.Collections.Generic;
using System.Linq;
using TestMVC4App.Models;

namespace TestMVC4ConsoleApp.CompareTools
{
    public class ComparerStringShifted : IEqualityComparer<StringDescriptor>
    {
        bool IEqualityComparer<StringDescriptor>.Equals(StringDescriptor x, StringDescriptor y)
        {
            if (!x.HasBeenMatched && !y.HasBeenMatched && x.IsOld != y.IsOld)
            {
                if (!string.IsNullOrEmpty(x.Value) && !string.IsNullOrEmpty(y.Value))
                {
                    if (x.Value.Contains(',') && y.Value == x.Value.Split(',')[1].Trim() + " " + x.Value.Split(',')[0].Trim())
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        x.MismatchDueToShiftedName = true;
                    }
                    if (y.Value.Contains(',') && x.Value == y.Value.Split(',')[1].Trim() + " " + y.Value.Split(',')[0].Trim())
                    {
                        x.HasBeenMatched = true;
                        y.HasBeenMatched = true;
                        y.MismatchDueToShiftedName = true;
                    }
                }
            }

            return false;
        }

        int IEqualityComparer<StringDescriptor>.GetHashCode(StringDescriptor obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}
