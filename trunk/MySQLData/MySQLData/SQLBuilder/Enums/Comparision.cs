using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLBuilder.Enums
{
    public enum Comparison
    {
        Equals,
        NotEquals,
        Like,
        NotLike,
        GreaterThan,
        GreaterOrEquals,
        LessThan,
        LessOrEquals,
        In,
        NotIn
    }
}
