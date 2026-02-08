# OpenAPI Guidelines

## Purpose
All externally-facing HTTP APIs must be described via OpenAPI.

## Standards
- Use `application/problem+json` for errors (see error-model.md).
- Define request/response schemas explicitly.
- Prefer explicit enums for status values.
- Document authentication requirements per endpoint.

## Versioning
Versioning: **URL versioning** using `/v1/...` for all public gateway endpoints.

## Gateway vs service specs
- Gateway spec is the public contract.
- Service specs may be internal but should exist for clarity and testing.
