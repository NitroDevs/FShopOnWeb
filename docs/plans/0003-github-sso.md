# 0003 GitHub SSO

## Goal

Add GitHub OAuth sign-in to FShopOnWeb after baseline local identity is established.

## Current Gap

- eShopOnWeb supports GitHub OAuth.
- FShopOnWeb has no external login providers.

## Scope

- Add GitHub OAuth configuration and callback handling.
- Surface "Sign in with GitHub" in the account UI.
- Map GitHub claims into the local user profile and local sign-in session.
- Document required local secrets and callback URLs.

## Dependencies

- [0002-authentication-and-account-flows.md](./0002-authentication-and-account-flows.md)

## Implementation Plan

1. Wire GitHub OAuth into the host authentication pipeline.
2. Add an external login button and callback endpoint.
3. Create or link local users for external identities.
4. Normalize email and claim handling so later role assignment remains deterministic.
5. Add configuration and setup documentation for local development.

## Acceptance Criteria

- A user can sign in with GitHub from the login page.
- Returning users reuse the same local account.
- Missing configuration disables the feature cleanly instead of breaking login.

## Risks

- External identity linking rules can create duplicate users if email and provider keys are not handled carefully.
