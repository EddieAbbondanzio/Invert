using System;
using System.Collections.Generic;

namespace Invert.Dev {
    public class Foo {
        [InjectConstructor]
        public Foo() {

        }
    }

    class Program {
        static void Main(string[] args) {
            InvertContainer container = new InvertContainer();
            container.Register<Foo>();

            Foo f = container.Resolve<Foo>();


            Console.ReadKey();
        }
    }
}
