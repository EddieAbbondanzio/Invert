using System;
using System.Collections.Generic;
using System.Reflection;

namespace Invert.Dev {
    public class Foo : IFoo {
        [InvertConstructor]
        public Foo() {

        }
    }

    public interface IFoo {

    }

    class Program {
        static void Main(string[] args) {

            InvertContainer container = new InvertContainer();
            container.Register<IFoo>().To<Foo>();

            Console.WriteLine(container.Resolve<IFoo>());

            Console.ReadKey();
        }
    }
}
