# Event Catalog

This is the authoritative list of domain events, producers, consumers, and routing.

> **Status**: Template. Populate as event contracts are finalized.

## Catalog table

| EventType | Producer | Consumers | Topic/Queue | SchemaVersion | Notes |
|----------|----------|-----------|------------|--------------|------|
| OrderPlaced | order-service | inventory-service, workflow-service, reporting-service | atlas.orders.v1 | 1 | |
| OrderCancelled | order-service | inventory-service, reporting-service | atlas.orders.v1 | 1 | |
| InventoryReserved | inventory-service | billing-service, reporting-service | atlas.inventory.v1 | 1 | |
| InventoryReservationFailed | inventory-service | order-service?, reporting-service | atlas.inventory.v1 | 1 | |
| InvoiceCreated | billing-service | workflow-service?, reporting-service | atlas.billing.v1 | 1 | |
| PaymentSucceeded | billing-service | order-service?, reporting-service | atlas.billing.v1 | 1 | |
| PaymentFailed | billing-service | workflow-service, reporting-service | atlas.billing.v1 | 1 | |
| ApprovalGranted | workflow-service | order-service, reporting-service | atlas.workflow.v1 | 1 | |
| ApprovalRejected | workflow-service | order-service, reporting-service | atlas.workflow.v1 | 1 | |

## Notes
- Rows marked with `?` are implementation choices; in v1, services react via events where needed and reporting consumes all events.
- Finalize per saga design and record in ADRs.
