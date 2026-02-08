# ADRs — Architecture Decision Records

ADRs capture *why* we chose something, alternatives considered, and consequences.

## Decision register (accepted)
Keep this list current as decisions are accepted.
- ADR-0001: Broker choice (Kafka)
- ADR-0002: Event envelope + versioning + Avro
- ADR-0003: Outbox + inbox idempotency
- ADR-0004: Keycloak OIDC + RBAC
- ADR-0005: Reporting service + read models
- ADR-0006: API gateway boundaries
- ADR-0007: Migration tooling
- ADR-0008: Event retry and DLQ policy
- ADR-0009: Pagination model
- ADR-0010: Registry and secrets strategy
- ADR-0011: Development workflow
- ADR-0012: Security scanning tools
- ADR-0013: Gateway rate limiting (deferred)
- ADR-0014: Backups and on-call SLAs
- ADR-0015: Keycloak local setup defaults
- ADR-0016: API gateway framework (NestJS)

## Workflow
1. Create a new ADR with the next sequence number.
2. Fill it completely before implementing the decision.
3. Link to affected docs (BUILD, EVENTS, API, DATA).
4. Never delete ADRs; supersede them with a new ADR.

## Template
See `0000-template.md`.

_Last updated: 2026-02-08_
