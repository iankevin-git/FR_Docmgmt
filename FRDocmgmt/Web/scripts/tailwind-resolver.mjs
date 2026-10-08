import { createRequire } from "node:module";
import { resolve } from "node:path";
import { pathToFileURL } from "node:url";

// Tailwind's default resolver escapes '#' in resolved paths as a NUL byte.
// Resolve explicit CSS imports with Node instead, so this workspace's 'c#'
// directory works. Leave other imports to Tailwind's normal resolver.
globalThis.__tw_resolve = (id, base) => {
  if (!id.endsWith(".css")) return undefined;

  try {
    const require = createRequire(pathToFileURL(resolve(base, "tailwind-resolver.cjs")));
    return require.resolve(id);
  } catch {
    return undefined;
  }
};
