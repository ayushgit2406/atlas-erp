# ADR-0008: Event Retry and DLQ Policy

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/events/outbox.md
  - docs/ARCHITECTURE.md
  - docs/runbooks/outbox-backlog.md

## Context
At-least-once delivery requires clear retry and poison message handling to protect data correctness.

## Decision
- Retry strategy: **exponential backoff with jitter**.
- Max retries before DLQ: **5** per handler.
- DLQ topic naming: **`atlas.<domain>.dlq.v1`**.

## Consequences
- Consumers must track retry counts per eventId/handler.
- Runbooks must include DLQ inspection and replay steps.

## Follow-ups
- [ ] Implement retry counters in consumers
- [ ] Add DLQ replay runbook steps when Kafka tooling is selected
