[![CI](https://github.com/Mahran1998/ReleasePulse/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/Mahran1998/ReleasePulse/actions/workflows/ci.yml)

# ReleasePulse — Release Readiness & QA Portal (C# + PostgreSQL + React/TS)

ReleasePulse is a lightweight “release readiness” portal for small teams.  
It helps you track **work items**, attach **manual test cases**, and see a simple **release dashboard** (manual pass rate + automation status).

This project was built to demonstrate a job-ready skill mix for software development + QA collaboration:
- **C# (.NET 8) + SQL (PostgreSQL) + TypeScript (React)**
- **Unit + integration testing**
- **CI/CD with GitHub Actions**
- **Manual testing workflow artifacts (tester pack)**

---

## What problem does it solve?
Small teams often struggle to answer: **“Are we ready to release?”**
- Stories are tracked in one place, test cases in another, and CI results somewhere else.
- Manual QA progress is hard to summarize consistently.

ReleasePulse makes readiness **visible and auditable** in one simple tool.

---

## Main components

### Backend (ASP.NET Core Web API, .NET 8)
- REST API for work items and test cases
- EF Core migrations + PostgreSQL
- Health endpoint for ops checks

### Database (PostgreSQL 16)
- Runs locally via Docker Compose
- Managed via EF Core migrations

### Frontend (React + TypeScript + Vite)
- Fast UI dev server and production build
- Built with Vite (modern frontend tooling used by React teams)  
  Vite provides dev server + build tooling for React apps.  
  :contentReference[oaicite:1]{index=1}

### QA Pack (Manual testing)
Located in `docs/qa/`:
- `smoke-checklist.md`
- `test-cases.md` (10 test cases)
- `bug-report-template.md`

---

## Tech stack
- **Backend:** ASP.NET Core Web API (.NET 8)
- **DB:** PostgreSQL 16 (Docker)
- **ORM:** EF Core 8 + Npgsql provider
- **Frontend:** React + TypeScript + Vite
- **Testing:** xUnit (unit + integration)
- **CI/CD:** GitHub Actions (build, test, artifacts)

---

## Local development

### 1) Start Postgres
```bash
docker compose up -d
docker compose ps
