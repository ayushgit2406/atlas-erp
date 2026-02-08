# Atlas ERP — Architecture

This document describes the system architecture, boundaries, and interaction patterns. It is **descriptive**, not a task list.

---

## 1. System model (C4-style summary)

### Actors
- Human operators (Ops/Admin/Finance/Approver)
- External sandbox payment provider (webhook source)

### Containers
- API Gateway (NestJS/TS)
- Domain services (.NET): Orders, Inventory, Billing, Workflow
- Reporting service (FastAPI)
- Keycloak (auth)
- Broker (Kafka)
- Postgres (per service) + Postgres (reporting)
- Redis

---

## 2. Boundaries and ownership

### Rules
- A domain service is the **only writer** to its database.
- Other services learn about changes via **events**.
- Reporting is **read-only** and never drives authoritative decisions.

### Anti-patterns (explicitly forbidden)
- Cross-service DB reads/writes
- Sharing domain model assemblies between services (beyond DTO/contracts)
- Synchronous “call chains” as correctness mechanism (events coordinate correctness)

---

## 3. Interaction patterns

### Command path (write)
Client → Gateway → Service (REST command) → Service DB transaction → Outbox → Broker publish

### Reaction path (async)
Broker → Consumer service → consumer DB transaction → outbox → publish follow-up events (if needed)

### Read path
Client → Reporting API (REST; GraphQL optional later) → Reporting DB (read models)

---

## 4. Consistency model and trade-offs

- Core services aim for **CP behavior** during partitions (prefer correctness over availability).
- Eventing and reporting are **AP-leaning** (allow delay/staleness; recover via retries).

CAP details are captured in: `docs/THREAT_MODEL.md` and relevant ADRs.

---

## 5. Reliability primitives

Mandatory primitives:
- correlationId propagation across HTTP and events
- idempotent consumers
- outbox pattern for publishers
- explicit retry policy and backoff strategy (exponential backoff with jitter; max 5 retries)
- dead-lettering strategy (DLQ topic `atlas.<domain>.dlq.v1`)

---

## 6. Extensibility

Adding a new domain service requires:
- its own DB, migrations, and health checks
- event contracts and consumers
- documented ownership boundaries
- ADR if a new technology is introduced

_Last updated: 2026-02-08_
