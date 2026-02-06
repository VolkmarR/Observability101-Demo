# Observability101-Demo

Demo project for learning OpenTelemetry (OTEL) observability in .NET applications.

## How to Run

### Prerequisites

Start the required Docker services:

```bash
docker-compose up -d
```

This will start:
- **Aspire Dashboard** - OpenTelemetry observability dashboard
  - Dashboard UI: http://localhost:18888
  - OTLP endpoint: http://localhost:4317
- **PostgreSQL Database** - Demo database
  - Port: 15432
  - Database: obs-example-db
  - User: obs-example
  - Password: obs-example-password

### Run the Application

1. Clone the repository
2. Navigate to the project directory
3. Start Docker services (see above)
4. Run the application:
   ```
   dotnet run
   ```
5. The API will be available at the configured port (typically https://localhost:5001)

## Step-by-Step Branches

This repository contains commits that demonstrate the progressive implementation of OpenTelemetry:

- Check out different commits to see each step of adding OTEL instrumentation to the project
- Follow the commit history to understand how observability features are added incrementally
