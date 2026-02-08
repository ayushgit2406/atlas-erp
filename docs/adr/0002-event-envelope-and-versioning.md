# ADR-0002: Event Envelope, Schema Format, and Versioning

- Status: Accepted
- Date: 2026-01-30
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/events/envelope.md
  - docs/events/versioning.md
  - docs/RELEASE.md

## Context
Events are cross-service contracts. They must be traceable and evolvable without breaking consumers.

## Decision
- Mandatory event envelope with correlationId/causationId/eventId.
- **Avro** is the schema format for payloads.
- Backward-compatible schema evolution for minor changes.
- Breaking changes require a new topic major version (e.g., `atlas.orders.v2`).

## Options considered
1. JSON Schema
2. Avro
3. Protobuf

## Pros/cons
### Avro
- Pros:
  - Strong Kafka ecosystem fit
  - Mature schema evolution patterns
  - Compact encoding
- Cons:
  - Requires tooling for inspection and generation

## Consequences
- Store `.avsc` files under `docs/events/schemas/`
- Validate event payloads in CI against schemas

## Follow-ups
- [ ] Add v1 Avro schemas as implementation starts
