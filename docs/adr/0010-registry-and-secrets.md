# ADR-0010: Container Registry and Secrets Strategy

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/DEPLOYMENT.md
  - docs/SECURITY.md

## Context
Images must be publishable and secrets must be handled safely for local and prod-like use.

## Decision
- Container registry: **GHCR** (GitHub Container Registry) by default.
- Local secrets: **.env + Docker secrets**.
- Prod‑like secrets: **SOPS (age)** with encrypted env files; Vault can be added later if needed.

## Consequences
- Local dev remains simple.
- Deployment automation can target GHCR and SOPS without vendor lock‑in.

## Follow-ups
- [ ] Add sample SOPS config when infra automation begins
