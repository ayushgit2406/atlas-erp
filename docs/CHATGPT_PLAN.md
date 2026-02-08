# Atlas ERP Docs Plan (ChatGPT Brief)

Purpose: Provide a single, structured summary of the entire `docs/` folder so a future ChatGPT session can quickly understand the system, the folder structure, and the build/run/ops expectations.

Last reviewed: 2026-02-08

## Quick Start (Reading Order)
1. `docs/BUILD.md`
2. `docs/ARCHITECTURE.md`
3. `docs/adr/` (locked decisions)
4. `docs/events/`
5. `docs/api/`
6. `docs/SECURITY.md`
7. `docs/OBSERVABILITY.md`
8. `docs/OPERATIONS.md` + `docs/runbooks/` + `docs/INCIDENT_RESPONSE.md`
9. `docs/demos/`

## Folder Structure Summary
- `docs/ARCHITECTURE.md`: C4-style system model, boundaries, interaction paths, consistency model, reliability primitives.
- `docs/BUILD.md`: authoritative build spec, scope, services, eventing, saga flow, roadmap, and explicit decision points.
- `docs/DEVELOPMENT.md`: local dev expectations, repo layout, env conventions, migration strategy placeholders.
- `docs/DEPLOYMENT.md`: container-based deployment strategy and open decisions.
- `docs/SECURITY.md`: authN/authZ, RBAC, secrets, auditability, security headers, vuln mgmt.
- `docs/THREAT_MODEL.md`: STRIDE-style threats and mitigations; open security decisions.
- `docs/OBSERVABILITY.md`: logging, metrics, correlation IDs, tracing phases, debug runbooks.
- `docs/OPERATIONS.md`: day-2 practices, health model, backups, reprocessing, dashboards.
- `docs/QUALITY.md`: formatting, error handling, dependency hygiene, naming, doc policy.
- `docs/TESTING.md`: test taxonomy, contract tests, idempotency, performance tests, CI gates.
- `docs/RELEASE.md`: versioning and release checklist.
- `docs/INCIDENT_RESPONSE.md`: incident response and on-call framework (includes postmortem template).
- `docs/NON_GOALS.md`: explicit exclusions.
- `docs/COST_AND_LICENSING.md`: zero-cost license posture.
- `docs/security-rbac-matrix.md`: RBAC role-to-scope template.
- `docs/adr/`: Architecture Decision Records (ADRs) and decision register.
- `docs/events/`: event envelope, naming, versioning, outbox, catalog template, schemas folder.
- `docs/api/`: OpenAPI, error model, idempotency, pagination/filtering standards.
- `docs/data/`: migrations, retention, ERD guidelines.
- `docs/runbooks/`: operational runbooks (lag, outbox backlog, replay, projections, debug).
- `docs/demos/`: demo scripts for saga flows and idempotency.

## System Summary
- Architecture: event-driven microservices with API gateway and reporting read model.
- Core services: Order, Inventory, Billing, Workflow (.NET 8). Gateway in NestJS (Node.js/TS, Express adapter). Reporting in Python/FastAPI.
- Data: Postgres per service + reporting Postgres. Redis for cache/idempotency.
- Auth: Keycloak (OIDC) with RBAC.
- Broker: Kafka (locked).
- Reliability: outbox/inbox, idempotent consumers, correlation IDs, DLQ strategy pending.

## Interaction Paths
- Command path: Client -> Gateway -> Service REST -> DB transaction -> Outbox -> Broker.
- Reaction path: Broker -> Consumer -> DB transaction -> Outbox -> Broker (if needed).
- Read path: Client -> Reporting API -> Reporting DB.

## Saga Flow (Primary)
- OrderPlaced -> InventoryReserved -> InvoiceCreated -> PaymentSucceeded -> (optional ApprovalGranted) -> OrderConfirmed.
- Failure paths and compensation in `docs/demos/order-saga-failure-paths.md`.

