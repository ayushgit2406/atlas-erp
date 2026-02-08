# Demo — Order Saga (Failure Paths)

## Purpose
Demonstrate correctness under failure and explicit compensation.

## Scenarios
1. Inventory reservation fails
2. Payment fails
3. Approval rejected
4. Duplicate event delivery (idempotency)

## For each scenario
- Trigger
- Expected events
- Expected state transitions
- Compensation actions

Inventory failure policy (v1): **Backorder** (order transitions to Backordered/PendingStock; reservation retries allowed).
Payment failure compensation (v1): mark payment failed; trigger workflow escalation; release inventory reservation if order is cancelled/voided by policy.
