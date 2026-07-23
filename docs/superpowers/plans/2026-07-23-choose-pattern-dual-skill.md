# choose-pattern dual skill (Claude + Grok) Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ship a project skill `choose-pattern` that recommends the best-fitting GoF pattern for a free-text task and, on confirm, scaffolds a C# scenario — available identically under Claude Code and Grok.

**Architecture:** Dual in-repo skill entrypoints (`.claude/skills/choose-pattern/SKILL.md` and `.grok/skills/choose-pattern/SKILL.md`) each contain the full procedure. Decision knowledge lives once at `skills/choose-pattern/references/pattern-catalog.md`. Agents read that catalog, score the task, optionally ask one disambiguation question, recommend, then scaffold `Scenarios/<TaskName>/Implementation.cs` and run `dotnet build`. No changes to the 23 canonical pattern folders, `README.md`, or `Program.cs`.

**Tech Stack:** Markdown Agent Skills (`SKILL.md` + YAML frontmatter), repo-relative reference docs, C# / .NET 6 SDK-style project (`Desing_Patterns.csproj`), `dotnet build`.

**Spec:** `docs/superpowers/specs/2026-07-23-choose-pattern-dual-skill-design.md`

## Global Constraints

- Catalog path is always `skills/choose-pattern/references/pattern-catalog.md` (repo root relative). Never copy the catalog into `.claude/` or `.grok/`.
- Both `SKILL.md` files must contain the full procedure (D1). Only frontmatter and ask-user tool names may differ.
- Procedure edits must be applied to **both** entrypoints in the same commit when possible.
- Scaffolds are C# only under `Scenarios/<PascalCaseTaskName>/Implementation.cs`.
- Never modify `README.md`, `Program.cs`, or folders under `Creational/`, `Structural/`, `Behavioral/`.
- One primary pattern scaffold per invocation (may mention combos in prose only).
- Skill `name`: `choose-pattern` (lowercase, hyphens only).
- Description focuses on **when to use**, not a workflow summary agents might follow instead of the body.

## File structure

| Path | Responsibility |
|------|----------------|
| `skills/choose-pattern/references/pattern-catalog.md` | Single source of 23-pattern decision data (intents, triggers, confusions, refs) |
| `.claude/skills/choose-pattern/SKILL.md` | Claude entrypoint: frontmatter + full procedure |
| `.grok/skills/choose-pattern/SKILL.md` | Grok entrypoint: frontmatter + full procedure (same body, tool name delta) |

No scripts, no tests project, no `.csproj` changes.

---

### Task 1: Shared pattern catalog

**Files:**
- Create: `skills/choose-pattern/references/pattern-catalog.md`

**Interfaces:**
- Consumes: catalog tables from `docs/superpowers/specs/2026-07-23-choose-pattern-dual-skill-design.md` (section “Catalog content”)
- Produces: markdown file agents load at skill runtime; must include all 23 patterns with Category, Intent, Trigger signals, Common confusions, Reference path

- [ ] **Step 1: Create directories**

From repo root (`D:\Desing-Patterns` or workspace root):

```powershell
New-Item -ItemType Directory -Force -Path "skills\choose-pattern\references"
```

Expected: path exists; no error if already present.

- [ ] **Step 2: Write the full catalog file**

Write `skills/choose-pattern/references/pattern-catalog.md` with **exactly** this content (do not omit patterns):

```markdown
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
```

- [ ] **Step 3: Verify catalog completeness**

Run from repo root (PowerShell):

```powershell
$catalog = Get-Content "skills\choose-pattern\references\pattern-catalog.md" -Raw
$patterns = @(
  '### Singleton','### Factory Method','### Abstract Factory','### Builder','### Prototype',
  '### Adapter','### Bridge','### Composite','### Decorator','### Facade','### Flyweight','### Proxy',
  '### Chain of Responsibility','### Command','### Interpreter','### Iterator','### Mediator',
  '### Observer','### State','### Strategy','### Template Method','### Visitor','### Memento'
)
$missing = $patterns | Where-Object { $catalog -notlike "*$_*" }
if ($missing) { Write-Error "Missing: $($missing -join ', ')" } else { Write-Output "OK: 23 patterns present" }
```

Expected: `OK: 23 patterns present`

