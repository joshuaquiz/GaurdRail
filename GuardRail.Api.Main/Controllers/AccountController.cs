using GuardRail.Api.Models;
using GuardRail.Api.Models.Requests;
using GuardRail.Api.Models.Responses;
using GuardRail.Core.Models.Models;
using GuardRail.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuardRail.Api.Main.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AccountController(
    IAccountManagementService accountManagementService,
    ILogger<AccountController> logger)
    : GhControllerBase<AccountController>(
        logger)
{
    [HttpPost(Name = nameof(CreateAccount))]
    public async Task<ApiResult> CreateAccount(
        [FromBody]
        CreateNewAccountRequest createNewAccountRequest,
        CancellationToken cancellationToken) =>
        await GhWrappedApiCall(
            async () =>
                await accountManagementService.CreateNewAccount(
                    createNewAccountRequest.AccountName,
                    createNewAccountRequest.FirstName,
                    createNewAccountRequest.LastName,
                    createNewAccountRequest.Phone,
                    createNewAccountRequest.Email,
                    cancellationToken));

    [HttpGet(Name = nameof(GetDashboardData))]
    public async Task<ApiResult<DashboardDataResponse>> GetDashboardData(
        [FromHeader]
        User user,
        CancellationToken cancellationToken) =>
        await GhWrappedApiCall(
            async () =>
            {
                var result = await accountManagementService.GetDashboardData(
                    user,
                    cancellationToken);
                return new DashboardDataResponse(
                    result.Accounts);
            });
}