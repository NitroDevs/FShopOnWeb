# 0004 Checkout and Orders

## Goal

Implement the purchase flow that turns a basket into an order and lets users review their order history.

## Current Gap

- FShopOnWeb has no checkout page.
- No order aggregate, address model, order service, or order history pages exist.
- Basket items are not connected to an authenticated buyer journey.

## Scope

- Add order domain types and persistence.
- Add checkout page and submit workflow.
- Create order from basket contents.
- Clear or archive the basket after successful checkout.
- Add "My Orders" and order detail pages for authenticated users.

## Dependencies

- [0008-database-migrations-and-seeding.md](./0008-database-migrations-and-seeding.md)
- [0002-authentication-and-account-flows.md](./0002-authentication-and-account-flows.md)
- [0001-storefront-parity.md](./0001-storefront-parity.md)

## Implementation Plan

1. Introduce order aggregate entities and tables.
2. Add checkout handlers and UI for shipping address capture.
3. Implement an order service that validates basket state and creates order items from catalog snapshots.
4. Delete or reset the basket after a successful checkout.
5. Build authenticated order history and order details pages.
6. Add tests for empty-basket checkout, successful checkout, and order retrieval authorization.

## Acceptance Criteria

- An authenticated user can check out a non-empty basket.
- Checkout creates a persisted order with item snapshots and totals.
- The basket is no longer active after checkout.
- Users can view only their own orders.

## Risks

- Without buyer identity and stable basket ownership, checkout will produce incorrect associations.
