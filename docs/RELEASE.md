# Atlas ERP — Release and Versioning

---

## 1. Versioning policy

We use **per-service semantic versions**, plus repo tags for milestone demos.

Recommendation for microservices: per-service versions plus a repo tag for demo milestones.

---

## 2. Changelog policy

- Maintain `CHANGELOG.md` at repo root
- Each release includes:
  - features
  - fixes
  - breaking changes
  - migration notes

---

## 3. Contract versioning

### Events
- schemaVersion in envelope
- Topic versions carry the major version (e.g., `atlas.orders.v1`), and the envelope `schemaVersion` tracks compatible schema evolution.

### APIs
- prefer backward compatible changes
- breaking changes require new versioned route prefix or new endpoint

---

## 4. Release checklist (template)

- [ ] tests green
- [ ] migrations generated and tested
- [ ] event schemas updated
- [ ] OpenAPI updated
- [ ] runbooks updated if operational behavior changed
- [ ] ADR written if any decision changed

_Last updated: 2026-01-30_
