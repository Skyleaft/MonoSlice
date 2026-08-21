# Stage 1: Build & Native AOT Compile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Install native compilation dependencies (clang compiler, zlib development headers)
RUN apt-get update && apt-get install -y clang zlib1g-dev

# Copy solution and project files for caching restore layer
COPY MonoSlice.slnx ./
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY global.json ./
COPY src/MonoSlice.Shared/MonoSlice.Shared.Abstractions/*.csproj ./src/MonoSlice.Shared/MonoSlice.Shared.Abstractions/
COPY src/MonoSlice.Shared/MonoSlice.Shared.Infrastructure/*.csproj ./src/MonoSlice.Shared/MonoSlice.Shared.Infrastructure/
COPY src/Modules/MonoSlice.Modules.Users/*.csproj ./src/Modules/MonoSlice.Modules.Users/
COPY src/Modules/MonoSlice.Modules.Catalog/*.csproj ./src/Modules/MonoSlice.Modules.Catalog/
COPY src/MonoSlice.Host/*.csproj ./src/MonoSlice.Host/

RUN dotnet restore -r linux-x64 src/MonoSlice.Host/MonoSlice.Host.csproj

# Copy source tree and compile to native AOT binary
COPY . .
WORKDIR /app/src/MonoSlice.Host
RUN dotnet publish -c Release -r linux-x64 --self-contained \
    -p:PublishAot=true \
    -p:StripSymbols=true \
    -o /out

# Stage 2: Ultra-lightweight Chiseled / Distroless Runtime (~35MB total image size)
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-noble-chiseled AS runtime
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /out .

# Run native compiled Linux binary directly without .NET runtime
ENTRYPOINT ["./MonoSlice.Host"]
