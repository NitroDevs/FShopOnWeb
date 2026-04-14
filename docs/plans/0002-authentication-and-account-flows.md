# 0002 Authentication and Account Flows

## Goal

Replace the static F# login page with working authentication and baseline account flows comparable to the C# application.

## Current Gap

- The F# repository renders a login form only.
- No sign-in handler, user persistence, registration, logout, password management, or authorization model is present.
- No protected pages depend on a real user identity yet.

## Scope

- Introduce ASP.NET Core Identity or an equivalent supported identity stack for F#.
- Implement working login and logout.
- Implement registration and basic account confirmation flow.
- Add authorization guards for authenticated-only routes such as checkout and order history.
- Seed the demo users expected by eShopOnWeb.

## Dependencies

- [0008-database-migrations-and-seeding.md](./0008-database-migrations-and-seeding.md)

## Implementation Plan

1. Choose the identity integration approach that fits Falco hosting without fighting middleware expectations.
2. Add identity entities, stores, cookie auth, and service registration.
3. Replace the static login page with bound GET/POST handlers and validation.
4. Add logout and registration routes with matching UI.
5. Seed `demouser@microsoft.com`, `productmgr@microsoft.com`, and `admin@microsoft.com`.
6. Protect future authenticated features behind route-level or handler-level authorization.

## Acceptance Criteria

- Login succeeds for seeded demo users.
- Logout clears the authenticated session.
- Registration creates a new user record.
- Protected pages redirect unauthenticated users to login.
- Identity data persists across restarts.

## Risks

- Falco route handling and ASP.NET Identity UI conventions may not align directly.
- Taking a shortcut with ad hoc auth will make later role and admin work harder.
