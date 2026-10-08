# FRDocmgmt

.NET 10 Clean Architecture with Minimal API endpoints and the existing Blazor UI.

```text
FRDocmgmt/
  FRDocmgmt.slnx
  Domain/          Entities and business invariants; no project dependencies
  Application/     Use cases, DTOs, validation, and repository interfaces → Domain
  Infrastructure/  Repository implementations and service registration → Application
  Api/             Minimal API endpoints and composition root → Application + Infrastructure
  Web/             Blazor pages, components, layouts, and static UI assets
```

Dependencies point inward. Application owns the repository interface; Infrastructure
implements it. Api references Infrastructure only to register its services at startup.
Keep endpoint handlers concerned with HTTP and put business behavior in Application
and Domain. Domain and Application do not depend on ASP.NET Core or persistence code.
Web is a separate presentation host with no direct persistence dependencies.
The existing starter pages run independently; they do not yet call the document API.

## Run

Install the .NET 10 SDK and Node.js 22.15 or newer (including npm). The Web project
uses Tailwind CSS v4 and restores its locked npm packages automatically during build.

```powershell
dotnet build FRDocmgmt/FRDocmgmt.slnx
dotnet run --project FRDocmgmt/Api/FRDocmgmt.Api.csproj --launch-profile http
```

Run the Blazor UI in a separate terminal:

```powershell
dotnet run --project FRDocmgmt/Web/FRDocmgmt.Web.csproj --launch-profile http
```

Open `http://localhost:5224` for the Blazor pages. The Minimal API runs at
`http://localhost:5223/api/documents`. HTTPS profiles use ports 7011 for Web and
7010 for Api. Visual Studio users can open `FRDocmgmt/FRDocmgmt.slnx` and configure
`FRDocmgmt.Api` and `FRDocmgmt.Web` as multiple startup projects.

## Frontend styling

Tailwind CSS v4 is built with the CLI from `FRDocmgmt/Web/Styles/tailwind.css` into
`FRDocmgmt/Web/wwwroot/css/tailwind.css`. Web builds and publishes compile the CSS
before processing static assets. The output and `node_modules` are generated and ignored.

Use prefixed utilities in Razor markup, for example `tw:flex tw:gap-4 tw:md:p-6`.
The source scanner reads `Components/`; use complete class names for conditional
styles. Bootstrap remains for existing starter pages. Tailwind Preflight is disabled
and its utilities are marked important so explicitly applied utilities take precedence.
The home page includes a small Tailwind example.

`scripts/tailwind-resolver.mjs` resolves explicit CSS imports through Node to handle
the `#` in this workspace's path. Keep it in the CLI scripts while using this version.
Automatic npm restore uses the system certificate store for this Windows environment.

For CSS development, run this in a separate terminal alongside the Web project:

```powershell
npm --prefix FRDocmgmt/Web run css:watch
```

To restore or build the styles manually:

```powershell
$env:NODE_OPTIONS = '--use-system-ca'
npm --prefix FRDocmgmt/Web ci
npm --prefix FRDocmgmt/Web run css:build
```

See the [Tailwind CLI documentation](https://tailwindcss.com/docs/installation/tailwind-cli)
and [frontend agent instructions](FRDocmgmt/Web/AGENTS.md).

## Document metadata example

| Method | Route | Behavior |
| --- | --- | --- |
| GET | `/api/documents` | List document metadata |
| GET | `/api/documents/{id}` | Get metadata; return 404 when missing |
| POST | `/api/documents` | Create metadata; return 201 with a Location header |

POST accepts `{ "title": "Example document" }`. Titles are required, limited to
200 characters, and trimmed by the domain entity. Invalid request data returns 400
with validation problem details. Requests support cancellation.

The example repository is thread safe and lives in memory for the lifetime of the
host. Data is lost when the app restarts. This example manages metadata only; file
uploads, authentication, and durable storage are not implemented. Add a database
repository in Infrastructure implementing `IDocumentRepository` and change
`AddInfrastructure` to register it when a persistence provider is chosen.

Sample requests are in `FRDocmgmt/Api/FRDocmgmt.Api.http`.
