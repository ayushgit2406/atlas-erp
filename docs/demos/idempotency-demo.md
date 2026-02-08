# Demo — Idempotency

## Goal
Show that retries do not create duplicate effects.

## Command idempotency
- Submit the same `POST /orders` with same Idempotency-Key twice
- Expected: second request returns same orderId; no duplicate events

## Event idempotency
- Deliver the same `InventoryReserved` event twice
- Expected: Billing creates only one invoice

**[FILL AFTER IMPLEMENTATION]** Provide concrete steps once idempotency storage is implemented.
