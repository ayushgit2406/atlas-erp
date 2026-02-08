# Event Naming Conventions

## eventType
- Use PascalCase nouns/verbs representing facts, e.g. `OrderPlaced`, `InventoryReserved`.
- Avoid `*Requested` unless it is an actual business fact (e.g., `ApprovalRequested` is OK).
- Do not encode environment or tenant in eventType.

## Topic naming (Kafka; locked)

We use **Model A: Versioned topics by domain**:
- `atlas.orders.v1`
- `atlas.inventory.v1`
- `atlas.billing.v1`
- `atlas.workflow.v1`

## Consumer group naming
- `<service>-<purpose>`, e.g. `reporting-projections`

This choice is locked in ADR-0001.

_Last updated: 2026-02-08_
