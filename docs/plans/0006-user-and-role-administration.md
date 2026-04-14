# 0006 User and Role Administration

## Goal

Add administrator features for users, roles, and role membership.

## Current Gap

- eShopOnWeb provides admin workflows and APIs for user CRUD, role CRUD, membership listing, and membership updates.
- FShopOnWeb has no role model, no admin screens, and no management endpoints.

## Scope

- Add role definitions and seeded assignments.
- Add admin workflows for listing users and roles.
- Support creating, editing, and deleting roles.
- Support creating, editing, and deleting users within the constraints used by eShopOnWeb.
- Support adding and removing role memberships.

## Dependencies

- [0002-authentication-and-account-flows.md](./0002-authentication-and-account-flows.md)
- [0008-database-migrations-and-seeding.md](./0008-database-migrations-and-seeding.md)
- [0007-public-api.md](./0007-public-api.md) if management is API-backed.

## Implementation Plan

1. Introduce role seeding for `Product Managers` and `Administrators`.
2. Add authorization policies for administrator-only management actions.
3. Add role management handlers and validation, including safe delete checks for assigned roles.
4. Add user management handlers, including protected demo-account behavior if parity is desired.
5. Add membership management workflows from both the user and role perspectives.
6. Add tests for permission boundaries and destructive-action safeguards.

## Acceptance Criteria

- Administrators can manage users and roles.
- Product managers and shoppers cannot access these tools.
- Assigned roles cannot be deleted without removal of memberships first.
- Demo bootstrap users and roles are seeded consistently.

## Risks

- Mixing role management rules into ad hoc logic instead of explicit policies will become hard to maintain.
