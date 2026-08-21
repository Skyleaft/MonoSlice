# MonoSlice 🍕

> **A modern, scalable, AOT-friendly .NET 10 Modular Monolith with Vertical Slice Architecture & Domain-Driven Design (DDD).**

[![.NET 10](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Vertical%20Slice%20%2B%20DDD-blue.svg)](#architecture-overview)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 🌟 Key Features

- **🎯 .NET 10 Minimal APIs**: High performance, modern API design without heavyweight MVC overhead.
- **🍰 Vertical Slice Architecture**: Features are organized by feature folder (Commands, Queries, Handlers, Endpoints, DTOs in one place) instead of technical layers.
- **🏰 Domain-Driven Design (DDD)**: Rich domain models, `AggregateRoot<TId>`, `Entity<TId>`, domain events, and repository/unit-of-work patterns.
- **⚡ AOT Build Friendly**: Uses compile-time source generation with **`Mediator.SourceGenerator`** and trim-safe patterns to minimize reflection.
- **🐘 PostgreSQL + EF Core**: Module-isolated `DbContext` per domain with automatic schema separation (`users`, `catalog`) and audit timestamp handling.
- **🐰 Native Messaging (RabbitMQ / Kafka)**: Native publisher and background consumer implementations without MassTransit, dynamically switchable via environment variables.
- **🔐 Hybrid Authentication & Authorization**:
  - ASP.NET Core Identity with **GuidV7** (`Guid.CreateVersion7()`) keys.
  - Custom **Composite Auth Middleware** supporting both **JWT Bearer** tokens and **Cookie Authentication**.
  - Role-based authorization (`Admin`, `User`, `Manager`).
  - Access token & Refresh token lifecycle.
- **🗺️ Mapster Mapping**: Fast, compile-time adaptable object mapping.
- **💾 Dual Caching Support**: Seamlessly switch between in-memory cache and **Redis** distributed cache via configuration.
- **🔭 OpenTelemetry & Jaeger**: Distributed tracing, metrics, and structured logs with OTLP exporter integration.
- **📜 Scalar OpenAPI UI**: Beautiful, interactive API documentation replacing default Swagger.
- **🛡️ DataAnnotations Validation**: Fast request validation executed via Mediator pipeline behavior.
- **📦 Standardized API Responses**: Every response wrapped in `ApiResponse<T>` with consistent error codes and validation details.
- **🐳 Docker & Docker Compose**: Complete setup including API, PostgreSQL 17, RabbitMQ Management, Redis, and Jaeger.

---

## 🏛️ Architecture Overview

```
MonoSlice/
├── src/
│   ├── MonoSlice.Host/                    # Composition root & API host
│   ├── MonoSlice.Shared/
│   │   ├── MonoSlice.Shared.Abstractions/ # Core interfaces, CQRS, DDD base types
│   │   └── MonoSlice.Shared.Infrastructure/ # Caching, Messaging, Middleware, Behaviors
│   └── Modules/
│       ├── MonoSlice.Modules.Users/       # Identity, JWT, Cookie auth, Role management
│       └── MonoSlice.Modules.Catalog/     # Sample domain module with CRUD & events
├── tests/
│   ├── MonoSlice.Modules.Users.Tests/    # Users module unit tests
│   ├── MonoSlice.Modules.Catalog.Tests/  # Catalog module unit tests
│   └── MonoSlice.IntegrationTests/       # Full integration tests with WebApplicationFactory
├── docker/
│   ├── Dockerfile                        # Multi-stage container build
│   └── docker-compose.yml                # Full local stack (API, DB, RabbitMQ, Redis, Jaeger)
├── .env.example                          # Environment variable templates
└── README.md
```

### Module Boundary Separation
Each module is a standalone class library containing:
- Its own **DbContext** and domain tables.
- Its own **Vertical Slices** (features).
- Clean dependency only on `MonoSlice.Shared.Abstractions` and `MonoSlice.Shared.Infrastructure`.
- Cross-module communication via asynchronous **Integration Events** over RabbitMQ/Kafka.

---

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for containers)

### 1. Clone & Build
```bash
git clone https://github.com/Skyleaft/MonoSlice.git
cd MonoSlice
dotnet restore
dotnet build
```