- [ ] **Step 4: Commit**

```powershell
git add skills/choose-pattern/references/pattern-catalog.md
git commit -m "Add shared GoF pattern catalog for choose-pattern skill"
```

---

### Task 2: Claude skill entrypoint

**Files:**
- Create: `.claude/skills/choose-pattern/SKILL.md`

**Interfaces:**
- Consumes: catalog at `skills/choose-pattern/references/pattern-catalog.md`
- Produces: Claude-discoverable skill `/choose-pattern` with full recommend → scaffold → build procedure
- Ask-user tool name in this file: `AskUserQuestion` (when available)

- [ ] **Step 1: Create directory**

```powershell
New-Item -ItemType Directory -Force -Path ".claude\skills\choose-pattern"
```

- [ ] **Step 2: Write Claude SKILL.md**

Write `.claude/skills/choose-pattern/SKILL.md` with **exactly** this content:

```markdown
---
name: choose-pattern
description: >
  Use when the user asks which GoF or design pattern fits a task, wants help
  picking among creational, structural, or behavioral patterns, runs
  /choose-pattern, or describes a design problem and needs a pattern
  recommendation (and optional C# scaffold) in this Design Patterns repo.
---

# Choose Pattern

Recommend the best-fitting GoF pattern from this repo's catalog for a free-text
task, then optionally scaffold a task-domain C# example under `Scenarios/`.

**Maintenance:** Procedure changes must also be applied to
`.grok/skills/choose-pattern/SKILL.md` in the same change when possible.
Catalog content is edited only in `skills/choose-pattern/references/pattern-catalog.md`.

## When to use

- User asks "which pattern should I use for …"
- User runs `/choose-pattern`
- User describes a design/variability problem and wants a GoF recommendation

## When not to use

- User wants to edit the 23 canonical reference implementations
- User wants multi-language scaffolds (this skill is C# only)
- User already named a pattern and only wants explanation of that one (answer directly; optional: still point at Reference)

## Procedure

### 1. Load the catalog

Read (from repo root):

`skills/choose-pattern/references/pattern-catalog.md`

Do not invent patterns outside this catalog. Do not copy the catalog into skill folders.

### 2. Score the task

From the user's free-text description, match against each pattern's **Trigger signals**.
Rank top candidates. Prefer signals that match the real variability/extension axis
in the task (what changes, what stays fixed).

### 3. Disambiguate if needed

- **Clear winner:** skip to step 4.
- **Two (rarely three) close patterns:** ask **exactly one** disambiguating question
  drawn from those patterns' **Common confusions** entries.
  - Prefer structured `AskUserQuestion` when available.
  - Otherwise ask in plain chat.
- Never ask more than one round of disambiguation before recommending.

### 4. Recommend (or decline)

Present:

1. **Primary recommendation** — pattern name + rationale tied to *this* task
2. **Runner-up(s)** — each with the specific trade-off that ruled it out
3. **Reference path(s)** from the catalog for reading
4. If the task has no real variability/extension axis (fixed single algorithm,
   no swap points, no structural mismatch to integrate): say **no pattern needed**
   and stop. Do not force-fit.

If the task idiomatically combines patterns, mention that in prose but pick **one**
primary for scaffolding.

### 5. Confirm before scaffolding

Ask whether to scaffold a starting implementation for the primary (or a runner-up
the user prefers). If the user declines, stop after the recommendation.

### 6. Scaffold

On confirmation:

1. Derive `<TaskName>` in PascalCase from the task (e.g. "payment retry with logging" → `PaymentRetryLogging`).
2. Create `Scenarios/<TaskName>/Implementation.cs`.
3. Model the **user's domain**, not a copy of the catalog Reference textbook example.
4. Follow this shape:

```csharp
//<PatternName> - <intent, one line>
//  Use cases: <use case, one line>
namespace <TaskName>;

//Real life example: <one-line description of the actual task>

/// <summary>
/// <GoF role, e.g. ConcreteDecorator>
/// </summary>
public class ... { ... }
```

5. Every type gets a `/// <summary>GoF role</summary>` (Context, Strategy, ConcreteStrategy, Component, Decorator, etc.).
6. Do **not** modify `README.md`, `Program.cs`, or anything under `Creational/`, `Structural/`, `Behavioral/`.
7. Do **not** edit `Desing_Patterns.csproj` (SDK-style glob includes new `.cs` files).

