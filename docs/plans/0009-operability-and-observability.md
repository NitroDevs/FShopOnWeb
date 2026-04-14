# 0009 Operability and Observability

## Goal

Add the minimum operational features present in eShopOnWeb that improve deployment confidence and runtime diagnosis.

## Current Gap

- No health checks are registered in FShopOnWeb.
- No structured troubleshooting middleware or operational endpoints are present.
- Hosting is intentionally minimal today, which is fine for the current sample but not for parity.

## Scope

- Add health checks for the web app and database dependencies.
- Add consistent exception handling and request diagnostics.
- Expose environment-safe operational endpoints.
- Add lightweight operational documentation.

## Dependencies

- [0008-database-migrations-and-seeding.md](./0008-database-migrations-and-seeding.md)

## Implementation Plan

1. Add ASP.NET Core health check registrations for app and database readiness.
2. Add a request/exception logging strategy that fits the current host.
3. Expose health endpoints and document expected responses.
4. Add a small verification checklist for local and CI runs.

## Acceptance Criteria

- Health endpoints report app and database readiness.
- Unhandled exceptions are logged consistently.
- Operational behavior is documented for contributors.

## Risks

- Observability added too early can become noise if the app surface is still unstable.
