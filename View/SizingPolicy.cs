using System;

namespace Rationally.Visio.View
{
    [Flags]
    public enum SizingPolicy
    {
        FixedSize = 0,
        ExpandXIfNeeded = 2,
        ShrinkXIfNeeded = 4,
        ExpandYIfNeeded = 8,
        ShrinkYIfNeeded = 16,
        All = 30
    }
}
