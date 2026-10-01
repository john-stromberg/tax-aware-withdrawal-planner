# Tax-Aware Withdrawal Planner

Workflow-oriented advisor tool for annual withdrawal sequencing across taxable, tax-deferred, and Roth accounts.

## Stack
- .NET 10 ASP.NET Core Web API
- Lightweight web UI
- Graph-ready architecture for future Office/CRM integrations

## MVP Features
- RMD-first handling
- Bracket-aware tax-deferred withdrawals
- Taxable-first flexibility
- Roth backstop usage
- Residual over-bracket fallback

## Run
```bash
dotnet restore tax-aware-withdrawal-planner.slnx
dotnet run --project src/TaxAwareWithdrawalPlanner.Api/TaxAwareWithdrawalPlanner.Api.csproj
```

Open:
- `/` for UI
- `POST /api/withdrawal-planner/build-plan` for API
