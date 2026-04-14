# FShopOnWeb vs eShopOnWeb Feature Comparison

Date: 2026-04-13

This comparison is based on the current `FShopOnWeb` repository and the current `main` branch of `NimblePros/eShopOnWeb`.

## Status Legend

- `Implemented`: present and functionally established in the repository.
- `Partial`: some visible pieces exist, but the full workflow or supporting infrastructure is incomplete.
- `Missing`: no meaningful implementation found.

## Feature Matrix

| Feature | eShopOnWeb (C#) | FShopOnWeb (F#) | Notes | Plan |
| --- | --- | --- | --- | --- |
| Catalog browsing, brand/type filtering, pagination | Implemented | Partial | F# home page renders catalog items and filter UI, but filtering and paging are not wired to query handling. | [0001-storefront-parity.md](./0001-storefront-parity.md) |
| Basket workflow | Implemented | Partial | F# supports viewing, add, and remove, but not quantity editing, cookie/user basket ownership, or checkout integration. | [0001-storefront-parity.md](./0001-storefront-parity.md) |
| Authentication and account flows | Implemented | Partial | F# has a rendered login page, but no working sign-in, logout, registration, or user store. | [0002-authentication-and-account-flows.md](./0002-authentication-and-account-flows.md) |
| GitHub single sign-on | Implemented | Missing | Present in C# via OAuth configuration and identity pages. | [0003-github-sso.md](./0003-github-sso.md) |
| Checkout and order history | Implemented | Missing | C# has checkout, order creation, My Orders, and order detail flows; none found in F#. | [0004-checkout-and-orders.md](./0004-checkout-and-orders.md) |
| Admin catalog management | Implemented | Missing | C# has admin catalog pages plus supporting API; F# has no admin area. | [0005-admin-catalog-management.md](./0005-admin-catalog-management.md) |
| User management | Implemented | Missing | C# admin supports user listing, create/update/delete, and role assignment safeguards. | [0006-user-and-role-administration.md](./0006-user-and-role-administration.md) |
| Role management and role membership | Implemented | Missing | C# exposes role CRUD and membership management; F# has none. | [0006-user-and-role-administration.md](./0006-user-and-role-administration.md) |
| Public API and token auth | Implemented | Missing | C# ships a dedicated `PublicApi` app with JWT auth and catalog/admin endpoints. | [0007-public-api.md](./0007-public-api.md) |
| Database architecture, migrations, identity persistence | Implemented | Partial | F# uses a single SQLite context recreated on startup; no migrations, identity schema, or production-style persistence story. | [0008-database-migrations-and-seeding.md](./0008-database-migrations-and-seeding.md) |
| Health checks and operational plumbing | Implemented | Missing | C# includes health checks, troubleshooting middleware, and richer hosting configuration. | [0009-operability-and-observability.md](./0009-operability-and-observability.md) |
| Automated testing breadth | Implemented | Partial | F# has limited basket tests; C# has unit, integration, functional, and API integration suites. | [0010-test-strategy.md](./0010-test-strategy.md) |

## Evidence Summary

### FShopOnWeb

- Storefront entry points: `src/Microsoft.eShopWeb.Web/Program.fs`, `src/Microsoft.eShopWeb.Web/Home/Home.Page.fs`
- Basket support: `src/Microsoft.eShopWeb.Web/Basket/Basket.Page.fs`, `src/Microsoft.eShopWeb.Web/Basket/Basket.Domain.fs`
- Account UI only: `src/Microsoft.eShopWeb.Web/Account/Login/Login.Page.fs`
- Current persistence shape: `src/Microsoft.eShopWeb.Web/Domain.fs`, `src/Microsoft.eShopWeb.Web/Persistence.fs`
- Current test surface: `tests/Basket/RemoveFromBasket.fs`

### eShopOnWeb

- Storefront and checkout: `src/Web/Pages/Index.cshtml.cs`, `src/Web/Pages/Basket/Checkout.cshtml.cs`
- Orders: `src/Web/Controllers/OrderController.cs`, `src/ApplicationCore/Services/OrderService.cs`
- Identity and GitHub auth: `src/Web/Program.cs`, `src/Web/Areas/Identity/Pages/Account/*`, `docs/features/sso-with-github.md`
- Admin portal: `src/BlazorAdmin/Pages/**/*`
- Public API: `src/PublicApi/**/*`
- User and role management: `docs/features/user-management.md`, `docs/features/role-management.md`, `docs/features/role-membership.md`
- Health checks: `src/Web/HealthChecks/*`
- Test breadth: `tests/UnitTests`, `tests/IntegrationTests`, `tests/FunctionalTests`, `tests/PublicApiIntegrationTests`

## Recommended Delivery Order

1. [0008-database-migrations-and-seeding.md](./0008-database-migrations-and-seeding.md)
2. [0002-authentication-and-account-flows.md](./0002-authentication-and-account-flows.md)
3. [0001-storefront-parity.md](./0001-storefront-parity.md)
4. [0004-checkout-and-orders.md](./0004-checkout-and-orders.md)
5. [0003-github-sso.md](./0003-github-sso.md)
6. [0007-public-api.md](./0007-public-api.md)
7. [0005-admin-catalog-management.md](./0005-admin-catalog-management.md)
8. [0006-user-and-role-administration.md](./0006-user-and-role-administration.md)
9. [0009-operability-and-observability.md](./0009-operability-and-observability.md)
10. [0010-test-strategy.md](./0010-test-strategy.md)
