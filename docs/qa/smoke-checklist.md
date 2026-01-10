# ReleasePulse — Smoke Checklist

Purpose: quick confidence check after changes / before demo.

## Environment
- Docker running
- PostgreSQL container up
- API running locally
- Swagger reachable

## Smoke Steps

### 1) DB is running
- Command:
  - docker compose ps
- Expected:
  - db container is Up (healthy)

### 2) API health
- Command:
  - curl http://localhost:5000/health
- Expected:
  - {"status":"ok"} (or similar)

### 3) Swagger reachable
- Open:
  - http://localhost:5000/swagger
- Expected:
  - Swagger UI loads

### 4) Create a Work Item (API)
- Use Swagger OR curl:
  - POST /work-items
- Expected:
  - 201 Created
  - Response contains Id + Title + Status

### 5) Work Items list
- GET /work-items
- Expected:
  - list includes the created Work Item

### 6) Add a Test Case under a Work Item
- POST /work-items/{id}/test-cases
- Expected:
  - 201 Created
  - Response contains WorkItemId

### 7) Update Test Case result Pass/Fail
- PATCH /test-cases/{id}
- Expected:
  - Result changes
  - UpdatedAt changes (new timestamp)

### 8) Release report
- GET /reports/release
- Expected:
  - Counts match manual tests (total/passed/failed/notRun)

### 9) Export CSV
- GET /reports/release.csv
- Expected:
  - downloads CSV
  - header row exists
