# GoF pattern catalog (this repo)

Single decision source for the `choose-pattern` skill.
Agents: score the user's task against **Trigger signals**, use **Common confusions**
for at most one disambiguation question, then point at **Reference** for reading.

Do not copy textbook examples from Reference into `Scenarios/` — scaffold the user's domain.

---

## Creational

### Singleton
- **Category:** Creational
- **Intent:** Ensure a class only has one instance, and provide a global point of access to it.
- **Trigger signals:** exactly one instance; shared global access point
- **Common confusions:** Static class → does it need to implement an interface / be lazily created / be swappable via DI? Then Singleton.
- **Reference:** `Creational/Singleton/Implementation.cs`

### Factory Method
- **Category:** Creational
- **Intent:** Define an interface for creating an object, but let subclasses decide which class to instantiate. Factory method lets a class defer instantiation to subclasses.
- **Trigger signals:** one product hierarchy, subclass decides concrete type; avoid `new` coupling; reuse existing creation paths
- **Common confusions:** Abstract Factory → one hierarchy (Factory Method) or families of related products across hierarchies (Abstract Factory)? · Builder → single-step subclass choice (Factory Method) or multi-step assembly of one complex object (Builder)? · Prototype → build fresh differently per subclass (Factory Method) or clone a configured instance more cheaply (Prototype)?
- **Reference:** `Creational/Factory Method/Implementation.cs`

### Abstract Factory
- **Category:** Creational
- **Intent:** Provide an interface for creating families of related or dependent objects without specifying their concrete classes. Similar to Factory Method but for interface families.
- **Trigger signals:** need consistent families of related objects (e.g. per-country / per-platform product sets)
- **Common confusions:** vs Factory Method — one hierarchy (Factory Method) or families across hierarchies (Abstract Factory)?
- **Reference:** `Creational/Abstract/Implementation.cs`

### Builder
- **Category:** Creational
- **Intent:** Separate the construction of a complex object from its representation so the same construction process can create different representations.
- **Trigger signals:** many optional parameters/steps; same construction process, different representations; multi-step assembly
- **Common confusions:** vs Factory Method/Abstract Factory — see Factory Method · Prototype → build via steps (Builder) or clone existing (Prototype)?
- **Reference:** `Creational/Builder/Implementation.cs`

### Prototype
- **Category:** Creational
- **Intent:** Specify the kinds of objects to create using a prototypical instance, and create new objects by copying this prototype.
- **Trigger signals:** cloning a configured instance is cheaper than constructing from scratch; system independent of how objects are created via copy
- **Common confusions:** vs Factory Method/Builder — see above
- **Reference:** `Creational/Prototype/Implementation.cs`

---

## Structural

### Adapter
- **Category:** Structural
- **Intent:** Convert the interface of a class into another interface clients expect. Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.
- **Trigger signals:** integrate a legacy/3rd-party class whose interface doesn't match what callers expect
- **Common confusions:** Facade → translating ONE interface to another (Adapter) or simplifying access to a whole subsystem (Facade)? · Bridge → retrofitting incompatible existing code (Adapter) or designed upfront so abstraction/implementation vary independently (Bridge)?
- **Reference:** `Structural/Adapter/ClassAdapterImplementation.cs`, `Structural/Adapter/ObjectAdapterImplementation.cs`

### Bridge
- **Category:** Structural
- **Intent:** Decouple an abstraction from its implementation so the two can vary independently.
- **Trigger signals:** need M abstractions × N implementations without class explosion; swap implementation independent of abstraction
- **Common confusions:** vs Adapter — see above · Strategy → structural split of two parallel hierarchies decided mostly upfront (Bridge) or a runtime-swappable algorithm via composition (Strategy)?
- **Reference:** `Structural/Bridge/Implementation.cs`

### Composite
- **Category:** Structural
- **Intent:** Compose objects into tree structures to represent part-whole hierarchies. Lets clients treat individual objects and compositions uniformly.
- **Trigger signals:** tree/part-whole hierarchy; treat single item and group uniformly
- **Common confusions:** Decorator → recursive whole-part containment treated uniformly (Composite) or linear stacking of added behavior around one component (Decorator)?
- **Reference:** `Structural/Composite/Implementation.cs`

### Decorator
- **Category:** Structural
- **Intent:** Attach additional responsibilities to an object dynamically. Provides a flexible alternative to subclassing for extending functionality.
- **Trigger signals:** wrap dynamically, stackable, same interface, add responsibilities at runtime (e.g. cross-cutting logging/caching/auth combined per environment)
- **Common confusions:** vs Composite — see above · Proxy → stacking N behaviors transparently (Decorator) or gating access via one controlling wrapper (Proxy)? · Chain of Responsibility → augmenting and always returning through every wrapper (Decorator) or passing along until exactly one handler acts (Chain)?
- **Reference:** `Structural/Decorator/Implementation.cs`

### Facade
- **Category:** Structural
- **Intent:** Provide a unified interface to a set of interfaces in a subsystem. Defines a higher-level interface that makes the subsystem easier to use.
- **Trigger signals:** simplify a complex subsystem behind one higher-level entry point
- **Common confusions:** vs Adapter — see above · Mediator → one-directional simplification of a subsystem's outward API (Facade) or centralized bidirectional coordination logic among colleague objects (Mediator)?
- **Reference:** `Structural/Facade/Implementation.cs`

