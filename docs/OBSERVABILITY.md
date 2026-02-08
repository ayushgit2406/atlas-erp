# Atlas ERP — Observability

Defines logging, metrics, and tracing standards.

---

## 1. Principles

- Every request and event must be traceable end-to-end.
- Logs must be structured, searchable, and correlation-friendly.
- Metrics must be sufficient to answer: *Is it healthy? Is it slow? Is it failing?*
- Tracing is optional early, but correlation IDs are mandatory from day one.

---

## 2. Correlation model

### 2.1 IDs
- `correlationId`: flows across services for a business request
- `causationId`: points to prior event/command that caused the current action
- `eventId`: unique per event message

### 2.2 Propagation
- HTTP header: `X-Correlation-Id` (default)
- Event envelope fields: correlationId + causationId + eventId

---

## 3. Logging

### 3.1 Log format
- JSON structured logs

### 3.2 Required fields
- timestamp (UTC)
- service
- environment
- level
- message
- correlationId
- requestId (HTTP)
- eventId (event handlers)
- aggregateType + aggregateId (when applicable)
- user/principal (when applicable)
- latencyMs (for HTTP handlers)
- outcome (success/failure + errorCode)

### 3.3 Error logging standard
- Do not log secrets or tokens.
- Use stable error codes (see `docs/api/error-model.md`).

---

## 4. Metrics

### 4.1 HTTP metrics (all services + gateway)
- request_count{route,method,status}
- request_latency_ms{route,method}
- error_count{route,error_code}

### 4.2 Event consumer metrics
- events_consumed_count{eventType}
- events_failed_count{eventType,error_code}
- consumer_lag{topic,consumer_group} (Kafka offsets and consumer group lag)

### 4.3 Outbox metrics
- outbox_pending_count
- outbox_publish_latency_ms
- outbox_failures_count

---

## 5. Tracing (optional early)

OpenTelemetry adoption:
- Phase 1–2: correlation IDs + structured logs
- Phase 6+: optional tracing
- Phase 7: full tracing (OTel SDK + exporter)

---

## 6. Debugging playbook

See:
- `docs/runbooks/debug-an-order.md`
- `docs/runbooks/consumer-lag.md`

_Last updated: 2026-01-30_
