using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Invert {
    /// <summary>
    /// IoC container for registering, and instantiating objects with.
    /// </summary>
    public sealed class InvertContainer {
        #region Fields
        /// <summary>
        /// The collection of dependencies registered with
        /// the IoC container.
        /// </summary>
        private readonly Dictionary<Type, IRegisteredDependency> registeredDependencies;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new IoC container.
        /// </summary>
        public InvertContainer() {
            registeredDependencies = new Dictionary<Type, IRegisteredDependency>();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Register a dependency to the container.
        /// </summary>
        /// <typeparam name="T">The dependency to register.</typeparam>
        /// <returns>The registered wrapper.</returns>
        public IRegisteredDependency Register<T>() {
            RegisteredDependency<T> registeredDependency = new RegisteredDependency<T>();

            //Check to see if we can add it.
            if(!registeredDependencies.TryAdd(registeredDependency.RegisteredType, registeredDependency)) {
                throw new DependencyCollisionException(registeredDependency.RegisteredType);
            }

            return registeredDependency;
        }

        /// <summary>
        /// Resolve a dependency from the container.
        /// </summary>
        /// <typeparam name="T">The dependency type.</typeparam>
        /// <returns>The located dependency.</returns>
        public T Resolve<T>() {
            Type type = typeof(T);
            RegisteredDependency<T> dependency = null;

            try {
                dependency = registeredDependencies[type] as RegisteredDependency<T>;
            }
            catch {
                throw new DependencyNotFoundException(type);
            }

            return Instantiate(dependency);
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Instantiate a new instance of the dependency
        /// </summary>
        /// <param name="dependency"></param>
        private T Instantiate<T>(RegisteredDependency<T> dependency) {
            //Review this later on:
            //https://stackoverflow.com/questions/752/get-a-new-object-instance-from-a-type/29239907
            ConstructorInfo constructor = dependency.ResolveType.GetConstructors().First(c => c.GetCustomAttribute<InjectConstructorAttribute>() != null);

            //Did we find one?
            if(constructor == null) {
                throw new MissingMethodException(string.Format("No constructor marked with an InjectConstructorAttribute found for type {0}", dependency.ResolveType));
            }

            ParameterInfo[] parameters = constructor.GetParameters();

            //Does it have any parameters?
            if(parameters.Length == 0) {
                return (T)Activator.CreateInstance(dependency.ResolveType);
            }

            throw new Exception();
        }
        #endregion

        #region Nested Class(es)
        /// <summary>
        /// Implementation of a registered dependency with the container.
        /// </summary>
        /// <typeparam name="T">The type it represents.</typeparam>
        private class RegisteredDependency<T> : IRegisteredDependency {
            #region Properties
            /// <summary>
            /// The type it is registered under.
            /// </summary>
            public Type RegisteredType { get; private set; }

            /// <summary>
            /// The type it resolves to.
            /// </summary>
            public Type ResolveType { get; private set; }
            #endregion

            #region Constructor(s)
            /// <summary>
            /// Create a new Registered dependency.
            /// </summary>
            public RegisteredDependency() {
                RegisteredType = typeof(T);
                ResolveType    = typeof(T);
            }
            #endregion

            #region Publics
            /// <summary>
            /// Change the type that the registered dependency resolves to.
            /// </summary>
            /// <typeparam name="U">The new type.</typeparam>
            public void To<U>() {
                ResolveType = typeof(U);
            }
            #endregion
        }
        #endregion
    }
}
