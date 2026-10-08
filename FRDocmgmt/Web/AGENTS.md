# Frontend agent

These instructions supplement the workspace `AGENTS.md` for `FRDocmgmt/Web/`.

## Responsibilities

- Maintain Blazor pages in `Components/Pages/`, layouts in `Components/Layout/`,
  and shared components under `Components/`.
- Keep global styles and static assets in `wwwroot/`. Use scoped `.razor.css`
  files for component-specific styles and `.razor.js` files for component scripts.
- Keep `Program.cs` focused on UI service registration, middleware, and Blazor
  endpoint mapping. Preserve interactive server rendering and antiforgery support.
- Use the `FRDocmgmt.Web` namespace. The scoped stylesheet is
  `FRDocmgmt.Web.styles.css`, referenced through `Assets` in `Components/App.razor`.

## UI implementation

- Use Tailwind CSS v4 utilities for new and updated UI styling. Keep components focused and
  extract reusable behavior when it serves the requested feature.
- Preserve navigation and existing pages unless their removal is requested.
- Use semantic markup, associated form labels, keyboard-accessible controls,
  visible focus states, and layouts that work on small screens.
- For data-driven features, provide loading, empty, validation, and failure states.
- Integrate with Api through an injected HTTP client or client service when needed.
  Keep backend URLs configurable and avoid persistence references in Web.
- The current starter pages do not call the document API. Do not imply integration
  exists until it is implemented and verified.

## Tailwind CSS v4

- Use complete, literal prefixed classes such as `tw:flex`, `tw:p-4`,
  `tw:md:grid-cols-2`, and `tw:hover:bg-blue-700`. The prefix comes before variants.
- Choose between complete class strings for conditional styles. Do not construct
  partial class names such as `tw:bg-@color-500`; the scanner cannot resolve them.
- Source CSS and theme customization belong in `Styles/tailwind.css`. The v4 CLI
  scans `Components/`, including Razor files and component code. Register additional
  source directories with `@source` when adding UI code elsewhere.
- Existing Bootstrap and scoped CSS remain for the starter pages. Tailwind's
  Preflight reset is disabled and utilities use `important` to override legacy
  styles when explicitly applied. Avoid adding new Bootstrap styling classes.
- Use utilities first; retain scoped CSS for behavior that utilities do not express
  clearly. Do not add v3 `@tailwind` directives or a v3 configuration file.
- Never edit generated `wwwroot/css/tailwind.css` or `node_modules/`. Commit
  `package.json` and `package-lock.json` when changing frontend dependencies.
- Node.js 22.15 or newer and npm are required. Web builds restore locked npm dependencies when
  needed and run `npm run css:build` before static asset processing.
- For live CSS changes, run `npm --prefix FRDocmgmt/Web run css:watch` in a separate
  terminal alongside the Web host. Rebuild after adding assets or before publishing.
- Keep the CSS resolver shim in `scripts/tailwind-resolver.mjs` while Tailwind's
  default resolver cannot handle this workspace's `#` path. Use the npm scripts
  rather than invoking the CLI directly. For manual npm restores on this machine,
  set `$env:NODE_OPTIONS = '--use-system-ca'` to use trusted system certificates.

## Validation

```powershell
dotnet build FRDocmgmt/Web/FRDocmgmt.Web.csproj
dotnet run --project FRDocmgmt/Web/FRDocmgmt.Web.csproj --launch-profile http
```

Commands run from the workspace root. Check changed pages at
`http://localhost:5224`, generated stylesheets, and relevant UI interactions.
HTTP route checks alone do not verify interactive Blazor behavior or visual layout.
Verify that the generated Tailwind stylesheet loads and includes the utilities used
by changed components. Use browser checks for computed styles when available.
