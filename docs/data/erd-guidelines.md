# ERD and Data Modeling Guidelines

## Principles
- Model invariants in the service DB where possible.
- Use explicit status enums.
- Use immutable history tables for auditability.
- Use unique constraints to enforce idempotency where appropriate.

## Deliverables
For each service:
- ERD diagram (can be ASCII/mermaid)
- Table definitions + rationale
- Index strategy

**Status**: Template. Populate during Phase 2+ as schemas are built.
