using GuardRail.Api.Models;
using GuardRail.Api.Models.Requests;
using GuardRail.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuardRail.Api.Main.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(
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
            {
                await accountManagementService.CreateNewAccount(
                    createNewAccountRequest.AccountName,
                    createNewAccountRequest.FirstName,
                    createNewAccountRequest.LastName,
                    createNewAccountRequest.Phone,
                    createNewAccountRequest.Email,
                    cancellationToken);
            });
}