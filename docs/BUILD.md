# Atlas ERP — Build Document

> **Purpose**: Defines *what* we are building, *why* each decision exists, and *how* the system is expected to be implemented.
> This is written for **enterprise-grade correctness** and **portfolio review by senior engineers**.

---

## 1. Project Intent

Atlas ERP is a modular, event-driven ERP backend intended to demonstrate:

- domain boundaries and ownership
- event-driven workflows with explicit compensation
- correctness-first and auditability-first design
- operational maturity (health checks, observability, runbooks)
- disciplined technology selection (no tool-collecting)

---

## 2. Scope

### 2.1 In-scope domains
- Order Management
- Inventory
- Billing & Payments (sandbox only)
- Workflow & Approvals
- Reporting / Analytics (read-only)

### 2.2 Out of scope
- Full admin UI and end-user UI (optional minimal demo surface only)
- Real money processing and PCI compliance
- Multi-region active-active deployment
- Vendor-managed SaaS dependencies (unless explicitly decided)

---

## 3. Architecture Summary

### 3.1 Components
- API Gateway: NestJS (Node.js + TypeScript, Express adapter)
- Domain Services: .NET 8 / C#
  - Order Service
  - Inventory Service
  - Billing Service
  - Workflow Service
- Event broker: **Apache Kafka**
- Reporting service: Python + FastAPI
- Auth: Keycloak (OIDC)
- Storage:
  - PostgreSQL (DB-per-service)
  - Redis (cache/idempotency)

### 3.2 Communication patterns
- Client → Gateway: REST (commands)
- Gateway → Services: REST (commands)
- Services → Broker: domain events (facts)
- Services ← Broker: event subscriptions (reactions)
- Billing ↔ External sandbox: webhooks
- Reporting ← Broker: event ingestion
- Client → Reporting: read-only queries (REST, optional GraphQL later)

---

## 4. Non-Negotiable Principles

1. **DB per service** (no shared DB)
2. **No shared business logic**
3. **Deterministic core** (ML can only advise)
4. **At-least-once event delivery** assumed → consumers must be idempotent
5. **Outbox pattern** (or equivalent) required for reliable event publication
6. **Read models are not authoritative**; service DBs remain source of truth
7. **Partition tolerance assumed**; explicit C vs A trade-offs documented

---

## 5. Service Responsibilities

### 5.1 Order Service
- owns order lifecycle and invariants
- emits: OrderPlaced, OrderCancelled, OrderConfirmed, OrderFulfilled

### 5.2 Inventory Service
- owns stock availability and reservations
- consumes: OrderPlaced, OrderCancelled
- emits: InventoryReserved, InventoryReservationFailed, InventoryReleased

### 5.3 Billing Service
- owns invoice creation and payment attempts (sandbox)
- consumes: InventoryReserved, InventoryReservationFailed
- emits: InvoiceCreated, PaymentSucceeded, PaymentFailed

### 5.4 Workflow Service
- owns approvals and escalation policies
- consumes: OrderPlaced / InvoiceCreated / PaymentFailed (policy-driven)
- emits: ApprovalRequested, ApprovalGranted, ApprovalRejected, EscalationTriggered

### 5.5 Reporting Service (FastAPI)
- consumes all domain events
- builds projections (read models)
- serves read-only APIs

---

## 6. Eventing

### 6.1 Event envelope (mandatory)
See `docs/events/envelope.md`.

### 6.2 Delivery semantics
- at-least-once delivery
- consumers must record processed eventIds (inbox table or equivalent)

### 6.3 Outbox pattern
See `docs/events/outbox.md`.

---

## 7. Distributed Workflows (Saga)

A primary saga coordinates:
OrderPlaced → InventoryReserved → InvoiceCreated → PaymentSucceeded → (optional) ApprovalGranted → OrderConfirmed

Failure scenarios and compensations are documented in:
- `docs/demos/order-saga-happy-path.md`
- `docs/demos/order-saga-failure-paths.md`

---

## 8. Build Phases (Roadmap)

1. Infrastructure bootstrap
2. Order service MVP
3. Inventory integration + reservations + compensation
4. Billing integration + webhook simulation
5. Workflow approvals + audit trail
6. Reporting projections + read APIs (GraphQL optional)
7. Production readiness (observability, failure drills, runbooks)
8. Optional ML (advisory only)

---

## 9. Decision Points (must be explicit)

All items below require explicit decisions and ADRs before implementation:

- **Broker**: Apache Kafka (locked)
- **Inventory policy**: Backorder on insufficient stock (locked)
- **Billing depth**: Ledger-style entries (locked)
- **Auth**: JWT validated by gateway and each service (locked)
- **API versioning**: /v1 routes for gateway (locked)
- **Idempotency**: Enforced by services; gateway forwards Idempotency-Key (locked)


- **Approval triggers**: policy-driven thresholds (documented in Workflow service; can evolve)
- **Read API shape**: REST-first; GraphQL deferred until read models stabilize (optional)


See ADR templates under `docs/adr/`.

---

## 10. Appendices

- A: Event catalog template — `docs/events/catalog.md`
- B: API conventions — `docs/api/README.md`
- C: Operations and runbooks — `docs/OPERATIONS.md`

_Last updated: 2026-01-30_
