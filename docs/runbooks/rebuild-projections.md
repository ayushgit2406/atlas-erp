# Runbook — Rebuild Reporting Projections

## When to use
- schema change in read models
- projection bug fix
- need to backfill historical data

## Steps (template)
1. Deploy new reporting schema/migrations
2. Clear projection tables (safe because non-authoritative)
3. Replay events from broker
4. Verify dashboards/endpoints

## Verification
- compare counts against source-of-truth services (spot checks)
- validate recent orders appear correctly
