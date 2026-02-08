# Atlas ERP — Development Guide

This document explains how to run Atlas ERP locally and how developers should work with the repo.

> **Note**: Concrete port mappings, env vars, and compose service names should be finalized once docker-compose is implemented.
> Until then, this doc contains **placeholders** marked as **[DECISION REQUIRED]** or **[FILL AFTER IMPLEMENTATION]**.

---

## 1. Prerequisites

- Docker + Docker Compose
- .NET 8 SDK
- Node.js (LTS) + **pnpm** (NestJS for gateway; CLI optional)
- Python 3.11+ (recommended) + **Poetry**
- (Optional) Make

---

## 2. Repo layout (expected)

- `services/api-gateway/`
- `services/order-service/`
- `services/inventory-service/`
- `services/billing-service/`
- `services/workflow-service/`
- `services/reporting-service/`
- `infra/` (compose, configs)
- `docs/`

---

## 3. Running locally (compose)

### 3.1 Start dependencies
```bash
docker compose up -d
```

**[FILL AFTER IMPLEMENTATION]** Expected containers:
- Postgres per service
- Redis
- Broker (Kafka)
- Keycloak
- Services

### 3.2 Verify health
Each service should expose:
- `GET /health` (liveness)
- `GET /ready` (readiness)

---

## 4. Environment variables (conventions)

### 4.1 Common variables
- `SERVICE_NAME`
- `ENVIRONMENT` (local/dev/staging/prod)
- `LOG_LEVEL`
- `CORRELATION_HEADER` (default: `X-Correlation-Id`)

### 4.2 DB variables (per service)
- `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER`, `DB_PASSWORD`

### 4.3 Broker variables (Kafka)
- `KAFKA_BOOTSTRAP_SERVERS`
- `KAFKA_TOPIC_PREFIX` (default: `atlas`)

## 5. Migrations

Migration tool strategy:
- .NET services: **EF Core migrations**
- Reporting: **Alembic**

Once selected, document:
- how to create migrations
- how to apply migrations
- how to reset local DBs safely

See `docs/data/migrations.md`.

---

## 6. Local auth (Keycloak)

Local defaults (subject to env overrides):
- realm name: **atlas-erp**
- client name: **atlas-gateway**
- roles: **Admin, Ops, Finance, Approver, Viewer**
- token acquisition workflow: **password flow** for local dev only

---

## 7. Developer workflow

### Branching
Branching model: **trunk-based** (short-lived branches).

### Commit hygiene
- Conventional commits: **required**

### Code formatting
- .NET: dotnet format?
- TS: prettier/eslint?
- Python: ruff/black?

See `docs/QUALITY.md`.

---

## 8. Troubleshooting (templates)

### Service cannot connect to DB
- verify compose health for Postgres container
- verify service env vars
- check network name

### Consumer lag/backlog
- see `docs/runbooks/consumer-lag.md`

_Last updated: 2026-02-08_
