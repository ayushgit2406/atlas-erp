# Idempotency

## Why
Clients and brokers may retry; idempotency prevents duplicates.

## Command idempotency (HTTP)
- Clients supply `Idempotency-Key` header.
- Services store key + request hash + result reference.
- If same key is re-used:
  - return the same outcome without repeating side effects.

## Event idempotency
- Consumers store processed `eventId`.
- Duplicate deliveries must not cause duplicate writes.

## Decisions
- Command idempotency is enforced by **services**. The gateway forwards `Idempotency-Key` but does not own idempotency state.
- TTL for idempotency records: **24 hours** (default; adjustable).
