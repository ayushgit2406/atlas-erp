# Atlas ERP — Threat Model (High-Level)

This is a lightweight threat model to demonstrate security maturity.
It is not a substitute for formal audits.

---

## 1. Assets

- Authentication tokens (JWT)
- Service databases (orders/inventory/billing/workflow/reporting)
- Event stream (contains business facts)
- Audit trails (approvals, billing ledger)
- Secrets (DB passwords, client secrets)

---

## 2. Trust boundaries

- Public network boundary: client ↔ gateway
- Internal boundary: gateway ↔ services
- Broker boundary: producers/consumers
- Admin boundary: Keycloak admin access

---

## 3. Threats (STRIDE template)

### Spoofing
- forged JWT tokens
- service identity spoofing

Mitigations:
- strict JWT validation (issuer, audience)
- short-lived tokens
- service-to-service auth (**[DECISION REQUIRED]**)

### Tampering
- event payload modification in transit
- DB tampering via exposed ports

Mitigations:
- Kafka auth + TLS (**[DECISION REQUIRED]**)
- restrict network exposure
- DB credentials least privilege

### Repudiation
- user denies having performed an action

Mitigations:
- audit logging with principal + correlationId
- immutable records for approvals/ledger

### Information disclosure
- logs containing PII or secrets
- unsecured admin endpoints

Mitigations:
- log redaction rules
- secure admin surfaces

### Denial of service
- request floods
- event consumer backlog

Mitigations:
- rate limiting at gateway (deferred for v1)
- consumer scaling strategy
- backpressure and DLQ policies

### Elevation of privilege
- insufficient RBAC enforcement

Mitigations:
- explicit RBAC matrix
- centralized policy tests (**[DECISION REQUIRED]**)

---

## 4. Security verification

- dependency scanning
- container scanning
- SAST/DAST (**[DECISION REQUIRED]**)

_Last updated: 2026-02-08_
