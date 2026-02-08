# ADR-0005: Reporting Service and Read Models

- Status: Accepted
- Date: 2026-01-30
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/BUILD.md
  - docs/ARCHITECTURE.md

## Context
Atlas ERP is read-heavy. Serving complex queries from transactional DBs increases coupling and load.

## Decision
- Implement a Reporting Service (Python + FastAPI).
- Consume Kafka events and build projections in a dedicated reporting Postgres DB.
- Serve read APIs via REST first; GraphQL may be added after read models stabilize.

## Consequences
- Reporting is eventually consistent and non-authoritative.
- Projection rebuild is supported via Kafka replay.

## Follow-ups
- [ ] Define initial projection tables and endpoints
