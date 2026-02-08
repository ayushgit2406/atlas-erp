# Atlas ERP — Incident Response

This document defines incident handling and on-call practices in a single, streamlined guide.
It is intended to be enterprise-ready without unnecessary ceremony.

---

## 1. Severity levels (template)

Example rubric:
- SEV-1: total outage / data integrity risk
- SEV-2: major degradation / partial outage
- SEV-3: minor degradation
- SEV-4: informational

---

## 2. On-call responsibilities (template)

- Respond to alerts within:
  - SEV-1: **15 minutes**
  - SEV-2: **60 minutes**
- Triage incidents using runbooks
- Escalate to service owners if required

---

## 3. Escalation policy

Escalation steps and contacts can remain placeholders for portfolio use.

---

## 4. Incident workflow

1. Detect (alert or report)
2. Triage
3. Mitigate (stop the bleeding)
4. Diagnose
5. Fix
6. Verify
7. Postmortem

---

## 5. Postmortems

- Blameless
- Include timeline, impact, root cause, contributing factors, action items

### Postmortem template

- Date:
- Severity:
- Owner:
- Status: Draft/Final

#### Summary
What happened in 2–3 sentences.

#### Impact
- Who/what was affected?
- Duration:
- Customer impact:

#### Timeline (UTC)
- T0:
- T+:

#### Root cause
Explain the primary cause.

#### Contributing factors
List secondary factors.

#### Detection
How was it detected? Why not earlier?

#### Resolution
What fixed it?

#### Action items
| Item | Owner | Due | Status |
|------|-------|-----|--------|

#### Lessons learned
What will we do differently?

---

_Last updated: 2026-02-08_
