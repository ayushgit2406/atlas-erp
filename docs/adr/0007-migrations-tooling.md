# ADR-0007: Migration Tooling

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/DEVELOPMENT.md
  - docs/data/migrations.md

## Context
Each service owns its schema and must run reproducible migrations in local dev and CI.

## Decision
- .NET services use **EF Core migrations**.
- Reporting service uses **Alembic**.

## Options considered
1. EF Core + Alembic (selected)
2. Flyway/Liquibase for all services
3. SQL scripts only

## Consequences
- .NET services require EF Core tooling in build containers.
- Reporting service requires Alembic configuration.
- Migration creation and apply steps must be documented per service.

## Follow-ups
- [ ] Add migration how‑to steps per service when scaffolds exist
