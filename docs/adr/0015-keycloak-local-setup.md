# ADR-0015: Keycloak Local Setup Defaults

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/SECURITY.md
  - docs/security-rbac-matrix.md
  - docs/DEVELOPMENT.md

## Context
Local development needs a consistent Keycloak realm and client configuration.

## Decision
- Realm name: **atlas-erp**.
- Gateway client: **atlas-gateway**.
- Default roles: **Admin, Ops, Finance, Approver, Viewer** (per RBAC matrix).

## Consequences
- Local auth scripts and docs can use fixed names.
- Production can override via env/config.

## Follow-ups
- [ ] Add sample realm export once Keycloak config is scripted
