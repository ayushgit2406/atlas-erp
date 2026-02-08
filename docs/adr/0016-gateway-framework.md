# ADR-0016: API Gateway Framework (NestJS)

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/BUILD.md
  - docs/ARCHITECTURE.md
  - docs/DEVELOPMENT.md
  - docs/COST_AND_LICENSING.md

## Context
The API Gateway needs enterprise-grade structure for auth, cross-cutting concerns, and testing.

## Decision
- The API Gateway will be built with **NestJS** on **Node.js + TypeScript**.
- The **Express adapter** is the default for ecosystem compatibility.
- Scope is **gateway only** (no other Node services implied).

## Consequences
- Gateway scaffolding will use NestJS conventions and DI.
- NestJS CLI/tooling may be used for scaffolding (optional).

## Follow-ups
- [ ] Scaffold gateway with NestJS when implementation starts
