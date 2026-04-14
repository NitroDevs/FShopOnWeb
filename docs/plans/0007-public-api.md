# 0007 Public API

## Goal

Introduce an API surface comparable to eShopOnWeb's `PublicApi` application.

## Current Gap

- FShopOnWeb is currently a server-rendered web app only.
- No JSON API, token auth, or external admin/client integration surface exists.

## Scope

- Decide whether the API should live in the existing F# host or a separate F# project.
- Expose catalog item, brand, and type endpoints.
- Expose authentication/token issuance if external clients are required.
- Expose admin-oriented endpoints for catalog, users, roles, and memberships as needed.
- Add OpenAPI or equivalent discoverability.

## Dependencies

- [0008-database-migrations-and-seeding.md](./0008-database-migrations-and-seeding.md)
- [0002-authentication-and-account-flows.md](./0002-authentication-and-account-flows.md)
- [0006-user-and-role-administration.md](./0006-user-and-role-administration.md) for admin API parity

## Implementation Plan

1. Choose API architecture: separate service for parity, or route group within the current app for simplicity.
2. Define DTOs and mapping for catalog resources.
3. Add authentication strategy for API callers.
4. Add admin endpoints only after role and user rules are explicit.
5. Add API integration tests around auth, validation, and authorization.

## Acceptance Criteria

- Catalog resources are queryable over JSON.
- Protected API routes reject unauthorized callers.
- Admin routes enforce role requirements.
- API behavior is covered by integration tests.

## Risks

- Shipping an API before domain boundaries are cleaned up will leak page-model assumptions into the contract.
