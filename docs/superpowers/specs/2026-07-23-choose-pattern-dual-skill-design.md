# `choose-pattern` skill — dual runtime design (Claude + Grok)

**Date:** 2026-07-23  
**Status:** Approved for planning  
**Supersedes (packaging only):** `docs/superpowers/specs/2026-07-22-choose-pattern-skill-design.md`  
**Preserves:** decision catalog content, end-to-end flow, scaffolding conventions from the 2026-07-22 design

## Purpose

A project skill for this repo that helps decide which of the 23 GoF patterns already
implemented here best fits a new task, and — on confirmation — scaffolds a task-specific
starting point for it.

This design extends the approved 2026-07-22 design so the skill works for **both Claude Code
and Grok**, using dual in-repo install paths and a single shared pattern catalog.

## Goals

- Same product behavior on Claude and Grok: recommend → (optional one question) → confirm →
  scaffold → `dotnet build`.
- Single source of decision knowledge (no catalog drift between runtimes).
- Procedure lives in both runtime entrypoints (full `SKILL.md` each); catalog is not copied.
- Never modify the 23 canonical pattern folders, `README.md`, or `Program.cs`.

## Non-goals

- Install scripts, symlinks, or user-global-only installs.
- Multi-language scaffolds (C# only, matching this repo).
- Automated multi-pattern scaffolding (may mention combos; scaffolds one primary per run).
- Full pressure-test TDD skill campaign (optional later; not required for v1).

## Package layout

```
skills/choose-pattern/
  references/
    pattern-catalog.md          # single source of decision knowledge (23 patterns)

.claude/skills/choose-pattern/
  SKILL.md                      # full procedure + Claude frontmatter

.grok/skills/choose-pattern/
  SKILL.md                      # full procedure + Grok frontmatter
```

### Packaging rules

| Rule | Detail |
|------|--------|
| Dual copies in-repo | Both `.claude/skills/choose-pattern/` and `.grok/skills/choose-pattern/` are committed. |
| Shared catalog (A2/C1) | Catalog lives only at `skills/choose-pattern/references/pattern-catalog.md`. Runtime skill folders do **not** contain a copy. |
| Full procedure in both entrypoints (D1) | Each `SKILL.md` contains the complete workflow. Differences are limited to frontmatter and platform tool names. |
| Repo-relative catalog path | Both procedures instruct the agent to read `skills/choose-pattern/references/pattern-catalog.md` from the workspace/repo root. |
| Maintenance | Procedure edits must be applied to **both** `SKILL.md` files. Catalog edits happen only under `skills/choose-pattern/references/`. |

## End-to-end flow

1. User describes a task/problem in free text (via `/choose-pattern` or natural-language
   triggers that match the skill description).
2. Agent reads `skills/choose-pattern/references/pattern-catalog.md`.
3. Agent scores the description against each pattern’s **trigger signals**.
4. **Clear winner:** proceed to recommendation.  
   **Ambiguous between 2 (rarely 3) close patterns:** ask exactly **one** targeted
   disambiguating question drawn from that pattern pair’s “common confusions” entry,
   using the runtime’s ask-user tool when available:
   - Claude: `AskUserQuestion` (or equivalent)
   - Grok: `ask_user_question` (or equivalent)
   - Fallback: plain chat question if no structured tool exists
5. Present:
   - **Primary recommendation** with rationale tied to the task’s specifics
   - **Runner-up(s)** with the specific trade-off that ruled them out
   - Pointer to the existing reference implementation(s) in this repo
   - If the task does not warrant a pattern (fixed single algorithm, no real
     variability/extension axis), say so plainly — no force-fit
6. On user confirmation of a pick, scaffold
   `Scenarios/<TaskName>/Implementation.cs` for the user’s actual task domain
   (not a copy of the repo’s textbook example).
7. Run `dotnet build` from the repo root; report pass/fail. No `.csproj` edits needed
   (SDK-style project globs all `.cs` files).
8. Do **not** modify `README.md` or `Program.cs`.

## Decision engine

Knowledge lives in `skills/choose-pattern/references/pattern-catalog.md`, one entry per
pattern:

- **Category** — Creational / Structural / Behavioral
- **Intent** — from this repo’s `README.md`
- **Trigger signals** — phrases/shapes in a task description that point at this pattern
- **Common confusions** — neighboring patterns and the one-question test to tell them apart
- **Reference path** — exact file(s) in this repo demonstrating it

### Catalog content (authoritative tables)

Content is taken from the 2026-07-22 design (copy into `pattern-catalog.md` at
implementation time).

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
| Interpreter | need to parse/evaluate a small custom grammar/expression language | Distinctive — but flag the repo's caveat: "when possible efficiency isn't required"; consider an existing parser library first | `Behavioral/Interpreter/Implementation.cs` |
| Iterator | traverse a custom collection without exposing its internal structure; support multiple simultaneous traversals | Visitor → pure traversal access (Iterator) or type-dispatched operations across a stable heterogeneous structure (Visitor)? (Also: does a native `IEnumerable` already suffice?) | `Behavioral/Iterator/Implementation.cs` |
| Mediator | many objects intercommunicate in complex, tangled ways; want to decouple colleagues from each other | vs Facade — see above · Observer → centralized many-to-many coordination logic (Mediator) or a one-to-many notify-on-change relationship with no central decision-maker (Observer)? | `Behavioral/Mediator/Implementation.cs` |
| Observer | many listeners must react automatically to one subject's state changes; pub/sub | vs Mediator — see above | `Behavioral/Observer/Implementation.cs` |
| State | behavior depends on internal state and must change at runtime; object transitions itself between well-defined states | Strategy → object switches its own behavior internally as state changes (State) or client picks/injects which variant to use (Strategy)? | `Behavioral/State/Implementation.cs` |
| Strategy | client picks/injects which algorithm/variant to use; many related classes differ only in behavior | vs State — see above · Command — see above · Template Method → whole algorithm swappable as a unit via composition (Strategy) or fixed skeleton with pluggable steps via subclassing (Template Method)? | `Behavioral/Strategy/Implementation.cs` |
| Template Method | fixed overall algorithm sequence, but individual steps vary by subclass | vs Strategy — see above | `Behavioral/Template Method/Implementation.cs` |
| Visitor | need to add new operations across a stable set of element classes often, without modifying those classes | vs Iterator — see above | `Behavioral/Visitor/Implementation.cs` |
| Memento | need undo/rollback/snapshot of an object's state without violating encapsulation | Command (undo) → undo by executing an inverse operation (Command) or by restoring a captured state snapshot (Memento)? | `Behavioral/Memento/Implementation.cs` |

## Scaffolding conventions

On confirmation, create `Scenarios/<TaskName>/Implementation.cs` (`<TaskName>` in PascalCase,
derived from the task), matching existing `Implementation.cs` style:

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

- Every class carries a `/// <summary>GoF role</summary>` doc comment (see e.g.
  `Behavioral/State/Implementation.cs`).
- Code models the user’s actual task domain — a new worked example, not a copy of the
  repo’s textbook stand-in.
- No changes to `README.md` or `Program.cs`.
- After writing, run `dotnet build` from the repo root; report pass/fail.

## Platform-specific SKILL.md requirements

### Shared procedure body

Both entrypoints document the same steps (flow § above), the catalog path, scaffolding
rules, build check, and maintenance dual-edit rule.

### Frontmatter

Both files use Agent Skills–compatible YAML:

```yaml
---
name: choose-pattern
description: >-
  Use when the user asks which GoF / design pattern fits a task, wants help
  picking among creational/structural/behavioral patterns, runs /choose-pattern,
  or describes a design problem and needs a pattern recommendation (and optional
  C# scaffold) in this Design Patterns repo.
---
```

Description rules:

- Focus on **when to use** (triggers), not a full workflow summary (so agents load the body).
- Include keywords: design pattern, GoF, which pattern, creational, structural, behavioral,
  scaffold, `/choose-pattern`.
- Keep third-person / trigger-oriented so both Claude and Grok can auto-invoke.

Grok entrypoint may additionally include optional Grok-only fields if useful for slash-menu
discovery (e.g. `when-to-use`), without changing the procedure body.

### Tool name mapping

| Step | Claude | Grok |
|------|--------|------|
| Disambiguation question | `AskUserQuestion` when available | `ask_user_question` when available |
| Read catalog / write scaffold | standard file tools | standard file tools |
| Build | shell: `dotnet build` | shell: `dotnet build` |

Procedure text should name the conceptual step (“ask one disambiguating question”) and note
the platform tool only where necessary so the two files stay nearly identical.

## Implementation approach

**Approach 1 — Spec-first dual package, single authoring pass:**

1. Write shared `pattern-catalog.md` from the tables above.
2. Author the procedure once; place it in both `SKILL.md` files with only frontmatter /
   tool-name deltas.
3. Light smoke verification (below) — not a full writing-skills RED/GREEN campaign for v1.

## Verification

1. Both skill paths exist with valid frontmatter (`name`, `description`).
2. Catalog path resolves from repo root.
3. Smoke scenarios (in-session or mental dry-run):
   - Clear Decorator-like task → Decorator primary; Proxy/Chain as plausible runners-up
   - Ambiguous Strategy vs State → exactly one disambiguation question
   - Task with no extension axis → explicit “no pattern needed”
4. Optional live confirm → `Scenarios/<TaskName>/Implementation.cs` + successful
   `dotnet build`.
5. Confirm no accidental edits to `README.md`, `Program.cs`, or the 23 pattern folders.

## Success criteria

- Identical recommend/scaffold/build behavior on Claude and Grok.
- Decision knowledge has a single file owner (`pattern-catalog.md`).
- Procedure drift is mitigated by an explicit dual-edit maintenance rule.

## Relation to prior design

| Topic | 2026-07-22 | 2026-07-23 (this doc) |
|-------|------------|------------------------|
| Flow, catalog tables, scaffolding | Authoritative content source | Preserved |
| Skill package path | `.claude/skills/choose-pattern/` only, catalog inside that tree | Dual `.claude/` + `.grok/`; catalog at `skills/choose-pattern/references/` |
| Runtime support | Claude Code | Claude Code + Grok |

## Out of scope

- Changes to the 23 existing canonical pattern folders, `README.md`, or `Program.cs`
- Multi-language support
- Combo-pattern scaffolding
- User-global skill installs as the primary distribution model
- Full subagent pressure-testing of the skill (may be a later enhancement)
