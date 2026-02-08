# Atlas ERP — Documentation Index

This folder contains the documentation set for building and operating **Atlas ERP** as an enterprise-grade, event-driven ERP backend.

## How to use these docs (recommended reading order)
1. **BUILD.md** — authoritative build specification and scope
2. **ARCHITECTURE.md** — system boundaries, interaction patterns, and trade-offs
3. **adr/** — locked decisions (ADRs)
4. **events/** — event contracts, versioning, and outbox pattern
5. **api/** — HTTP contracts, idempotency, and error model
6. **SECURITY.md** — authentication, authorization, and auditability
7. **OBSERVABILITY.md** — logging, metrics, tracing, and debugging
8. **OPERATIONS.md** + **runbooks/** + **INCIDENT_RESPONSE.md** — day-2 operations and failure handling
9. **demos/** — scripted end-to-end scenarios

## Decision hygiene (no assumptions)
Some documents contain **[DECISION REQUIRED]** markers. These represent items that must be explicitly decided and recorded in an ADR before implementation.
See `docs/adr/README.md` for the decision register.

## Document map
- Core
  - BUILD.md
  - ARCHITECTURE.md
  - DEVELOPMENT.md
  - DEPLOYMENT.md
  - SECURITY.md
  - OBSERVABILITY.md
  - TESTING.md
  - OPERATIONS.md
  - QUALITY.md
  - RELEASE.md
  - INCIDENT_RESPONSE.md
  - THREAT_MODEL.md
- Contracts
  - events/ (event envelope, naming, versioning, catalog template)
  - api/ (API conventions, error model, idempotency)
  - data/ (migrations, retention, ERD guidelines)
- Governance
  - adr/ (decision records)
- Portfolio
  - demos/ (scripted walkthroughs)

_Last updated: 2026-02-08_
