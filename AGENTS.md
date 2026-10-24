# AGENTS.md

Guidance for AI agents and contributors working in this repository.

## Project

EvoSC# is a server controller for Trackmania, written in C# on .NET 10. The solution is `EvoSC.sln`.

- `src/EvoSC` is the host application.
- `src/EvoSC.Common`, `src/EvoSC.Commands`, `src/EvoSC.Manialinks` and `src/EvoSC.Modules` are the core libraries.
- `src/Modules/*` holds the modules. Each module is its own project.
- `tests/` mirrors the source layout, with module tests in `tests/Modules/<Module>.Tests`.

## Build and test

```
dotnet build
dotnet test
```

Build `src/EvoSC/EvoSC.csproj` to check that a change works across all modules. Manialink templates (`.mt`) are
compiled at runtime, so a successful build does not prove that a template is valid.

## Creating a module

Use an existing module, for example `LocalRecordsModule` or `MapListModule`, as a reference.

A module typically contains:

- `<Name>Module.csproj` and `info.toml` (name, title, summary, version, author and dependencies).
- `<Name>Module.cs`, a class deriving from `EvoScModule` and marked `[Module(IsInternal = true)]` for internal modules.
- `Controllers/` for chat commands, event subscriptions and manialink routes.
- `Services/` and `Interfaces/` for logic. Services are registered with `[Service]`.
- `Config/` for settings interfaces marked `[Settings]`.
- `Database/` for models and repositories, and `Migrations/` for FluentMigrator migrations.
- `Templates/` for manialink templates. Add `<EmbeddedResource Include="Templates\**\*" />` to the csproj.

### Registering internal modules

Every internal module (`[Module(IsInternal = true)]`) must be added in three places, or it is not built into the
server or loaded:

1. **`src/EvoSC/InternalModules.cs`**: add a `using` for the module's namespace and a `typeof(<Name>Module)` entry to
   the `Modules` array. This is what loads the module and runs its migrations. A module missing from this list is
   silently never loaded.
2. **`src/EvoSC/EvoSC.csproj`**: add a `ProjectReference` to the module's csproj.
3. **`EvoSC.sln`**: add the project, for example with `dotnet sln EvoSC.sln add src/Modules/<Name>/<Name>.csproj`.
   `dotnet sln add` also adds `x64` and `x86` platform configurations; delete those lines afterwards. Do not add
   build configurations to the solution. Only `Debug|Any CPU` and `Release|Any CPU` are used.

If the module depends on another module, also reference that project in its csproj and list it under
`[dependencies]` in `info.toml`.

## Conventions

- Match the style of the surrounding code, including its comment density and naming.
- Target `net10.0` with `Nullable` and `ImplicitUsings` enabled.
- With .NET 10, `Where`, `OrderBy`, `ToListAsync` and similar calls on LinqToDB queries can be ambiguous with
  `System.Linq.AsyncEnumerable`. Qualify them, as in `Queryable.Where(...)` or `AsyncExtensions.ToListAsync(...)`.
- Migration numbers must be unique across the whole project.
- Remove TODOs and debug code before opening a pull request, and see `CONTRIBUTING.md` for the pull request process.
