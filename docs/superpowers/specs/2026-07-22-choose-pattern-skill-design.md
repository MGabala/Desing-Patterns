# `choose-pattern` skill — design

**Date:** 2026-07-22
**Status:** Approved for planning
**Packaging superseded by:** `docs/superpowers/specs/2026-07-23-choose-pattern-dual-skill-design.md`
(Claude + Grok dual install, shared catalog at `skills/choose-pattern/references/`).
Behavior tables and scaffolding conventions below remain the content source for that design.

## Purpose

A project skill for this repo (`.claude/skills/choose-pattern/`) that helps decide which of
the 23 GoF patterns already implemented here best fits a new task, and — on confirmation —
scaffolds a task-specific starting point for it.

This repo is a personal catalog of canonical GoF pattern implementations, one per folder
(`Creational/`, `Structural/`, `Behavioral/`), each with a reference `Implementation.cs` and
a one-paragraph intent/use-case entry in `README.md`. The skill turns that catalog into an
active decision aid instead of a passive reference.

## End-to-end flow

1. User describes a task/problem in free text (no fixed question format required).
2. The skill scores the description against `references/pattern-catalog.md` (see below).
3. **Clear winner:** proceeds straight to a recommendation.
   **Ambiguous between 2 (rarely 3) close patterns:** asks exactly **one** targeted
   disambiguating question (via `AskUserQuestion` where available), drawn from that
   pattern's "common confusions" entry, then proceeds.
4. Presents:
   - **Primary recommendation** with rationale tied to the task's specifics.
   - **Runner-up(s)** with the specific trade-off that ruled them out — the rejection
     reasoning is often as useful as the pick itself.
   - A pointer to the existing reference implementation in this repo (e.g.
     `Structural/Decorator/Implementation.cs`) as reading material.
   - If the task genuinely doesn't warrant a pattern (fixed single algorithm, no real
     variability/extension axis), the skill says so plainly instead of force-fitting one.
5. If the user confirms a pick, the skill scaffolds
   `Scenarios/<TaskName>/Implementation.cs` — code written for the user's actual task
   domain, not a copy of the repo's existing textbook example — following this repo's
   existing conventions (see "Scaffolding conventions" below).
6. Runs `dotnet build` from the repo root and reports pass/fail. No project-file changes
   are needed: the SDK-style `.csproj` globs all `.cs` files automatically (confirmed —
   `Desing_Patterns.csproj` only lists an `<ItemGroup>` `<Folder>` entry for one empty
   directory, no explicit `<Compile Include>` list).
7. `README.md` and `Program.cs` are **not** modified — the 23-pattern catalog stays a
   pristine, untouched reference.

## Decision engine

Knowledge lives in `references/pattern-catalog.md`, one entry per pattern:

- **Category** — Creational / Structural / Behavioral (mirrors repo folder structure)
- **Intent** — from this repo's `README.md`
- **Trigger signals** — phrases/shapes in a task description that point at this pattern
- **Common confusions** — neighboring patterns it's most often mistaken for, and the
  one-question test to tell them apart
- **Reference path** — exact file(s) in this repo demonstrating it

### Full catalog content

**Creational**

| Pattern | Trigger signals | Confused with → disambiguating test | Reference |
|---|---|---|---|
| Singleton | exactly one instance; shared global access point | Static class → does it need to implement an interface / be lazily created / be swappable via DI? Then Singleton. | `Creational/Singleton/Implementation.cs` |
| Factory Method | one product hierarchy, subclass decides concrete type; avoid `new` coupling | Abstract Factory → one hierarchy (Factory Method) or families of related products across hierarchies (Abstract Factory)? · Builder → single-step subclass choice (Factory Method) or multi-step assembly of one complex object (Builder)? · Prototype → build fresh differently per subclass (Factory Method) or clone a configured instance more cheaply (Prototype)? | `Creational/Factory Method/Implementation.cs` |
| Abstract Factory | need consistent families of related objects (e.g. per-country/per-platform product sets) | vs Factory Method — see above | `Creational/Abstract/Implementation.cs` |
| Builder | many optional parameters/steps; same construction process, different representations | vs Factory Method/Abstract Factory — see above · Prototype → build via steps (Builder) or clone existing (Prototype)? | `Creational/Builder/Implementation.cs` |
| Prototype | cloning a configured instance is cheaper than constructing from scratch | vs Factory Method/Builder — see above | `Creational/Prototype/Implementation.cs` |

**Structural**

| Pattern | Trigger signals | Confused with → disambiguating test | Reference |
|---|---|---|---|
| Adapter | integrate a legacy/3rd-party class whose interface doesn't match what callers expect | Facade → translating ONE interface to another (Adapter) or simplifying access to a whole subsystem (Facade)? · Bridge → retrofitting incompatible existing code (Adapter) or designed upfront so abstraction/implementation vary independently (Bridge)? | `Structural/Adapter/ClassAdapterImplementation.cs`, `ObjectAdapterImplementation.cs` |
| Bridge | need M abstractions × N implementations without class explosion; swap implementation independent of abstraction | vs Adapter — see above · Strategy → structural split of two parallel hierarchies decided mostly upfront (Bridge) or a runtime-swappable algorithm via composition (Strategy)? | `Structural/Bridge/Implementation.cs` |
| Composite | tree/part-whole hierarchy; treat single item and group uniformly | Decorator → recursive whole-part containment treated uniformly (Composite) or linear stacking of added behavior around one component (Decorator)? | `Structural/Composite/Implementation.cs` |
| Decorator | wrap dynamically, stackable, same interface, add responsibilities at runtime (e.g. cross-cutting logging/caching/auth combined per environment) | vs Composite — see above · Proxy → stacking N behaviors transparently (Decorator) or gating access via one controlling wrapper (Proxy)? · Chain of Responsibility → augmenting and always returning through every wrapper (Decorator) or passing along until exactly one handler acts (Chain)? | `Structural/Decorator/Implementation.cs` |
| Facade | simplify a complex subsystem behind one higher-level entry point | vs Adapter — see above · Mediator → one-directional simplification of a subsystem's outward API (Facade) or centralized bidirectional coordination logic among colleague objects (Mediator)? | `Structural/Facade/Implementation.cs` |
| Flyweight | huge number of fine-grained similar objects straining memory; intrinsic vs extrinsic state split | Singleton → many instances sharing an immutable core (Flyweight) or exactly one instance needed (Singleton)? | `Structural/Flyweight/Implementation.cs` |
| Proxy | control/gate access to an object (lazy init, remote, protection, caching) behind the same interface | vs Decorator/Chain of Responsibility — see above | `Structural/Proxy/Implementation.cs` |

