using System;
using System.Collections.Generic;
using System.Text;

namespace Invert {
    /// <summary>
    /// Attribute to indicate which constructor should be used by
    /// Invert to build the object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class InvertConstructorAttribute : Attribute {
    }
}
