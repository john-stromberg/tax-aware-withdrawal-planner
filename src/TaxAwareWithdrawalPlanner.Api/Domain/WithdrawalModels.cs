namespace TaxAwareWithdrawalPlanner.Api.Domain;

public sealed class WithdrawalPlanRequest
{
    public string HouseholdName { get; set; } = string.Empty;
    public int PlanningYear { get; set; }
    public decimal SpendingNeed { get; set; }
    public decimal TaxableBalance { get; set; }
    public decimal TaxDeferredBalance { get; set; }
    public decimal RothBalance { get; set; }
    public decimal RmdAmount { get; set; }
    public decimal OrdinaryIncomeAlreadyRealized { get; set; }
    public decimal TargetTopBracketIncome { get; set; }
}

public sealed class WithdrawalStep
{
    public string Source { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Rationale { get; set; } = string.Empty;
}

public sealed class WithdrawalPlanResult
{
    public string HouseholdName { get; set; } = string.Empty;
    public int PlanningYear { get; set; }
    public decimal RequestedSpendingNeed { get; set; }
    public decimal FundedSpendingNeed { get; set; }
    public decimal RemainingGap { get; set; }
    public List<WithdrawalStep> Steps { get; set; } = [];
    public decimal EndingTaxableBalance { get; set; }
    public decimal EndingTaxDeferredBalance { get; set; }
    public decimal EndingRothBalance { get; set; }
}
