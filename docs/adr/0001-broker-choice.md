# ADR-0001: Broker Choice — Kafka vs RabbitMQ

- Status: Accepted
- Date: 2026-01-30
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/BUILD.md
  - docs/ARCHITECTURE.md
  - docs/events/README.md
  - docs/OPERATIONS.md

## Context
Atlas ERP is event-driven. The broker must support durable event retention and replay to:
- coordinate sagas and compensation
- rebuild reporting projections
- support audit/debugging workflows

## Decision
Use **Apache Kafka** as the event broker.

## Options considered
1. Apache Kafka
2. RabbitMQ

## Pros/cons
### Apache Kafka
- Pros:
  - Replay via offsets enables deterministic projection rebuilds
  - Consumer groups + partitions support scaling
  - Strong enterprise event-streaming backbone
- Cons:
  - Operational complexity (topics/partitions/offsets)
  - Local setup is heavier than RabbitMQ

### RabbitMQ
- Pros:
  - Simpler local ops
  - Flexible routing
- Cons:
  - Replay/long-lived event log semantics are less idiomatic than Kafka

## Consequences
- Adopt versioned Kafka topics per domain (see docs/events/naming.md)
- Implement outbox publishers + idempotent consumers
- Add Kafka runbooks for lag and replay

## Follow-ups
- [x] Update docs/events/naming.md
- [ ] Add broker-specific operational steps in runbooks during implementation
