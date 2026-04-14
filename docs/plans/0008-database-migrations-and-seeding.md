# 0008 Database Migrations and Seeding

## Goal

Replace the current development-only persistence approach with a durable schema and migration story that can support identity, orders, and admin features.

## Current Gap

- FShopOnWeb recreates the SQLite database on startup in development.
- Catalog and basket data share a minimal single context.
- No migrations, identity schema, or order schema exist.
- Seed data is limited and coupled directly to startup.

## Scope

- Preserve SQLite as a valid local default unless there is a clear reason to move off it.
- Introduce migrations for catalog, basket, identity, and order data.
- Refactor seeding so it is idempotent and environment-aware.
- Decide whether to keep a single database or split catalog/app identity concerns like the C# sample.

## Implementation Plan

1. Model the target persistence boundaries needed by auth, basket, and orders.
2. Stop deleting the database on startup.
3. Add EF Core migrations and migration application strategy.
4. Refactor seeding into explicit routines for catalog data, roles, and demo users.
5. Add documentation for local initialization and reset flows.

## Acceptance Criteria

- Database contents survive application restart.
- Schema changes are managed through migrations.
- Demo seed data can be applied repeatedly without duplication.
- Identity and order features have the schema support they need.

## Risks

- Leaving startup-driven destructive seeding in place will block every downstream feature.