**Behavioral**

| Pattern | Trigger signals | Confused with → disambiguating test | Reference |
|---|---|---|---|
| Chain of Responsibility | unknown/variable handler for a request; multiple candidate handlers, pass along until handled | vs Decorator/Proxy — see above | `Behavioral/Chain of responsibility/Implementation.cs` |
| Command | need to queue, log, delay, or undo/redo an action; treat a request as a first-class object | Strategy → encapsulating an action to be invoked/undone later (Command) or an interchangeable algorithm selected by a client (Strategy)? | `Behavioral/Command/Implementation.cs` |
| Interpreter | need to parse/evaluate a small custom grammar/expression language | (Distinctive — but flag the repo's own caveat: "when possible efficiency isn't required"; consider an existing parser library first) | `Behavioral/Interpreter/Implementation.cs` |
| Iterator | traverse a custom collection without exposing its internal structure; support multiple simultaneous traversals | Visitor → pure traversal access (Iterator) or type-dispatched operations across a stable heterogeneous structure (Visitor)? (Also: does a native `IEnumerable` already suffice?) | `Behavioral/Iterator/Implementation.cs` |
| Mediator | many objects intercommunicate in complex, tangled ways; want to decouple colleagues from each other | vs Facade — see above · Observer → centralized many-to-many coordination logic (Mediator) or a one-to-many notify-on-change relationship with no central decision-maker (Observer)? | `Behavioral/Mediator/Implementation.cs` |
| Observer | many listeners must react automatically to one subject's state changes; pub/sub | vs Mediator — see above | `Behavioral/Observer/Implementation.cs` |
| State | behavior depends on internal state and must change at runtime; object transitions itself between well-defined states | Strategy → object switches its own behavior internally as state changes (State) or client picks/injects which variant to use (Strategy)? | `Behavioral/State/Implementation.cs` |
| Strategy | client picks/injects which algorithm/variant to use; many related classes differ only in behavior | vs State — see above · Command — see above · Template Method → whole algorithm swappable as a unit via composition (Strategy) or fixed skeleton with pluggable steps via subclassing (Template Method)? | `Behavioral/Strategy/Implementation.cs` |
| Template Method | fixed overall algorithm sequence, but individual steps vary by subclass | vs Strategy — see above | `Behavioral/Template Method/Implementation.cs` |
| Visitor | need to add new operations across a stable set of element classes often, without modifying those classes | vs Iterator — see above | `Behavioral/Visitor/Implementation.cs` |
| Memento | need undo/rollback/snapshot of an object's state without violating encapsulation | Command (undo) → undo by executing an inverse operation (Command) or by restoring a captured state snapshot (Memento)? | `Behavioral/Memento/Implementation.cs` |

## Scaffolding conventions

On confirmation, the skill creates `Scenarios/<TaskName>/Implementation.cs`
(`<TaskName>` in PascalCase, derived from the task), matching the style of the repo's
existing `Implementation.cs` files:

```csharp
//<PatternName> - <intent, one line>
//  Use cases: <use case, one line>
namespace <TaskName>;

//Real life example: <one-line description of the actual task>

/// <summary>
/// <GoF role, e.g. "ConcreteDecorator">
/// </summary>
public class ... { ... }
```

- Every class carries a `/// <summary>GoF role</summary>` doc comment, matching the
  pattern visible in `Behavioral/State/Implementation.cs:8,21,57,122` (`State`,
  `ConcreteState`, `ConcreteState`, `Context`) — the scaffold reads as a labeled instance
  of the pattern.
- Code models the user's actual task domain (e.g. real service-call wrapping for
  logging/metrics/auth) — a new worked example, not a copy of the repo's existing
  textbook stand-in (bank account, car builder, etc.).
- No changes to `README.md` or `Program.cs`.
- After writing, run `dotnet build` from the repo root; report pass/fail to the user.

## Skill package structure

```
.claude/skills/choose-pattern/
  SKILL.md                      — frontmatter (name, description) + the procedure above:
                                   when/how to consult the catalog, the one-question
                                   disambiguation rule, scaffolding steps, build check
  references/pattern-catalog.md — the full 23-pattern table above
```

Two files only: `SKILL.md` stays a lean procedure; the large lookup table lives in
`references/` so it loads into context only when the skill actually runs.

The `description` frontmatter is written so Claude Code can auto-trigger this skill when
a user describes a task and asks something like "which pattern should I use for this" or
"help me pick a design pattern" — without needing to invoke it by name.

## Out of scope

- No changes to the 23 existing canonical pattern folders, `README.md`, or `Program.cs`.
- No multi-language support — scaffolds are C#, matching this repo.
- No automated combo-pattern scaffolding (e.g. Strategy + Factory together) — the skill
  may *mention* that a task idiomatically combines two patterns, but scaffolds one
  primary pattern per invocation.
