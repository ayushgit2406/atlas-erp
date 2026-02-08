# Event Envelope (Mandatory)

All events published to the broker must use this envelope.

## Fields

- `eventId` (UUID): unique identifier for this event
- `eventType` (string): semantic name of the event (see naming.md)
- `schemaVersion` (string or int): schema version for payload
- `occurredAt` (UTC ISO-8601): when the fact occurred
- `producer` (string): service name
- `correlationId` (string): end-to-end trace identifier
- `causationId` (string|null): id of the command/event that caused this
- `aggregateType` (string): e.g., Order, InventoryReservation
- `aggregateId` (string): id of the aggregate instance
- `payload` (object): event-specific payload
- `metadata` (object, optional): headers/flags (must be non-sensitive)

## Example (template)
```json
{
  "eventId": "00000000-0000-0000-0000-000000000000",
  "eventType": "OrderPlaced",
  "schemaVersion": "1",
  "occurredAt": "2026-01-30T00:00:00Z",
  "producer": "order-service",
  "correlationId": "corr-...",
  "causationId": null,
  "aggregateType": "Order",
  "aggregateId": "order-...",
  "payload": {},
  "metadata": {}
}
```

## Notes
- Do not include secrets or tokens in payloads or metadata.
- Prefer explicit fields over free-form blobs.
