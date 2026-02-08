# Atlas ERP — Operations (Day-2)

This document defines operational practices: runtime behaviors, incident response, and operational guardrails.

---

## 1. Operational goals

- predictable startup/shutdown behavior
- clear health signals
- safe reprocessing and recovery
- auditable operations
- reproducible troubleshooting steps (runbooks)

---

## 2. Health model

- `/health` (liveness): process is up
- `/ready` (readiness): dependencies reachable (DB, broker, auth)

Readiness must fail if:
- DB not reachable
- broker not reachable (if service depends on it)
- required secrets/config missing

---

## 3. Backups and recovery

Backup policy:
- frequency: **daily**
- retention window: **30 days**
- restore drill cadence: **quarterly**

Minimum expectation:
- scripted DB dump/restore for each Postgres instance
- documented restore procedure

---

## 4. Event reprocessing policy

- Reprocessing is necessary for rebuilding projections.
- Reprocessing must be safe (idempotent consumers).

See:
- `docs/runbooks/reprocess-events.md`
- `docs/runbooks/rebuild-projections.md`

---

## 5. Operational dashboards (portfolio-ready)

Even if not fully implemented, define dashboards:
- HTTP error rate and latency
- consumer failures and lag
- outbox backlog
- DB connection saturation

---

## 6. Change management

- ADR required for architecture changes
- contract versioning required for breaking schema changes
- release notes required (see `docs/RELEASE.md`)

_Last updated: 2026-02-08_
