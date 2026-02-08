# Atlas ERP — Engineering Quality Standards

---

## 1. Code style and formatting

Tools:
- .NET: `dotnet format` + `.editorconfig`
- TypeScript: `eslint` + `prettier`
- Python: `ruff` (lint+format) (optionally `black` if desired)

All services must enforce formatting in CI.

---

## 2. Error handling standards

- Use stable error codes (see `docs/api/error-model.md`)
- Avoid throwing raw exceptions to clients
- Return problem details payloads for REST

---

## 3. Dependency hygiene

- lockfile required (package-lock.json/pnpm-lock.yaml/poetry.lock)
- avoid unpinned dependencies
- security scanning enabled (CodeQL + Trivy)

---

## 4. Naming conventions

- Services: kebab-case for containers, PascalCase in code
- Events: PascalCase for eventType, versioned
- Topics: `atlas.<domain>.v<major>` (locked)

See `docs/events/naming.md`.

---

## 5. Documentation policy

- Every public API change requires updating OpenAPI docs
- Every event contract change requires schema update and version notes
- Every architectural decision requires an ADR

_Last updated: 2026-02-08_
