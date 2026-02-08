# ADR-0004: Keycloak OIDC + RBAC Enforcement

- Status: Accepted
- Date: 2026-01-30
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/SECURITY.md
  - docs/security-rbac-matrix.md

## Context
Enterprise systems require centralized identity, enforceable authorization, and auditability.

## Decision
- Use **Keycloak** as the OIDC provider.
- Gateway validates JWT and enforces coarse route-level protection.
- Services validate JWT claims and enforce fine-grained authorization (defense-in-depth).
- RBAC roles map to scopes as defined in the RBAC matrix.

## Consequences
- Keycloak realm/client setup is required in infra.
- Services must implement auth middleware/policies.

## Follow-ups
- [ ] Implement minimal realm + client + roles for local dev
