using System;
using System.Collections.Generic;
using System.Text;

namespace Invert {
    /// <summary>
    /// Attribute to indicate that a constructor parameter is a value
    /// that should be injected by Invert.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class InjectAttribute : Attribute {
    }
}
