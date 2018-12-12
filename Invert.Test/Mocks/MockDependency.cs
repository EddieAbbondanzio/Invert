using System;
using System.Collections.Generic;
using System.Text;

namespace Invert.Test {
    public sealed class MockDependency : IMockDependency {
        [InvertConstructor]
        public MockDependency() {

        }
    }
}
