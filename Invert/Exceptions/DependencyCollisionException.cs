using System;
using System.Collections.Generic;
using System.Text;

namespace Invert {
    /// <summary>
    /// Exception for when a collision occurs in the IoC container.
    /// </summary>
    public sealed class DependencyCollisionException : Exception {
        /// <summary>
        /// Create a new collision exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        public DependencyCollisionException(Type collisionType) : base(string.Format("{0} is already registered.", collisionType)) {
        }
    }
}
