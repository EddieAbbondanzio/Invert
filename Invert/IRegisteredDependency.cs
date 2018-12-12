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
        /// <returns>This instance of the registered dependency.</returns>
        IRegisteredDependency To<U>();

        /// <summary>
        /// When resolved, a new instance of the dependency is returned
        /// each time. This is the default.
        /// </summary>
        /// <returns>This instance of the registered dependency.</returns>
        IRegisteredDependency Prototype();

        /// <summary>
        /// When resolved, the same instance of the dependency is returned
        /// each time.
        /// </summary>
        /// <returns>This instance of the registered dependency.</returns>
        IRegisteredDependency Singleton();
    }
}
