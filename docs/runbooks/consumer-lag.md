# Runbook — Consumer Lag / Backlog

## Symptoms
- reporting/read models stale
- increasing event backlog
- higher processing latency

## Immediate checks
1. Verify broker health
2. Check consumer logs for errors
3. Check DLQ (if enabled)

## Mitigation
- scale consumers (**[DECISION REQUIRED: scaling model]**)
- temporarily increase partitions (recreate topic with higher partitions; rebalance consumers)

_Last updated: 2026-02-08_

## Root cause analysis
- slow handler (DB contention)
- poison message
- dependency outage

## Follow-up
- add metrics and alerts
- improve idempotency/retry strategy
