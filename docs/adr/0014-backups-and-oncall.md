# ADR-0014: Backups and On‑Call SLAs

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/OPERATIONS.md
  - docs/INCIDENT_RESPONSE.md

## Context
Even for local-first learning, backup and response expectations should be documented.

## Decision
- Backup frequency: **daily**.
- Retention window: **30 days**.
- Restore drill cadence: **quarterly**.
- On‑call response SLAs:
  - SEV‑1: **15 minutes**
  - SEV‑2: **60 minutes**

## Consequences
- Runbooks should reflect backup/restore procedures.
- Incident response expectations are explicit.

## Follow-ups
- [ ] Add backup scripts once infra is implemented
