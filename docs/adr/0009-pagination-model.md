# ADR-0009: Pagination Model

- Status: Accepted
- Date: 2026-02-08
- Decision owner: Ayush Raj Singh
- Related docs:
  - docs/api/pagination-filtering.md

## Context
List endpoints need a consistent pagination model across services and gateway.

## Decision
Use **offset pagination** with `page` and `pageSize` for v1 APIs.

## Consequences
- Simple client support and predictable total counts.
- Large datasets may require a future cursor‑based v2 if needed.

## Follow-ups
- [ ] Document defaults/limits per endpoint when APIs are defined
