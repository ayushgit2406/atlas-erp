# Atlas ERP

Atlas ERP is an enterprise‑grade, event‑driven ERP backend designed to demonstrate correctness, operational maturity, and clear architectural boundaries.

## What this repo contains
- API Gateway (NestJS on Node.js + TypeScript, Express adapter)
- Domain services (Order, Inventory, Billing, Workflow) in .NET 8
- Reporting service (Python + FastAPI)
- Kafka event broker
- Postgres per service + reporting Postgres
- Redis for cache/idempotency
- Keycloak for OIDC authentication

## Docs
Start with `docs/README.md` for the full documentation map, reading order, and decisions.

## Status
This repository is under active development. Decisions are tracked via ADRs in `docs/adr/`.

## License
See `docs/COST_AND_LICENSING.md` for licensing posture.
