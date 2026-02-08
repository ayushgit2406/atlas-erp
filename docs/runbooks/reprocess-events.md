# Runbook — Reprocess Events

## Purpose
Safely replay events to rebuild derived state (typically reporting projections).

## Preconditions
- Consumers are idempotent
- You know the replay source (broker retention vs stored archive)

## Steps (template)
1. Pause projection writers (optional)
2. Reset consumer offsets (**broker-specific**)
3. Re-run consumers
4. Validate projection correctness
5. Resume normal processing

## Risks
- duplicate side effects if consumers are not idempotent
- long replay time causing extended staleness

Kafka replay (template):
```bash
kafka-consumer-groups --bootstrap-server <broker> \
  --group <consumer-group> \
  --topic <topic> \
  --reset-offsets --to-earliest --execute
```

_Last updated: 2026-02-08_
