# Non-Goals and Explicit Exclusions

This document lists technologies and features intentionally **not** included in Atlas ERP,
to avoid ambiguity during review.

## Data stores
- **MongoDB** — not used as a system-of-record or read model.
  Reason: ERP data benefits from relational integrity, auditability, and ACID guarantees.

## APIs
- **GraphQL (early)** — deferred until read models stabilize.
- **gRPC** — not required without measured performance bottlenecks.

## Frontend
- No full UI or micro-frontends.
- Only minimal demo surfaces if required for walkthroughs.

## Cloud / SaaS
- No managed Kafka, DBs, or auth providers.
- System is designed to be self-hosted and cost-free.

## Payments
- No real payment gateways or PCI scope.
- Billing is sandboxed and event-driven.

These exclusions are intentional and documented to demonstrate architectural restraint.
