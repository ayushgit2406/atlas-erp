# Runbook — Debug an Order End-to-End

## Goal
Trace a single order through Orders → Inventory → Billing → Workflow → Reporting.

## Inputs
- orderId
- correlationId (preferred)
- time window

## Procedure
1. Gateway logs: locate request by orderId/correlationId
2. Order service logs: confirm state transition + emitted events
3. Broker: confirm OrderPlaced published
4. Inventory logs: confirm reservation attempt outcome
5. Billing logs: confirm invoice/payment processing
6. Workflow logs: approval decisions (if applicable)
7. Reporting: projection updated and query returns expected state

## If mismatch found
- verify consumer idempotency store
- check consumer lag
- check outbox backlog
