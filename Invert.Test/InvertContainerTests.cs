using System;
using Xunit;

namespace Invert.Test {
    /// <summary>
    /// Tests pertaining to the Invert Container.
    /// </summary>
    public class InvertContainerTests {
        /// <summary>
        /// Unit test to test registering a dependency with the
        /// container.
        /// </summary>
        [Fact]
        public void RegisterDependencyTest() {
            InvertContainer container = new InvertContainer();
            container.Register<MockDependency>();

            Assert.True(container.HasDependency<MockDependency>());
        }

        /// <summary>
        /// Unit test to register a concrete type under an interface
        /// with the container.
        /// </summary>
        [Fact]
        public void RegisterInterfaceDependencyTest() {
            InvertContainer container = new InvertContainer();
            container.Register<IMockDependency>().To<MockDependency>();

            Assert.True(container.HasDependency<IMockDependency>());
        }

        /// <summary>
        /// Unit test to test for an excpetion when a type is registered
        /// and an existing type already exists.
        /// </summary>
        [Fact]
        public void DependencyCollisionTest() {
            InvertContainer container = new InvertContainer();
            container.Register<MockDependency>();

            Assert.Throws<DependencyCollisionException>(() => { container.Register<MockDependency>(); });
        }

        /// <summary>
        /// Unit test to test for an exception when a dependency
        /// type is not found.
        /// </summary>
        [Fact]
        public void DependencyMissingTest() {
            InvertContainer container = new InvertContainer();

            Assert.Throws<DependencyNotFoundException>(() => { container.Resolve<MockDependency>(); });
        }

        /// <summary>
        /// Unit test to test for an exception if no constructor
        /// on the dependency is marked with an InvertConstructor
        /// attribute.
        /// </summary>
        [Fact]
        public void DependencyWithNoConstructorTest() {
            InvertContainer container = new InvertContainer();
            container.Register<BadMockDependency>();

            Assert.Throws<MissingMethodException>(() => { container.Resolve<BadMockDependency>(); });
        }

        /// <summary>
        /// Unit test to ensure a singleton returns the same instance
        /// when resolved from the container.
        /// </summary>
        [Fact]
        public void SingletonResolveTest() {
            InvertContainer container = new InvertContainer();
            container.Register<MockDependency>().Singleton();

            MockDependency dependency1 = container.Resolve<MockDependency>();
            MockDependency dependency2 = container.Resolve<MockDependency>();

            Assert.Same(dependency1, dependency2);
        }

        /// <summary>
        /// Unit test to test a instance returns a new instance of the 
        /// dependency each time it's resolved from the container.
        /// </summary>
        [Fact]
        public void InstanceResolveTest() {
            InvertContainer container = new InvertContainer();
            container.Register<MockDependency>();

            MockDependency dependency1 = container.Resolve<MockDependency>();
            MockDependency dependency2 = container.Resolve<MockDependency>();

            Assert.NotSame(dependency1, dependency2);
        }

        /// <summary>
        /// Unit test to test resolving a dependency that has a child
        /// dependency that has not been registered with the container.
        /// </summary>
        [Fact]
        public void NestedDependencyMissingResolveTest() {
            InvertContainer container = new InvertContainer();
            container.Register<MockParentDependency>();

            Assert.Throws<DependencyNotFoundException>(() => { container.Resolve<MockParentDependency>(); });
        }

        /// <summary>
        /// Unit test to test resolve a dependency that has a child
        /// dependency.
        /// </summary>
        [Fact]
        public void NestedDependencyResolveTest() {
            InvertContainer container = new InvertContainer();
            container.Register<MockParentDependency>();
            container.Register<MockChildDependency>();

            Assert.NotNull(container.Resolve<MockParentDependency>());
        }
    }
}
