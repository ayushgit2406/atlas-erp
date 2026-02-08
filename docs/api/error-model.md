# Error Model (REST)

## Goals
- Stable error codes for clients
- Consistent shape across services
- Safe messages (no internal secrets)

## Response format
Use RFC 7807 Problem Details.

Example:
```json
{
  "type": "https://atlas.erp/errors/ORDER_NOT_FOUND",
  "title": "Order not found",
  "status": 404,
  "detail": "Order with id '...' was not found.",
  "instance": "/orders/...",
  "extensions": {
    "errorCode": "ORDER_NOT_FOUND",
    "correlationId": "..."
  }
}
```

## Error code taxonomy (template)
Orders:
- ORDER_NOT_FOUND
- ORDER_INVALID_STATE
- ORDER_VALIDATION_FAILED

Inventory:
- INVENTORY_INSUFFICIENT
- INVENTORY_RESERVATION_CONFLICT

Billing:
- INVOICE_NOT_FOUND
- PAYMENT_FAILED

Workflow:
- APPROVAL_NOT_FOUND
- APPROVAL_NOT_ALLOWED

**[DECISION REQUIRED]** finalize taxonomy and ensure consistent naming.
