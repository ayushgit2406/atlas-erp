# Atlas ERP — Deployment Guide

This document defines how the system *would* be deployed in a production-like environment.
For portfolio purposes, actual deployment may remain local compose, but the deployment model should be coherent and defendable.

---

## 1. Deployment targets

Target(s):
- **Local Docker Compose** (primary)
- Optional future: Kubernetes (kind/minikube) for learning and realism

This doc assumes a container-based deployment.

---

## 2. Artifact strategy

- Build each service into a container image.
- Version images using semantic versioning.
- Publish images to **GHCR** by default.

---

## 3. Configuration management

- Use environment variables for runtime config.
- Do not bake secrets into images.
- Provide `.env.example` files (never real secrets).

Secrets strategy:
- local: **.env + Docker secrets**
- prod-like: **SOPS (age)**; Vault can be added later if required

---

## 4. Health, readiness, and rollouts

- Liveness: `/health`
- Readiness: `/ready` must validate dependencies (DB, broker, etc.)

Rollout strategy (**[DECISION REQUIRED]**):
- blue/green
- rolling updates
- canary

---

## 5. Data migration strategy

- Migrations run at deploy-time or as a dedicated job.
- Backward compatible schema changes preferred.

See `docs/data/migrations.md`.

---

## 6. Operational constraints

- Broker must be durable and monitored.
- DB backups must be configured and tested.
- Observability must be enabled before production.

See `docs/OBSERVABILITY.md` and `docs/OPERATIONS.md`.

_Last updated: 2026-02-08_