## Locked Decisions (From ADRs and Build)
- Broker: Apache Kafka. (ADR-0001)
- Event schemas: Avro. (ADR-0002)
- Event envelope with correlationId/causationId/eventId. (ADR-0002)
- Versioned topics by domain (Model A). (ADR-0001 + events/naming.md)
- Outbox + inbox pattern; HTTP idempotency enforced by services. (ADR-0003)
- Auth: Keycloak OIDC + RBAC; gateway validates JWT, services enforce authZ. (ADR-0004)
- Reporting service: Python + FastAPI; REST-first read API, GraphQL deferred. (ADR-0005)
- API Gateway responsibilities are thin (auth, routing, header normalization). (ADR-0006)
- Inventory policy: backorder on insufficient stock. (BUILD.md)
- Billing depth: ledger-style entries. (BUILD.md)
- API versioning: `/v1` for gateway. (BUILD.md)
- Idempotency: services enforce; gateway forwards `Idempotency-Key`. (BUILD.md)
- Gateway framework: NestJS with Express adapter (gateway only). (ADR-0016)
- Migrations: EF Core for .NET; Alembic for reporting. (ADR-0007)
- Retry/DLQ: exponential backoff with jitter, max 5 retries, DLQ `atlas.<domain>.dlq.v1`. (ADR-0008)
- Pagination: offset `page` + `pageSize`. (ADR-0009)
- Registry: GHCR. Secrets: local `.env` + Docker secrets; prod-like SOPS (age). (ADR-0010)
- Dev workflow: trunk-based + Conventional Commits. (ADR-0011)
- Security scanning: CodeQL + Trivy. (ADR-0012)
- Rate limiting: deferred. (ADR-0013)
- Backups: daily, 30-day retention, quarterly restore drill. (ADR-0014)
- Keycloak local defaults: realm `atlas-erp`, client `atlas-gateway`, roles Admin/Ops/Finance/Approver/Viewer. (ADR-0015)

## Open Decisions and Placeholders
These are explicitly marked as **[DECISION REQUIRED]** or **[FILL AFTER IMPLEMENTATION]** across docs.
- Rollout strategy (blue/green vs rolling vs canary).
- Final lint/format tooling decisions where not locked.
- Scaling model for consumers and broker-specific replay commands.
- CORS policy and security headers specifics.
- Service-to-service auth if any internal HTTP remains.
- Broker TLS/auth configuration.
- Centralized authorization policy tests.
- Load testing tooling (k6/locust) and CI quality gates.
- Final error code taxonomy.
- Data retention durations, PII handling, deletion policy.

## Build Roadmap (Phases)
1. Infrastructure bootstrap (compose, broker, databases, auth).
2. Order service MVP.
3. Inventory integration and compensation.
4. Billing integration and webhook simulation.
5. Workflow approvals + audit trail.
6. Reporting projections + read APIs.
7. Production readiness (observability, failure drills, runbooks).
8. Optional ML advisory (non-authoritative).

## Contracts and Conventions
- Events: mandatory envelope, Avro schemas, versioned topics, idempotent consumers, outbox/inbox.
- HTTP: OpenAPI specs, RFC 7807 errors, `/v1` versioning, idempotency via `Idempotency-Key`.
- Logging: JSON logs with correlationId, causationId, eventId, outcome, latency.
- Metrics: HTTP latency/error rates, consumer lag, outbox backlog, failures.
- Tracing: optional early, later OpenTelemetry.

## Security and Threat Model
- Keycloak OIDC with RBAC; scopes enforced at services.
- Auditability required for command handlers and approvals.
- STRIDE threats documented with mitigations and open decisions.

## Operations and Runbooks
- Health endpoints: `/health` (liveness) and `/ready` (readiness).
- Runbooks: consumer lag, outbox backlog, debug an order, reprocess events, rebuild projections.
- Reprocessing and projection rebuild require idempotent consumers.

## Testing Strategy
- Unit, integration, contract, and end-to-end tests.
- Event schema validation against Avro.
- Idempotency tests for both commands and events.
- Performance testing deferred to later phase.

## Release and Versioning
- Per-service semantic versions + repo tags for milestone demos.
- Changelog at repo root.
- Release checklist includes migrations, schemas, OpenAPI, runbooks, ADRs.

## Demos
- Order saga happy path and failure paths.
- Idempotency demo (command and event).

## Non-Goals
- No full UI, no micro-frontends.
- No managed SaaS dependencies.
- No real payment processing / PCI scope.
- GraphQL and gRPC deferred.

## Cost and Licensing
- All core components are open-source with $0 licensing cost.
