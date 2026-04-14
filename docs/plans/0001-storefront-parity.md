# 0001 Storefront Parity

## Goal

Close the gap between the current F# storefront and the C# storefront for catalog browsing and basket management.

## Current Gap

- Catalog filters render in F# but do not drive query-based filtering.
- Pagination UI renders, but paging is not implemented.
- Basket supports add/remove only.
- Basket ownership is effectively "first basket in the database", not tied to a cookie or authenticated buyer.
- Basket quantity updates and basket summary parity are incomplete.

## Scope

- Add query-driven catalog filtering by brand and type.
- Implement real pagination with page index and page size.
- Introduce basket ownership via cookie for anonymous users.
- Support quantity updates in the basket page.
- Add basket summary calculations consistent with the current UI.
- Preserve current Falco-based page structure.

## Dependencies

- [0008-database-migrations-and-seeding.md](./0008-database-migrations-and-seeding.md) for a durable persistence model.
- [0002-authentication-and-account-flows.md](./0002-authentication-and-account-flows.md) if baskets should merge or associate with signed-in users.

## Implementation Plan

1. Add explicit query models for home page filtering and paging.
2. Move catalog queries into a dedicated service module instead of assembling page state inline.
3. Add persisted basket identity using a long-lived cookie.
4. Refactor basket mutations to operate on the current basket, not the first basket found.
5. Extend the basket page with quantity update actions and recalculated totals.
6. Add tests for filtering, paging, and basket update behavior.

## Acceptance Criteria

- Brand and type selections affect the catalog result set.
- Pagination changes the catalog result set and page controls.
- Anonymous users retain a basket across requests.
- Basket quantities can be updated without breaking add/remove.
- Tests cover the new storefront workflows.

## Risks

- Current `emptyBasket` and first-record lookup behavior will cause correctness bugs if not replaced early.
- UI parity may expose persistence shortcomings before migrations are added.
