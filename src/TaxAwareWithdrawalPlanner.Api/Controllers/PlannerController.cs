using Microsoft.AspNetCore.Mvc;
using TaxAwareWithdrawalPlanner.Api.Application.Contracts;
using TaxAwareWithdrawalPlanner.Api.Domain;

namespace TaxAwareWithdrawalPlanner.Api.Controllers;

[ApiController]
[Route("api/withdrawal-planner")]
public sealed class PlannerController : ControllerBase
{
    private readonly IWithdrawalPlannerService _planner;

    public PlannerController(IWithdrawalPlannerService planner)
    {
        _planner = planner;
    }

    [HttpPost("build-plan")]
    public ActionResult<WithdrawalPlanResult> BuildPlan([FromBody] WithdrawalPlanRequest request)
    {
        return Ok(_planner.BuildPlan(request));
    }
}
