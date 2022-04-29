Desing_Patterns

!  Singleton - The intent of the singleton pattern is to ensure that a class only has one instance, and to provide a global point of access to it.
  Use cases: when there must be exactly one instance of a class

  Factory Method - The intent of the factory method pattern is to define an interface for creating an object, but to let subclasses decide which class to instantiate. Factory method lets a class defer instantiation to subclasses.
  Use cases:  enable reusing of existing objects
 
  Abstract Factory - Similar as Factory method but depends on Interface families
  Use cases: when a system should be independent of how its products are created, composed and represented.
 
!  Builder - The intent of the builder pattern is to separate the construction of a complex object from its representation. By doing so, the same construction process can create different representation.
  Use cases:  When you want to make the algorithm for creating a complex objects.
  
  Prototype - The intent of this pattern is to specify the kinds of objects to create using a prototypical instance, and create new objects by copying this prototype. 
   Use cases: When a system should be independent of how its objects are created.
  
  Adapter - The intent of this pattern is to convert the interface of a class into another interface clients expect. Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.
  Use cases: Allows two different classess work together via Interfaces.
  
  Bridge - The intent of this pattern is to decouple an abstraction from its implementation so the two can vary independently.
  Use cases: Used to let abstraction and implementations vary independently.
  
  Decorator - The intent of this pattern is to attach additional responsibilities to an object dynamically. A decorator thus provides a flexible alternative to subclassing for extending functionality.
  Use cases: attach additional responsibilities to an object dynamically.
  
  Composite -  The intent of this pattern is to compose object into tree structures to represent part-whole hierarchies. As such, it lets clients treat individual objects and compositions of objects uniformly: as if they all were individual objects.
  Use cases: provide a transparent, easy way to work with tree-hierarchy.
  
  Facade - The intent of this pattern is to provide a unified interface to a set of interfaces in a subsystem. It defines a higher-level interface that makes the subsystem easier to use.
  Use cases: Providing a simple interface into a complex subsystem.
  
  Proxy - The intent of this pattern is to provide a surrogate or placeholder for another object to control access to it.
  Use cases: when you want to provide a local representative, create expensive objects on demand, caching or locking scenario, different access rules.
  
  Flyweight - The intent of this pattern is to use sharing to support large number of fine-grained objects efficiently. It does that by sharing parts of the state between these objects instead of keeping all that state in all of the objects.
  Use cases: 
  
  Template - The intent of this pattern is to define the skeleton of an algorithm in an operation, deferring some steps to subclasses. It lets subclasses redefine certain steps of an algorithm without changing the algorithm's structure.
  Use cases: define the skeleton of an algorithm in an operation, deferring some steps to subclasses.
  
! Strategy - The intent of this pattern is to define a family of algorithms, encapsulate each one, and make them interchangeable. Strategy lets the algorithm vary independently from clients that use it.
  Use cases: When many related classes differ only in their behavior
  
  Command - The intent of this pattern is to encapsulate a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.
  Use cases: if you want to specify, queue and execute requests at different times
  
  Memento - The intent of this pattern is to capture and externalize an object's internal state so that the object can be restored to this state later, without violating encapsulation.
  Use cases: When object must be saved and could be restored later
  
  Mediator - The intent of this pattern is to define an object - the mediator - that encapsulates how a set of objects interact. It does that by forcing objects to communicate via that mediator.
  Use cases: sets of objects communicate in well-defined but complex ways.
  
  Chain of resposibility - The intent of this pattern is to avoid coupling the sender of a request to its receiver by giving more than one object a chance to handle the request. It does that by chaining the receiving objects and passing the request along the chain until an object handles it.
  Use cases: If one or more objects may handle a request and the handler isn't known beforehand, if there is more than one handlers for issue reuqests
  
  Observer - The intent of this pattern is to define a one to many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.
  Use cases: when one object requires changing others, and you dont know in advance how many object need to be changed.
  
  State - The intent of this pattern is to allow an object to alter its behavior when its internal state changes. The object will appear to change its class.
  Use cases: object's behavior depends on its state and it must change it at runtime.
  
  Iterator - The intent of this pattern is to provide a way to access the elements of an aggregate object sequentially wwwithout exposing its underlying representation.
  Use cases: iterate over list for example.
  
  Visitor - The intent of this pattern is to represent an operation to be performed on the elements of an object structure. Visitor lets you define a new operation without changing the classes of the elements on which it operates.
  Use cases: object structure contains many classes of objects with differing interfaces. 
  
  Interpreter - The intent of this pattern is to, given a language, define a representation for its grammar along with an interpreter that uses the representation to interpret sentences in the language.
  Use cases: when possible efficiency isn't required.
