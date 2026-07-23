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
