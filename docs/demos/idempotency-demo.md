# Demo — Idempotency

## Goal
Show that retries do not create duplicate effects.

## Command idempotency
- Submit the same `POST /orders` with same Idempotency-Key twice
- Expected: second request returns same orderId; no duplicate events

## Event idempotency
- Deliver the same `InventoryReserved` event twice
- Expected: Billing creates only one invoice

Example (template):
1. Send `POST /v1/orders` twice with the same `Idempotency-Key`.
2. Expect the same `orderId` response both times.
3. Verify only one `OrderPlaced` event was published.
