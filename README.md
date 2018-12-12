# Invert
An inversion of control (IoC) container framework for C# that supports .NET Core 2.1+. This project was designed to be minimalistic and simple to use, along with minimal bloat. Syntax is similar to other IoC container frameworks such as Ninject, as this is what inspired it. It supports constructor injection for dependency injection.

## How to Use

The first step is to create a new IoC container. 

```c#
InvertContainer container = new InvertContainer();
```

Then register dependencies with it.

```c#
container.Register<Foo>();
container.Register<IBar>().To<Bar>();
```

To resolve these later on use `Resolve<T>()`.

```c#
Foo f = container.Resolve<Foo>();
IBar b = container.Resolve<IBar>();
```

### Defining Dependencies
The only extra requirement Invert needs is to have a constructor on each class with the InvertConstructorAttribute. If no constructor is found then a MethodNotFoundException will be thrown.

```c#
public class Foo {
  [InvertConstructor]
  public Foo() {
  }
}

public class Bar : IBar {
  [InvertConstructor]
  public Bar() {
  }
}
```

Then anytime we need one of the dependencies we can resolve them:

```c#
Foo foo = container.Resolve<Foo>();
IBar bar = container.Resolve<IBar>();
```

### Singletons or Instances 
If a dependency should be a singleton, simply call `.Singleton()` when registering the dependency. The default behaviour for each dependency is to return a new instance which can be set via `.Instance()`.

```c#
container.Register<Foo>().Singleton();
```

Then anytime the dependency is resolved the same instance will be returned.

```c#
Foo foo1 = container.Resolve<Foo>();
Foo foo2 = container.Resolve<Foo>();

Console.WriteLine(object.ReferenceEquals(foo1, foo2)); //Prints: "True"
```

### Dependencies of Dependencies

Invert uses recursion to resolve each dependency therefore it's perfectly acceptable to register a dependency with the container that requires other dependencies. Just be sure to register the child dependencies with the container before attempting to resolve the parent dependency.

```c#
public class Parent {
  public Child { get; set; }
  
  [InvertConstructor]
  public Parent(Child child) {
    Child = child;
  }
}

public class Child {
  [InvertConstructor]
  public Child() {
  }
}

container.Register<Child>();
container.Register<Parent>();

Parent parent = container.Resolve<Parent>();  //parent.Child has been properly instantiated.

```



