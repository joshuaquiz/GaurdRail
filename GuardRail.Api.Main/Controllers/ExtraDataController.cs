using GuardRail.Api.Models;
using GuardRail.Core.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GuardRail.Api.Main.Controllers;

[ApiController]
[Route("[controller]")]
public class ExtraDataController(
    ILogger<AccessPointController> logger)
    : GhControllerBase<AccessPointController>(
        logger)
{
    [HttpGet(Name = nameof(ListAccessPointTypes))]
    public ApiResult<string[]> ListAccessPointTypes() =>
        GhWrappedApiCall(
            () =>
                Enum.GetNames(
                    typeof(
                        AccessPointType)));
}