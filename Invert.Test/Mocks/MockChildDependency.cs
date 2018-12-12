using System;
using System.Collections.Generic;
using System.Text;

namespace Invert.Test {
    public sealed class MockChildDependency {
        [InvertConstructor]
        public MockChildDependency() {
        }
    }
}
