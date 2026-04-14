# 0005 Admin Catalog Management

## Goal

Add an administrative surface for catalog item creation, editing, and deletion.

## Current Gap

- eShopOnWeb exposes admin catalog management through a dedicated admin UI and supporting API.
- FShopOnWeb has no admin routes, authorization policy, or product management workflow.

## Scope

- Add an admin area or equivalent route group in the F# app.
- Support list, create, edit, and delete for catalog items.
- Support brand/type lookup and image URI editing.
- Restrict access to product manager or administrator roles.

## Dependencies

- [0002-authentication-and-account-flows.md](./0002-authentication-and-account-flows.md)
- [0006-user-and-role-administration.md](./0006-user-and-role-administration.md)
- [0007-public-api.md](./0007-public-api.md) if the admin UI is split from the web app like the C# version.
- [0008-database-migrations-and-seeding.md](./0008-database-migrations-and-seeding.md)

## Implementation Plan

1. Decide whether admin lives in the same F# web app or as a separate client.
2. Add role-protected catalog management endpoints.
3. Add server-side validation for catalog item fields.
4. Add brand/type management support needed by catalog item editing.
5. Add tests for authorized and unauthorized product management actions.

## Acceptance Criteria

- Product managers can create, edit, and delete catalog items.
- Unauthorized users cannot access admin operations.
- Catalog changes appear correctly in the storefront.

## Risks

- Building admin UI before stable auth, roles, and persistence will create rework.
