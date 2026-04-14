# 0010 Test Strategy

## Goal

Grow FShopOnWeb test coverage from isolated basket behavior to repository-level confidence comparable to eShopOnWeb.

## Current Gap

- FShopOnWeb currently has a narrow basket-focused test surface.
- eShopOnWeb has unit, integration, functional, and API integration test suites.
- The current F# architecture puts page composition, EF access, and workflow logic close together, which makes targeted testing harder.

## Scope

- Add unit tests for domain and workflow modules.
- Add integration tests for persistence-backed flows.
- Add end-to-end or handler-level tests for key web scenarios.
- Add API integration tests if [0007-public-api.md](./0007-public-api.md) is implemented.

## Dependencies

- All other feature plans, because test strategy should track implemented capabilities.

## Implementation Plan

1. Refactor business logic behind smaller modules where direct testing is practical.
2. Define a test pyramid for the F# solution.
3. Add coverage for auth, basket, checkout, orders, admin authz, and migrations.
4. Add CI execution expectations for fast and slow suites.

## Acceptance Criteria

- Core workflows have automated coverage at the appropriate level.
- Regression-prone authorization and persistence paths are tested.
- The repository documents how to run all test suites.

## Risks

- If logic remains embedded inside page handlers, tests will stay fragile and expensive to write.
