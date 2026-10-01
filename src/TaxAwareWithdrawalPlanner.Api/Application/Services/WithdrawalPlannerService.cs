using TaxAwareWithdrawalPlanner.Api.Application.Contracts;
using TaxAwareWithdrawalPlanner.Api.Domain;

namespace TaxAwareWithdrawalPlanner.Api.Application.Services;

public sealed class WithdrawalPlannerService : IWithdrawalPlannerService
{
    public WithdrawalPlanResult BuildPlan(WithdrawalPlanRequest request)
    {
        var need = Math.Max(0, request.SpendingNeed);
        var taxable = Math.Max(0, request.TaxableBalance);
        var taxDeferred = Math.Max(0, request.TaxDeferredBalance);
        var roth = Math.Max(0, request.RothBalance);

        var steps = new List<WithdrawalStep>();
        decimal funded = 0;

        // 1) Mandatory RMD first (counts toward spending funding)
        var rmd = Math.Min(Math.Max(0, request.RmdAmount), taxDeferred);
        if (rmd > 0)
        {
            taxDeferred -= rmd;
            funded += rmd;
            steps.Add(new WithdrawalStep
            {
                Source = "Tax-Deferred (RMD)",
                Amount = rmd,
                Rationale = "Required minimum distribution must be taken first."
            });
        }

        // 2) Taxable account withdrawals next for flexibility
        var remainingNeed = Math.Max(0, need - funded);
        if (remainingNeed > 0 && taxable > 0)
        {
            var fromTaxable = Math.Min(remainingNeed, taxable);
            taxable -= fromTaxable;
            funded += fromTaxable;
            steps.Add(new WithdrawalStep
            {
                Source = "Taxable",
                Amount = fromTaxable,
                Rationale = "Use taxable assets before realizing additional ordinary income where possible."
            });
        }

        // 3) Bracket-aware tax-deferred distributions (beyond RMD)
        remainingNeed = Math.Max(0, need - funded);
        if (remainingNeed > 0 && taxDeferred > 0)
        {
            var bracketRoom = Math.Max(0, request.TargetTopBracketIncome - request.OrdinaryIncomeAlreadyRealized - rmd);
            var fromTaxDeferred = Math.Min(remainingNeed, Math.Min(taxDeferred, bracketRoom));

            if (fromTaxDeferred > 0)
            {
                taxDeferred -= fromTaxDeferred;
                funded += fromTaxDeferred;
                steps.Add(new WithdrawalStep
                {
                    Source = "Tax-Deferred (Bracket-Aware)",
                    Amount = fromTaxDeferred,
                    Rationale = "Fill remaining income room up to target bracket limit."
                });
            }
        }

        // 4) Roth as tax-efficient backstop
        remainingNeed = Math.Max(0, need - funded);
        if (remainingNeed > 0 && roth > 0)
        {
            var fromRoth = Math.Min(remainingNeed, roth);
            roth -= fromRoth;
            funded += fromRoth;
            steps.Add(new WithdrawalStep
            {
                Source = "Roth",
                Amount = fromRoth,
                Rationale = "Use Roth for remaining spending need to limit additional taxable income."
            });
        }

        // 5) Last resort tax-deferred over bracket target
        remainingNeed = Math.Max(0, need - funded);
        if (remainingNeed > 0 && taxDeferred > 0)
        {
            var fromOverBracket = Math.Min(remainingNeed, taxDeferred);
            taxDeferred -= fromOverBracket;
            funded += fromOverBracket;
            steps.Add(new WithdrawalStep
            {
                Source = "Tax-Deferred (Over Bracket Target)",
                Amount = fromOverBracket,
                Rationale = "Residual gap funding after exhausting other sources."
            });
        }

        return new WithdrawalPlanResult
        {
            HouseholdName = request.HouseholdName,
            PlanningYear = request.PlanningYear,
            RequestedSpendingNeed = need,
            FundedSpendingNeed = funded,
            RemainingGap = Math.Max(0, need - funded),
            Steps = steps,
            EndingTaxableBalance = taxable,
            EndingTaxDeferredBalance = taxDeferred,
            EndingRothBalance = roth
        };
    }
}
