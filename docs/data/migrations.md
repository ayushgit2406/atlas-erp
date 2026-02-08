# Migrations Strategy

## Requirements
- Each service owns its schema and migrations.
- Migrations must be reproducible and executed in CI for validation.
- Backward compatible changes preferred.

## Tooling
Selected tools:
- .NET services: **EF Core migrations**
- Reporting service: **Alembic**

## Workflow (template)
1. Create migration
2. Review migration
3. Apply migration in local dev
4. Apply migration in CI integration tests
5. Apply migration at deploy-time

## Rollback policy
Rollback policy:
- Down migrations: **avoid in prod; prefer forward fixes**
- Restore from backup: **required for rollback**

_Last updated: 2026-02-08_
