# MonoSlice — Task Tracker

## Solution Foundation
- [x] `Directory.Build.props` — Shared build properties
- [x] `Directory.Packages.props` — Central package management
- [x] `.editorconfig` — Code style
- [x] `MonoSlice.sln` — Solution file
- [x] `global.json` — SDK pinning

## Shared Abstractions
- [x] Project file
- [x] Response wrapper (`ApiResponse<T>`)
- [x] CQRS interfaces (ICommand, IQuery, handlers)
- [x] DDD base classes (Entity, AggregateRoot, IDomainEvent)
- [x] Repository & UnitOfWork interfaces
- [x] Event bus abstractions (IEventBus, IntegrationEvent)
- [x] Cache abstraction (ICacheService)
- [x] Current user interface (ICurrentUser)
- [x] Pagination (PaginatedList)

## Shared Infrastructure
- [x] Project file
- [x] Caching (Memory + Redis)
- [x] Messaging — RabbitMQ native publisher/consumer
- [x] Messaging — Kafka native publisher/consumer
- [x] Messaging — EventBus dispatcher + config
- [x] Middleware — ExceptionHandling
- [x] Middleware — RequestLogging
- [x] Behaviors — LoggingBehavior
- [x] Behaviors — ValidationBehavior
- [x] Persistence — BaseDbContext
- [x] OpenTelemetry setup (Traces, Metrics, Logging)
- [x] Mapster configuration
- [x] Service collection extensions

## Users Module
- [x] Project file
- [x] Domain — ApplicationUser, ApplicationRole with GuidV7
- [x] Persistence — UsersDbContext + schema config
- [x] Auth — JwtTokenService
- [x] Auth — CompositeAuthMiddleware (JWT + Cookie)
- [x] Auth — CurrentUserService
- [x] Auth — AuthSettings
- [x] Features — Register
- [x] Features — Login & Logout
- [x] Features — RefreshToken
- [x] Features — GetProfile
- [x] Features — AssignRole (Role-based auth)
- [x] SeedRoles background service
- [x] Module registration (AddUsersModule / UseUsersAuth / MapUsersEndpoints)

## Catalog Module
- [x] Project file
- [x] Domain — Product aggregate, domain events, integration events
- [x] Persistence — CatalogDbContext + schema config
- [x] Features — CreateProduct (publishes async event to bus)
- [x] Features — GetProduct (caching integration)
- [x] Features — ListProducts (pagination + search)
- [x] Features — UpdateProduct (cache invalidation)
- [x] Features — DeleteProduct
- [x] Integration event handler (native async consumer)
- [x] In-process domain event handler
- [x] Module registration (AddCatalogModule / MapCatalogEndpoints)

## Host Project
- [x] Project file with Mediator.SourceGenerator
- [x] Program.cs — Composition root (Minimal APIs, Scalar UI, HealthChecks, RateLimiter, CORS)
- [x] appsettings.json + appsettings.Development.json
- [x] .env.example

## Docker
- [x] Dockerfile (multi-stage build)
- [x] docker-compose.yml (API, PostgreSQL 17, RabbitMQ, Redis, Jaeger)
- [x] .dockerignore

## Tests
- [x] Users module unit tests (RegisterCommandHandler)
- [x] Catalog module unit tests (Domain & CommandHandler)
- [x] Integration tests project (WebApplicationFactory)

## Documentation
- [x] README.md
- [x] task.md
