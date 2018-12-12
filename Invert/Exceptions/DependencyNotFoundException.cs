using System;
using System.Collections.Generic;
using System.Text;

namespace Invert {
    /// <summary>
    /// Exception for when a dependency is not found.
    /// </summary>
    public sealed class DependencyNotFoundException : Exception {
        #region Constructor(s)
        /// <summary>
        /// Create a new dependency not found exception.
        /// </summary>
        /// <param name="missingType">The missing dependency type.</param>
        public DependencyNotFoundException(Type missingType) : base(string.Format("{0} was not found", missingType)) { }
        #endregion
    }
}
