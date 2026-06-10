# dotnet-clean-architecture

> Production-ready Clean Architecture template for .NET 8 — not a tutorial, a working starting point.

[![.NET](https://img.shields.io/badge/.NET%208-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com)
[![License](https://img.shields.io/badge/license-MIT-blue?style=flat-square)](LICENSE)
[![Docker](https://img.shields.io/badge/Docker-ready-2496ED?style=flat-square&logo=docker&logoColor=white)](docker-compose.yml)

Built from 10 years of production patterns across enterprise systems for Home Depot (US), Zurich Insurance, and major Brazilian banks. This is how distributed systems actually need to be structured when they handle real volume.

---

## Structure

```
src/
├── Domain/                 # Entities, value objects, domain events
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Events/
│   └── Exceptions/
├── Application/            # Use cases, commands, queries, interfaces
│   ├── Commands/           # CQRS write side
│   ├── Queries/            # CQRS read side
│   ├── Interfaces/         # Ports (abstractions)
│   └── Validators/         # FluentValidation
├── Infrastructure/         # EF Core, repositories, external services
│   ├── Persistence/
│   ├── Repositories/
│   └── Services/
└── WebApi/                 # ASP.NET Core entry point
    ├── Controllers/
    ├── Middleware/
    └── Extensions/

tests/
├── Unit/
├── Integration/
└── Architecture/           # ArchUnitNET — enforce layer boundaries
```

## What's included

**Application Layer**
- CQRS with MediatR — commands and queries fully separated
- FluentValidation pipeline behavior
- AutoMapper for DTO mapping
- Generic repository interface

**Infrastructure Layer**
- Entity Framework Core + SQL Server
- Repository pattern implementation
- Unit of Work
- Database migrations

**Web API**
- JWT authentication + refresh tokens
- Role-based authorization
- Global exception handler middleware
- Serilog structured logging with correlation IDs
- Health checks (DB + custom)
- Swagger/OpenAPI with JWT support

**DevOps**
- Docker + Docker Compose
- GitHub Actions CI pipeline
- `.editorconfig` + `Directory.Build.props`

## Getting Started

```bash
git clone https://github.com/paulookino/dotnet-clean-architecture
cd dotnet-clean-architecture

# Restore and build
dotnet restore
dotnet build

# Run migrations
dotnet ef database update --project src/Infrastructure

# Run
dotnet run --project src/WebApi
```

Or with Docker:

```bash
docker-compose up --build
```

## Design Decisions

**Why CQRS without Event Sourcing?**
Event Sourcing adds significant complexity. Most systems don't need it. CQRS alone gives you the read/write separation and scalability benefits for 95% of use cases.

**Why repository pattern over direct EF?**
EF DbContext is already a unit of work + repository. But abstracting it lets you test application logic without hitting a database, and lets you swap persistence implementations without touching use cases. At enterprise scale, that matters.

**Why FluentValidation over Data Annotations?**
Validation logic belongs in the Application layer, not on DTOs. FluentValidation keeps it testable, composable, and out of the presentation layer.

## License

MIT
