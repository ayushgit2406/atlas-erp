# Atlas ERP — RBAC Matrix

This document defines the role-to-permission mapping.

> **Status**: Draft template. Replace **[DECISION REQUIRED]** items with explicit choices.

---

## 1. Roles (candidate set)

- Admin
- Ops
- Finance
- Approver
- Viewer

Final role list is locked for v1.

---

## 2. Permission scopes (candidate set)

Orders:
- orders:read
- orders:write
- orders:admin

Inventory:
- inventory:read
- inventory:manage

Billing:
- billing:read
- billing:manage

Workflow:
- workflow:read
- workflow:approve
- workflow:admin

Reporting:
- reporting:read

Scopes are enforced at services; roles are mapped to scopes in Keycloak using **client roles** for the gateway client.

---

## 3. Role → scopes mapping (fill in)

| Role | Scopes |
|------|--------|
| Admin | orders:admin, inventory:manage, billing:manage, workflow:admin, reporting:read |
| Ops | orders:write, orders:read, inventory:manage, workflow:read, reporting:read |
| Finance | billing:manage, billing:read, orders:read, reporting:read |
| Approver | workflow:approve, workflow:read, orders:read, reporting:read |
| Viewer | orders:read, inventory:read, billing:read, workflow:read, reporting:read |

---

## 4. Enforcement points

- Gateway: coarse-grained route protection
- Services: fine-grained authorization at command handlers and sensitive queries
- Reporting: read-only (still requires auth)

_Last updated: 2026-01-30_
