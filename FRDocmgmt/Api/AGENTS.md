# Backend agent: API host

These instructions supplement the workspace `AGENTS.md` for `FRDocmgmt/Api/`.
Backend ownership also includes Domain, Application, and Infrastructure as defined
in the workspace instructions.

## API implementation

- Use Minimal APIs. Organize endpoints by feature under `Endpoints/` and register
  route groups through mapping extension methods.
- Keep `Program.cs` focused on the composition root and HTTP pipeline. Register
  Application and Infrastructure through their service-registration extensions.
- Keep handlers thin: bind and validate input, call an Application service, then
  map the result to an HTTP response. Place business rules in the inner layers.
- Use typed results and explicit route names where appropriate. Keep response
  status codes and contracts consistent with existing endpoints.
- Return 201 with a Location header for creation, 404 for missing resources, and
  400 with validation problem details for invalid requests.
- Preserve request validation and problem-details handling. Pass cancellation
  tokens through service and repository calls.
- Keep Blazor components, static UI assets, and UI middleware in Web.
- Never expose stack traces, secrets, or persistence details in API responses.

## Existing document contract

- `GET /api/documents`: list metadata.
- `GET /api/documents/{id}`: return metadata or 404.
- `POST /api/documents`: accept a required title of at most 200 characters and
  return created metadata. Domain trims the title.
- Storage is currently a singleton, thread-safe in-memory repository. Preserve
  its lifetime and behavior unless persistence changes are part of the task.

## Validation

```powershell
dotnet build FRDocmgmt/FRDocmgmt.slnx
dotnet run --project FRDocmgmt/Api/FRDocmgmt.Api.csproj --launch-profile http
```

Commands run from the workspace root. Use `http://localhost:5223` and the requests
in `FRDocmgmt.Api.http` to check affected routes. Check successful responses,
invalid requests, missing resources, and Location headers when creation changes.
Api should not serve the Blazor homepage or UI routes.
