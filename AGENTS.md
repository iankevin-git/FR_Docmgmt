# Project agent instructions

## Scope and project context

These instructions apply to the entire workspace. Read any more specific
`AGENTS.md` in the directories you edit. Explicit user instructions take priority.

FRDocmgmt uses .NET 10, C#, ASP.NET Core Minimal APIs, and Blazor with interactive
server rendering. The solution is `FRDocmgmt/FRDocmgmt.slnx`.

## Agent responsibilities

### Frontend agent

- Own `FRDocmgmt/Web/`: Razor pages, reusable components, layouts, styles, static
  assets, UI configuration, and presentation behavior.
- Follow `FRDocmgmt/Web/AGENTS.md` for frontend implementation and validation.
- Use Tailwind CSS v4 for new and updated UI styling, following the Web project's
  `tw:` utility prefix and CSS build workflow.
- Consume backend functionality through HTTP when API integration is requested.
  Keep database access and business rules out of UI components.
- Coordinate API contract changes with the backend role. Do not change backend
  projects solely to work around a UI issue.

### Backend agent

- Own `FRDocmgmt/Domain/`, `FRDocmgmt/Application/`,
  `FRDocmgmt/Infrastructure/`, and `FRDocmgmt/Api/`.
- Put business entities and invariants in Domain; use cases, DTOs, and repository
  interfaces in Application; persistence implementations in Infrastructure; HTTP
  endpoints and host configuration in Api.
- Follow `FRDocmgmt/Api/AGENTS.md` when editing the API host.
- Preserve API contracts unless the task calls for a change. Communicate route,
  request, response, and validation changes to the frontend role.

These are responsibility definitions for agent work. They do not require spawning
separate agents for every task. For work spanning both roles, agree on the contract
and coordinate shared-file edits before implementing dependent changes.

## Architecture rules

- Domain has no project dependencies or framework/persistence concerns.
- Application depends on Domain and must not depend on Api, Web, Infrastructure,
  or ASP.NET Core.
- Infrastructure depends on Application and implements its abstractions.
- Api depends on Application and Infrastructure. Reference Infrastructure for
  service registration at the composition root; endpoint handlers call Application.
- Web is a separate presentation host. Do not reference Api or Infrastructure
  projects from Web or move Blazor assets back into Api.
- The current repository is in memory and loses data on host restart. Do not
  describe it as durable storage or introduce a database provider without task scope.

## Working conventions

- Inspect existing code before editing and preserve unrelated user changes.
- Use nullable reference types, async I/O, dependency injection, and cancellation
  tokens where appropriate. Follow the naming and formatting of nearby C# code.
- Make focused changes. Add dependencies only when needed for the requested work.
- Do not edit generated `bin/`, `obj/`, or `.vs/` artifacts or commit user-specific
  `*.csproj.user` settings.
- Update `README.md` when changing architecture, launch commands, ports, or API
  behavior. Update `FRDocmgmt/Api/FRDocmgmt.Api.http` for changed example requests.

## Build and run

Run these commands from the workspace root:

```powershell
dotnet build FRDocmgmt/FRDocmgmt.slnx
dotnet run --project FRDocmgmt/Api/FRDocmgmt.Api.csproj --launch-profile http
dotnet run --project FRDocmgmt/Web/FRDocmgmt.Web.csproj --launch-profile http
```

Run the two hosts in separate terminals. Api uses `http://localhost:5223`; Web
uses `http://localhost:5224`. HTTPS profiles use ports 7010 and 7011 respectively.
Check whether a host is already running before starting another on the same port.
Do not stop an existing user process unless needed and authorized by the task.

## Verification and handoff

- For code changes, build the affected projects; build the solution for shared
  contract or project-reference changes.
- Use meaningful checks for changed behavior. Backend changes should check success,
  validation, and missing-resource responses. Frontend changes should check affected
  routes, stylesheet loading, and interactions when browser access is available.
- Documentation-only changes need a content review rather than a build.
- Report what changed, checks performed, and any remaining limitations. Do not claim
  a check passed if it was not run.
