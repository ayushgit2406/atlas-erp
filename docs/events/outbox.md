# Outbox and Inbox Patterns

## Outbox (publisher reliability)
Problem: DB commit succeeds but event publish fails.

Solution:
- Write domain state changes and an outbox row in one DB transaction.
- A background publisher reads pending outbox rows and publishes them.
- Mark outbox row as published (or keep status + retry count).

## Inbox (consumer idempotency)
Problem: at-least-once delivery causes duplicate event processing.

Solution:
- Before applying an event, check if `eventId` already processed.
- Store processed `eventId` in an inbox table with timestamp and handler name.

## Required behavior
- outbox publisher must be retryable
- publish must be idempotent where possible
- consumer handlers must be idempotent

## Retry and DLQ policy (locked)
- Retry: **exponential backoff with jitter**
- Max retries per handler: **5**
- DLQ: publish poison messages to **`atlas.<domain>.dlq.v1`**

_Last updated: 2026-02-08_
