# ADR-0006: API Gateway Boundaries

- Status: Accepted
- Date: 2026-01-30
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/ARCHITECTURE.md
  - docs/api/README.md

## Context
Gateways can simplify client integration but risk becoming a monolith if they own business logic.

## Decision
Gateway responsibilities:
- JWT validation, coarse authorization
- routing to services
- header normalization (X-Correlation-Id, Idempotency-Key)
- optional rate limiting later

Gateway exclusions:
- domain invariants
- cross-domain business workflows
- authoritative data ownership

## Consequences
- Services remain authoritative for business correctness.
- Gateway remains thin and maintainable.

## Follow-ups
- [ ] Implement header propagation and request logging
