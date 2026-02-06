# Observability101-Demo

A simple .NET 10 demo showcasing basic observability (OpenTelemetry) with a Postgres database and the Microsoft Aspire dashboard.

## Getting started

Prerequisites:
- Docker Desktop (or compatible Docker engine)
- .NET SDK 10.0 (to run the app locally)
- Free local ports: 18888 (Aspire dashboard), 4317 (OTLP gRPC), 15432 (Postgres)

### 1) Start infrastructure with Docker Compose
From the repository root:

- Start containers (Aspire + Postgres):
  - Windows PowerShell: `docker compose up -d`
- Stop containers: `docker compose down`
- Stop and remove data volume: `docker compose down -v` (this clears the database)

What gets started:
- Aspire Dashboard: http://localhost:18888
- OTLP gRPC ingest (mapped to host): http://localhost:4317
- Postgres: Host=localhost; Port=15432; Db=obs-example-db; User=obs-example; Password=obs-example-password

You can open the Aspire dashboard at http://localhost:18888 to observe traces, logs, and metrics once the app is running.

### 2) Run the application
Run the API locally with .NET (Development profile):

- Windows PowerShell (from repo root): `dotnet run --project .\Observability101`

The API will listen on:
- http://localhost:5036
- Swagger UI: http://localhost:5036/swagger

Note:
- The app is configured to export telemetry to OTLP using the values in `Observability101/appsettings.Development.json`:
  - `OTEL_EXPORTER_OTLP_ENDPOINT`: `http://localhost:4317`
  - `OTEL_EXPORTER_OTLP_PROTOCOL`: `grpc`
- On startup (Demo only), the database is dropped and re-created automatically.

### 3) Generate some activity
Use Swagger at http://localhost:5036/swagger to call endpoints and generate traces/metrics/logs. Then switch to the Aspire dashboard (http://localhost:18888) to view activity.

### Troubleshooting
- If you don’t see telemetry in Aspire:
  - Ensure Docker containers are running: `docker ps` should show the Aspire and Postgres containers.
  - Verify OTLP endpoint and protocol in `Observability101/appsettings.Development.json`.
  - Ensure port 4317 is not blocked by a firewall and that the Aspire container is exposing ingestion (compose maps host 4317 to container 18889).
- If the API cannot connect to Postgres, confirm the database container is healthy and that port 15432 is free.

### Useful commands
- Rebuild containers after changes: `docker compose up -d --pull always`
- View container logs:
  - Aspire: `docker logs obs101-aspire-container`
  - Postgres: `docker logs obs101-postgres-container`
