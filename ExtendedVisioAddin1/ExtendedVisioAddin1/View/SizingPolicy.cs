using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1.View
{
    [Flags]
    public enum SizingPolicy
    {
        FixedSize = 0,
        ExpandXIfNeeded = 2,
        ShrinkXIfNeeded = 4,
        ExpandYIfNeeded = 8,
        ShrinkYIfNeeded = 16
    }
}
