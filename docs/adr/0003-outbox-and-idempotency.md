# ADR-0003: Outbox Pattern and Idempotency

- Status: Accepted
- Date: 2026-01-30
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/events/outbox.md
  - docs/api/idempotency.md
  - docs/runbooks/outbox-backlog.md

## Context
Atlas ERP assumes retries and at-least-once delivery. Without safeguards, duplicates can corrupt state.

## Decision
- Producers use an **Outbox** table written in the same DB transaction as domain changes.
- Consumers store processed `eventId` in an **Inbox** table to ensure idempotency.
- HTTP commands support `Idempotency-Key`, enforced by **services**.

## Options considered
1. Best-effort publish (no outbox)
2. Distributed transactions
3. Outbox + Inbox (selected)

## Consequences
- Each service DB includes `outbox_events`.
- Each consumer includes an inbox store keyed by eventId.
- Runbooks cover outbox backlog and replay.

## Follow-ups
- [ ] Implement outbox publisher worker per service
- [ ] Implement inbox table per consumer
