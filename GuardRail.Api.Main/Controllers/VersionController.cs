using GuardRail.Api.Models;
using GuardRail.Api.Models.Responses;
using GuardRail.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuardRail.Api.Main.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController(
    IVersionManagementService versionManagementService,
    ILogger<VersionController> logger)
    : GhControllerBase<VersionController>(
        logger)
{
    [HttpGet(Name = nameof(VersionCheck))]
    public async Task<ApiResult<VersionCheckResponse>> VersionCheck(
        [FromQuery]
        Version version,
        CancellationToken cancellationToken) =>
        await GhWrappedApiCall(
            async () =>
            {
                var result = await versionManagementService.VersionCheck(
                    version,
                    cancellationToken);
                return new VersionCheckResponse(
                    result.IsLatest,
                    result.IsUpdateRequired,
                    result.LatestVersion,
                    result.DownloadUrl);
            });
}