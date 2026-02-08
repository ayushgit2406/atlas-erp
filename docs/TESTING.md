# Atlas ERP — Testing Strategy

Defines the test taxonomy and quality gates for an enterprise-grade backend.

---

## 1. Test types

### 1.1 Unit tests
- Domain invariants and state transitions
- Pure business rules (no IO)

### 1.2 Integration tests
- Service + DB + migrations
- Outbox publisher to broker (in-container)

### 1.3 Contract tests
- HTTP contracts (OpenAPI)
- Event schema compatibility

### 1.4 End-to-end tests
- Full saga across compose environment:
  order → inventory → billing → workflow → reporting projection

---

## 2. Event contract testing

- Maintain schemas under `docs/events/schemas/`
- Validate produced messages against schema
- Validate consumer compatibility against past versions

Schema format: **Avro** (locked).

---

## 3. Idempotency testing

- Duplicate event delivery should not duplicate state changes
- Duplicate command submission should return stable result

See `docs/api/idempotency.md`.

---

## 4. Performance testing (Phase 7)

- Load tests for command APIs (k6/locust) **[DECISION REQUIRED]**
- Consumer throughput tests
- DB connection pool tuning tests

---

## 5. Quality gates

**[DECISION REQUIRED]** CI gates:
- lint + format
- unit tests required
- integration tests required
- contract tests required
- coverage thresholds (if used)

_Last updated: 2026-01-30_
