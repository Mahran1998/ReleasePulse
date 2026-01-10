# Manual Test Cases — ReleasePulse (MVP)

Format: ID / Title / Type / Preconditions / Steps / Expected

---

## RP-QA-001 — Create WorkItem (valid)
Type: API
Preconditions: API running; DB running
Steps:
1. POST /work-items with body:
   { "title": "Story 1", "description": "desc" }
2. GET /work-items
Expected:
- POST returns 201 with Id, Title="Story 1", Status="Backlog"
- GET list contains "Story 1"

---

## RP-QA-002 — Create WorkItem (empty title rejected)
Type: API (Validation)
Preconditions: API running
Steps:
1. POST /work-items with body:
   { "title": "   ", "description": "x" }
Expected:
- 400 Bad Request (or validation error)
- WorkItem is not created

---

## RP-QA-003 — Status transitions valid (Backlog → InDev → ReadyForQa → Done)
Type: API
Preconditions: WorkItem exists
Steps:
1. PATCH /work-items/{id}/status to InDev
2. PATCH /work-items/{id}/status to ReadyForQa
3. PATCH /work-items/{id}/status to Done
Expected:
- Each PATCH returns 200
- Final Status is Done

---

## RP-QA-004 — Status skipping rejected (Backlog → ReadyForQa)
Type: API (Business rule)
Preconditions: New WorkItem exists (Status=Backlog)
Steps:
1. PATCH /work-items/{id}/status to ReadyForQa
Expected:
- 400 Bad Request (or error)
- Status remains Backlog

---

## RP-QA-005 — Add Test Case to missing WorkItem returns 404
Type: API
Preconditions: none
Steps:
1. POST /work-items/{random-guid}/test-cases
Expected:
- 404 Not Found

---

## RP-QA-006 — Add Test Case under WorkItem and list it
Type: API
Preconditions: WorkItem exists
Steps:
1. POST /work-items/{id}/test-cases with:
   { "steps": "Do X", "expected": "See Y" }
2. GET /work-items/{id}/test-cases
Expected:
- POST returns 201 with TestCase Id
- GET returns list containing the new TestCase

---

## RP-QA-007 — Mark Test Case Pass updates UpdatedAt
Type: API
Preconditions: TestCase exists
Steps:
1. Note current UpdatedAt from TestCase response (or GET list)
2. PATCH /test-cases/{id} with:
   { "actual": "Worked", "result": "Pass", "testerNote": "OK" }
Expected:
- Result becomes Pass
- UpdatedAt is later than before

---

## RP-QA-008 — Release report counts are correct
Type: API (Aggregation)
Preconditions: Create 2 test cases: one Pass, one Fail
Steps:
1. Ensure at least 2 TestCases exist
2. Set one to Pass and one to Fail via PATCH
3. GET /reports/release
Expected:
- manualTests.total >= 2
- passed >= 1, failed >= 1
- passRatePercent matches passed/total

---

## RP-QA-009 — Export CSV contains expected headers
Type: API (Export)
Preconditions: at least 1 test exists
Steps:
1. GET /reports/release.csv
Expected:
- Response Content-Type is text/csv
- First line header includes:
  Id,WorkItemId,Result,Steps,Expected,Actual,TesterNote,CreatedAt,UpdatedAt

---

## RP-QA-010 — Swagger reachable
Type: UI (Swagger)
Preconditions: API running
Steps:
1. Open http://localhost:5000/swagger
Expected:
- Swagger UI loads without errors
- All endpoints visible
