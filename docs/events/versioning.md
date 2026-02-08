# Event Versioning and Compatibility

## Why versioning matters
Consumers must be able to process messages safely as producers evolve.

## Compatibility goals
Compatibility policy: **Backward compatible** for minor schema changes (additive optional fields). Breaking changes require a new topic major version.

## Recommended rules (template)
- Adding optional fields is non-breaking
- Removing fields is breaking
- Changing field meaning is breaking
- Changing type is breaking
- Renaming fields is breaking (unless alias supported)

## Strategy (locked)
- Compatible changes: increment `schemaVersion` and keep the same `eventType` on the current topic.
- Breaking changes: publish to a new topic major version (e.g. `atlas.orders.v2`).

This strategy is locked in ADR-0002.

## Schema format
Schema format: **Avro** (locked).
- JSON Schema: easiest to start, human-readable
- Avro: strong with Kafka + schema registry patterns
- Protobuf: strong for gRPC and strict typing

_Last updated: 2026-02-08_
