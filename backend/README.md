# WhojooSite

## Setup

Make sure `WhojooSite.AppHost` has all necessary secrets by performing the following commands. Replace the secrets with
your preference.
Before executing these commands you need to first cd to the `WhojooSite.AppHost` project.

```bash
dotnet user-secrets set "Parameters:PostgresPassword" "{YOUR_POSTGRES_PASSWORD}"
dotnet user-secrets set "Parameters:PostgresUsername" "{YOUR_POSTGRES_USERNAME}"
```

Make sure you add an `appsettings.Development.json` with the following values:

```json
{
  "Parameters": {
    "PostgresPort": {YOUR_POSTGRES_PORT}
  }
}
```
