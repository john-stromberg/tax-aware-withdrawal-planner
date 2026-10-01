using TaxAwareWithdrawalPlanner.Api.Domain;

namespace TaxAwareWithdrawalPlanner.Api.Application.Contracts;

public interface IWithdrawalPlannerService
{
    WithdrawalPlanResult BuildPlan(WithdrawalPlanRequest request);
}