### 7. Build check

From repo root:

```powershell
dotnet build
```

Report pass/fail (and errors if any). Fix scaffold compile errors if introduced by the scaffold itself.

## Hard rules

- Never force a pattern when none fits.
- Never scaffold more than one primary pattern per invocation.
- Never copy the Reference `Implementation.cs` wholesale into `Scenarios/`.
- Catalog is read-only decision input; keep the 23-pattern catalog pristine.
```

- [ ] **Step 3: Verify frontmatter and catalog path**

```powershell
$skill = Get-Content ".claude\skills\choose-pattern\SKILL.md" -Raw
if ($skill -notmatch '(?m)^name:\s*choose-pattern\s*$') { Write-Error 'bad name' }
if ($skill -notmatch 'skills/choose-pattern/references/pattern-catalog\.md') { Write-Error 'missing catalog path' }
if ($skill -notmatch 'AskUserQuestion') { Write-Error 'missing AskUserQuestion' }
if ($skill -notmatch 'Scenarios/') { Write-Error 'missing scaffold path' }
Write-Output 'OK: Claude skill entrypoint'
```

Expected: `OK: Claude skill entrypoint`

- [ ] **Step 4: Commit**

```powershell
git add .claude/skills/choose-pattern/SKILL.md
git commit -m "Add Claude choose-pattern skill entrypoint"
```

---

### Task 3: Grok skill entrypoint

**Files:**
- Create: `.grok/skills/choose-pattern/SKILL.md`

**Interfaces:**
- Consumes: same catalog path as Task 1
- Produces: Grok-discoverable skill `/choose-pattern` with procedure identical to Task 2 except ask-user tool name
- Ask-user tool name in this file: `ask_user_question` (when available)
- Procedure body must match Claude's steps 1–7 and Hard rules; only frontmatter extras and tool name differ

- [ ] **Step 1: Create directory**

```powershell
New-Item -ItemType Directory -Force -Path ".grok\skills\choose-pattern"
```

- [ ] **Step 2: Write Grok SKILL.md**

Write `.grok/skills/choose-pattern/SKILL.md` with **exactly** this content:

```markdown
---
name: choose-pattern
description: >
  Use when the user asks which GoF or design pattern fits a task, wants help
  picking among creational, structural, or behavioral patterns, runs
  /choose-pattern, or describes a design problem and needs a pattern
  recommendation (and optional C# scaffold) in this Design Patterns repo.
when-to-use: >
  Use when asked which design pattern / GoF pattern to use, help picking a
  pattern, /choose-pattern, or pattern recommendation for a coding task.
---

# Choose Pattern

Recommend the best-fitting GoF pattern from this repo's catalog for a free-text
task, then optionally scaffold a task-domain C# example under `Scenarios/`.

**Maintenance:** Procedure changes must also be applied to
`.claude/skills/choose-pattern/SKILL.md` in the same change when possible.
Catalog content is edited only in `skills/choose-pattern/references/pattern-catalog.md`.

## When to use

- User asks "which pattern should I use for …"
- User runs `/choose-pattern`
- User describes a design/variability problem and wants a GoF recommendation

## When not to use

- User wants to edit the 23 canonical reference implementations
- User wants multi-language scaffolds (this skill is C# only)
- User already named a pattern and only wants explanation of that one (answer directly; optional: still point at Reference)

## Procedure

### 1. Load the catalog

Read (from repo root):

`skills/choose-pattern/references/pattern-catalog.md`

Do not invent patterns outside this catalog. Do not copy the catalog into skill folders.

### 2. Score the task

From the user's free-text description, match against each pattern's **Trigger signals**.
Rank top candidates. Prefer signals that match the real variability/extension axis
in the task (what changes, what stays fixed).

### 3. Disambiguate if needed

- **Clear winner:** skip to step 4.
- **Two (rarely three) close patterns:** ask **exactly one** disambiguating question
  drawn from those patterns' **Common confusions** entries.
  - Prefer structured `ask_user_question` when available.
  - Otherwise ask in plain chat.
- Never ask more than one round of disambiguation before recommending.

### 4. Recommend (or decline)

Present:

1. **Primary recommendation** — pattern name + rationale tied to *this* task
2. **Runner-up(s)** — each with the specific trade-off that ruled it out
3. **Reference path(s)** from the catalog for reading
4. If the task has no real variability/extension axis (fixed single algorithm,
   no swap points, no structural mismatch to integrate): say **no pattern needed**
   and stop. Do not force-fit.

If the task idiomatically combines patterns, mention that in prose but pick **one**
primary for scaffolding.

### 5. Confirm before scaffolding

Ask whether to scaffold a starting implementation for the primary (or a runner-up
the user prefers). If the user declines, stop after the recommendation.

### 6. Scaffold

On confirmation:

1. Derive `<TaskName>` in PascalCase from the task (e.g. "payment retry with logging" → `PaymentRetryLogging`).
2. Create `Scenarios/<TaskName>/Implementation.cs`.
3. Model the **user's domain**, not a copy of the catalog Reference textbook example.
4. Follow this shape:

```csharp
//<PatternName> - <intent, one line>
//  Use cases: <use case, one line>
namespace <TaskName>;

//Real life example: <one-line description of the actual task>

/// <summary>
/// <GoF role, e.g. ConcreteDecorator>
/// </summary>
public class ... { ... }
```

5. Every type gets a `/// <summary>GoF role</summary>` (Context, Strategy, ConcreteStrategy, Component, Decorator, etc.).
6. Do **not** modify `README.md`, `Program.cs`, or anything under `Creational/`, `Structural/`, `Behavioral/`.
7. Do **not** edit `Desing_Patterns.csproj` (SDK-style glob includes new `.cs` files).

### 7. Build check

From repo root:

```powershell
dotnet build
```

Report pass/fail (and errors if any). Fix scaffold compile errors if introduced by the scaffold itself.

## Hard rules

- Never force a pattern when none fits.
- Never scaffold more than one primary pattern per invocation.
- Never copy the Reference `Implementation.cs` wholesale into `Scenarios/`.
- Catalog is read-only decision input; keep the 23-pattern catalog pristine.
```

- [ ] **Step 3: Verify Grok entrypoint and dual-edit cross-links**

```powershell
$g = Get-Content ".grok\skills\choose-pattern\SKILL.md" -Raw
$c = Get-Content ".claude\skills\choose-pattern\SKILL.md" -Raw
if ($g -notmatch 'ask_user_question') { Write-Error 'Grok missing ask_user_question' }
if ($c -notmatch 'AskUserQuestion') { Write-Error 'Claude missing AskUserQuestion' }
if ($g -notmatch 'skills/choose-pattern/references/pattern-catalog\.md') { Write-Error 'Grok missing catalog path' }
if ($g -notmatch '\.claude/skills/choose-pattern/SKILL\.md') { Write-Error 'Grok missing dual-edit note' }
if ($c -notmatch '\.grok/skills/choose-pattern/SKILL\.md') { Write-Error 'Claude missing dual-edit note' }
# Procedure cores should match after stripping tool-name tokens
$norm = {
  param($t)
  ($t -replace '(?s)^---.*?---\s*','' -replace 'AskUserQuestion','ASKTOOL' -replace 'ask_user_question','ASKTOOL' -replace 'when-to-use:.*?(?=\n---|\n#)','').Trim()
}
# Soft check: both mention same scaffold path and hard rules
foreach ($s in @($g,$c)) {
  if ($s -notmatch 'Scenarios/<TaskName>/Implementation\.cs' -and $s -notmatch 'Scenarios/`?<TaskName>`?/Implementation') {
    if ($s -notmatch 'Scenarios/') { Write-Error 'missing Scenarios' }
  }
  if ($s -notmatch 'Never force a pattern') { Write-Error 'missing hard rules' }
}
Write-Output 'OK: Grok skill entrypoint + dual maintenance notes'
```

Expected: `OK: Grok skill entrypoint + dual maintenance notes`

- [ ] **Step 4: Commit**

```powershell
git add .grok/skills/choose-pattern/SKILL.md
git commit -m "Add Grok choose-pattern skill entrypoint"
```

---

### Task 4: Package verification (smoke)

**Files:**
- Verify only (no new files required)
- Optional live scaffold under `Scenarios/` only if executing a real skill run; default smoke does **not** leave scenario files unless intentionally testing build

**Interfaces:**
- Consumes: artifacts from Tasks 1–3
- Produces: pass/fail checklist aligned with spec verification section

- [ ] **Step 1: Layout check**

```powershell
$required = @(
  'skills\choose-pattern\references\pattern-catalog.md',
  '.claude\skills\choose-pattern\SKILL.md',
  '.grok\skills\choose-pattern\SKILL.md'
)
foreach ($p in $required) {
  if (-not (Test-Path $p)) { Write-Error "Missing $p" }
}
# Catalog must not be duplicated under runtime skill folders
if (Test-Path '.claude\skills\choose-pattern\references') { Write-Error 'Do not copy catalog into .claude' }
if (Test-Path '.grok\skills\choose-pattern\references') { Write-Error 'Do not copy catalog into .grok' }
Write-Output 'OK: layout'
```

Expected: `OK: layout`

- [ ] **Step 2: Mental / dry-run recommendation matrix**

Without writing code, confirm the skill procedure + catalog would yield:

| Prompt (summary) | Expected primary | Notes |
|------------------|------------------|-------|
| Stack logging + caching + auth around an HTTP client, same interface, combine per env | Decorator | Runners-up: Proxy (access gate), Chain (single handler) |
| Object behavior flips through Approved/Pending/Rejected by itself at runtime | State | Disambiguate vs Strategy if client injects the mode |
| Compute tax with a fixed formula and no swap points | No pattern needed | Force-fit forbidden |

Document nothing permanent; this step is a reviewer checklist while implementing.

- [ ] **Step 3: Optional live scaffold build (only if running end-to-end)**

If the implementer runs a live confirmation test, use a throwaway domain and delete afterward **or** keep one intentional sample:

Example task: "decorate a weather API client with logging and retry"

Scaffold would create e.g. `Scenarios/WeatherApiLoggingRetry/Implementation.cs`, then:

```powershell
dotnet build
```

Expected: `Build succeeded` (0 errors).

If this step is skipped, repo `dotnet build` must still succeed with no scenario added:

```powershell
dotnet build
```

Expected: `Build succeeded`.

- [ ] **Step 4: Confirm pristine catalog folders**

```powershell
git status --porcelain
```

Expected: either clean, or only intentional skill/catalog files from this plan. No unexpected changes under `Creational/`, `Structural/`, `Behavioral/`, `README.md`, or `Program.cs`.

- [ ] **Step 5: Final commit if any verification notes or fixes**

Only if Step 3–4 produced fixes:

```powershell
git add -A
git status
git commit -m "Fix choose-pattern skill package after smoke verification"
```

If clean, skip.

---

## Plan self-review

### Spec coverage

| Spec requirement | Task |
|------------------|------|
| Dual `.claude/` + `.grok/` install | Tasks 2, 3 |
| Shared catalog at `skills/choose-pattern/references/pattern-catalog.md` | Task 1 |
| Full procedure in both SKILL.md (D1) | Tasks 2, 3 |
| Recommend → one question → confirm → scaffold → build | Tasks 2, 3 procedure |
| 23-pattern tables / triggers / confusions / refs | Task 1 full content |
| Scaffold conventions + GoF role summaries | Tasks 2, 3 step 6 |
| No README / Program.cs / canonical folder edits | Hard rules in both skills; Task 4 layout |
| Frontmatter name + when-to-use description | Tasks 2, 3 |
| Platform tool names AskUserQuestion / ask_user_question | Tasks 2, 3 |
| Dual-edit maintenance rule | Both SKILL.md headers |
| Smoke verification | Task 4 |
| Approach 1 single authoring pass | Tasks ordered catalog → Claude → Grok → verify |

### Placeholder scan

No TBD/TODO/"similar to Task N" gaps; full file contents inlined.

### Consistency

- Catalog path identical in both skills and Task 1.
- Skill name `choose-pattern` everywhere.
- Scaffold path `Scenarios/<TaskName>/Implementation.cs` everywhere.
- Tool names differ only as specified.

---

## Execution notes

Implement Tasks 1→4 in order. Prefer one commit per task as written.

After the plan is executed, both runtimes should auto-discover `/choose-pattern` from project skill folders (Claude: `.claude/skills/`, Grok: `.grok/skills/`).
