using System;
using System.Collections.Generic;
using System.Text;

namespace Invert.Test {
    public sealed class MockParentDependency {
        #region Properties
        public MockChildDependency Child { get; }
        #endregion

        [InvertConstructor]
        public MockParentDependency(MockChildDependency child) {
            Child = child;
        }
    }
}
