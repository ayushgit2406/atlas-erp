# Atlas ERP — Security

This document describes authentication, authorization, and core security standards.

> No assumptions are made about exact realm/client names until Keycloak config is implemented.
> Placeholders are marked **[FILL AFTER IMPLEMENTATION]**.

---

## 1. Security goals

- Ensure only authenticated users can execute commands
- Enforce least privilege access (RBAC)
- Provide traceability for audit (who did what, when)
- Protect secrets and sensitive configuration
- Maintain secure defaults for HTTP, events, and admin surfaces

---

## 2. Authentication (AuthN)

### 2.1 Identity provider
- Keycloak (OIDC)

### 2.2 Token validation
- The API Gateway validates JWT signature, issuer, audience, and expiry.
- Services validate JWT claims relevant to authorization (defense-in-depth).
  - Re-validate signatures in services if required by policy.

---

## 3. Authorization (AuthZ)

### 3.1 Role model
**[DECISION REQUIRED]** define roles and map to permissions.
Template roles:
- Admin
- Ops
- Finance
- Approver
- Viewer

### 3.2 Permission model
Use a scope-based permission model (example):
- `orders:read`, `orders:write`
- `inventory:read`, `inventory:manage`
- `billing:read`, `billing:manage`
- `workflow:read`, `workflow:approve`

Define role → scopes mapping in: `docs/security-rbac-matrix.md` (generated as part of this doc set).

---

## 4. Service-to-service security

Service-to-service communication is **event-driven** via Kafka. Direct internal HTTP is avoided for correctness flows to reduce coupling.

If internal HTTP is used:
- service principals must be issued by Keycloak
- requests must propagate correlation IDs
- sensitive endpoints must require service-level scopes

---

## 5. Secrets management

Rules:
- secrets never committed
- `.env.example` provided
- rotate credentials periodically

Secrets mechanism:
- local: **.env + Docker secrets**
- prod-like: **SOPS (age)**; Vault optional later

---

## 6. Auditability requirements

- All command handlers must log:
  - authenticated principal
  - action
  - aggregateId
  - correlationId
- Workflow approvals must store immutable decision records.
- Billing must store immutable ledger entries if ledger approach is chosen.

---

## 7. Security headers (Gateway)

- HSTS (if TLS)
- X-Content-Type-Options
- X-Frame-Options (if relevant)
- CORS policy (**[DECISION REQUIRED]**)

---

## 8. Vulnerability management

- Dependabot (or equivalent)
- SAST checks: **CodeQL**
- Container scanning: **Trivy**

---

_Last updated: 2026-02-08_