### Flyweight
- **Category:** Structural
- **Intent:** Use sharing to support large numbers of fine-grained objects efficiently by sharing intrinsic state.
- **Trigger signals:** huge number of fine-grained similar objects straining memory; intrinsic vs extrinsic state split
- **Common confusions:** Singleton → many instances sharing an immutable core (Flyweight) or exactly one instance needed (Singleton)?
- **Reference:** `Structural/Flyweight/Implementation.cs`

### Proxy
- **Category:** Structural
- **Intent:** Provide a surrogate or placeholder for another object to control access to it.
- **Trigger signals:** control/gate access (lazy init, remote, protection, caching) behind the same interface
- **Common confusions:** vs Decorator/Chain of Responsibility — see Decorator
- **Reference:** `Structural/Proxy/Implementation.cs`

---

## Behavioral

### Chain of Responsibility
- **Category:** Behavioral
- **Intent:** Avoid coupling the sender of a request to its receiver by giving more than one object a chance to handle the request. Chain the receiving objects and pass the request along until an object handles it.
- **Trigger signals:** unknown/variable handler; multiple candidate handlers; pass along until handled
- **Common confusions:** vs Decorator/Proxy — see Decorator
- **Reference:** `Behavioral/Chain of responsibility/Implementation.cs`

### Command
- **Category:** Behavioral
- **Intent:** Encapsulate a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.
- **Trigger signals:** need to queue, log, delay, or undo/redo an action; treat a request as a first-class object
- **Common confusions:** Strategy → encapsulating an action to be invoked/undone later (Command) or an interchangeable algorithm selected by a client (Strategy)?
- **Reference:** `Behavioral/Command/Implementation.cs`

### Interpreter
- **Category:** Behavioral
- **Intent:** Given a language, define a representation for its grammar along with an interpreter that uses the representation to interpret sentences in the language.
- **Trigger signals:** need to parse/evaluate a small custom grammar/expression language
- **Common confusions:** Distinctive — flag efficiency caveat ("when possible efficiency isn't required"); prefer an existing parser library when available
- **Reference:** `Behavioral/Interpreter/Implementation.cs`

### Iterator
- **Category:** Behavioral
- **Intent:** Provide a way to access the elements of an aggregate object sequentially without exposing its underlying representation.
- **Trigger signals:** traverse a custom collection without exposing internal structure; multiple simultaneous traversals
- **Common confusions:** Visitor → pure traversal access (Iterator) or type-dispatched operations across a stable heterogeneous structure (Visitor)? Also: does native `IEnumerable` already suffice?
- **Reference:** `Behavioral/Iterator/Implementation.cs`

### Mediator
- **Category:** Behavioral
- **Intent:** Define an object that encapsulates how a set of objects interact, forcing objects to communicate via that mediator.
- **Trigger signals:** many objects intercommunicate in complex, tangled ways; want to decouple colleagues from each other
- **Common confusions:** vs Facade — see Facade · Observer → centralized many-to-many coordination logic (Mediator) or a one-to-many notify-on-change relationship with no central decision-maker (Observer)?
- **Reference:** `Behavioral/Mediator/Implementation.cs`

### Observer
- **Category:** Behavioral
- **Intent:** Define a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.
- **Trigger signals:** many listeners must react automatically to one subject's state changes; pub/sub
- **Common confusions:** vs Mediator — see above
- **Reference:** `Behavioral/Observer/Implementation.cs`

### State
- **Category:** Behavioral
- **Intent:** Allow an object to alter its behavior when its internal state changes. The object will appear to change its class.
- **Trigger signals:** behavior depends on internal state and must change at runtime; object transitions itself between well-defined states
- **Common confusions:** Strategy → object switches its own behavior internally as state changes (State) or client picks/injects which variant to use (Strategy)?
- **Reference:** `Behavioral/State/Implementation.cs`

### Strategy
- **Category:** Behavioral
- **Intent:** Define a family of algorithms, encapsulate each one, and make them interchangeable. Strategy lets the algorithm vary independently from clients that use it.
- **Trigger signals:** client picks/injects which algorithm/variant to use; many related classes differ only in behavior
- **Common confusions:** vs State — see above · Command — see Command · Template Method → whole algorithm swappable as a unit via composition (Strategy) or fixed skeleton with pluggable steps via subclassing (Template Method)?
- **Reference:** `Behavioral/Strategy/Implementation.cs`

### Template Method
- **Category:** Behavioral
- **Intent:** Define the skeleton of an algorithm in an operation, deferring some steps to subclasses. Lets subclasses redefine certain steps without changing the algorithm's structure.
- **Trigger signals:** fixed overall algorithm sequence, but individual steps vary by subclass
- **Common confusions:** vs Strategy — see above
- **Reference:** `Behavioral/Template Method/Implementation.cs`

### Visitor
- **Category:** Behavioral
- **Intent:** Represent an operation to be performed on the elements of an object structure. Visitor lets you define a new operation without changing the classes of the elements on which it operates.
- **Trigger signals:** need to add new operations across a stable set of element classes often, without modifying those classes; object structure has many classes with differing interfaces
- **Common confusions:** vs Iterator — see Iterator
- **Reference:** `Behavioral/Visitor/Implementation.cs`

### Memento
- **Category:** Behavioral
- **Intent:** Capture and externalize an object's internal state so that the object can be restored to this state later, without violating encapsulation.
- **Trigger signals:** need undo/rollback/snapshot of an object's state without violating encapsulation
- **Common confusions:** Command (undo) → undo by executing an inverse operation (Command) or by restoring a captured state snapshot (Memento)?
- **Reference:** `Behavioral/Memento/Implementation.cs`
