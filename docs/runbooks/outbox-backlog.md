# Runbook — Outbox Backlog / Publisher Stuck

## Symptoms
- DB state changes exist but downstream services not reacting
- outbox_pending_count grows

## Immediate actions
1. Verify publisher process is running
2. Inspect outbox table for error status / retry count
3. Verify broker connectivity/auth

## Mitigations
- restart publisher
- temporarily increase retry/backoff limits (default: exponential backoff with jitter, max 5 retries)
- move poison messages to DLQ (`atlas.<domain>.dlq.v1`)

_Last updated: 2026-02-08_

## Preventive actions
- add alerts for outbox backlog
- implement circuit breakers
