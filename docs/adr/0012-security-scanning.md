# ADR-0012: Security Scanning Tools

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/SECURITY.md
  - docs/QUALITY.md

## Context
Enterprise posture requires automated security checks without heavy ops overhead.

## Decision
- SAST: **CodeQL**.
- Container scanning: **Trivy**.

## Consequences
- CI needs CodeQL and Trivy jobs.
- Findings should block releases once CI is wired.

## Follow-ups
- [ ] Add CI workflow definitions when pipeline is introduced
