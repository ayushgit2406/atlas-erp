# Demo — Order Saga (Happy Path)

## Goal
Demonstrate end-to-end flow:
OrderPlaced → InventoryReserved → InvoiceCreated → PaymentSucceeded → (optional) ApprovalGranted → OrderConfirmed

## Preconditions
- All services running in compose
- Auth available (Keycloak realm `atlas-erp`, client `atlas-gateway`)
- Kafka configured
- Reporting projections enabled

## Steps (template)
1. Create an order via gateway:
   - `POST /orders`
2. Verify Order service persisted and emitted `OrderPlaced`.
3. Verify Inventory service consumed and emitted `InventoryReserved`.
4. Verify Billing service created invoice and emitted `InvoiceCreated`.
5. Trigger sandbox payment success webhook (or simulation) and verify `PaymentSucceeded`.
6. If approvals enabled, verify approval event(s) then confirm order.

## Expected outputs
- Logs show consistent correlationId across services.
- Reporting API returns updated `order_summary`.

Example (template):
```bash
curl -X POST http://localhost:3000/v1/orders \\
  -H 'Authorization: Bearer <token>' \\
  -H 'Idempotency-Key: demo-001' \\
  -H 'Content-Type: application/json' \\
  -d '{\"customerId\":\"c-1\",\"items\":[{\"sku\":\"sku-1\",\"qty\":1}]}'
```

_Last updated: 2026-02-08_
