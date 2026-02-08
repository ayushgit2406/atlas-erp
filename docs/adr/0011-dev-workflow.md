# ADR-0011: Development Workflow

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/DEVELOPMENT.md
  - docs/QUALITY.md

## Context
Consistent workflow reduces friction and keeps quality predictable across services.

## Decision
- Branching model: **trunk‑based** (short‑lived branches).
- Commit conventions: **Conventional Commits**.

## Consequences
- CI must run on main and short‑lived branches.
- Release notes can be generated later if desired.

## Follow-ups
- [ ] Add commit message examples in CONTRIBUTING (if added)
