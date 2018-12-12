using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly Dictionary<Type, RegisteredDependency> registeredDependencies;

        /// <summary>
        /// The singletons that have already been instantiated.
        /// </summary>
        private readonly Dictionary<Type, object> singletons;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new IoC container.
        /// </summary>
        public InvertContainer() {
            registeredDependencies = new Dictionary<Type, RegisteredDependency>();
            singletons = new Dictionary<Type, object>();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Register a dependency to the container.
        /// </summary>
        /// <typeparam name="T">The dependency to register.</typeparam>
        /// <returns>The registered wrapper.</returns>
        public IRegisteredDependency Register<T>() {
            Type dependencyType = typeof(T);
            RegisteredDependency registeredDependency = new RegisteredDependency(dependencyType);

            //Check to see if we can add it.
            if(!registeredDependencies.TryAdd(dependencyType, registeredDependency)) {
                throw new DependencyCollisionException(dependencyType);
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
            RegisteredDependency dependency = ResolveDependency(type);
            return (T)Instantiate(dependency);
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Resolve a dependency with the container. If no matching
        /// dependency is found, an exception is thrown.
        /// </summary>
        /// <param name="type">The type to look for.</param>
        /// <returns>The found dependency.</returns>
        private RegisteredDependency ResolveDependency(Type type) {
            RegisteredDependency dependency = null;

            try {
                dependency = registeredDependencies[type];
            }
            catch {
                throw new DependencyNotFoundException(type);
            }
            return dependency;
        }

        /// <summary>
        /// Instantiate a new instance of the dependency
        /// </summary>
        /// <param name="dependency"></param>
        private object Instantiate(RegisteredDependency dependency) {
            //Review this later on:
            //https://stackoverflow.com/questions/752/get-a-new-object-instance-from-a-type/29239907
            ConstructorInfo constructor = dependency.ResolveType.GetConstructors().First(c => c.GetCustomAttribute<InvertConstructorAttribute>() != null);

            //Did we find one?
            if(constructor == null) {
                throw new MissingMethodException(string.Format("No constructor marked with an InjectConstructorAttribute found for type {0}", dependency.ResolveType));
            }

            ParameterInfo[] parameterInfos = constructor.GetParameters();

            //Does it have any parameters?
            if (parameterInfos.Length == 0) {
                object instance = null;

                //Is it a singleton?
                if (dependency.IsSingleton) {
                    if (singletons.TryGetValue(dependency.ResolveType, out instance)) {
                        return instance;
                    }
                    else {
                        instance = Activator.CreateInstance(dependency.ResolveType);
                        singletons.Add(dependency.ResolveType, instance);

                        return instance;
                    }
                }
                else {
                    return Activator.CreateInstance(dependency.ResolveType);
                }
            }
            else {
                List<object> parameters = new List<object>();

                for(int i = 0; i < parameterInfos.Length; i++) {
                    RegisteredDependency dependencyChild = ResolveDependency(parameterInfos[i].ParameterType);
                    parameters.Add(Instantiate(dependencyChild));
                }

                return Activator.CreateInstance(dependency.ResolveType, parameters.ToArray());
            }
        }
        #endregion

        #region Nested Class(es)
        /// <summary>
        /// Implementation of a registered dependency with the container.
        /// </summary>
        private class RegisteredDependency : IRegisteredDependency {
            #region Properties
            /// <summary>
            /// The type it resolves to.
            /// </summary>
            public Type ResolveType { get; private set; }
            
            /// <summary>
            /// How it should be instantiated.
            /// </summary>
            public bool IsSingleton { get; private set; }
            #endregion

            #region Constructor(s)
            /// <summary>
            /// Create a new Registered dependency.
            /// </summary>
            /// <param name="type">The type of it.</param>
            public RegisteredDependency(Type type) {
                ResolveType = type;
            }
            #endregion

            #region Publics
            /// <summary>
            /// Change the type that the registered dependency resolves to.
            /// </summary>
            /// <typeparam name="U">The new type.</typeparam>
            public IRegisteredDependency To<U>() {
                ResolveType = typeof(U);
                return this;
            }

            /// <summary>
            /// When resolved, a new instance is retuend each time.
            /// </summary>
            public IRegisteredDependency Prototype() {
                IsSingleton = false;
                return this;
            }

            /// <summary>
            /// When resolved, the same instance is returned each time.
            /// </summary>
            /// <returns></returns>
            public IRegisteredDependency Singleton() {
                IsSingleton = true;
                return this;
            }
            #endregion
        }
        #endregion
    }
}