### 2. Run Tests
```bash
dotnet test
```

### 3. Run with Docker Compose (Recommended)
Launch the entire stack (PostgreSQL, RabbitMQ, Redis, Jaeger, and API):
```bash
cd docker
docker-compose up --build -d
```

Access the services:
- **Scalar API Reference**: [http://localhost:8080/scalar](http://localhost:8080/scalar)
- **Health Check**: [http://localhost:8080/health](http://localhost:8080/health)
- **RabbitMQ Management**: [http://localhost:15672](http://localhost:15672) (User: `guest`, Pass: `guest`)
- **Jaeger Tracing UI**: [http://localhost:16686](http://localhost:16686)

---

## ⚙️ Configuration & Environment Variables

Every setting can be overridden via environment variables or `.env`:

| Variable | Default | Description |
|---|---|---|
| `ConnectionStrings__UsersDb` | `Host=localhost;Database=monoslice_users...` | PostgreSQL connection string for Users module |
| `ConnectionStrings__CatalogDb` | `Host=localhost;Database=monoslice_catalog...` | PostgreSQL connection string for Catalog module |
| `Auth__JwtSecret` | `MonoSlice_Super_Secret_Key...` | Symmetric secret key for JWT signing |
| `Auth__AccessTokenExpiryMinutes` | `60` | JWT expiration time in minutes |
| `Auth__EnableCookieAuth` | `true` | Enables cookie-based fallback authentication |
| `Cache__Provider` | `Memory` | Cache backend: `Memory` or `Redis` |
| `Cache__Redis__ConnectionString` | `localhost:6379` | Redis connection string (if Provider is Redis) |
| `Messaging__Provider` | `RabbitMQ` | Event broker: `RabbitMQ` or `Kafka` |
| `Messaging__RabbitMQ__Host` | `localhost` | RabbitMQ server hostname |
| `Messaging__Kafka__BootstrapServers` | `localhost:9092` | Kafka broker servers list |
| `OpenTelemetry__Endpoint` | `http://localhost:4317` | OTLP gRPC collector endpoint (e.g. Jaeger) |

---

## 📡 API Endpoints

### 👤 Users Module (`/api/users`)
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `POST` | `/api/users/register` | Register new user account | Anonymous |
| `POST` | `/api/users/login` | Login and receive JWT + refresh token | Anonymous |
| `POST` | `/api/users/logout` | Sign out current user session | Authorized |
| `POST` | `/api/users/refresh-token` | Refresh an expired access token | Anonymous |
| `GET` | `/api/users/me` | Get profile of logged-in user | Authorized |
| `POST` | `/api/users/assign-role` | Assign role (`Admin`, `Manager`, `User`) | Admin only |

### 📦 Catalog Module (`/api/catalog`)
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/catalog/products` | List products with pagination & search | Anonymous |
| `GET` | `/api/catalog/products/{id}` | Get product details (Cached) | Anonymous |
| `POST` | `/api/catalog/products` | Create product (publishes async event) | Admin, Manager |
| `PUT` | `/api/catalog/products/{id}` | Update product details | Admin, Manager |
| `DELETE` | `/api/catalog/products/{id}` | Delete product | Admin only |

---

## 🧱 Vertical Slice Structure Example

A typical feature slice contains everything in a single, focused directory:

```
src/Modules/MonoSlice.Modules.Catalog/Features/CreateProduct/
├── CreateProductCommand.cs        # Input DTO with DataAnnotations + Response DTO
├── CreateProductCommandHandler.cs # Core business logic & persistence
└── CreateProductEndpoint.cs       # Minimal API endpoint definition & route mapping
```

---

## 🧪 Testing Strategy

- **Unit Tests**: Test handlers in isolation with `NSubstitute` mocks and in-memory EF Core.
- **Domain Tests**: Verify domain model invariants, state transitions, and domain events.
- **Integration Tests**: End-to-end API testing using `WebApplicationFactory<Program>` without external infrastructure dependencies.

```bash
# Run all tests
dotnet test --logger "console;verbosity=normal"
```

---

## 📄 License
This project is licensed under the [MIT License](LICENSE).
