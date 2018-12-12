using System;
using System.Collections.Generic;
using System.Text;

namespace Invert {
    /// <summary>
    /// Interface for a dependency that is registered with the IoC
    /// container to implement.
    /// </summary>
    public interface IRegisteredDependency {
        /// <summary>
        /// Change the type of the dependency that is instantiated
        /// when Resolved() is called on the container.
        /// </summary>
        /// <typeparam name="U">The type to register it as.</typeparam>
        void To<U>();
    }
}
