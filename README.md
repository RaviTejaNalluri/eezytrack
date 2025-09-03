## EezyTrack

A minimal task and project tracking backend built with .NET 9 Minimal APIs, EF Core, and SQLite. It supports basic entities: Users (implicit via header), Groups, Projects, and Tasks, with simple plan limits.

### Tech stack
- **Runtime**: .NET 9
- **Web**: ASP.NET Core Minimal APIs
- **Persistence**: Entity Framework Core + SQLite
- **OpenAPI**: JSON exposed in Development

### Features
- Create and list Groups; invite members with plan limits
- Create and list Projects within a Group (plan-limited)
- Create and list Tasks in a Project; assign user; update status
- Simple current user resolution via `X-User-Email` header (defaults to `test@example.com`)

## Getting started

### Prerequisites
- .NET SDK 9.x installed

### Clone and run
```bash
cd /Users/raviteja/Desktop/Test/eezytrack/EezyTrack.api
dotnet restore
dotnet run
```

The API will listen at `http://localhost:5111` (see `Properties/launchSettings.json`).

In Development, the root redirects to OpenAPI JSON at `/openapi/v1.json`:
```
http://localhost:5111/openapi/v1.json
```

### Database
- Uses SQLite with connection string `Data Source=eezytrack.db` (see `appsettings.Development.json`).
- A local `eezytrack.db` file will be created in the `EezyTrack.api` project directory if it does not exist.
- EF Core migrations are included under `EezyTrack.api/Migrations/`.

Run migrations (optional, recommended)
```bash
cd /path/Test/eezytrack/EezyTrack.api
dotnet tool install --global dotnet-ef || true
dotnet ef database update
```

## Usage

### Auth header
Set the current user via the `X-User-Email` header. If omitted, it defaults to `test@example.com`.

Example header:
```
X-User-Email: alice@example.com
```

### Groups
- Create group
```bash
curl -X POST "http://localhost:5111/api/groups?name=MyGroup" \
  -H "X-User-Email: alice@example.com"
```

- List my groups
```bash
curl "http://localhost:5111/api/groups" \
  -H "X-User-Email: alice@example.com"
```

- Invite member (enforces plan limits)
```bash
curl -X POST "http://localhost:5111/api/groups/<groupId>/invite?email=bob@example.com"
```

### Projects
- Create project in a group
```bash
curl -X POST "http://localhost:5111/api/projects?groupId=<groupId>&name=Website"
```

- List projects in a group
```bash
curl "http://localhost:5111/api/projects?groupId=<groupId>"
```

### Tasks
- Create task in a project
```bash
curl -X POST "http://localhost:5111/api/tasks?projectId=<projectId>&title=Homepage&description=Build%20hero&dueDateUtc=2025-09-01T00:00:00Z"
```

- List tasks in a project
```bash
curl "http://localhost:5111/api/tasks?projectId=<projectId>"
```

- Assign task
```bash
curl -X POST "http://localhost:5111/api/tasks/<taskId>/assign?userId=<userId>"
```

- Update task status
```bash
curl -X POST "http://localhost:5111/api/tasks/<taskId>/status?status=InProgress"
```

## Configuration

Key files:
- `EezyTrack.api/appsettings.Development.json` contains the SQLite connection string.
- `EezyTrack.api/Program.cs` wires routes and OpenAPI JSON for Development.

Environment:
- `ASPNETCORE_ENVIRONMENT=Development` by default via `Properties/launchSettings.json`.

## Notes and limitations
- No authentication or authorization; caller identity is from `X-User-Email`.
- Minimal validation and error handling.
- No Swagger UI by default; OpenAPI JSON only in Development.
- Plan limits are hardcoded in `Application/Plans/PlanLimits.cs`.

## Project structure
```
EezyTrack.api/
  Application/
    Contracts/ICurrentUser.cs
    Plans/PlanLimits.cs
  Domain/
    Entities/{User,Group,GroupMember,Project,TaskItem}.cs
    Enums/PlanTier.cs
  Endpoints/{GroupsEndpoints,ProjectsEndpoints,TasksEndpoints}.cs
  Infrastructure/{AppDbContext,CurrentUser}.cs
  Migrations/*
  Program.cs
  Properties/launchSettings.json
```

## Common tasks

Add a migration
```bash
cd /Users/raviteja/Desktop/Test/eezytrack/EezyTrack.api
dotnet ef migrations add <Name>
```

Update database
```bash
dotnet ef database update
```

Run tests
```bash
# No tests yet; add a test project, then:
dotnet test
```

## License
No license specified.


