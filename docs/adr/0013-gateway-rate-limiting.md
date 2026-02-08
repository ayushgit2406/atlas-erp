# ADR-0013: Gateway Rate Limiting

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/SECURITY.md
  - docs/THREAT_MODEL.md

## Context
Rate limiting is a common control, but early development focuses on core correctness.

## Decision
Defer rate limiting for v1; implement later if needed.

## Consequences
- Gateway remains thin in early phases.
- Rate limiting can be added as a discrete feature later.

## Follow-ups
- [ ] Revisit once public endpoints stabilize
